package avatar.module.battleRecord.dao;

import avatar.entity.battlerecord.UserBattleScoreEntity;
import avatar.module.user.UserDao;
import avatar.util.GameData;

import java.util.Date;
import java.util.List;

/**
 * 记录玩家每一回合总战绩接口
 */
public class UserBattleScoreDao {
    private UserBattleScoreDao() {
    }
    private static final  UserBattleScoreDao instance = new UserBattleScoreDao();
    public static final UserBattleScoreDao getInstance(){
        return instance;
    }
    //============接口============//
    //插入
    /**
     * 保存一条总战绩记录
     * @param entity 战绩记录
     * @return true 插入成功；false 插入失败
     */
    public boolean insertUserBattleScore(UserBattleScoreEntity entity){
        boolean ret = insertDB(entity);
        return ret;
    }

    //更新
    /**
     * 更新一条总战绩
     * @param entity 战绩记录
     * @return true 插入成功；false 插入失败
     */
    public boolean updateUserBattleScore(UserBattleScoreEntity entity){
        boolean ret = updateDB(entity);
        return ret;
    }

    //获取
    /**
     * 根据玩家id获取最新几条战绩记录
     * @param playerId 玩家id
     * @param count 查询几条
     * @return 玩家战绩记录集合
     */
    public List<UserBattleScoreEntity> getUserBattleScoreByPlayerId(int playerId, int count){
        return getUserBattleScoreByPlayerIdDB(playerId, count);
    }

    /**
     * 根据房间id获取该房间各个玩家的战绩记录
     * @param roomId 房间id
     * @return 该房间的战绩记录集合
     */
    public List<UserBattleScoreEntity> getUserBattleScoreByRoomId(int roomId){
        return getUserBattleScoreByRoomIdDB(roomId);
    }

    /**
     * 根据玩家id和房间id获取该玩家在该房间下的战绩记录
     * @param roomId 房间id
     * @param playerId 玩家id
     * @return 该玩家在该房间下的战绩记录
     */
    public UserBattleScoreEntity getUserBattleScoreByRoomIdAndPlayerId(int roomId, int playerId){
        return getUserBattleScoreByRoomIdAndPlayerIdDB(roomId, playerId);
    }
    //===========cache===========//



    //============DB=============//
    //插入
    /**
     * 插入一条战绩记录到数据库
     * @param entity 战绩记录
     * @return true 插入成功；false 插入失败
     */
    private boolean insertDB(UserBattleScoreEntity entity){
        boolean ret = GameData.getDB().insert(entity);
        if (!ret){
            //log
            return ret;
        }
        return ret;
    }

    //更新
    /**
     * 更新一条战绩记录
     * @param entity 战绩记录
     * @return true 更新成功；false 更新失败
     */
    private boolean updateDB(UserBattleScoreEntity entity){
        boolean ret = GameData.getDB().update(entity);
        if (!ret){
            //log
            return ret;
        }
        return ret;
    }

    //获取
    /**
     * 根据玩家id获取最新几条战绩记录
     * @param playerId 玩家id
     * @param count 查询几条
     * @return 战绩记录集合
     */
    private List<UserBattleScoreEntity> getUserBattleScoreByPlayerIdDB(int playerId, int count){
        String sql = "SELECT * FROM userbattlescore WHERE userId = ? ORDER BY time DESC LIMIT ?";
        Object[] param = {playerId, count};
        List<UserBattleScoreEntity> userBattleScores = GameData.getDB().list(UserBattleScoreEntity.class, sql, param);
        return userBattleScores;
    }

    /**
     * 根据房间id获取战绩记录
     * @param roomId 房间id
     * @return 战绩记录集合
     */
    private List<UserBattleScoreEntity> getUserBattleScoreByRoomIdDB(int roomId){
        String sql = "SELECT * FROM userbattlescore WHERE roomId = ?";
        Object[] param = {roomId};
        List<UserBattleScoreEntity> userBattleScores = GameData.getDB().list(UserBattleScoreEntity.class, sql, param);
        return userBattleScores;
    }

    /**
     * 根据房间id和玩家id获取该玩家在该房间的战绩记录
     * @param roomId 房间id
     * @param playerId 玩家ID
     * @return 该玩家在该房间的战绩记录
     */
    private UserBattleScoreEntity getUserBattleScoreByRoomIdAndPlayerIdDB(int roomId, int playerId){
        String sql = "SELECT * FROM userbattlescore WHERE roomId = ? AND userId = ?";
        Object[] param = {roomId, playerId};
        UserBattleScoreEntity userBattleScore = GameData.getDB().get(UserBattleScoreEntity.class, sql, param);
        return userBattleScore;
    }
}
