package avatar.module.trusteeship;

import avatar.entity.room.RoomPlayer;
import avatar.module.room.service.RoomDataService;
import avatar.util.trigger.SchedulerSample;
import com.yaowan.game.common.scheduler.ScheduledTask;
import org.apache.commons.collections.CollectionUtils;

import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.TimeUnit;

/**
 * 托管工具
 */
public class TrusteeshipService {

    /**
     * 正在托管的用户
     */
    private static final Map<Integer, Boolean> trusteeshipUserIds = new ConcurrentHashMap<>(1000);

    /**
     * 将用户设置为托管状态
     */
    public static void addTrusteeshipUser(int userId) {
        trusteeshipUserIds.put(userId, true);
    }

    /**
     * 取消用户的托管状态
     */
    public static void removeTrusteeshipUser(int userId) {
        trusteeshipUserIds.remove(userId);
    }

    /**
     * 取消房间里所有用户的托管状态
     */
    public static void removeTrusteeshipUserByRoomId(int roomId) {
        List<RoomPlayer> roomPlayers = RoomDataService.getInstance().loadAllInRoom(roomId);
        removeTrusteeshipUserByRoomPlayers(roomPlayers);
    }

    /**
     * 判断玩家是否在托管状态
     */
    public static boolean isTrusteeship(int userId) {
        return trusteeshipUserIds.containsKey(userId);
    }

    /**
     * 取消roomPlayers托管状态
     */
    public static void removeTrusteeshipUserByRoomPlayers(List<RoomPlayer> roomPlayers) {
        if (CollectionUtils.isEmpty(roomPlayers)) {
            return;
        }
        roomPlayers.forEach(roomPlayer -> trusteeshipUserIds.remove(roomPlayer.getPlayerId()));
    }

    /**
     * 延时指定秒后，执行托管，任务由调用着传入
     */
    public static void trusteeshipTask(int userId, long delay, ScheduledTask task) {
        if (trusteeshipUserIds.containsKey(userId)) {
            SchedulerSample.delayed(TimeUnit.SECONDS.toMillis(delay), task);
        }
    }

}
