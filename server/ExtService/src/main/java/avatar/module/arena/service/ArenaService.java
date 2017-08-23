package avatar.module.arena.service;

import avatar.entity.arena.ArenaApplyUserEntity;
import avatar.entity.arena.ArenaConfig;
import avatar.module.arena.ArenaConfigDataService;
import avatar.module.arena.dao.ArenaApplyUserEntityDao;
import avatar.protobuf.Hall;
import avatar.util.DateUtil;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.Date;

/**
 * 比赛场 报名数据接口
 */
public class ArenaService {
    private  static final Logger logger= LoggerFactory.getLogger(ArenaService.class);
    private static final Log log= LogFactory.getLog(ArenaService.class);
    private ArenaService() {
    }
    private static final ArenaService instance=new ArenaService();
    public static final ArenaService getInstance(){
        return instance;
    }

    /**
     * 获取比赛场配置信息
     */
    public void getArenaInfo(int userId,Hall.CheckApplyStatusS2C.Builder builder){
        ArenaApplyUserEntity entity = ArenaApplyUserEntityDao.getInstance().loadByUserId(userId);
        int status = ArenaApplyUserEntity.Status.NORMAL.getId();
        if(entity != null){
            status = entity.getIsApply();
        }
        ArenaConfig config = ArenaConfigDataService.getInstance().getArenaConfig();

        String startTime = config.getPlayStartTime();
        String[] arrStart = startTime.split(":");
        long start = DateUtil.getTodayMillisecond(Integer.parseInt(arrStart[0]) , Integer.parseInt(arrStart[1]) , 0);
        String endTime = config.getPlayEndTime();
        String[] arrEnd = endTime.split(":");
        long end = DateUtil.getTodayMillisecond(Integer.parseInt(arrEnd[0]) , Integer.parseInt(arrEnd[1]) , 0);

        builder.setCurrentTime(System.currentTimeMillis());//服务器当前时间
        builder.setStartTime(start);
        builder.setEndTime(end);
        builder.setStatus(status);
    }

    /**
     * 玩家报名
     */
    public void applyArena(int userId, Hall.ApplyCompetitionS2C.Builder builder){
        int status = ArenaApplyUserEntity.Status.NORMAL.getId();  // 未报名状态
        //查询玩家是否存在报名记录 如果存在  在判断玩家是否已经报名 不存在 新增一条报名记录
        ArenaApplyUserEntity entity = ArenaApplyUserEntityDao.getInstance().loadByUserId(userId);
        if(entity == null){
            //不存在报名信息
            //新增报名记录
            ArenaApplyUserEntity arenaApplyUserEntity=new ArenaApplyUserEntity();
            arenaApplyUserEntity.setApplyTime(new Date());
            arenaApplyUserEntity.setUserId(userId);
            arenaApplyUserEntity.setIsApply(1);
            boolean ret = ArenaApplyUserEntityDao.getInstance().insert(arenaApplyUserEntity);
            if(ret==true){
                builder.setStatus(1);
            }else{
                builder.setStatus(0);
            }
        }else{
            if(entity.getIsApply()==status){
                //用户未报名 修改用户为报名状态
                entity.setIsApply(1);
                entity.setApplyTime(new Date());
                boolean ret = ArenaApplyUserEntityDao.getInstance().update(entity);
                if(ret==true){
                    builder.setStatus(1);
                }else{
                    builder.setStatus(0);
                }
            }else{
                builder.setStatus(1);
            }
        }
    }

    /**
     *申请参加比赛
     */
    public void JoinArena(int userId, Hall.JoinCompetitionS2C.Builder builder) {
        ArenaApplyUserEntity entity = ArenaApplyUserEntityDao.getInstance().loadByUserId(userId);
        int status = ArenaApplyUserEntity.Status.NORMAL.getId();//未报名状态
        if(entity == null){//不存在用户报名信息
            builder.setStatus(status);
            log.info("****用户不可以申请参加比赛场*******************");
        }else{//存在用户报名信息
            if(entity.getIsApply()==status){ //用户未报名
                builder.setStatus(status);
                log.info("****用户不可以申请参加比赛场*****************");
            }else{
                builder.setStatus(ArenaApplyUserEntity.Status.APPLY.getId());
                log.info("****用户可以申请参加比赛场******************");
            }
        }
    }
}
