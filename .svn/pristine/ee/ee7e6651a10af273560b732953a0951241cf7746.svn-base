package avatar.module.mahjong.service;

import avatar.entity.battlerecord.BattleScoreEntity;
import avatar.entity.room.Room;
import avatar.entity.room.RoomPlayer;
import avatar.entity.roomType.RoomType;
import avatar.entity.userinfo.UserEntity;
import avatar.protobuf.Cmd;
import avatar.module.battleRecord.dao.BattleScoreDao;
import avatar.module.battleRecord.service.BattleRecordService;
import avatar.module.item.ItemService;
import avatar.module.mahjong.*;
import avatar.module.mahjong.action.Operated;
import avatar.module.mahjong.operate.ActionType;
import avatar.module.mahjong.operate.CanDoOperate;
import avatar.module.mahjong.operate.Operate;
import avatar.module.mahjong.score.Calculator;
import avatar.module.mahjong.score.TotalCalculator;
import avatar.module.room.service.RoomDataService;
import avatar.module.roomType.RoomTypeService;
import avatar.module.user.UserDao;
import avatar.protobuf.Cmd;
import avatar.util.MessageUtil;
import com.alibaba.fastjson.JSON;
import org.apache.commons.collections.CollectionUtils;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;

/**
 * 结算服务
 */
public class SettlementService {

    private static final SettlementService instance = new SettlementService();

    private static final BaseGameService baseGameService = BaseGameService.getInstance();

    public static SettlementService getInstance() {
        return instance;
    }

    /**
     * 单局结算
     *
     * @param winnerCanDoOperates 胡牌的玩家操作
     */
    public void settlement(MahjongGameData mahjongGameData, List<CanDoOperate> winnerCanDoOperates)
            throws IllegalAccessException, InstantiationException, ClassNotFoundException {
        Room room = RoomDataService.getInstance().getRoomByRoomId(mahjongGameData.getRoomId());
        List<Integer> playTypeList = room.getPlayTypeList();
        List<Integer> roomUserIds = RoomDataService.getInstance().getRoomUserIds(room.getId());
        // 消耗资源
        RoomType roomType = RoomTypeService.getInstance().getRoomTypeByType(room.getRoomType());
        if (roomType.getIsCost() == 1 && !mahjongGameData.isSpend()) {
            if (playTypeList.contains(15)){//房主扣
                ItemService.getInstance().costItem(room.getCreateUserId(), roomType.getItemTypeId(),
                        roomType.getNeedScore());
                mahjongGameData.setSpend(true);

            }else {//AA扣
                for ( int userId : roomUserIds) {
                    ItemService.getInstance().costItem(userId, roomType.getItemTypeId(),roomType.getNeedScore()/roomUserIds.size());
                    mahjongGameData.setSpend(true);
                }
            }

        }

        addOperated(mahjongGameData, winnerCanDoOperates);

        List<BattleScoreEntity> battleScores = genBattleScores(mahjongGameData, winnerCanDoOperates);

        // 设置下一局庄家
        setNextBankerSite(mahjongGameData, winnerCanDoOperates);

        // 保存战绩和回放数据
        BattleRecordService.getInstance().saveBattleRecord(
                mahjongGameData.getRoomId(),
                mahjongGameData.getCurrentTimes(),
                JSON.toJSONString(mahjongGameData.getReplay()),
                battleScores
        );

        // 广播单局结算
        MessageUtil.broadcastCurrentRoundScore(mahjongGameData, winnerCanDoOperates, battleScores);

        mahjongGameData.setLastWinCanDoOperate(new ArrayList<>(winnerCanDoOperates));

        mahjongGameData.getCanDoOperates().clear();

        // 下一局游戏
        BaseGameService.getInstance().ready4NextGameOrFinishGame(mahjongGameData, room);
    }

    /**
     * 设置下一局庄家
     *
     * @param room
     * @param winnerCanDoOperates 胡牌的玩家操作
     */
    private void setNextBankerSite(MahjongGameData mahjongGameData, List<CanDoOperate> winnerCanDoOperates) {
        Room room = RoomDataService.getInstance().getRoomByRoomId(mahjongGameData.getRoomId());

        // 如果是只有一个人胡牌（自摸或吃胡），则胡牌的玩家坐庄
        // 如果无人胡牌，则庄家的下家坐庄
        // 如果一炮多响，点炮的人做庄
        int nextBankerSite;
        if (CollectionUtils.isEmpty(winnerCanDoOperates)) {
            int currentBankerSeat = mahjongGameData.getBankerSite();
            if (currentBankerSeat == room.getNeedPlayerNum()) {
                nextBankerSite = 1;
            } else {
                nextBankerSite = ++currentBankerSeat;
            }
        } else if (winnerCanDoOperates.size() == 1) {
            List<RoomPlayer> roomPlayers = RoomDataService.getInstance()
                    .loadRoomPlayerListByState(mahjongGameData.getRoomId(), RoomPlayer.State.READY.getCode());
            nextBankerSite = baseGameService.getSeatByUserId(roomPlayers, winnerCanDoOperates.get(0).getUserId());
        } else {
            List<RoomPlayer> roomPlayers = RoomDataService.getInstance()
                    .loadRoomPlayerListByState(mahjongGameData.getRoomId(), RoomPlayer.State.READY.getCode());
            nextBankerSite = baseGameService.getSeatByUserId(roomPlayers, winnerCanDoOperates.get(0).getSpecialUserId());
        }

        mahjongGameData.setNextBankerSite(nextBankerSite);
    }

    /**
     * 生成单局结算
     * 自摸、3种抢杠、吃胡、流局的情况，都要考虑
     *
     * @param winnerCanDoOperates 胡牌的玩家
     */
    public List<BattleScoreEntity> genBattleScores(MahjongGameData mahjongGameData, List<CanDoOperate> winnerCanDoOperates)
            throws ClassNotFoundException, IllegalAccessException, InstantiationException {
        List<BattleScoreEntity> scores = new ArrayList<>(mahjongGameData.getPersonalMahjongInfos().size());

        Date now = new Date();

        // 找到胡牌的具体类型
        Operate operate = null;
        if (CollectionUtils.isNotEmpty(winnerCanDoOperates) && !winnerCanDoOperates.get(0).getOperates().isEmpty()) {
            operate = winnerCanDoOperates.get(0).getOperates().get(0);
        }

        // 胡牌的玩家id
        List<Integer> winnerUserIds = new ArrayList<>(winnerCanDoOperates.size());
        for (CanDoOperate canDoOperate : winnerCanDoOperates) {
            winnerUserIds.add(canDoOperate.getUserId());
        }

        // 生成每个胡牌玩家的BattleScoreEntity
        for (PersonalMahjongInfo personalMahjongInfo : mahjongGameData.getPersonalMahjongInfos()) {
            UserEntity user = UserDao.getInstance().loadUserEntityByUserId(personalMahjongInfo.getUserId());

            BattleScoreEntity score = new BattleScoreEntity();
            score.setRoomId(mahjongGameData.getRoomId());
            score.setCurrentTimes(mahjongGameData.getCurrentTimes());
            score.setPlayerId(personalMahjongInfo.getUserId());
            score.setPlayerNickName(user.getNickName());
            score.setHuTime(now);

            // 暗杠、明杠次数
            int anGangTimes = 0;
            int mingGangTimes = 0;
            for (Combo combo : personalMahjongInfo.getGangs()) {
                if (combo.getCmd() == Cmd.C2S_ROOM_COMMON_AN_GANG
                        || combo.getCmd() == Cmd.C2S_ROOM_BACK_AN_GANG) {
                    anGangTimes++;
                } else if (combo.getCmd() == Cmd.C2S_ROOM_ZHI_GANG
                        || combo.getCmd() == Cmd.C2S_ROOM_COMMON_PENG_GANG
                        || combo.getCmd() == Cmd.C2S_ROOM_BACK_PENG_GANG) {
                    mingGangTimes++;
                }
            }
            score.setAnGangTimes(anGangTimes);
            score.setMingGangTimes(mingGangTimes);

            if (winnerUserIds.contains(personalMahjongInfo.getUserId())) {
                // 是否自摸
                if (operate == null) {
                    score.setIsZiMo(BattleScoreEntity.IsZiMo.NOT_ZI_MO.getId());
                } else if (operate.getActionType() == ActionType.ZI_MO
                        || operate.getActionType() == ActionType.QIANG_ZHI_GANG_HU
                        || operate.getActionType() == ActionType.QIANG_AN_GANG_HU
                        || operate.getActionType() == ActionType.QIANG_PENG_GANG_HU) {
                    score.setIsZiMo(BattleScoreEntity.IsZiMo.ZI_MO.getId());
                } else {
                    score.setIsZiMo(BattleScoreEntity.IsZiMo.NOT_ZI_MO.getId());
                }

                // 胡牌名称
                score.setHuName(operate == null ? "" : operate.getRule().getName());

                // 输或赢的情况
                if (operate == null) {
                    score.setWinOrLose(BattleScoreEntity.WinOrLose.NONE.getId());
                } else if (operate.getActionType() == ActionType.ZI_MO
                        || operate.getActionType() == ActionType.QIANG_ZHI_GANG_HU
                        || operate.getActionType() == ActionType.QIANG_AN_GANG_HU
                        || operate.getActionType() == ActionType.QIANG_PENG_GANG_HU) {
                    score.setWinOrLose(BattleScoreEntity.WinOrLose.ZI_MO.getId());
                } else if (operate.getActionType() == ActionType.CHI_HU) {
                    score.setWinOrLose(BattleScoreEntity.WinOrLose.JIE_PAO.getId());
                }
            } else {
                // 是否自摸
                score.setIsZiMo(BattleScoreEntity.IsZiMo.NOT_ZI_MO.getId());

                // 胡牌名称
                score.setHuName(operate == null ? "" : operate.getRule().getName());

                // 输或赢的情况
                if (operate == null) {
                    score.setWinOrLose(BattleScoreEntity.WinOrLose.NONE.getId());
                } else if (operate.getActionType() == ActionType.ZI_MO
                        || operate.getActionType() == ActionType.QIANG_ZHI_GANG_HU
                        || operate.getActionType() == ActionType.QIANG_AN_GANG_HU
                        || operate.getActionType() == ActionType.QIANG_PENG_GANG_HU) {
                    score.setWinOrLose(BattleScoreEntity.WinOrLose.OTHER_USER_ZI_MO.getId());
                } else if (operate.getActionType() == ActionType.CHI_HU) {
                    if (personalMahjongInfo.getUserId() == winnerCanDoOperates.get(0).getSpecialUserId()) {
                        score.setWinOrLose(BattleScoreEntity.WinOrLose.DIAN_PAO.getId());
                    } else {
                        score.setWinOrLose(BattleScoreEntity.WinOrLose.NONE.getId());
                    }
                }
            }
            scores.add(score);
        }

        // 算分
        Calculator calculator = (Calculator) Class.forName(MahjongConfig.calculator).newInstance();
        List<PersonalMahjongInfo> winnerInfos = new ArrayList<>(winnerUserIds.size());
        for (Integer winnerUserId : winnerUserIds) {
            winnerInfos.add(PersonalMahjongInfo.getMyInfo(mahjongGameData.getPersonalMahjongInfos(), winnerUserId));
        }
        calculator.calculate(scores, mahjongGameData, winnerCanDoOperates, winnerInfos);

        return scores;
    }

    /**
     * 计算总结算
     */
    public List<BattleScoreEntity> genTotalScores(MahjongGameData mahjongGameData)
            throws ClassNotFoundException, IllegalAccessException, InstantiationException {
        // 算分
        TotalCalculator totalCalculator = (TotalCalculator) Class.forName(MahjongConfig.totalCalculator).newInstance();

        List<BattleScoreEntity> scores = BattleScoreDao.getInstance().getBattleScoreByRoomId(mahjongGameData.getRoomId());
        List<BattleScoreEntity> totalScores = totalCalculator.calculate(scores, mahjongGameData);
        return totalScores;
    }

    /**
     * 记录用户的胡牌操作
     */
    private void addOperated(MahjongGameData mahjongGameData, List<CanDoOperate> winnerCanDoOperates) {
        for (CanDoOperate winnerCanDoOperate : winnerCanDoOperates) {
            PersonalMahjongInfo myInfo =
                    PersonalMahjongInfo.getMyInfo(mahjongGameData.getPersonalMahjongInfos(), winnerCanDoOperate.getUserId());

            List<Mahjong> mahjongList = new ArrayList<>(14);
            for (Combo combo : myInfo.getGangs()) {
                mahjongList.addAll(combo.getMahjongs());
            }
            for (Combo combo : myInfo.getPengs()) {
                mahjongList.addAll(combo.getMahjongs());
            }
            mahjongList.addAll(myInfo.getHandCards());
            mahjongList.add(winnerCanDoOperate.getSpecialMahjong());

            if (winnerCanDoOperate.getOperates().get(0).getActionType() == ActionType.ZI_MO) {
                mahjongGameData.addOperated(
                        Operated.newZiMo(
                                winnerCanDoOperate.getUserId(),
                                winnerCanDoOperate.getSpecialMahjong(),
                                mahjongList
                        ));
            } else if (winnerCanDoOperate.getOperates().get(0).getActionType() == ActionType.CHI_HU) {
                mahjongGameData.addOperated(
                        Operated.newChiHu(
                                winnerCanDoOperate.getUserId(),
                                winnerCanDoOperate.getSpecialUserId(),
                                winnerCanDoOperate.getSpecialMahjong(),
                                mahjongList
                        ));
            } else if (winnerCanDoOperate.getOperates().get(0).getActionType() == ActionType.QIANG_AN_GANG_HU) {
                mahjongGameData.addOperated(
                        Operated.newQiangAnGangHu(
                                winnerCanDoOperate.getUserId(),
                                winnerCanDoOperate.getSpecialUserId(),
                                winnerCanDoOperate.getSpecialMahjong(),
                                mahjongList
                        ));
            } else if (winnerCanDoOperate.getOperates().get(0).getActionType() == ActionType.QIANG_PENG_GANG_HU) {
                mahjongGameData.addOperated(
                        Operated.newQiangPengGangHu(
                                winnerCanDoOperate.getUserId(),
                                winnerCanDoOperate.getSpecialUserId(),
                                winnerCanDoOperate.getSpecialMahjong(),
                                mahjongList
                        ));
            } else if (winnerCanDoOperate.getOperates().get(0).getActionType() == ActionType.QIANG_ZHI_GANG_HU) {
                mahjongGameData.addOperated(
                        Operated.newQiangZhiGangHu(
                                winnerCanDoOperate.getUserId(),
                                winnerCanDoOperate.getSpecialUserId(),
                                winnerCanDoOperate.getSpecialMahjong(),
                                mahjongList
                        ));
            }

        }
    }

}
