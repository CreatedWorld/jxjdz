package avatar;

import avatar.task.CenterReloadConfigTask;
import avatar.task.CleanRoomTask;
import avatar.task.InitRoomCodeTask;
import avatar.task.MatchTask;
import avatar.util.trigger.SchedulerSample;

/**
 * 中心服务器定时器初始化
 */
public class CenterInit extends ServerInit{
    @Override
    public void init() {
        SchedulerSample.init();
        SchedulerSample.delayed(5000 , new InitRoomCodeTask());
        SchedulerSample.register(60*60*1000 , 60*1000 , new CleanRoomTask());
        SchedulerSample.register(5*1000 , 5*1000 , new MatchTask());

        SchedulerSample.register(60 * 1000 , 60 * 1000 , new CenterReloadConfigTask());
    }
}
