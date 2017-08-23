package avatar.module.arena.service;

import avatar.entity.arena.ArenaRankingEntity;
import avatar.module.arena.dao.ArenaRankingDao;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import java.util.List;

/**
 *比赛场排名接口
 */
public class ArenaRankingService {

    private static final Log log= LogFactory.getLog(ArenaRankingService.class);
    private ArenaRankingService() {
    }
    private static final ArenaRankingService instance=new ArenaRankingService();
    public static final ArenaRankingService getInstance(){
        return instance;
    }
    /**
     * 获取比赛场最新前10名排名信息
     */
    public void getArenaRankingLists(){
        List<ArenaRankingEntity> entityList = ArenaRankingDao.getInstance().loadRankEntity();
        for (ArenaRankingEntity entity:entityList) {
            System.out.print("玩家名称："+entity.getUserName());
            System.out.print("玩家分数："+entity.getScore());
            System.out.println();
        }
    }
    /**
     *添加最新的比赛场排名
     */
    public boolean insertArenaRankList(List<ArenaRankingEntity> list){
        return ArenaRankingDao.getInstance().insertRankEntityList(list);
    }
}
