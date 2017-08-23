package avatar;

import avatar.task.HeartLinkTask;
import avatar.task.LinkCenterTask;
import avatar.util.trigger.SchedulerSample;

/**
 * Created by Administrator on 2017/4/25.
 */
public class RoomInit extends ServerInit{
    @Override
    public void init() {
         initScheduler();
    }

    private void initScheduler(){
        //启动定时器
        SchedulerSample.init();
        SchedulerSample.delayed(2000 ,new LinkCenterTask());
        SchedulerSample.register(3000 , new HeartLinkTask());
    }
}
