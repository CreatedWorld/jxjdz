package avatar.module.arena.task;

import avatar.util.trigger.CronTable;

/**
 * 开始竞技赛前n分钟的消息推送
 */
public class ArenaStartTask extends CronTable {
    @Override
    public void run() {
        System.out.println("开始竞技赛前n分钟的消息推送");
    }
}
