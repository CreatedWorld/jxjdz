package avatar.module.mahjong.score;

import avatar.entity.battlerecord.BattleScoreEntity;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.PersonalMahjongInfo;

import java.util.ArrayList;
import java.util.List;

/**
 * 全民麻将的总结算算分器
 */
public class QuanMingTotalCalulator implements TotalCalculator {

    @Override
    public List<BattleScoreEntity> calculate(List<BattleScoreEntity> battleScores, MahjongGameData mahjongGameData) {
        List<BattleScoreEntity> totalScores = new ArrayList<>(mahjongGameData.getPersonalMahjongInfos().size());
        for (PersonalMahjongInfo info : mahjongGameData.getPersonalMahjongInfos()) {
            BattleScoreEntity totalScore = new BattleScoreEntity();
            totalScore.setPlayerId(info.getUserId());
            totalScores.add(totalScore);


            for (BattleScoreEntity battleScore : battleScores) {
                if (battleScore.getPlayerId() == info.getUserId()) {
                    // 自摸次数
                    if (battleScore.getIsZiMo() == BattleScoreEntity.IsZiMo.ZI_MO.getId()) {
                        totalScore.setIsZiMo(totalScore.getIsZiMo() + 1);
                    }

                    // 暗杠、明杠次数、总分数
                    totalScore.setAnGangTimes(totalScore.getAnGangTimes() + battleScore.getAnGangTimes());
                    totalScore.setMingGangTimes(totalScore.getMingGangTimes() + battleScore.getMingGangTimes());
                    totalScore.setScore(totalScore.getScore() + battleScore.getScore());
                }

                // 接炮次数
                if (battleScore.getWinOrLose() == BattleScoreEntity.WinOrLose.JIE_PAO.getId()
                        && battleScore.getPlayerId() == info.getUserId()) {
                    totalScore.setJiePaoUserId(totalScore.getJiePaoUserId() + 1);
                }

                // 点炮次数
                if (battleScore.getWinOrLose() == BattleScoreEntity.WinOrLose.DIAN_PAO.getId()
                        && battleScore.getPlayerId() == info.getUserId()) {
                    totalScore.setDianPaoUserId(totalScore.getDianPaoUserId() + 1);
                }
            }
        }

        return totalScores;
    }

}
