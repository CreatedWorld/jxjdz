package avatar.apiregister.enterroom;

import avatar.entity.battlerecord.UserBattleScoreEntity;
import avatar.entity.room.Room;
import avatar.entity.room.RoomPlayer;
import avatar.entity.userinfo.UserEntity;
import avatar.facade.SystemEventHandler;
import avatar.module.battleRecord.dao.UserBattleScoreDao;
import avatar.module.mahjong.*;
import avatar.module.mahjong.dao.MahjongGameDataDao;
import avatar.module.mahjong.offline.OfflineManager;
import avatar.module.mahjong.service.ListenCardService;
import avatar.module.room.dao.RoomDao;
import avatar.module.room.service.RoomDataService;
import avatar.module.user.UserDao;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import avatar.util.LogUtil;
import avatar.util.OperateUtil;
import avatar.module.trusteeship.TrusteeshipService;
import org.apache.commons.collections.CollectionUtils;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 房间服务器处理玩家加入房间消息
 */
@Service
public class JoinRoomApi extends SystemEventHandler<Battle.JoinRoomC2S, Session> {

    protected JoinRoomApi() {
        super(Cmd.C2S_ROOM_JOIN_ROOM);
    }

    @Override
    public void method(Session session, Battle.JoinRoomC2S msg) throws Exception {
        if (session == null) {
            return;
        }
        int userId = session.getUserId();

        OfflineManager.removeOfflineUser(userId);

        TrusteeshipService.removeTrusteeshipUser(userId);

        // 获取房间号
        String roomCode = msg.getRoomCode();
        Room room = RoomDao.getInstance().loadRoomByRoomCode(roomCode);
        MahjongGameData mahjongGameData = MahjongGameDataDao.getInstance().get(room.getId());

        Battle.JoinRoomS2C.Builder builder = Battle.JoinRoomS2C.newBuilder();
        builder.setRoomCode(roomCode);
        builder.setInnings(room.getRounds());
        builder.setCurInnings(room.getCurRounds());

        builder.setCreateId(room.getCreateUserId());
        builder.addAllPlayType(room.getPlayTypeList());

        List<RoomPlayer> roomPlayers = RoomDataService.getInstance().loadAllInRoom(room.getId());
        boolean isPlaying;
        isPlaying = roomPlayers.size() >= room.getNeedPlayerNum()
                && roomPlayers.stream().allMatch(p -> p.getState() == RoomPlayer.State.READY.getCode());

        builder.setIsStart(isPlaying);
        if (isPlaying) {
            builder.setLeftCardCount(mahjongGameData.getLeftCards().size());
            if (room.getTime() != null) {
                builder.setStartTime(room.getTime().getTime());
            }

            // 添加申请解散房间的信息
            List<MahjongGameData.ApplyDissolveData> applyDissolveList = mahjongGameData.getApplyDissolveList();
            if (CollectionUtils.isNotEmpty(applyDissolveList)) {
                builder.setHasDisloveApply(true);
                for (MahjongGameData.ApplyDissolveData applyDissolveData : applyDissolveList) {
                    builder.addAgreeIds(applyDissolveData.getUserId());
                    builder.setDisloveRemainTime(MahjongConfig.DISSOLVE_REMAIN_TIME);
                    builder.setDisloveRemainUT(applyDissolveList.get(0).getStartTime().getTime());
                }
            }

            // 添加听牌
            List<Mahjong> listenCards = ListenCardService.getInstance().findListenCards(mahjongGameData, userId);
            builder.addAllTingCards(Mahjong.parseToCodes(listenCards));
        } else {
            builder.setLeftCardCount(0);
        }

        if (CollectionUtils.isNotEmpty(roomPlayers)) {
            for (RoomPlayer roomPlayer : roomPlayers) {

                UserEntity userEntity = UserDao.getInstance().loadUserEntityByUserId(roomPlayer.getPlayerId());

                Battle.PlayerInfoVOS2C.Builder playerInfoVOS2CBuilder = Battle.PlayerInfoVOS2C.newBuilder()
                        .setUserId(roomPlayer.getPlayerId())
                        .setSit(roomPlayer.getSeat())
                        .setName(userEntity.getNickName())
                        .setIsMaster(roomPlayer.getSeat() == 1)
                        .setHeadIcon(userEntity.getImageUrl())
                        .setSex(userEntity.getSex())
                        .setIsReady(roomPlayer.getState() == RoomPlayer.State.READY.getCode())
                        .setIsOnline(!OfflineManager.isOffline(roomPlayer.getPlayerId()));


                // 断线重连，游戏已经开始，则把游戏数据也返回给客户端
                if (room.getState() == Room.State.STARTED.getCode()) {

                    PersonalMahjongInfo myInfo = PersonalMahjongInfo.getMyInfo(
                            mahjongGameData.getPersonalMahjongInfos(),
                            roomPlayer.getPlayerId()
                    );

                    UserBattleScoreEntity score = UserBattleScoreDao
                            .getInstance()
                            .getUserBattleScoreByRoomIdAndPlayerId(room.getId(), roomPlayer.getPlayerId());

                    if (score != null) {
                        playerInfoVOS2CBuilder.setScore(score.getAllScore());
                    }


                    playerInfoVOS2CBuilder.setIsBanker(mahjongGameData.getBankerSite() == roomPlayer.getSeat());

                    if (isPlaying) {
                        playerInfoVOS2CBuilder.addAllHandCards(Mahjong.parseToCodes(myInfo.getHandCards()))
                                .addAllPutCards(Mahjong.parseToCodes(myInfo.getOutMahjong()));

                        if (myInfo.getTouchMahjong() != null) {
                            playerInfoVOS2CBuilder.setGetCard(myInfo.getTouchMahjong().getCode());
                        }

                        if (CollectionUtils.isNotEmpty(myInfo.getPengs()) || CollectionUtils.isNotEmpty(myInfo.getGangs())) {
                            for (Combo combo : myInfo.getPengs()) {
                                Battle.PengGangCardVO.Builder PengGangCardVOBuilder = Battle.PengGangCardVO.newBuilder();
                                PengGangCardVOBuilder.addAllPengGangCards(Mahjong.parseToCodes(combo.getMahjongs()))
                                        .setTargetUserId(combo.getTargetUserId());
                                playerInfoVOS2CBuilder.addPengGangCards(PengGangCardVOBuilder);
                            }
                            for (Combo combo : myInfo.getGangs()) {
                                Battle.PengGangCardVO.Builder PengGangCardVOBuilder = Battle.PengGangCardVO.newBuilder();
                                PengGangCardVOBuilder.addAllPengGangCards(Mahjong.parseToCodes(combo.getMahjongs()))
                                        .setTargetUserId(combo.getTargetUserId());
                                playerInfoVOS2CBuilder.addPengGangCards(PengGangCardVOBuilder);
                            }
                        }

                        if (CollectionUtils.isNotEmpty(mahjongGameData.getCanDoOperates())) {
                            if (CollectionUtils.isNotEmpty(mahjongGameData.getMultipleChiHus())) {
                                for (MultipleChiHu multipleChiHu : mahjongGameData.getMultipleChiHus()) {
                                    if (multipleChiHu.getCanDoOperate().getUserId() == userId
                                            && multipleChiHu.getSelect() == null) {
                                        Battle.PushPlayerActTipS2C actTips = OperateUtil.parseCanDoOperateToPushPlayerActTipS2C(
                                                multipleChiHu.getCanDoOperate()
                                        );
                                        builder.setPlayerTipAct(actTips);
                                    }
                                }
                            } else {
                                Battle.PushPlayerActTipS2C actTips = OperateUtil.parseCanDoOperateToPushPlayerActTipS2C(
                                        mahjongGameData.getCanDoOperates().get(0)
                                );
                                builder.setPlayerTipAct(actTips);
                            }
                        }
                    }
                }
                Battle.PlayerInfoVOS2C playerInfoVOS2C = playerInfoVOS2CBuilder.build();
                builder.addPlayInfoArr(playerInfoVOS2C);
            }
        }

        LogUtil.getLogger().debug("S2C_ROOM_JOIN_ROOM:{} receiveUserId={}\n{}"
                , Cmd.S2C_ROOM_JOIN_ROOM
                , userId
                , builder.build().toString()
        );
        session.sendClient(Cmd.S2C_ROOM_JOIN_ROOM, builder.build().toByteArray());
    }

    private static Map<Integer, List<Mahjong>> map = new HashMap<>();
    private static Mahjong mahjong;

    private static void initMahjong(int userId) {
        List<Mahjong> list = Mahjong.getAllMahjongs();
        for (int i = 0; i < 4; i++) {
            List<Mahjong> userMahjong = new ArrayList<>();
            for (int j = 0; j < 13; j++) {
                userMahjong.add(list.remove(j));
            }
            int id = 100 + i;
            if (i == 3) {
                id = userId;
                mahjong = list.remove(0);
                userMahjong.add(mahjong);
            }
            map.put(id, userMahjong);
        }
    }

}
