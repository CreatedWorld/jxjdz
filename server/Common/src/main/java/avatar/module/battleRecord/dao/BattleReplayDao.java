package avatar.module.battleRecord.dao;

import avatar.entity.battlerecord.BattleReplayEntity;
import avatar.util.GameData;

import java.util.List;

/**
 * 玩家战斗回放数据接口
 */
public class BattleReplayDao {
    private BattleReplayDao() {
    }

    private static final BattleReplayDao instance = new BattleReplayDao();

    public static final BattleReplayDao getInstance() {
        return instance;
    }

    private final String key = "battleReplay_%d";

    //=============接口==================//
    private String getKey(int roomId) {
        return String.format(key, roomId);
    }

    //insert

    /**
     * 保存该房间当前局的回放记录的信息<br></br>
     * 注意：这里插入后立即返回插入的数据。调用者必须保证线性安全
     *
     * @param entity 回访记录
     * @return 0 插入失败；1插入成功；
     */
    public int insertBattleReplayAndReturnId(BattleReplayEntity entity) {
        boolean ret = insertDB(entity);
        if (ret) {
            removeCache(entity.getRoomId());
            BattleReplayEntity last = loadLastId();
            if (last == null) {
                return 0;
            }
            return last.getId();
        }
        return 0;
    }

    /**
     * 根据房间id获取该房间所有的回放记录
     *
     * @param roomId 房间id
     * @return 该房间所有的回放记录
     */
    public List<BattleReplayEntity> getBattleReplaysByRoomId(int roomId) {
        List<BattleReplayEntity> list = loadCacheList(roomId);
        if (list != null && list.size() > 0) {
            return list;
        }
        list = getBattleReplaysByRoomIdDB(roomId);
        if (list != null && list.size() > 0) {
            setCache(roomId, list);
        }
        return list;
    }

    /**
     * 根据房间id和局获取一条回放记录
     *
     * @param roomId  房间id
     * @param innings 局
     * @return 一条回放记录
     */
    public BattleReplayEntity getReplayContentByRoomIdAndInnings(int roomId, int innings) {
        List<BattleReplayEntity> list = getBattleReplaysByRoomId(roomId);
        for (BattleReplayEntity battleReplayEntity : list) {
            if (battleReplayEntity.getInnings() == innings) {
                return battleReplayEntity;
            }
        }
        return null;
    }

    //==============cache===================//
    private List<BattleReplayEntity> loadCacheList(int roomId) {
        // return GameData.getCache().getList(getKey(roomId));
        return null;
    }

    private void removeCache(int roomId) {
        GameData.getCache().removeCache(getKey(roomId));
    }

    private void setCache(int roomId, List<BattleReplayEntity> list) {
        // GameData.getCache().setList(getKey(roomId) , list);
    }

    //==============db=====================//

    /**
     * 保存一条回放记录到DB中
     *
     * @param entity 回放记录
     * @return true 保存成功； false 保存失败
     */
    private boolean insertDB(BattleReplayEntity entity) {
        String sql = "insert into battlereplay (`roomId`, `roomCode`, `innings`, `replayContent`) " +
                "VALUE(?,?,?,?);";
        boolean ret = GameData.getDB().insert(sql, new Object[]{entity.getRoomId(), entity.getRoomCode(), entity.getInnings(),
                entity.getReplayContent()});
        return ret;
    }

    private BattleReplayEntity loadLastId() {
        String sql = "select last_insert_id() as id;";
        return GameData.getDB().get(BattleReplayEntity.class, sql, new Object[]{});
    }

    /**
     * 根据房间id查询该房间所有的回放记录
     *
     * @param roomId 房间id
     * @return 该房间所有的回放记录
     */
    private List<BattleReplayEntity> getBattleReplaysByRoomIdDB(int roomId) {
        String sql = "SELECT * FROM battlereplay WHERE roomId = ? ORDER BY gamesTime ASC";
        Object[] param = {roomId};
        List<BattleReplayEntity> battleReplays = GameData.getDB().list(BattleReplayEntity.class, sql, param);
        return battleReplays;
    }
}
