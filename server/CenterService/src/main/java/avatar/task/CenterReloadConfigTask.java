package avatar.task;

import avatar.InitCenterDBConfig;
import com.yaowan.game.common.scheduler.ScheduledTask;

/**
 * 定时从数据库中加载配置
 */
public class CenterReloadConfigTask extends ScheduledTask{
    public CenterReloadConfigTask() {
        super("Center Reload Config");
    }

    @Override
    public void run() {
        InitCenterDBConfig.initAllConfig();
    }
}
