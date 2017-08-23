package avatar.module.mahjong.score;


import avatar.entity.battlerecord.BattleScoreEntity;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.PersonalMahjongInfo;
import avatar.module.mahjong.operate.CanDoOperate;

import java.util.List;

/**
 * 计算单局结算个人得分，有胡牌（输赢）的时候才需要计算。否则直接算0分。作用是计算BattleScoreEntity的score字段
 */
public interface Calculator {

    /**
     * 计算方法
     *
     * @param battleScores         初始化好的每个玩家的BattleScoreEntity
     * @param mahjongGameData      麻将游戏数据
     * @param winnerCanDoOperates  胡牌玩家的CanDoOperate
     * @param personalMahjongInfos 胡牌玩家的信息
     */
    void calculate(List<BattleScoreEntity> battleScores,
                   MahjongGameData mahjongGameData,
                   List<CanDoOperate> winnerCanDoOperates,
                   List<PersonalMahjongInfo> personalMahjongInfos
    );
}
