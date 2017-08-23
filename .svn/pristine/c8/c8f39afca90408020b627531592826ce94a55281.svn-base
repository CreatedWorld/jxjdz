package avatar.task;

import avatar.InitExtDBConfing;
import com.yaowan.game.common.scheduler.ScheduledTask;

/**
 * 定时从数据库中读取最新的配置
 */
public class ReloadConfigTask extends ScheduledTask{
    public ReloadConfigTask() {
        super("reload config");
    }

    @Override
    public void run() {
        InitExtDBConfing.initAllConfig();
    }
}
