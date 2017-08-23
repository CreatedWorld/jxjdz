package avatar.module.room;

import avatar.entity.match.LastMatchUserList;
import avatar.entity.match.UserMatchData;
import avatar.entity.room.Room;
import avatar.entity.room.RoomPlayType;
import avatar.entity.room.RoomPlayer;
import avatar.global.*;
import avatar.module.room.dao.RoomDao;
import avatar.module.room.dao.RoomPlayerDao;
import avatar.module.room.service.RoomDataService;
import avatar.module.roomType.RoomTypeService;
import avatar.net.session.Session;
import avatar.protobuf.SysInner;
import avatar.util.GameData;
import avatar.util.RandomUtil;

import java.util.*;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.CopyOnWriteArrayList;

/**
 * 房间匹配业务
 */
public class MatchRoomService {
    private static final MatchRoomService instance = new MatchRoomService();
    private static final int maxCount = 125;//同一个房间类型中每次最多匹配出n桌。（避免大量db操作）
    private static final long timeout = 20 * 1000;
    private static final int tableCount = 10;//小于等于10桌的用户

    private static final Map<String , List<UserMatchData>> waitMatchMap = new ConcurrentHashMap<>();
    private static final Map<Integer , LastMatchUserList> lastMatchMap = new HashMap<>();
    private static final Map<String , List<UserMatchData>> matchedMap = new ConcurrentHashMap<>();
    private static final List<UserMatchData> timeoutList = new ArrayList<>();

    private final String matchKey = "%d_%d";

    public static final MatchRoomService getInstance(){
        return instance;
    }
    private String getMatchKey(int roomType , int roomRounds){
        String key = String.format(matchKey , roomType , roomRounds);
        return key;
    }

    private List<UserMatchData> getMatchList(int roomType , int roomRounds){
        return waitMatchMap.get(getMatchKey(roomType , roomRounds));
    }


    /**
     * 处理玩家点击匹配操作。先把玩家放到一个匹配队列中，系统线程定时轮询这个队列
     * @param userId                玩家id
     * @param roomType              需要匹配的房间类型
     * @param roomRounds            房间局数
     * @param hallServerLocalName   玩家所在的大厅的别名
     */
    public int putUserIntoMatchList(int userId , int roomType , int roomRounds , String hallServerLocalName){
        Room room = RoomDataService.getInstance().getInRoomByUserId(userId);
        if(room != null){
            //已经在房间中，不能再次匹配
            return ClientCode.ALREADY_IN_ROOM;
        }
        //先放入匹配队列
        putMatchUserInList(userId , roomType , roomRounds , hallServerLocalName);
        return ClientCode.SUCCESS;
    }

    /**
     * 取消匹配
     * @param userId        用户id
     * @param roomType      房间类型
     * @param roomRounds    房间回合数
     */
    public UserMatchData cancelMatch(int userId , int roomType , int roomRounds){
        List<UserMatchData> list =  getMatchList(roomType , roomRounds);
        if(list == null || list.size() == 0){
            return null;
        }
        for (UserMatchData userMatchData : list){
            if(userMatchData.getUserId() == userId){
                list.remove(userMatchData);
                //发送消息给前端。
                return userMatchData;
            }
        }
        return null;
    }

    /**
     * 小于等于n桌需要的用户数，采用随机组合
     */
    private boolean match0(List<UserMatchData> list , int roomNeedPlayerCount ,
                                             List<List<UserMatchData>> retList){
        List<UserMatchData> temp = new CopyOnWriteArrayList<>();
        temp.addAll(list);
        boolean match = false;
        while (temp.size() != 0){
            int index = RandomUtil.iRand(temp.size());
            UserMatchData data = temp.get(index);
            List<UserMatchData> userList;
            if(retList.size() == 0){
                userList = new ArrayList<>();
                retList.add(userList);
            }else{
                userList = retList.get(retList.size() - 1);
            }
            if(userList.size() >= roomNeedPlayerCount){
                userList = new ArrayList<>();
                retList.add(userList);
            }
            userList.add(data);
            //从队列中移除
            temp.remove(data);
            match = true;
        }
        return match;
    }

    /**
     * 大于2桌需要的用户数，分成2个队列来轮询。
     * @param list  当前队列
     */
    private void match1(List<UserMatchData> list , int roomNeedPlayerCount ,
                                             List<List<UserMatchData>> retList){
        List<UserMatchData> temp = new CopyOnWriteArrayList<>();
        temp.addAll(list);
        int size = temp.size();
        int index = size / 2;

        List<UserMatchData> list1 = new ArrayList<>();
        List<UserMatchData> list2 = new ArrayList<>();
        //将所有匹配用户分为2个List
        list1.addAll(temp.subList(0 , index));
        list2.addAll(temp.subList(index , size));

        for(int i = 0 ; i < list1.size() ; i ++){
            List<UserMatchData> data;
            if(retList.size() == 0){
                data = new ArrayList<>();
            }else{
                data = retList.get(retList.size() - 1);
            }
            UserMatchData user1 = list1.get(i);
            LastMatchUserList lastMatch = lastMatchMap.get(user1.getUserId());
            //满一桌用户人数，则开始匹配下一桌的用户
            if(data.size() == roomNeedPlayerCount){
                data = new ArrayList<>();
                retList.add(data);
            }
            data.add(user1);
            if(list2.size() == 0){
                continue;
            }

            boolean has = false;
            for(int j = 0 ; j < list2.size() ; j++){
                UserMatchData user2 = list2.get(j);
                if(lastMatch != null && lastMatch.hasMatched(user2.getUserId())){
                    continue;
                }
                data.add(user2);
                has = true;
            }
            if(!has){
                data.add(list2.get(0));
            }

            //如果超过最大桌数，则返回
            if(retList.size() > maxCount){
                return ;
            }
        }
        return ;
    }

    /**
     * 获得超时的用户列表
     */
    private List<UserMatchData> timeoutList(List<UserMatchData> list){
        List<UserMatchData> ret = new ArrayList<>();
        for(UserMatchData userMatchData : list){
            if(System.currentTimeMillis() - userMatchData.getStartMatch().getTime() > timeout){
                ret.add(userMatchData);
//                list.remove(userMatchData);
            }
        }
        if(ret.size() == 0){
            return ret;
        }
        for(UserMatchData userMatchData : ret){
            list.remove(userMatchData);
        }
        return ret;
    }

    //匹配逻辑
    public void match(){
        Map<String , List<UserMatchData>> map = new HashMap<>();
        map.putAll(waitMatchMap);

        if(map.size() == 0){
            return;
        }
        Set<String> keys = map.keySet();
        for(String key : keys){
            List<UserMatchData> list = map.get(key);
            if(list == null || list.size() == 0){
                continue;
            }
            List<UserMatchData> timeout = timeoutList(list);//过滤超时用户
            if(timeout != null && timeout.size() > 0) {
                timeoutList.addAll(timeout);
            }
            if(list.size() == 0){
                continue;
            }

            int count = RoomTypeService.getInstance().getSeatNumByRoomType(list.get(0).getRoomType());
            if(list.size() < count) {
                continue;
            }
            //每一个List<UserMatchData>为一个房间的用户，这里会返回多个房间列表
            List<List<UserMatchData>> retList = new CopyOnWriteArrayList<>();
            boolean ret = false;
//            if(list.size() <= count * tableCount){//
                ret = match0(list , count , retList);
//            }else{
//                match1(list , count , retList);
//                ret = true;
//            }
            if(ret){
                //放入等待创建房间的队列
                if(retList.size() > 0){
                    for(List<UserMatchData> l : retList){
                        if(l != null && l.size() == count){
                            List<UserMatchData> matchData;
                            if(matchedMap.containsKey(key)){
                                matchData = matchedMap.get(key);
                            }else{
                                matchData = new ArrayList<>();
                            }
                            matchData.addAll(l);
                            matchedMap.put(key , matchData);
                            //从等待的匹配队列中移除
                            retList.remove(l);
                        }else{
                            //add match list
                            addNotMatch(l);
                        }
                    }
                }
            }
        }
    }

    /**
     * 没有匹配到的，重新放回匹配队列中
     */
    private void addNotMatch(List<UserMatchData> l){
        if(l != null && l.size() > 0){
            for(int i = 0 ; i < l.size() ; i ++){
                UserMatchData notMatch = l.get(i);
                System.out.println("========重新匹配的玩家====  " + notMatch.getUserId());

                putMatchUserInList(notMatch.getUserId(), notMatch.getRoomType() ,
                        notMatch.getRoomRounds() ,notMatch.getFromHallServerName());
            }
        }
    }

    private List<Integer> initPlayType(){
        List<Integer> playType = new ArrayList<>();
        playType.add(0);
        return playType;
    }

    /**
     * 为已经分配好的用户创建房间
     */
    public void createMatchRoom(){
        Map<String , List<UserMatchData>> map = new HashMap<>();
        map.putAll(matchedMap);
        matchedMap.clear();
        if(map.size() == 0){
            return;
        }
        Room room;
        ServerUpdate serverUpdate;
        List<Integer> playType = initPlayType();
        Set<String> keys = map.keySet();
        if(keys.size() == 0){
            return;
        }
        for(String key : keys) {
            List<UserMatchData> list = map.get(key);
            if(list == null || list.size() == 0){
                continue;
            }
            String[] arr = key.split("_");
            int roomType = Integer.parseInt(arr[0]);
            int roomRounds = Integer.parseInt(arr[1]);
            int size = RoomTypeService.getInstance().getSeatNumByRoomType(roomType);
            if(list.size() < size){
                continue;
            }

            List<UserMatchData> ids = new ArrayList<>();
            List<RoomPlayer> tempRoomPlayers = new ArrayList<>();
            for(int i = 0 ; i < list.size();){
                //如果上一个房间已经满，继续轮询玩家填充下一个房间
                if(ids.size() == size){
                    ids.clear();
                }
                UserMatchData userMatchData = list.get(0);
                ids.add(userMatchData);
                list.remove(userMatchData);
                if(ids.size() == size){
                    //分配房间并且发送给大厅服务器，大厅服务器通知玩家
                    serverUpdate = ServerCenterManager.getInstance().
                            randomServerByName(Config.getInstance().getRoomServerName());
                    if(serverUpdate == null){
                        continue;
                    }
                    //创建房间
                    room = CreateRoomService.getInstance().initRoom(serverUpdate.getClientIp() , serverUpdate.getClientPort(),
                            roomType , roomRounds , ids.get(0).getUserId() , serverUpdate.getRemoteServerName() , playType);
                    int roomId = RoomDao.getInstance().addAndReturnId(room);
                    room = RoomDao.getInstance().loadRoomByRoomId(roomId);
                    if(room == null){
                       continue;
                    }
                    //创建roomPlayers
                    tempRoomPlayers.clear();
                    for(int j = 0 ; j < size ; j ++){
                        //注意，这里位置是固定的
                        RoomPlayer roomPlayer = CreateRoomService.getInstance().initRoomPlayer(roomId , ids.get(j).getUserId() , (j+1));
                        tempRoomPlayers.add(roomPlayer);
                    }
                    boolean addBatch = RoomPlayerDao.getInstance().addBatch(tempRoomPlayers);
                    //分发给用户
                    if(addBatch){
                        for(int j = 0 ; j < size ; j ++) {
                            int uId = ids.get(j).getUserId();
                            sendMatchSuc(uId , ids.get(j).getFromHallServerName() , roomId , room.getRoomCode() ,
                                    room.getRoomType() , room.getRounds() , room.getRoomIp() , room.getPort(), (j+1));
                            LastMatchUserList lastMatchUserList = new LastMatchUserList(uId , ids);
                            lastMatchMap.put(uId , lastMatchUserList);
                        }
                    }
                }
            }
        }
    }

    /**
     * 从匹配列表中移除已经匹配到的
     */
    public void removeMatched(){
        Set<String> matching = waitMatchMap.keySet();//匹配队列中的数据
        if(matching.size() == 0){
            return;
        }
        Set<String> matched = matchedMap.keySet();//已经匹配到的数据
        if (matched.size() == 0)
            return;

        for(String key : matching){
            if(!matchedMap.containsKey(key)){
                continue;
            }
            List<UserMatchData> waitList = waitMatchMap.get(key);
            List<UserMatchData> matchedList = matchedMap.get(key);
            waitList.removeAll(matchedList);
            waitMatchMap.put(key , waitList);

//            for(UserMatchData data : waitMatchMap.get(key)){
//                System.out.println("=======剩下的未匹配的=====" + data.getUserId());
//            }
        }
    }

    /**
     * 匹配成功并创建了房间，发送回大厅，大厅通知该用户
     * @param userId            用户id
     * @param hallServerName    大厅别名（区别于当前用户在那个大厅）
     * @param roomId            房间id
     * @param roomCode          房间号
     * @param roomType          房间类型
     * @param roomRounds        局数
     * @param serverIp          房间服务器的ip
     * @param port              房间服务器的端口
     * @param seat              座位号
     */
    private void sendMatchSuc(int userId , String hallServerName ,int roomId , String roomCode , int roomType ,
                              int roomRounds , String serverIp ,int port , int seat){
        Session session = GameData.getSessionManager().getSessionByRemoteServerName(hallServerName);
        if(session == null){
            return;
        }
        System.out.println("====================match suc ==================userId=" + userId);
        SysInner.InnerMatchSucCenter2Hall.Builder b = SysInner.InnerMatchSucCenter2Hall.newBuilder();
        b.setRoomId(roomId);
        b.setRoomCode(roomCode);
        b.setRoomRounds(roomRounds);
        b.setRoomType(roomType);
        b.setRoomServerIp(serverIp);
        b.setRoomServerPort(port);
        b.setSeat(seat);
        b.setUserId(userId);
        session.sendClient(InnerCmd.Center2Hall_Inner_MatchSuc , b.build().toByteArray());
    }


    /**
     * 添加玩家到匹配队列中
     * @param userId                玩家id
     * @param roomType              房间类型
     * @param roomRounds            房间局数
     * @param hallServerLocalName   玩家所在的大厅的别名
     */
    private void putMatchUserInList(int userId , int roomType , int roomRounds , String hallServerLocalName){
        UserMatchData userMatchData = new UserMatchData(userId , roomType ,roomRounds ,
                hallServerLocalName , new Date());
        String key = getMatchKey(roomType , roomRounds);
        List<UserMatchData> list;
        if(waitMatchMap.containsKey(key)){
            list = getMatchList(roomType , roomRounds);
            //已经在匹配队列中，删除原来的匹配信息。再添加一条信息
            for(UserMatchData userMatchData1 : list){
                if(userMatchData1.getUserId() == userId){
                    list.remove(userMatchData1);
                    break;
                }
            }
        }else{
            list = new ArrayList<>();
        }
        list.add(userMatchData);
        waitMatchMap.put(key , list);
    }


    public void handleTimeoutList(){
        List<UserMatchData> list = new ArrayList<>();
        list.addAll(timeoutList);
        timeoutList.clear();

        if(list.size() == 0){
            return;
        }
        for(UserMatchData data : list) {
            int userId = data.getUserId();

            Session hallSession = GameData.getSessionManager().getSessionByRemoteServerName(data.getFromHallServerName());
            if(hallSession != null) {
                SysInner.InnerCancelMatchCenter2Hall.Builder builder = SysInner.InnerCancelMatchCenter2Hall.newBuilder();
                builder.setUserId(userId);
                builder.setClientCode(ClientCode.SUCCESS);
                hallSession.sendClient(InnerCmd.Center2Hall_Inner_CancelMatchRoom, builder.build().toByteArray());
            }
        }
    }

    public static void main(String[] args){
        List<Integer> list = new ArrayList<>();
        list.add(1);
        list.add(1);
        list.add(2);
        list.add(3);
        list.add(4);
        list.add(5);

        int size = list.size();
        int index = size / 2;

        List<Integer> list1 = new ArrayList<>();
        list1.addAll(list.subList(0 , index));
        List<Integer> list2 = new ArrayList<>();
        list2.addAll(list.subList(index , size));

        for(Integer i : list1){
            System.out.println("i = " + i);
        }

        for(Integer i : list2){
            System.out.println("j = " + i);
        }
    }
}
