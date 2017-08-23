package avatar.task;

import avatar.module.arena.ArenaConfigDataService;
import avatar.module.room.MatchRoomService;
import com.yaowan.game.common.scheduler.ScheduledTask;

/**
 * 匹配队列
 */
public class MatchTask extends ScheduledTask{
    public MatchTask() {
        super("Match Task");
    }

    @Override
    public void run() {
        try {
            if(System.currentTimeMillis() > ArenaConfigDataService.getInstance().getStartTime()  && System.currentTimeMillis()
                    < ArenaConfigDataService.getInstance().getEndTime()){
                MatchRoomService.getInstance().match();
                MatchRoomService.getInstance().removeMatched();
                MatchRoomService.getInstance().createMatchRoom();
                MatchRoomService.getInstance().handleTimeoutList();
            }
        }catch (Exception e){
            e.printStackTrace();
        }
    }
}
