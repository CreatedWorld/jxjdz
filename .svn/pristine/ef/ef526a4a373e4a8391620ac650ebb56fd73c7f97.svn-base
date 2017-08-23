package avatar.module.arena.dao;

import avatar.entity.arena.ArenaRankingEntity;
import avatar.util.GameData;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import java.util.List;

/**
 * 比赛场排名Dao
 */
public class ArenaRankingDao {
    private static final Log log= LogFactory.getLog(ArenaRankingDao.class);

    private ArenaRankingDao() {
    }

    private static final ArenaRankingDao instance=new ArenaRankingDao();

    public static final ArenaRankingDao getInstance(){
        return instance;
    }

    /**
     * 获取比赛场最新前十名信息
     */
    public List<ArenaRankingEntity> loadRankEntity(){
        //===========load cache
        List<ArenaRankingEntity> entityList= loadCache();
       if(entityList != null){
           return  entityList;
       }
        //===========db
        entityList=loadDB();
        if(entityList != null){
            sendCache(entityList);
        }
        //log
        return entityList;
    }

    private void sendCache(List<ArenaRankingEntity> entityList) {
        //log
    }

    /**
     * 插入最新的比赛场排名信息
     */
    public boolean insertRankEntityList(List<ArenaRankingEntity> rankingEntityList){
        return insertRankEntityListDB(rankingEntityList);
    }

    private boolean insertRankEntityListDB(List<ArenaRankingEntity> rankingEntityList) {
        //修正数据库的数据为旧的数据信息
        GameData.getDB().delete(ArenaRankingEntity.class,"update arenaranking set isNew=0",new Object[]{});
        boolean ret = GameData.getDB().insert(rankingEntityList);
       if(!ret){
           log.info("插入最新的比赛场排名信息失败");
       }
        return ret;
    }


    private List<ArenaRankingEntity> loadCache() {
    return null;
    }
    public List<ArenaRankingEntity> loadDB(){
        return GameData.getDB().list(ArenaRankingEntity.class, "select * from arenaranking where isNew=1  order by  score desc", new Object[]{});
    }

}
