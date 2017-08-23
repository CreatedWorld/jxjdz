package avatar.module.mahjong.score;

import avatar.entity.battlerecord.BattleScoreEntity;
import avatar.entity.room.RoomPlayer;
import avatar.module.mahjong.Combo;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.PersonalMahjongInfo;
import avatar.module.mahjong.action.Operated;
import avatar.module.mahjong.operate.ActionType;
import avatar.module.mahjong.operate.CanDoOperate;
import avatar.module.mahjong.operate.Operate;
import avatar.module.room.service.RoomDataService;
import avatar.protobuf.Cmd;
import org.apache.commons.collections.CollectionUtils;

import java.util.List;

/**
 * 全民麻将的算分器，作用是计算BattleScoreEntity的score字段
 */
public class QuanMingCalulator implements Calculator {

    /**
     * 计算方法
     *
     * @param battleScores         初始化好的每个玩家的BattleScoreEntity
     * @param mahjongGameData      麻将游戏数据
     * @param winnerCanDoOperates  胡牌玩家的CanDoOperate
     * @param personalMahjongInfos 胡牌玩家的信息
     */
    @Override
    public void calculate(List<BattleScoreEntity> battleScores,
                          MahjongGameData mahjongGameData,
                          List<CanDoOperate> winnerCanDoOperates,
                          List<PersonalMahjongInfo> personalMahjongInfos) {
        Operate operate = null;
        if (CollectionUtils.isNotEmpty(winnerCanDoOperates)) {
            operate = winnerCanDoOperates.get(0).getOperates().get(0);
        }

        List<RoomPlayer> roomPlayers = RoomDataService.getInstance().
                loadRoomPlayerListByState(mahjongGameData.getRoomId(), RoomPlayer.State.READY.getCode());

        // 庄家userId
        int bankerUserId = 0;
        for (RoomPlayer roomPlayer : roomPlayers) {
            if (roomPlayer.getSeat() == mahjongGameData.getBankerSite()) {
                bankerUserId = roomPlayer.getPlayerId();
                break;
            }
        }

        for (PersonalMahjongInfo personalMahjongInfo : personalMahjongInfos) {
            // 赢家是否庄家
            boolean winnerIsBanker = personalMahjongInfo.getUserId() == bankerUserId;

            // 流局
            if (operate == null) {
                // 什么都不用做，得分默认为0
                return;
            }

            if (operate.getActionType() == ActionType.ZI_MO) {

                // 杠上开花，按中胡算
                if (isGangShangHua(mahjongGameData)) {
                    calculateZiMoMiddleHu(battleScores, mahjongGameData, personalMahjongInfo, bankerUserId, winnerIsBanker);
                } else if ("豪华七对胡".equalsIgnoreCase(operate.getRule().getName())
                        || "清一色七对胡".equalsIgnoreCase(operate.getRule().getName())
                        || "清一色一条龙胡".equalsIgnoreCase(operate.getRule().getName())) {
                    calculateZiMoBigHu(battleScores, mahjongGameData, personalMahjongInfo, bankerUserId, winnerIsBanker);
                } else if ("普通七对胡".equalsIgnoreCase(operate.getRule().getName())
                        || "一条龙胡".equalsIgnoreCase(operate.getRule().getName())
                        || "清一色胡".equalsIgnoreCase(operate.getRule().getName())) {
                    calculateZiMoMiddleHu(battleScores, mahjongGameData, personalMahjongInfo, bankerUserId, winnerIsBanker);
                } else {
                    calculateZiMoSmallHu(battleScores, mahjongGameData, personalMahjongInfo, bankerUserId, winnerIsBanker);
                }
            } else if (operate.getActionType() == ActionType.CHI_HU) {
                int putOutMahjongUserId = winnerCanDoOperates.get(0).getSpecialUserId();
                if ("豪华七对胡".equalsIgnoreCase(operate.getRule().getName())
                        || "清一色七对胡".equalsIgnoreCase(operate.getRule().getName())
                        || "清一色一条龙胡".equalsIgnoreCase(operate.getRule().getName())) {
                    calculateChiHuBigHu(battleScores, mahjongGameData, personalMahjongInfo, bankerUserId, winnerIsBanker, putOutMahjongUserId);
                } else if ("普通七对胡".equalsIgnoreCase(operate.getRule().getName())
                        || "一条龙胡".equalsIgnoreCase(operate.getRule().getName())
                        || "清一色胡".equalsIgnoreCase(operate.getRule().getName())) {
                    calculateChiHuMiddleHu(battleScores, mahjongGameData, personalMahjongInfo, bankerUserId, winnerIsBanker, putOutMahjongUserId);
                } else {
                    calculateChiHuSmallHu(battleScores, mahjongGameData, personalMahjongInfo, bankerUserId, winnerIsBanker, putOutMahjongUserId);
                }
            }
        }

        // 计算杠得分
        calculateGangScore(battleScores, mahjongGameData, winnerCanDoOperates);
    }

    /**
     * 计算杠得分
     * 你碰的一万 然后你摸了一万 是明杠 谁出的那张碰牌谁输三分
     * 你三个一万 又摸了一张一万属于暗杠 每人输各2分
     * 我手上有3个一万，然后别人打了一张1万，放杠的输3分
     *
     * @param battleScores        初始化好的每个玩家的BattleScoreEntity
     * @param mahjongGameData     麻将游戏数据
     * @param winnerCanDoOperates 胡牌玩家的CanDoOperate
     */
    private void calculateGangScore(List<BattleScoreEntity> battleScores,
                                    MahjongGameData mahjongGameData,
                                    List<CanDoOperate> winnerCanDoOperates) {
        // 流局不用算杠分
        if (CollectionUtils.isEmpty(winnerCanDoOperates)) {
            return;
        }

        for (PersonalMahjongInfo personalMahjongInfo : mahjongGameData.getPersonalMahjongInfos()) {
            int myUserId = personalMahjongInfo.getUserId();
            for (Combo combo : personalMahjongInfo.getGangs()) {
                if (combo.getTargetUserId() == myUserId) {
                    // 暗杠每家输2分
                    for (BattleScoreEntity battleScore : battleScores) {
                        if (battleScore.getPlayerId() == myUserId) {
                            battleScore.setScore(2 * (battleScores.size() - 1) + battleScore.getScore());
                        } else {
                            battleScore.setScore(battleScore.getScore() - 2);
                        }
                    }
                } else {
                    // 明杠，放杠者输3分
                    BattleScoreEntity loseScore =
                            battleScores
                                    .stream()
                                    .filter(s -> s.getPlayerId() == combo.getTargetUserId())
                                    .findFirst()
                                    .get();
                    loseScore.setScore(loseScore.getScore() - 3);

                    // 明杠者加3分
                    BattleScoreEntity winnerScore =
                            battleScores
                                    .stream()
                                    .filter(s -> s.getPlayerId() == myUserId)
                                    .findFirst()
                                    .get();
                    winnerScore.setScore(winnerScore.getScore() + 3);
                }
            }
        }
    }

    /**
     * 判断是否杠上花
     */
    private boolean isGangShangHua(MahjongGameData mahjongGameData) {
        if (mahjongGameData == null) {
            return false;
        }

        if (CollectionUtils.isEmpty(mahjongGameData.getOperated())) {
            return false;
        }

        List<Operated> operateds = mahjongGameData.getOperated();
        if (operateds.size() < 2) {
            return false;
        }


        // 获取倒数第2个操作，如果是杠，则是杠上花
        Operated last2Step = operateds.get(operateds.size() - 2);
        if (last2Step.getActionType().isGang()) {
            return true;
        }

        return false;
    }

    /**
     * 计算吃胡小胡分数
     */
    private void calculateChiHuSmallHu(List<BattleScoreEntity> battleScores,
                                       MahjongGameData mahjongGameData,
                                       PersonalMahjongInfo personalMahjongInfo,
                                       int bankerUserId,
                                       boolean winnerIsBanker,
                                       int putOutMahjongUserId) {
        // 小胡（平胡）
        // 闲家点炮庄家 输6分
        // 庄家点炮闲家 输6分
        // 闲家点炮闲家 输4分
        for (BattleScoreEntity battleScore : battleScores) {
            if (winnerIsBanker) {
                if (battleScore.getPlayerId() == personalMahjongInfo.getUserId()) {
                    battleScore.setScore(6);
                } else if (battleScore.getPlayerId() == putOutMahjongUserId) {
                    battleScore.setScore(battleScore.getScore() - 6);
                }
            } else {
                if (battleScore.getPlayerId() == personalMahjongInfo.getUserId()) {
                    if (putOutMahjongUserId == bankerUserId) {
                        battleScore.setScore(6);
                    } else {
                        battleScore.setScore(4);
                    }
                } else if (battleScore.getPlayerId() == putOutMahjongUserId) {
                    if (battleScore.getPlayerId() == bankerUserId) {
                        battleScore.setScore(battleScore.getScore() - 6);
                    } else {
                        battleScore.setScore(battleScore.getScore() - 4);
                    }
                }
            }
        }
    }

    /**
     * 计算吃胡中胡分数
     */
    private void calculateChiHuMiddleHu(List<BattleScoreEntity> battleScores,
                                        MahjongGameData mahjongGameData,
                                        PersonalMahjongInfo personalMahjongInfo,
                                        int bankerUserId,
                                        boolean winnerIsBanker,
                                        int putOutMahjongUserId) {
        // 中胡（小七对 清一色 一条龙）
        // 闲家点炮庄家 输12分
        // 庄家点炮闲家 输12分
        // 闲家点炮闲家 输8分
        for (BattleScoreEntity battleScore : battleScores) {
            if (winnerIsBanker) {
                if (battleScore.getPlayerId() == personalMahjongInfo.getUserId()) {
                    battleScore.setScore(12);
                } else if (battleScore.getPlayerId() == putOutMahjongUserId) {
                    battleScore.setScore(battleScore.getScore() - 12);
                }
            } else {
                if (battleScore.getPlayerId() == personalMahjongInfo.getUserId()) {
                    if (putOutMahjongUserId == bankerUserId) {
                        battleScore.setScore(12);
                    } else {
                        battleScore.setScore(8);
                    }

                } else if (battleScore.getPlayerId() == putOutMahjongUserId) {
                    if (battleScore.getPlayerId() == bankerUserId) {
                        battleScore.setScore(battleScore.getScore() - 12);
                    } else {
                        battleScore.setScore(battleScore.getScore() - 8);
                    }
                }
            }
        }
    }

    /**
     * 计算吃胡大胡分数
     */
    private void calculateChiHuBigHu(List<BattleScoreEntity> battleScores,
                                     MahjongGameData mahjongGameData,
                                     PersonalMahjongInfo personalMahjongInfo,
                                     int bankerUserId,
                                     boolean winnerIsBanker,
                                     int putOutMahjongUserId) {
        // 大胡（豪华七对  清一色7对 清一色一条龙）
        // 闲家点炮庄家 输16分
        // 庄家点炮闲家 输16分
        // 闲家点炮闲家 输12分
        for (BattleScoreEntity battleScore : battleScores) {
            if (winnerIsBanker) {
                if (battleScore.getPlayerId() == personalMahjongInfo.getUserId()) {
                    battleScore.setScore(16);
                } else if (battleScore.getPlayerId() == putOutMahjongUserId) {
                    battleScore.setScore(battleScore.getScore() - 16);
                }
            } else {
                if (battleScore.getPlayerId() == personalMahjongInfo.getUserId()) {
                    if (putOutMahjongUserId == bankerUserId) {
                        battleScore.setScore(16);
                    } else {
                        battleScore.setScore(12);
                    }
                } else if (battleScore.getPlayerId() == putOutMahjongUserId) {
                    if (battleScore.getPlayerId() == bankerUserId) {
                        battleScore.setScore(battleScore.getScore() - 16);
                    } else {
                        battleScore.setScore(battleScore.getScore() - 12);
                    }
                }
            }
        }
    }

    /**
     * 计算自摸中胡分数
     */
    private void calculateZiMoSmallHu(List<BattleScoreEntity> battleScores,
                                      MahjongGameData mahjongGameData,
                                      PersonalMahjongInfo personalMahjongInfo,
                                      int bankerUserId,
                                      boolean winnerIsBanker) {
        // 小胡（平胡）
        // 庄家自摸 三家闲家各输4分，庄家赢12分
        // 闲家自摸 庄家输4分 闲家输2分
        for (BattleScoreEntity battleScore : battleScores) {
            if (winnerIsBanker) {
                if (battleScore.getPlayerId() == personalMahjongInfo.getUserId()) {
                    battleScore.setScore(4 * (mahjongGameData.getPersonalMahjongInfos().size() - 1));
                } else {
                    battleScore.setScore(-4);
                }
            } else {
                if (battleScore.getPlayerId() == personalMahjongInfo.getUserId()) {
                    battleScore.setScore(2 * (mahjongGameData.getPersonalMahjongInfos().size() - 2) + 4);
                } else {
                    if (battleScore.getPlayerId() == bankerUserId) {
                        battleScore.setScore(-4);
                    } else {
                        battleScore.setScore(-2);
                    }
                }
            }
        }

        // 自摸暗杠 各家输2分 赢6分
        if (personalMahjongInfo.getGangs().stream().anyMatch(
                combo -> combo.getCmd() == Cmd.C2S_ROOM_COMMON_AN_GANG || combo.getCmd() == Cmd.C2S_ROOM_BACK_AN_GANG)
                ) {
            for (BattleScoreEntity battleScore : battleScores) {
                if (battleScore.getPlayerId() == personalMahjongInfo.getUserId()) {
                    battleScore.setScore(
                            battleScore.getScore() +
                                    (
                                            2 * (mahjongGameData.getPersonalMahjongInfos().size() - 1)
                                    )
                    );
                } else {
                    battleScore.setScore(battleScore.getScore() - 2);
                }
            }
        }
    }


    /**
     * 计算自摸中胡分数
     */
    private void calculateZiMoMiddleHu(List<BattleScoreEntity> battleScores,
                                       MahjongGameData mahjongGameData,
                                       PersonalMahjongInfo personalMahjongInfo,
                                       int bankerUserId,
                                       boolean winnerIsBanker) {
        // 中胡（小七对 清一色 一条龙）
        // 庄家自摸 三家闲家各输12分 庄家赢36分
        // 闲家自摸 庄家输12分 闲家输6分 闲家赢24分
        for (BattleScoreEntity battleScore : battleScores) {
            if (winnerIsBanker) {
                if (battleScore.getPlayerId() == personalMahjongInfo.getUserId()) {
                    battleScore.setScore(12 * (mahjongGameData.getPersonalMahjongInfos().size() - 1));
                } else {
                    battleScore.setScore(-12);
                }
            } else {
                if (battleScore.getPlayerId() == personalMahjongInfo.getUserId()) {
                    battleScore.setScore(6 * (mahjongGameData.getPersonalMahjongInfos().size() - 2) + 12);
                } else {
                    if (battleScore.getPlayerId() == bankerUserId) {
                        battleScore.setScore(-12);
                    } else {
                        battleScore.setScore(-6);
                    }
                }
            }
        }
    }

    /**
     * 计算自摸大胡分数
     */
    private void calculateZiMoBigHu(List<BattleScoreEntity> battleScores,
                                    MahjongGameData mahjongGameData,
                                    PersonalMahjongInfo personalMahjongInfo,
                                    int bankerUserId,
                                    boolean winnerIsBanker) {
        // 大胡（豪华七对  清一色7对 清一色一条龙）
        // 庄家自摸 三家闲家各输16分 庄家赢48分
        // 闲家自摸 庄家输16分 闲家输8分 闲家赢32分
        for (BattleScoreEntity battleScore : battleScores) {
            if (winnerIsBanker) {
                if (battleScore.getPlayerId() == personalMahjongInfo.getUserId()) {
                    battleScore.setScore(16 * (mahjongGameData.getPersonalMahjongInfos().size() - 1));
                } else {
                    battleScore.setScore(-16);
                }
            } else {
                if (battleScore.getPlayerId() == personalMahjongInfo.getUserId()) {
                    battleScore.setScore(8 * (mahjongGameData.getPersonalMahjongInfos().size() - 2) + 16);
                } else {
                    if (battleScore.getPlayerId() == bankerUserId) {
                        battleScore.setScore(-16);
                    } else {
                        battleScore.setScore(-8);
                    }
                }
            }
        }
    }
}
