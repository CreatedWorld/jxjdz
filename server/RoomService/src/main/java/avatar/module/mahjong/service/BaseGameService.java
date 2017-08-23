package avatar.module.mahjong.service;

import avatar.entity.battlerecord.BattleScoreEntity;
import avatar.entity.room.Room;
import avatar.entity.room.RoomPlayType;
import avatar.entity.room.RoomPlayer;
import avatar.module.mahjong.*;
import avatar.module.mahjong.dao.MahjongGameDataDao;
import avatar.module.mahjong.draw.Draw;
import avatar.module.mahjong.operate.ActionType;
import avatar.module.mahjong.operate.CanDoOperate;
import avatar.module.mahjong.operate.scanner.GetACardScanner;
import avatar.module.mahjong.operate.scanner.PlayACardScanner;
import avatar.module.mahjong.operate.scanner.QiangAnGangScanner;
import avatar.module.mahjong.replay.Action;
import avatar.module.room.dao.RoomDao;
import avatar.module.room.dao.RoomPlayerDao;
import avatar.module.room.service.RoomDataService;
import avatar.module.trusteeship.TrusteeshipService;
import avatar.task.AutoPassTask;
import avatar.task.AutoPlayACardTask;
import avatar.util.LogUtil;
import avatar.util.MessageUtil;
import avatar.util.MockMahjongUtil;
import avatar.util.RoomPlayTypeUtil;
import org.apache.commons.collections.CollectionUtils;
import org.apache.commons.lang.math.RandomUtils;

import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashSet;
import java.util.List;

public class BaseGameService {

    private static final BaseGameService instance = new BaseGameService();

    public static BaseGameService getInstance() {
        return instance;
    }

    private static final RoomDataService roomDataService = RoomDataService.getInstance();

    private static final GetACardScanner getACardScanner = GetACardScanner.getInstance();

    private static final PlayACardScanner playACardScanner = PlayACardScanner.getInstance();

    private static final QiangAnGangScanner qiangAnGangScanner = QiangAnGangScanner.getInstance();

    private static final MahjongGameDataDao mahjongGameDataDao = MahjongGameDataDao.getInstance();

    /**
     * 扫描有没有用户可以抢杠
     * 没有，则给user发一张牌
     * 有则广播可操作列表
     *
     * @param mahjongGameData 游戏数据
     * @param mahjong         别人打出或自己摸到的麻将
     * @param userId          需要杠的玩家
     */
    public void scanAnyUserQiangGangHandler(MahjongGameData mahjongGameData, Mahjong mahjong, int userId) throws Exception {
        // 判断其他玩家有没有抢杠
        List<CanDoOperate> canOperates = qiangAnGangScanner.scan(mahjongGameData, mahjong, userId);
        if (canOperates.isEmpty()) {
            // 给杠的用户摸一张牌
            handleCommonUserTouchAMahjong(mahjongGameData, userId);
        } else {
            mahjongGameData.setCanDoOperates(canOperates);

            // 推送用户抢杠提示
            MessageUtil.pushPlayerActionTips(mahjongGameData);

            mahjongGameDataDao.save(mahjongGameData);
        }
    }


    /**
     * 进入下一局游戏，或结束游戏
     */
    public void ready4NextGameOrFinishGame(MahjongGameData mahjongGameData, Room room)
            throws IllegalAccessException, InstantiationException, ClassNotFoundException {
        int nextRound = genNextRound(mahjongGameData);
        if (nextRound <= mahjongGameData.getTotalTimes()) {
            // 还没到最后一局，可以继续一下局
            // 玩家状态改为待准备
            List<RoomPlayer> roomPlayers = RoomDataService.getInstance()
                    .loadRoomPlayerListByState(mahjongGameData.getRoomId(), RoomPlayer.State.READY.getCode());
            for (RoomPlayer roomPlayer : roomPlayers) {
                roomPlayer.setState(RoomPlayer.State.UNREADY.getCode());
                RoomPlayerDao.getInstance().update(roomPlayer);
            }

        } else {
            List<BattleScoreEntity> battleScores = SettlementService.getInstance().genTotalScores(mahjongGameData);
            MessageUtil.broadcastTotalScore(battleScores);

            // 更改房间状态和房间用户状态
            room.setState(Room.State.DISSOLVED);
            RoomDao.getInstance().update(room);

            mahjongGameDataDao.remove(room.getId());

            List<RoomPlayer> roomPlayers =
                    roomDataService.loadRoomPlayerListByState(room.getId(), RoomPlayer.State.READY.getCode());
            RoomPlayerDao.getInstance().deleteAllRoomPlayerInRoom(roomPlayers);
        }
    }


    /**
     * 初始化麻将游戏，将麻将打乱，分成n份，每份13只，还有剩余的牌
     * 庄家在最后会摸多一张牌，即庄家有14张
     * 初始数据的生成与实际的玩家无关，只需要确定玩家的人数和庄家座位即可。
     * 初始数据生成后，由调用者封装实际玩家手牌数据
     *
     * @param roomId    roomId
     * @param PlayerNum 玩家人数，决定分多少副手牌
     * @param isSpend   是否消耗了创建房间的资源
     */
    @SuppressWarnings("unchecked")
    public MahjongGameData initGameData(int roomId, int PlayerNum, int currentTimes, int totalTimes, int bankerSeat, boolean isSpend)
            throws InvocationTargetException, IllegalAccessException, NoSuchMethodException {
        // 获取所有麻将牌
        List<Mahjong> allMahjongs;
        String mockMahjongMethod = MahjongConfig.mockMahjongMethod;
        if (mockMahjongMethod == null || "false".equalsIgnoreCase(mockMahjongMethod)) {
            allMahjongs = Mahjong.getAllMahjongs();
        } else {
            Method method = MockMahjongUtil.class.getDeclaredMethod(mockMahjongMethod, new Class[0]);
            allMahjongs = (List<Mahjong>) method.invoke(null);
        }

        // 根据每个项目的房间类型的玩法不同，来过滤掉某些麻将，不用发牌
        Room room = roomDataService.getRoomByRoomId(roomId);
        RoomPlayTypeUtil.filterMahjongs(room, allMahjongs);

        // 创建麻将游戏的数据结构
        MahjongGameData mahjongGameData = new MahjongGameData();
        mahjongGameData.setRoomId(roomId);
        List<PersonalMahjongInfo> personalMahjongInfos = new ArrayList<>(PlayerNum);
        List<Mahjong> leftCards = new ArrayList<>(allMahjongs.size() - PlayerNum * MahjongConfig.HAND_CARD_NUMBER);
        mahjongGameData.setPersonalMahjongInfos(personalMahjongInfos);
        mahjongGameData.setLeftCards(leftCards);
        mahjongGameData.setCurrentTimes(currentTimes);
        mahjongGameData.setTotalTimes(totalTimes);
        mahjongGameData.setBankerSite(bankerSeat);
        mahjongGameData.setSpend(isSpend);

        // 设置宝牌
        mahjongGameData.setTreasureCard(Mahjong.HONG_ZHONG);

        // 宝牌能变成的麻将
        mahjongGameData.setMakeUpMahjongs(new HashSet<>(Mahjong.removeFlower(allMahjongs)));

        // 掷骰
        mahjongGameData.setDices(rollDice());

        if (mockMahjongMethod == null || "false".equalsIgnoreCase(mockMahjongMethod)) {
            Collections.shuffle(allMahjongs);
        }

        // 创建每个玩家的手牌对象
        for (int i = 0; i < PlayerNum; i++) {
            PersonalMahjongInfo personalMahjongInfo = new PersonalMahjongInfo();
            personalMahjongInfos.add(personalMahjongInfo);

            personalMahjongInfo.setOutMahjong(new ArrayList<>(10));

            personalMahjongInfo.setHandCards(new ArrayList<>(MahjongConfig.HAND_CARD_NUMBER));

            // 碰列表
            List<Combo> pengs = new ArrayList<>(1);
            personalMahjongInfo.setPengs(pengs);

            // 杠列表
            ArrayList<Combo> combos = new ArrayList<>(1);
            personalMahjongInfo.setGangs(combos);
        }

        // 把牌分给玩家
        List<Mahjong> temp;
        for (PersonalMahjongInfo mahjongInfo : personalMahjongInfos) {
            temp = mahjongInfo.getHandCards();
            for (int i = 0; i < MahjongConfig.HAND_CARD_NUMBER; i++) {
                temp.add(allMahjongs.remove(allMahjongs.size() - 1));
            }
            Collections.sort(mahjongInfo.getHandCards());
        }

        // 庄家摸多一张牌
        personalMahjongInfos.get(mahjongGameData.getBankerSite() - 1).setTouchMahjong(allMahjongs.remove(allMahjongs.size() - 1));

        // 剩下的牌放在leftCards
        leftCards.addAll(allMahjongs);

        return mahjongGameData;
    }

    /**
     * 掷骰
     */
    private Integer[] rollDice() {
        return new Integer[]{
                RandomUtils.nextInt(6) + 1,
                RandomUtils.nextInt(6) + 1};
    }


    /**
     * 判断房间内是否所有位置都有玩家加入，并且所有玩家都已准备
     *
     * @param roomId 房间id
     */
    public boolean isAllReady(int roomId) {
        Room room = roomDataService.getRoomByRoomId(roomId);

        int count = roomDataService.count(roomId);
        if (count != room.getNeedPlayerNum()) {
            return false;
        }

        int readyCount = roomDataService.readyCount(roomId);
        if (readyCount != room.getNeedPlayerNum()) {
            return false;
        }

        return true;
    }

    /**
     * 正常摸剩余麻将的最后一只
     */
    public Mahjong touchAMahjong(MahjongGameData mahjongGameData) {
        Mahjong touchMahjong = mahjongGameData.getLeftCards().remove(mahjongGameData.getLeftCards().size() - 1);

        return touchMahjong;
    }

    /**
     * 判断玩家有没有执行操作的权利
     *
     * @param userId     玩家id
     * @param actionType 玩家需要执行的操作
     */
    public boolean canOperate(MahjongGameData mahjongGameData, int userId, ActionType actionType) {
        List<CanDoOperate> canDoOperates = mahjongGameData.getCanDoOperates();

        if (canDoOperates.isEmpty()) {
            throw new RuntimeException(String.format(
                    "房间id=%s，可操作列表为空",
                    mahjongGameData.getRoomId()
            ));
        }

        // 如果是一炮多响，则吃胡和过的操作不一定在index 0
        Room room = roomDataService.getRoomByRoomId(mahjongGameData.getRoomId());
        if (!room.getPlayTypeList().contains(RoomPlayType.NO_MULTIPLE_CHI_HU.getId())
                && (actionType == ActionType.CHI_HU || actionType == ActionType.PASS)) {
            if (canDoOperates
                    .stream()
                    .noneMatch(canDoOperate -> canDoOperate.getUserId() == userId
                            && canDoOperate
                            .getOperates()
                            .stream()
                            .anyMatch(operate -> operate.getActionType() == actionType)

                    )) {
                throw new RuntimeException(String.format("该玩家不能执行%s操作", actionType.getName()));
            }
        } else {
            CanDoOperate canDoOperate = canDoOperates.get(0);

            if (canDoOperate.getUserId() != userId) {
                throw new RuntimeException("未轮到该玩家操作");
            }

            if (canDoOperate.getOperates().stream().noneMatch(operate -> operate.getActionType() == actionType)) {
                throw new RuntimeException(String.format("该玩家不能执行%s操作", actionType.getName()));
            }
        }

        return true;
    }

    /**
     * 处理正常摸牌
     *
     * @param userId 当前请求api的用户
     */
    public void handleCommonNextUserTouchAMahjong(MahjongGameData mahjongGameData, int userId) throws Exception {
        // 一个玩家出牌后，轮到下一个玩家摸牌
        int nextUserId = getNextTouchMahjongUserId(mahjongGameData, userId);

        handleCommonUserTouchAMahjong(mahjongGameData, nextUserId);
    }


    /**
     * 处理正常摸牌
     *
     * @param userId 需要摸牌的用户
     */
    @SuppressWarnings("unchecked")
    public void handleCommonUserTouchAMahjong(MahjongGameData mahjongGameData, int userId) throws Exception {
        // 是否平局（没有牌可以摸）
        boolean isDraw = isDraw(mahjongGameData);
        if (isDraw) {
            SettlementService.getInstance().settlement(mahjongGameData, Collections.EMPTY_LIST);
            return;
        }

        Mahjong touchMahjong = touchAMahjong(mahjongGameData);
        PersonalMahjongInfo myInfo = PersonalMahjongInfo.getMyInfo(mahjongGameData.getPersonalMahjongInfos(), userId);
        myInfo.setTouchMahjong(touchMahjong);


        // todo 添加玩家执行的动作，用于回放
        // mahjongGameData.addAction(Action.newAct(mahjongGameData.getCanDoOperates().get(0)));

        // 广播用户摸牌
        MessageUtil.broadcastPlayerAction(mahjongGameData, userId, touchMahjong.getCode(), ActionType.GET_A_MAHJONG);

        // 扫描用户是否有胡、暗杠操作
        List<CanDoOperate> canDoOperates = getACardScanner.scan(mahjongGameData, touchMahjong, userId);

        if (TrusteeshipService.isTrusteeship(userId)) {
            if (canDoOperates.isEmpty()) {
                AutoPlayACardTask task = new AutoPlayACardTask(userId, mahjongGameData);
                TrusteeshipService.trusteeshipTask(userId, 2, task);
            } else {
                AutoPassTask task = new AutoPassTask(userId, mahjongGameData);
                TrusteeshipService.trusteeshipTask(userId, 2, task);
            }
        }

        mahjongGameData.setCanDoOperates(canDoOperates);
        mahjongGameDataDao.save(mahjongGameData);

        // 推送用户操作
        MessageUtil.pushPlayerActionTips(mahjongGameData);
    }


    /**
     * 本轮玩家已经出牌，获取下一个出牌的玩家
     *
     * @param currentUserId 本轮已经出牌的玩家id
     */
    public int getNextTouchMahjongUserId(MahjongGameData mahjongGameData, int currentUserId) {
        // 本轮已经出牌的座位号
        int userSeat = 0;
        for (int i = 0; i < mahjongGameData.getPersonalMahjongInfos().size(); i++) {
            PersonalMahjongInfo info = mahjongGameData.getPersonalMahjongInfos().get(i);
            if (info.getUserId() == currentUserId) {
                userSeat = RoomPlayerDao.getInstance().loadRoomPlayerByPlayerId(currentUserId).getSeat();
                break;
            }
        }

        // 获取下一个座位号
        int nextSeat = userSeat + 1;
        // 如果座位号next大于玩家人数，则座位号改为1，从头开始
        if (nextSeat > mahjongGameData.getPersonalMahjongInfos().size()) {
            nextSeat = 1;
        }

        for (int i = 0; i < mahjongGameData.getPersonalMahjongInfos().size(); i++) {
            PersonalMahjongInfo info = mahjongGameData.getPersonalMahjongInfos().get(i);
            RoomPlayer roomPlayer = RoomPlayerDao.getInstance().loadRoomPlayerByPlayerId(info.getUserId());
            if (roomPlayer.getSeat() == nextSeat) {
                return roomPlayer.getPlayerId();
            }
        }

        throw new RuntimeException("找不到下一个座位的玩家");
    }

    /**
     * 判断是否平局
     */
    private boolean isDraw(MahjongGameData mahjongGameData)
            throws ClassNotFoundException, IllegalAccessException, InstantiationException {
        Draw draw = (Draw) Class.forName(MahjongConfig.drawClass).newInstance();
        return draw.isDraw(mahjongGameData);
    }

    /**
     * 如果全部人都已准备，则开始游戏，发牌
     */
    public MahjongGameData isAllReadyAndStartGame(RoomPlayer roomPlayer) throws Exception {
        // 判断是否全部人都已准备
        if (!isAllReady(roomPlayer.getRoomId())) {
            return null;
        }

        // 获取房间数据，初始化游戏数据
        Room room = roomDataService.getRoomByRoomId(roomPlayer.getRoomId());
        List<RoomPlayer> readyPlayers = roomDataService.loadRoomPlayerListByState(room.getId(),
                RoomPlayer.State.READY.getCode());

        Collections.sort(readyPlayers);

        MahjongGameData oldMahjongGameDate = mahjongGameDataDao.get(room.getId());

        int nextRound = genNextRound(oldMahjongGameDate);
        MahjongGameData mahjongGameData = BaseGameService.getInstance().initGameData(
                roomPlayer.getRoomId(),
                room.getNeedPlayerNum(),
                nextRound,
                room.getRounds(),
                oldMahjongGameDate == null ? 1 : oldMahjongGameDate.getNextBankerSite(),
                oldMahjongGameDate != null && oldMahjongGameDate.isSpend()
        );

        for (int i = 0; i < room.getNeedPlayerNum(); i++) {
            RoomPlayer player = readyPlayers.get(i);
            PersonalMahjongInfo personalMahjongInfo = mahjongGameData.getPersonalMahjongInfos().get(i);
            personalMahjongInfo.setUserId(player.getPlayerId());
        }

        // 庄家摸牌后进行可操作扫描
        List<CanDoOperate> canDoOperates = getACardScanner.scan(
                mahjongGameData,
                mahjongGameData.getPersonalMahjongInfos().get(mahjongGameData.getBankerSite() - 1).getTouchMahjong(),
                mahjongGameData.getPersonalMahjongInfos().get(mahjongGameData.getBankerSite() - 1).getUserId()
        );

        mahjongGameData.setCanDoOperates(canDoOperates);
        mahjongGameDataDao.save(mahjongGameData);

        // 设置房间为游戏开始状态
        if (room.getState() != Room.State.STARTED.getCode()) {
            room.setState(Room.State.STARTED);
            RoomDao.getInstance().update(room);
        }

        roomDataService.setStartNewRound(room.getId(), nextRound);

        mahjongGameData.addAction(Action.newActTips(mahjongGameData.getCanDoOperates().get(0)));

        return mahjongGameData;
    }


    public int getSeatByUserId(List<RoomPlayer> roomPlayers, int userId) {
        for (RoomPlayer roomPlayer : roomPlayers) {
            if (roomPlayer.getPlayerId() == userId) {
                return roomPlayer.getSeat();
            }
        }
        LogUtil.getLogger().warn("无法根据userId找到座位号,[roomPlayers:{}][userId:{}]", roomPlayers, userId);
        return 1;
    }

    public int getUserIdBySeat(List<RoomPlayer> roomPlayers, int seat) {
        for (RoomPlayer roomPlayer : roomPlayers) {
            if (roomPlayer.getSeat() == seat) {
                return roomPlayer.getPlayerId();
            }
        }
        return -1;
    }

    /**
     * 生成下一局的局数
     *
     * @param oldMahjongGameDate 上一局的游戏数据
     */
    private int genNextRound(MahjongGameData oldMahjongGameDate) {
        int newTimes = 1;
        if (oldMahjongGameDate == null) {
            return newTimes;
        }

        // 流局，局数不变
        if (CollectionUtils.isEmpty(oldMahjongGameDate.getLastWinCanDoOperate())) {
            return oldMahjongGameDate.getCurrentTimes();
        }

        int winnerUserId;

        // 一炮多响，赢家为放炮的人
        if (oldMahjongGameDate.getLastWinCanDoOperate().size() > 1) {
            winnerUserId = oldMahjongGameDate.getLastWinCanDoOperate().get(0).getSpecialUserId();
        } else {
            winnerUserId = oldMahjongGameDate.getLastWinCanDoOperate().get(0).getUserId();
        }

        List<RoomPlayer> roomPlayers = RoomDataService
                .getInstance()
                .loadRoomPlayerListByState(oldMahjongGameDate.getRoomId(), RoomPlayer.State.READY.getCode());
        int lastBankerUserId = getUserIdBySeat(roomPlayers, oldMahjongGameDate.getBankerSite());

        if (winnerUserId == lastBankerUserId) {
            return oldMahjongGameDate.getCurrentTimes();
        } else {
            return oldMahjongGameDate.getCurrentTimes() + 1;
        }
    }

}
