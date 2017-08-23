package avatar.task;

import avatar.entity.room.Room;
import avatar.global.Config;
import avatar.global.InnerCmd;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.dao.MahjongGameDataDao;
import avatar.module.room.service.RoomDataService;
import avatar.net.session.Session;
import avatar.protobuf.SysInner;
import avatar.util.GameData;
import avatar.util.LogUtil;
import com.yaowan.game.common.scheduler.ScheduledTask;
import org.apache.commons.collections.CollectionUtils;

/**
 * 检查房间的玩家是否在300秒内都选择了同意或拒绝，如果300秒后都未选择，则自动解散
 */
public class CheckDissolveRoomTask extends ScheduledTask {
    private int roomId;

    public CheckDissolveRoomTask(int roomId) {
        super("check dissolve room task");
        this.roomId = roomId;
    }

    @Override
    public void run() {
        LogUtil.getLogger().debug("已经过了300秒，房间id={}的玩家未选择同意或拒绝，正在自动解散房间", roomId);
        Session roomSession = GameData.getSessionManager().getSessionByRemoteServerName(
                Config.getInstance().getCenterServerName());
        if (roomSession != null) {
            Room room = RoomDataService.getInstance().getRoomByRoomId(roomId);
            if (room == null) {
                return;
            }

            MahjongGameData mahjongGameData = MahjongGameDataDao.getInstance().get(roomId);
            if (CollectionUtils.isNotEmpty(mahjongGameData.getApplyDissolveList())) {
                SysInner.InnerDissolveRoom2Center.Builder builder = SysInner.InnerDissolveRoom2Center.newBuilder();
                builder.setUserId(room.getCreateUserId());
                roomSession.sendClient(InnerCmd.Room2Center_Inner_DISSOLVE, builder.build().toByteArray());
            }
        }
    }
}
