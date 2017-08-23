package avatar.module.battleRecord.dao;

import avatar.entity.battlerecord.BattleScoreEntity;
import avatar.util.GameData;

import java.util.List;

/**
 * 记录玩家战绩分数接口
 */
public class BattleScoreDao {
    private BattleScoreDao() {
    }

    private static final BattleScoreDao instance = new BattleScoreDao();

    public static final BattleScoreDao getInstance() {
        return instance;
    }

    private final String key = "battleReplay_%d";

    private String getKey(int battleReplayId) {
        return String.format(key, battleReplayId);
    }
    //============接口=============//
    //插入

    /**
     * 插入一条分数记录
     *
     * @param entity 回战绩分数记录
     * @return true 插入成功；false 插入失败
     */
    public boolean insertBattleScore(BattleScoreEntity entity) {
        boolean ret = insertDB(entity);
        if (ret) {
            removeCache(entity.getBattleReplayId());
        }
        return ret;
    }

    //查询

    /**
     * 根据回放记录id获取该条回放记录的分数列表
     *
     * @param battleReplayId 回放记录id
     * @return 该条回放记录的分数列表
     */
    public List<BattleScoreEntity> getBattleScoreByBattleReplayId(int battleReplayId) {
        List<BattleScoreEntity> list = loadCache(battleReplayId);
        if (list != null && list.size() > 0) {
            return list;
        }
        list = getBattleScoresByBattleReplayIdDB(battleReplayId);
        if (list != null && list.size() > 0) {
            setCache(battleReplayId, list);
        }
        return list;
    }

    //查询

    /**
     * 根据房间id获取分数列表
     *
     * @param roomId 房间id
     * @return 该条回放记录的分数列表
     */
    public List<BattleScoreEntity> getBattleScoreByRoomId(int roomId) {
        List<BattleScoreEntity> battleScoreEntities = getBattleScoresByRoomIdDB(roomId);
        return battleScoreEntities;
    }

    //============cache==========//
    private List<BattleScoreEntity> loadCache(int battleReplayId) {
        // return GameData.getCache().getList(getKey(battleReplayId));
        return null;
    }

    private void setCache(int battleReplayId, List<BattleScoreEntity> list) {
        // GameData.getCache().setList(getKey(battleReplayId) , list);
    }

    private void removeCache(int battleReplayId) {
        GameData.getCache().removeCache(getKey(battleReplayId));
    }

    //===========DB=============//
    //插入

    /**
     * 保存一条分数记录到DB中
     *
     * @param entity 分数记录
     * @return true 保存成功 false 保存失败
     */
    private boolean insertDB(BattleScoreEntity entity) {
        boolean ret = GameData.getDB().insert(entity);
        if (!ret) {
            //log
        }
        return ret;
    }

    //查询

    /**
     * 根据房间id获取分数列表
     *
     * @param roomId 房间id
     * @return 分数记录列表
     */
    private List<BattleScoreEntity> getBattleScoresByRoomIdDB(int roomId) {
        String sql = "SELECT * FROM battlescore WHERE roomId = ?";
        Object[] param = {roomId};
        List<BattleScoreEntity> battleScoreEntityList = GameData.getDB().list(BattleScoreEntity.class, sql, param);
        return battleScoreEntityList;
    }

    //查询

    /**
     * 根据回放记录的id获取该回放记录的分数列表
     *
     * @param battleReplayId 回放记录的id
     * @return 分数记录列表
     */
    private List<BattleScoreEntity> getBattleScoresByBattleReplayIdDB(int battleReplayId) {
        String sql = "SELECT * FROM battlescore WHERE battleReplayId = ?";
        Object[] param = {battleReplayId};
        List<BattleScoreEntity> battleScoreEntityList = GameData.getDB().list(BattleScoreEntity.class, sql, param);
        return battleScoreEntityList;
    }
}
