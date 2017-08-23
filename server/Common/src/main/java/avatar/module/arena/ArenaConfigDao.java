package avatar.module.arena;

import avatar.entity.arena.ArenaConfig;
import avatar.util.GameData;
/**
 * 比赛场配置表
 */
public class ArenaConfigDao {
    private static final ArenaConfigDao instance=new ArenaConfigDao();
    public static final ArenaConfigDao getInstance(){
        return instance;
    }
    public ArenaConfig loadArenaConfig(){
        String sql = "select * from config_arena order by id desc";
        ArenaConfig config = GameData.getDB().get(ArenaConfig.class, sql, new Object[]{});
        return config;
    }

}
