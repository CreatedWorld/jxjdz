package avatar;

import avatar.module.announcement.AnnounceTask;
import avatar.module.arena.task.ArenaStartTask;
import avatar.module.offline.UserOfflineService;
import avatar.module.user.StopServerService;
import avatar.module.user.UserService;
import avatar.task.*;
import avatar.util.trigger.SchedulerSample;

/**
 * 系统定时器初始化地方
 */
public class ExtInit extends ServerInit {
    @Override
    public void init() {
        initListenInternalEvent();
        initScheduler();
    }

    private void initListenInternalEvent(){
        UserService.getInstance();
        UserOfflineService.getInstance();
        StopServerService.getInstance();
    }

    private void initScheduler() {
        //启动定时器
        SchedulerSample.init();
        SchedulerSample.delayed(2000, new LinkLoginTask());
        SchedulerSample.delayed(2000, new LinkCenterTask());
        SchedulerSample.register(10000, 5000, new HeartLinkTask());

        //系统每10分钟轮询公告内容
        SchedulerSample.register(10000, 5000, new AnnounceTask());
        //每1分钟重新加载配置
        SchedulerSample.register(60 * 1000, 60 * 1000, new ReloadConfigTask());

        //每天晚上20：55分推送公告
        SchedulerSample.cronTable("ArenaStart", "0 55 20 * * *", new ArenaStartTask());

        // SchedulerSample.delayed(5000, new AdminDaoTask());
    }
}