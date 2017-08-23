package avatar.apiregister.room;

import avatar.entity.room.Room;
import avatar.entity.room.RoomPlayer;
import avatar.facade.SystemEventHandler;
import avatar.module.exception.RoomPlayerStateNotMatchException;
import avatar.module.exception.UserNotInRoomException;
import avatar.module.mahjong.Mahjong;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.PersonalMahjongInfo;
import avatar.module.mahjong.service.BaseGameService;
import avatar.module.room.dao.RoomPlayerDao;
import avatar.module.room.service.RoomDataService;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import avatar.util.GameData;
import avatar.util.LogUtil;
import avatar.util.OperateUtil;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

/**
 * 准备
 */
@Service
public class ReadyApi extends SystemEventHandler<Battle.ReadyC2S, Session> {
    protected ReadyApi() {
        super(Cmd.C2S_ROOM_READY);
    }

    private static final BaseGameService baseGameService = BaseGameService.getInstance();

    private static final RoomDataService roomDataService = RoomDataService.getInstance();

    @Override
    public void method(Session session, Battle.ReadyC2S msg) throws Exception {
        int userId = session.getUserId();
        System.out.println("========specifiedUserId = " + userId);

        // 个人准备
        RoomPlayer roomPlayer = RoomPlayerDao.getInstance().loadRoomPlayerByPlayerId(userId);
        if (roomPlayer == null) {
            throw new UserNotInRoomException(String.format("玩家[user=%s]不在房间中，无法执行准备接口！", userId));
        }

        if (roomPlayer.getState() != RoomPlayer.State.UNREADY.getCode()) {
            throw new RoomPlayerStateNotMatchException(
                    String.format(
                            "玩家的状态为[%s],无法执行准备接口！",
                            RoomPlayer.State.parse(roomPlayer.getState()).getName()
                    )
            );
        }

        roomPlayer.setState(RoomPlayer.State.READY.getCode());
        roomPlayer.setTime();
        RoomPlayerDao.getInstance().update(roomPlayer);

        // 准备广播
        Battle.PushReadyS2C readyS2C = Battle.PushReadyS2C.newBuilder()
                .setUserId(userId)
                .build();
        byte[] bytes = readyS2C.toByteArray();
        List<Integer> roomUserIds = roomDataService.getRoomUserIds(roomPlayer.getRoomId());
        for (Integer roomUserId : roomUserIds) {
            Session sessionByUserId = GameData.getSessionManager().getSessionByUserId(roomUserId);
            if (sessionByUserId != null) {
                sessionByUserId.sendClient(Cmd.S2C_ROOM_READY_BROADCAST, bytes);
            }

        }

        // 游戏是否开始
        MahjongGameData mahjongGameData = baseGameService.isAllReadyAndStartGame(roomPlayer);
        if (mahjongGameData != null) {
            Room room = roomDataService.getRoomByRoomId(mahjongGameData.getRoomId());

            // 庄家userId
            int bankerUserId = 0;
            List<RoomPlayer> roomPlayers = RoomDataService.getInstance()
                    .loadRoomPlayerListByState(mahjongGameData.getRoomId(), RoomPlayer.State.READY.getCode());
            for (RoomPlayer player : roomPlayers) {
                if (player.getSeat() == mahjongGameData.getBankerSite()) {
                    bankerUserId = player.getPlayerId();
                }
            }

            // 掷骰
            List<Integer> dices = Arrays.asList(mahjongGameData.getDices());

            for (PersonalMahjongInfo info : mahjongGameData.getPersonalMahjongInfos()) {
                List<Integer> handCards = new ArrayList<>(info.getHandCards().size());
                for (Mahjong mahjong : info.getHandCards()) {
                    handCards.add(mahjong.getCode());
                }

                Battle.PushPlayerActTipS2C actTips = null;
                if (!mahjongGameData.getCanDoOperates().isEmpty()) {
                    actTips = OperateUtil.parseCanDoOperateToPushPlayerActTipS2C(mahjongGameData.getCanDoOperates().get(0));
                }

                Session userSession = GameData.getSessionManager().getSessionByUserId(info.getUserId());

                Integer touchedMahjongCode = info.getTouchMahjong() != null ? info.getTouchMahjong().getCode() : null;

                Battle.GameStart_S2C gameStartS2C = Battle.GameStart_S2C.newBuilder()
                        .setBankerUserId(bankerUserId)
                        .addAllDices(dices)
                        .setCurrentTimes(mahjongGameData.getCurrentTimes())
                        .addAllHandCards(handCards)
                        .setTouchMahjongCode(touchedMahjongCode == null ? 0 : touchedMahjongCode)
                        .setLeftCardCount(mahjongGameData.getLeftCards().size())
                        .setPushPlayerActTipS2C(actTips)
                        .setStartTime(room.getTime() == null ? 0 : room.getTime().getTime())
                        .build();


                LogUtil.getLogger().debug("S2C_ROOM_GAME_START_BROADCAST:{} receiveUserId={}\n{}",
                        Cmd.S2C_ROOM_JOIN_ROOM,
                        info.getUserId(),
                        gameStartS2C.toString()
                );
                if (userSession != null) {
                    userSession.sendClient(
                            Cmd.S2C_ROOM_GAME_START_BROADCAST,
                            gameStartS2C.toByteArray()
                    );
                }
            }
        }
    }

}
