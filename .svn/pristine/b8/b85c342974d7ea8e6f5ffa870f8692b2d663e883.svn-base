package avatar.module.announcement;

import com.yaowan.game.common.scheduler.ScheduledTask;

/**
 * 系统定时n分钟轮询一次公告
 */
public class AnnounceTask extends ScheduledTask{
    public AnnounceTask() {
        super("AnnounceTask");
    }

    @Override
    public void run() {
        AnnouncementService.getInstance().getAndSendAnnounce();
    }
}
