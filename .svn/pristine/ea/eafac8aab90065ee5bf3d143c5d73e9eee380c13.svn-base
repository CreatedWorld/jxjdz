package avatar.module.arena.dao;

import avatar.entity.arena.ArenaApplyUserEntity;
import avatar.util.GameData;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

/**
 *比赛场报名dao
 */
public class ArenaApplyUserEntityDao {
    private static final Log log= LogFactory.getLog(ArenaApplyUserEntityDao.class);
    private ArenaApplyUserEntityDao() {
    }
    private static final ArenaApplyUserEntityDao instance=new ArenaApplyUserEntityDao();
    public static final ArenaApplyUserEntityDao getInstance(){
        return instance;
    }

    private final String key = "ArenaApplyUser_%d";
    private String getKey(int userId){
        return String.format(key , userId);
    }

    /**
     * 获取用户报名信息
     * @param userId
     * @return
     */
    public ArenaApplyUserEntity loadByUserId(int userId){
        //load cache
        ArenaApplyUserEntity entity = loadCache(userId);
        if(entity != null){
            return entity;
        }
        entity = loadDB(userId);
        if(entity != null){
            setCache(entity);
        }
        return entity;
    }

    public boolean update(ArenaApplyUserEntity entity){
        removeCache(entity);
        return updateDB(entity);
    }

    public boolean insert(ArenaApplyUserEntity entity){
        return insertDB(entity);
    }


    private ArenaApplyUserEntity loadCache(int userId){
        return (ArenaApplyUserEntity) GameData.getCache().get(getKey(userId));
    }

    private void setCache(ArenaApplyUserEntity entity){
        GameData.getCache().set(getKey(entity.getUserId()) , entity);
    }

    private void removeCache(ArenaApplyUserEntity entity){
        GameData.getCache().removeCache(getKey(entity.getUserId()));
    }

    private ArenaApplyUserEntity loadDB(int userId){
        return GameData.getDB().get(ArenaApplyUserEntity.class , userId);
    }

    private boolean updateDB(ArenaApplyUserEntity entity){
        boolean ret = GameData.getDB().update(entity);
        if(!ret){
            //log
            log.info("更新用户报名信息失败");
        }
        return ret;
    }

    private boolean insertDB(ArenaApplyUserEntity entity){
        boolean ret = GameData.getDB().insert(entity);
        if(!ret){
            //log
            log.info("插入用户报名信息失败");
        }
        return ret;
    }
}
