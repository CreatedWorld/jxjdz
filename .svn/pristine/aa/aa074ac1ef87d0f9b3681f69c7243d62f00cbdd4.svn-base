package avatar.module.battleRecord.service;

import avatar.entity.battlerecord.BattleReplayEntity;
import avatar.entity.battlerecord.BattleScoreEntity;
import avatar.entity.battlerecord.UserBattleScoreEntity;
import avatar.entity.room.Room;
import avatar.entity.userinfo.UserEntity;
import avatar.module.battleRecord.dao.BattleReplayDao;
import avatar.module.battleRecord.dao.BattleScoreDao;
import avatar.module.battleRecord.dao.UserBattleScoreDao;
import avatar.module.room.dao.RoomDao;
import avatar.module.user.UserDao;
import org.apache.commons.collections.CollectionUtils;

import java.util.*;

/**
 * 战绩接口
 */
public class BattleRecordService {
    private static final BattleRecordService instance = new BattleRecordService();

    public static final BattleRecordService getInstance() {
        return instance;
    }

    /**
     * 保存一局战绩：保存该局的回放记录以及玩家得分记录
     *
     * @param roomId        房间id
     * @param innings       局
     * @param replayContent 回放数据
     * @param playerScores  玩家得分：key为玩家id；value为玩家得分
     * @return true 保存成功；false 保存失败
     */
    public boolean saveBattleRecord(int roomId, int innings, String replayContent, List<BattleScoreEntity> playerScores) {
        Room room = RoomDao.getInstance().loadRoomByRoomId(roomId);

        // 回放记录
        BattleReplayEntity battleReplayEntity = new BattleReplayEntity();
        battleReplayEntity.setRoomId(roomId);
        battleReplayEntity.setRoomCode(room.getRoomCode());
        battleReplayEntity.setInnings(innings);
        battleReplayEntity.setReplayContent(replayContent);
        battleReplayEntity.setGamesTime(new Date());
        int battleReplayId = BattleReplayDao.getInstance().insertBattleReplayAndReturnId(battleReplayEntity);
        if (battleReplayId == 0) {
            return false;
        }

        // 个人分数
        for (BattleScoreEntity playerScore : playerScores) {
            playerScore.setBattleReplayId(battleReplayId);
            BattleScoreDao.getInstance().insertBattleScore(playerScore);
        }

        //有总战绩记录就更新，没有就新增
        List<UserBattleScoreEntity> userBattleScores = UserBattleScoreDao.getInstance().getUserBattleScoreByRoomId(roomId);
        if (CollectionUtils.isEmpty(userBattleScores)) {
            for (BattleScoreEntity playerScore : playerScores) {
                UserEntity player = UserDao.getInstance().loadUserEntityByUserId(playerScore.getPlayerId());

                UserBattleScoreEntity userBattleScoreEntity = new UserBattleScoreEntity();
                userBattleScoreEntity.setTime(new Date());
                userBattleScoreEntity.setAllScore(playerScore.getScore());
                userBattleScoreEntity.setUserName(player.getNickName());
                userBattleScoreEntity.setRoomId(roomId);
                userBattleScoreEntity.setRoomCode(room.getRoomCode());
                userBattleScoreEntity.setUserId(playerScore.getPlayerId());
                boolean result = UserBattleScoreDao.getInstance().insertUserBattleScore(userBattleScoreEntity);
                if (!result) {
                    return result;
                }
            }

        } else {
            for (UserBattleScoreEntity userBattleScore : userBattleScores) {
                BattleScoreEntity scoreEntity = null;
                for (BattleScoreEntity playerScore : playerScores) {
                    if (playerScore.getPlayerId() == userBattleScore.getUserId()) {
                        scoreEntity = playerScore;
                    }
                }
                userBattleScore.setTime(new Date());
                userBattleScore.setAllScore(userBattleScore.getAllScore() + scoreEntity.getScore());
                boolean result = UserBattleScoreDao.getInstance().updateUserBattleScore(userBattleScore);
                if (!result) {
                    return result;
                }
            }
        }

        return true;
    }

    /**
     * 根据玩家id查询最新几条战绩记录
     *
     * @param playerId 玩家id
     * @param count    查询几条
     * @return 该玩家最新几条战绩记录
     */
    public List<UserBattleScoreEntity> getUserBattleScoreByPlayerId(int playerId, int count) {
        List<UserBattleScoreEntity> userBattleScores = UserBattleScoreDao.getInstance().getUserBattleScoreByPlayerId(playerId, count);
        return userBattleScores;
    }

    /**
     * 根据房间id查询战绩记录
     *
     * @param roomId 房间id
     * @return 该房间里各个玩家的战绩记录
     */
    public List<UserBattleScoreEntity> getUserBattleScoreByRoomId(int roomId) {
        List<UserBattleScoreEntity> userBattleScores = UserBattleScoreDao.getInstance().getUserBattleScoreByRoomId(roomId);
        return userBattleScores;
    }

    /**
     * 根据房间id获取该房间所有的回放记录
     *
     * @param roomId 房间id
     * @return 该房间所有的回放记录
     */
    public List<BattleReplayEntity> getBattleReplaysByRoomId(int roomId) {
        List<BattleReplayEntity> battleReplays = BattleReplayDao.getInstance().getBattleReplaysByRoomId(roomId);
        return battleReplays;
    }

    /**
     * 根据回放记录id获取该回放记录的得分记录
     *
     * @param battleReplayId 回放记录id
     * @return 该局各个玩家的得分记录集合
     */
    public List<BattleScoreEntity> getBattleScoreByBattleReplayId(int battleReplayId) {
        List<BattleScoreEntity> battleScores = BattleScoreDao.getInstance().getBattleScoreByBattleReplayId(battleReplayId);
        return battleScores;
    }

    /**
     * 根据房间id和局获取回放记录
     *
     * @param roomId  房间id
     * @param innings 局
     * @return 一条回放记录
     */
    public BattleReplayEntity getBattleReplayByRoomIdAndInnings(int roomId, int innings) {
        return BattleReplayDao.getInstance().getReplayContentByRoomIdAndInnings(roomId, innings);
    }
}
