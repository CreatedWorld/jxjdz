package avatar.module.arena;

import avatar.entity.arena.ArenaConfig;
import avatar.util.DateUtil;

/**
 * 比赛场配置信息service
 */
public class ArenaConfigDataService {
    private static final ArenaConfigDataService instance=new ArenaConfigDataService();
    public static final ArenaConfigDataService getInstance(){
        return instance;
    }
    private static volatile ArenaConfig config = null;
    //系统启动的时候加载
    public void loadArenaConfig(){
        //load db
        config = ArenaConfigDao.getInstance().loadArenaConfig();
    }
    /**
     * 获取比赛场配置信息
     */
    public ArenaConfig getArenaConfig(){
        return config;
    }

    public long getStartTime(){
        String[] arrStart = config.getPlayStartTime().split(":");
        return DateUtil.getTodayMillisecond(Integer.parseInt(arrStart[0]) , Integer.parseInt(arrStart[1]) , 0);
    }

    public long getEndTime(){
        String[] arrStart = config.getPlayEndTime().split(":");
        return DateUtil.getTodayMillisecond(Integer.parseInt(arrStart[0]) , Integer.parseInt(arrStart[1]) , 0);
    }

    public long getStartApplayTime(){
        String[] arrStart = config.getApplyStartTime().split(":");
        return DateUtil.getTodayMillisecond(Integer.parseInt(arrStart[0]) , Integer.parseInt(arrStart[1]) , 0);
    }

    public long getEndApplyTime(){
        String[] arrStart = config.getApplyEndTime().split(":");
        return  DateUtil.getTodayMillisecond(Integer.parseInt(arrStart[0]) , Integer.parseInt(arrStart[1]) , 0);
    }
}
