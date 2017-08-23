package avatar.util;

import avatar.entity.battlerecord.BattleScoreEntity;
import avatar.entity.room.Room;
import avatar.module.mahjong.Combo;
import avatar.module.mahjong.Mahjong;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.PersonalMahjongInfo;
import avatar.module.mahjong.operate.ActionType;
import avatar.module.mahjong.operate.CanDoOperate;
import avatar.module.mahjong.replay.Action;
import avatar.module.room.service.RoomDataService;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import org.apache.commons.collections.CollectionUtils;

import java.util.List;

/**
 * 发送通用的vo到客户端
 */
public class MessageUtil {

    /**
     * 广播用户执行的操作
     *
     * @param userId      执行操作的用户Id
     * @param mahjongCode 执行操作的麻将
     * @param actionType  执行操作的类型
     */
    public static void broadcastPlayerAction(MahjongGameData mahjongGameData, int userId, int mahjongCode, ActionType actionType) {
        broadcastPlayerAction(mahjongGameData, userId, 0, mahjongCode, actionType);
    }

    /**
     * 广播用户执行的操作
     *
     * @param userId      执行操作的用户Id
     * @param mahjongCode 执行操作的麻将
     * @param actionType  执行操作的类型
     */
    public static void broadcastPlayerAction(MahjongGameData mahjongGameData, int userId, int target, int mahjongCode, ActionType actionType) {
        mahjongGameData.addAction(Action.newAct(userId, target, mahjongCode, actionType));

        Battle.PushPlayerActS2C pushPlayerActS2C = OperateUtil.parsePlayerActionToPushPlayerActS2C(userId, target, actionType, mahjongCode);
        byte[] bytes = pushPlayerActS2C.toByteArray();
        List<Integer> roomUserIds = RoomDataService.getInstance().getRoomUserIds(mahjongGameData.getRoomId());
        for (Integer roomUserId : roomUserIds) {
            Session sessionByUserId = GameData.getSessionManager().getSessionByUserId(roomUserId);
            if (sessionByUserId != null) {
                LogUtil.getLogger().debug("S2C_ROOM_PLAYER_ACT_BROADCAST:{} receiveUserId={}\n{}",
                        Cmd.S2C_ROOM_PLAYER_ACT_BROADCAST,
                        roomUserId,
                        pushPlayerActS2C.toString()
                );
                sessionByUserId.sendClient(Cmd.S2C_ROOM_PLAYER_ACT_BROADCAST, bytes);
            }
        }
    }

    /**
     * 广播单局结算
     */
    public static void broadcastCurrentRoundScore(MahjongGameData mahjongGameData,
                                                  List<CanDoOperate> winnerCanDoOperates,
                                                  List<BattleScoreEntity> scores) {
        Battle.PushMatchResultS2C.Builder broadcastBuilder = Battle.PushMatchResultS2C.newBuilder();

        for (BattleScoreEntity score : scores) {
            if (score.isWinner()) {
                broadcastBuilder.addHuUserId(score.getPlayerId());
            }

            if (score.getWinOrLose() == BattleScoreEntity.WinOrLose.DIAN_PAO.getId()) {
                broadcastBuilder.setHuedUserId(score.getPlayerId());
            } else if (score.getWinOrLose() == BattleScoreEntity.WinOrLose.JIE_PAO.getId()) {
                broadcastBuilder.setJiePaoUserId(score.getPlayerId());
            } else if (score.getWinOrLose() == BattleScoreEntity.WinOrLose.ZI_MO.getId()) {
                broadcastBuilder.setZiMoUserId(score.getPlayerId());
            }

            PersonalMahjongInfo myInfo = PersonalMahjongInfo.getMyInfo(mahjongGameData.getPersonalMahjongInfos(), score.getPlayerId());

            Battle.PlayerMatchResultVOS2C.Builder playerInfoBuilder = Battle.PlayerMatchResultVOS2C.newBuilder()
                    .setUserId(score.getPlayerId())
                    .setAddScore(score.getScore())
                    .addAllHandCards(Mahjong.parseToCodes(myInfo.getHandCards()))
                    .setAnGangCount(score.getAnGangTimes())
                    .setMingGangCount(score.getMingGangTimes());
            if (score.isWinner() && CollectionUtils.isNotEmpty(winnerCanDoOperates)) {
                playerInfoBuilder.addHandCards(winnerCanDoOperates.get(0).getSpecialMahjong().getCode());
                playerInfoBuilder.setHuDesc(score.getHuName());
            }


            for (Combo combo : myInfo.getGangs()) {
                Battle.PengGangCardVO.Builder cardBuilder = Battle.PengGangCardVO.newBuilder();
                cardBuilder.addAllPengGangCards(Mahjong.parseToCodes(combo.getMahjongs()))
                        .setTargetUserId(combo.getTargetUserId());
                playerInfoBuilder.addPengGangs(cardBuilder);
            }
            for (Combo combo : myInfo.getPengs()) {
                Battle.PengGangCardVO.Builder cardBuilder = Battle.PengGangCardVO.newBuilder();
                cardBuilder.addAllPengGangCards(Mahjong.parseToCodes(combo.getMahjongs()))
                        .setTargetUserId(combo.getTargetUserId());
                playerInfoBuilder.addPengGangs(cardBuilder);
            }


            broadcastBuilder.addResultInfos(playerInfoBuilder);

            // 设置点炮的牌
            if (CollectionUtils.isNotEmpty(winnerCanDoOperates)) {
                broadcastBuilder.setHuedCard(winnerCanDoOperates.get(0).getSpecialMahjong().getCode());
            }

        }

        Battle.PushMatchResultS2C pushMatchResultS2C = broadcastBuilder.build();
        byte[] bytes = pushMatchResultS2C.toByteArray();
        List<Integer> roomUserIds = RoomDataService.getInstance().getRoomUserIds(mahjongGameData.getRoomId());
        for (Integer roomUserId : roomUserIds) {
            Session sessionByUserId = GameData.getSessionManager().getSessionByUserId(roomUserId);
            if (sessionByUserId != null) {
                LogUtil.getLogger().debug("S2C_ROOM_SINGLE_SCORE_BROADCAST:{} receiveUserId={}\n{}",
                        Cmd.S2C_ROOM_SINGLE_SCORE_BROADCAST,
                        roomUserId,
                        pushMatchResultS2C.toString()
                );
                sessionByUserId.sendClient(Cmd.S2C_ROOM_SINGLE_SCORE_BROADCAST, bytes);
            }
        }
    }

    /**
     * 广播总结算
     */
    public static void broadcastTotalScore(List<BattleScoreEntity> scores) {
        Battle.PushRoomResultS2C.Builder pushRoomResultS2CBuilder = Battle.PushRoomResultS2C.newBuilder();
        for (BattleScoreEntity score : scores) {
            pushRoomResultS2CBuilder.addResultInfos(Battle.PlayerRoomResultVOS2C.newBuilder()
                    .setUserId(score.getPlayerId())
                    .setSelfHuCount(score.getIsZiMo())
                    .setOtherHuCount(score.getJiePaoUserId())
                    .setSendPaoCount(score.getDianPaoUserId())
                    .setAnGangCount(score.getAnGangTimes())
                    .setMingGangCount(score.getMingGangTimes())
                    .setAddScore(score.getScore()).build());
        }

        Battle.PushRoomResultS2C pushRoomResultS2C = pushRoomResultS2CBuilder.build();
        byte[] bytes = pushRoomResultS2C.toByteArray();
        for (BattleScoreEntity score : scores) {
            Session mySession = GameData.getSessionManager().getSessionByUserId(score.getPlayerId());
            if (mySession != null) {
                LogUtil.getLogger().debug("S2C_ROOM_TOTAL_SCORE_BROADCAST:{} receiveUserId={}\n{}",
                        Cmd.S2C_ROOM_TOTAL_SCORE_BROADCAST,
                        score.getPlayerId(),
                        pushRoomResultS2C.toString()
                );
                mySession.sendClient(Cmd.S2C_ROOM_TOTAL_SCORE_BROADCAST, bytes);
            }
        }


    }

    /**
     * 推送操作提示给玩家
     */
    public static void pushPlayerActionTips(MahjongGameData mahjongGameData) {
        pushPlayerActionTips(mahjongGameData.getCanDoOperates().get(0));
        mahjongGameData.addAction(Action.newActTips(mahjongGameData.getCanDoOperates().get(0)));
    }

    /**
     * 推送操作提示给玩家
     */
    public static void pushPlayerActionTips(CanDoOperate canDoOperate) {
        Battle.PushPlayerActTipS2C actTips = OperateUtil.parseCanDoOperateToPushPlayerActTipS2C(canDoOperate);

        Room room = RoomDataService.getInstance().getInRoomByUserId(canDoOperate.getUserId());
        List<Integer> roomUserIds = RoomDataService.getInstance().getRoomUserIds(room.getId());
        for (Integer roomUserId : roomUserIds) {
            Session mySession = GameData.getSessionManager().getSessionByUserId(roomUserId);
            if (mySession != null) {
                LogUtil.getLogger().debug("S2C_ROOM_PLAYER_ACT_TIP_BROADCAST:{} receiveUserId={}\n{}",
                        Cmd.S2C_ROOM_PLAYER_ACT_TIP_BROADCAST,
                        canDoOperate.getUserId(),
                        actTips.toString()
                );
                mySession.sendClient(Cmd.S2C_ROOM_PLAYER_ACT_TIP_BROADCAST, actTips.toByteArray());
            }
        }
    }
}
