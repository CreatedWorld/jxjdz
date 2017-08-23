package avatar.module.mahjong.score;


import avatar.entity.battlerecord.BattleScoreEntity;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.PersonalMahjongInfo;

import java.util.List;
import java.util.Map;

/**
 * 总结算分器，累加单局结算的数据
 */
public interface TotalCalculator {

    /**
     * 计算方法
     *
     * @param battleScores        单局结算时所有人的数据
     * @param mahjongGameData     麻将游戏数据
     * @param personalMahjongInfo 胡牌玩家的信息
     */
    List<BattleScoreEntity> calculate(List<BattleScoreEntity> battleScores, MahjongGameData mahjongGameData);
}
