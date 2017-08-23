package avatar.module.trusteeship;

import com.yaowan.game.common.scheduler.ScheduledTask;

public abstract class TrusteeshipTask extends ScheduledTask {

    protected TrusteeshipTask(String name) {
        super(name);
    }

    /**
     * 需要自动托管操作的userId
     */
    protected int userId;

}
