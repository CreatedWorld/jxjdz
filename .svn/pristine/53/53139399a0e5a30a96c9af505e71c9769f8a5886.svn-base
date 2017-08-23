package avatar.module.room;

import avatar.entity.room.Room;
import avatar.entity.room.RoomPlayer;
import avatar.global.ClientCode;
import avatar.global.Config;
import avatar.global.ServerCenterManager;
import avatar.global.ServerUpdate;
import avatar.module.room.dao.RoomDao;
import avatar.module.room.dao.RoomPlayerDao;
import avatar.module.room.service.RoomDataService;
import avatar.module.roomType.RoomTypeService;
import avatar.protobuf.SysInner;
import avatar.util.StringUtil;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.*;


/**
 * 处理每个玩家的创建房间命令，加入房间命令<br></br>
 * 这里需要注意的是：对于一个大厅session的请求，是在一个线程组中执行。线程组线程分为create&join 线程、match
 */
public class CreateRoomService {

    private static final Logger log = LoggerFactory.getLogger(CreateRoomService.class);

    private static final Map<Integer, List<Room>> map = new HashMap<Integer, List<Room>>();
    private static final CreateRoomService instance = new CreateRoomService();

    public static final CreateRoomService getInstance() {
        return instance;
    }

    /**
     * 创建房间
     *
     * @param userId       玩家id
     * @param type         房间类型
     * @param rounds       房间局数
     * @param createUserId 创建者id
     */
    public int createRoom(int userId, int type, int rounds, int createUserId, List<Integer> playType,
                          int seat, SysInner.InnerCreateRoomCenter2Hall.Builder builder) {

        //查看玩家是否在房间中
        boolean check = checkInRoom(userId);
        if (check) {
            return ClientCode.ALREADY_IN_ROOM;
        }
        //筛选服务器
        ServerUpdate serverUpdate = ServerCenterManager.getInstance().
                randomServerByName(Config.getInstance().getRoomServerName());
        if (serverUpdate == null) {
            return ClientCode.FAILED;
        }

        //创建room对象
        Room room = initRoom(serverUpdate.getClientIp(), serverUpdate.getClientPort(),
                type, rounds, createUserId, serverUpdate.getClientProxyName(), playType);
        int roomId = RoomDao.getInstance().addAndReturnId(room);
        if (roomId == 0) {
            //log
            log.error("[CreateRoomService]createRoom , insert room failed . userId = "
                    + userId + ", type = " + type);
            return ClientCode.FAILED;
        }
        room = RoomDao.getInstance().loadRoomByRoomId(roomId);
        //创建roomPlayer对象
        if (seat == 0) {
            seat = initSeat(seat, null);
        }
        RoomPlayer roomPlayer = initRoomPlayer(roomId, userId, seat);
        RoomPlayerDao.getInstance().add(roomPlayer);

        builder.setRoomCode(room.getRoomCode());
        builder.setRoomServerIp(room.getRoomIp());
        builder.setRoomServerPort(room.getPort());
        builder.setSeat(roomPlayer.getSeat());
        return ClientCode.SUCCESS;
    }

    /**
     * 加入房间
     *
     * @param userId   玩家id
     * @param seat     座位号
     * @param roomCode 房间号
     */
    public int joinRoom(int userId, int seat, String roomCode,
                        SysInner.InnerJoinInRoomCenter2Hall.Builder builder,
                        SysInner.InnerJoinInRoomCenter2Room.Builder roomBuild) {
        builder.setUserId(userId);

        //检查玩家是否已经存在房间中
        Room room = RoomDao.getInstance().loadRoomByRoomCode(roomCode);
        if (room == null) {
            builder.setRoomCode(roomCode);
            builder.setRoomServerIp("");
            builder.setRoomServerPort(0);
            builder.setRoomType(0);
            builder.setRounds(0);
            builder.setSeat(0);
            return ClientCode.NO_ROOM;
        }
        boolean check = checkInRoom(userId);
        if (check) {
            builder.setRoomCode(roomCode);
            builder.setRoomServerIp("");
            builder.setRoomServerPort(0);
            builder.setRoomType(0);
            builder.setRounds(0);
            builder.setSeat(0);
            return ClientCode.ALREADY_IN_ROOM;
        }

        //获取所有在同一个房间的玩家
        List<RoomPlayer> list = RoomDataService.getInstance().loadAllInRoom(room.getId());
        if (list != null && list.size() >= RoomTypeService.getInstance().getSeatNumByRoomType(room.getRoomType())) {
            builder.setRoomCode(roomCode);
            builder.setRoomServerIp("");
            builder.setRoomServerPort(0);
            builder.setRoomType(0);
            builder.setRounds(0);
            builder.setSeat(0);
            return ClientCode.OVERFLOW_ROOM_PLAYERS;
        }

        //如果座位已经有一个用户，则提示不能加入该座位
        for (RoomPlayer roomPlayer : list) {
            if (roomPlayer.getSeat() == seat) {
                builder.setRoomCode(roomCode);
                builder.setRoomServerIp("");
                builder.setRoomServerPort(0);
                builder.setRoomType(0);
                builder.setRounds(0);
                builder.setSeat(0);
                return ClientCode.NO_SEAT_IN_ROOM;
            }
        }

        //选择一个座位号，如果有指定，则给定指定的座位号
        int seatNew = initSeat(seat, list);

        //添加一个房间玩家对象
        RoomPlayer roomPlayer = initRoomPlayer(room.getId(), userId, seatNew);
        RoomPlayerDao.getInstance().add(roomPlayer);

        builder.setRoomCode(roomCode);
        builder.setRoomServerIp(room.getRoomIp());
        builder.setRoomServerPort(room.getPort());
        builder.setRoomType(room.getRoomType());
        builder.setRounds(room.getRounds());
        builder.setSeat(roomPlayer.getSeat());


        roomBuild.setRoomCode(roomCode);
        roomBuild.setUserId(userId);
        roomBuild.setSeat(roomPlayer.getSeat());
        roomBuild.setRoomType(room.getRoomType());
        roomBuild.setRoomId(room.getId());
        roomBuild.setRounds(room.getRounds());
        roomBuild.setRoomServerIp(room.getRoomIp());
        roomBuild.setRoomServerPort(room.getPort());
        roomBuild.setRoomServerLocalName(room.getRoomLocalName());

        return ClientCode.SUCCESS;
    }


    public Room initRoom(String ip, int port, int type, int rounds, int createUserId,
                         String roomLocalName, List<Integer> playType) {
        Room room = new Room();
        room.setRoomIp(ip);
        room.setPort(port);
        room.setNeedPlayerNum(RoomTypeService.getInstance().getSeatNumByRoomType(type));
        room.setState(Room.State.CREATED);
        room.setRoomCode(GeneratorRoomCodeService.getInstance().getNextRoomCode());
        room.setRoomType(type);
        room.setRounds(rounds);
        // DEBUGING 房间局数
        // room.setRounds(2);

        room.setCreateUserId(createUserId);
        room.setTime();
        room.setRoomLocalName(roomLocalName);
        String playTypeStr = "0";
        if (playType != null && playType.size() > 0) {
            playTypeStr = StringUtil.join(playType, ",");
        }
        room.setPlayType(playTypeStr);
        room.setCurRounds(0);
        return room;
    }

    public RoomPlayer initRoomPlayer(int roomId, int userId, int seat) {
        RoomPlayer roomPlayer = new RoomPlayer();
        roomPlayer.setPlayerId(userId);
        roomPlayer.setRoomId(roomId);
        roomPlayer.setState(RoomPlayer.State.UNREADY.getCode());
        roomPlayer.setSeat(seat);
        roomPlayer.setTime();
        return roomPlayer;
    }

    /**
     * 检查玩家已经在房间中
     *
     * @param userId
     * @return
     */
    private boolean checkInRoom(int userId) {
        Room room = RoomDataService.getInstance().getInRoomByUserId(userId);
        if (room != null) {
            return true;
        }
        return false;
    }

    /**
     * 随机生成座位号<br></br>
     *
     * @param seat 传进来的座位号（如果没有，则生成）
     * @param list 已经在房间里面的玩家队列
     */
    public int initSeat(int seat, List<RoomPlayer> list) {
        if (seat != 0) {
            return seat;
        }
        if (list == null || list.size() == 0) {
            // 第一个进房间的人，座位为1
            // return RandomUtil.iRand(1 , ROOM_SEAT_SIZE);
            return 1;
        }
        List<Integer> seatIds = new ArrayList<>();
        for (RoomPlayer roomPlayer : list) {
            seatIds.add(roomPlayer.getSeat());
        }
        Collections.sort(seatIds);
        Room room = RoomDao.getInstance().loadRoomByRoomId(list.get(0).getRoomId());
        int needPlayerNum = room.getNeedPlayerNum() == 0 ? RoomTypeService.getInstance()
                .getSeatNumByRoomType(room.getRoomType())
                : room.getNeedPlayerNum();
        for (int i = 1; i <= needPlayerNum; i++) {
            if (seatIds.indexOf(i) == -1) {
                return i;
            }
        }
        return 0;
    }
}
