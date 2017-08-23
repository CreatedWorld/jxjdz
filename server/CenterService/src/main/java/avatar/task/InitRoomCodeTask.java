package avatar.task;

import avatar.module.room.GeneratorRoomCodeService;
import com.yaowan.game.common.scheduler.ScheduledTask;

/**
 * 系统启动，首先生成n个房间号roomCode在内存（队列）中,<br></br>
 * 每次创建房间的时候，从内存中拿一个roomCode。直到剩下x个了，再次生成到数量为n个在内存中
 */
public class InitRoomCodeTask extends ScheduledTask{
    public InitRoomCodeTask() {
        super("init roomCode in list");
    }

    @Override
    public void run() {
        GeneratorRoomCodeService.getInstance().initInterval();
        GeneratorRoomCodeService.getInstance().initRoomCode();
    }
}
