package avatar.task;

import avatar.module.room.CleanRoomService;
import com.yaowan.game.common.scheduler.ScheduledTask;

/**
 * 定时清理房间
 */
public class CleanRoomTask extends ScheduledTask{
    public CleanRoomTask() {
        super("clean room task");
    }

    @Override
    public void run() {
        CleanRoomService.getInstance().recycleRoom();
    }
}
