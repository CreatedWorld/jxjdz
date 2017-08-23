package avatar.apiregister.exitroom;

import avatar.entity.room.Room;
import avatar.facade.SystemEventHandler;
import avatar.module.mahjong.MahjongConfig;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.dao.MahjongGameDataDao;
import avatar.module.room.service.RoomDataService;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import avatar.task.CheckDissolveRoomTask;
import avatar.util.GameData;
import avatar.util.trigger.SchedulerSample;
import org.springframework.stereotype.Service;

import java.util.List;

/**
 * 申请解散房间<br></br>
 * 广播给其他玩家，有用户申请解散房间操作
 */
@Service
public class ApplyDissolveRoomApi extends SystemEventHandler<Battle.ApplyDissolveRoomC2S, Session> {
    protected ApplyDissolveRoomApi() {
        super(Cmd.C2S_ROOM_APPLY_DISSOLVE);
    }

    @Override
    public void method(Session session, Battle.ApplyDissolveRoomC2S msg) throws Exception {
        int userId = session.getUserId();
        Room room = RoomDataService.getInstance().getInRoomByUserId(userId);
        if (room == null) {
            return;
        }
        //检查是否已经申请了，如果是，不处理
        MahjongGameData mahjongGameData = MahjongGameDataDao.getInstance().get(room.getId());
        if (mahjongGameData == null) {
            return;
        }
        mahjongGameData.putApplyDissolve(userId);

        SchedulerSample.delayed(MahjongConfig.DISSOLVE_REMAIN_TIME * 1000, new CheckDissolveRoomTask(room.getId()));

        List<Integer> ids = RoomDataService.getInstance().getRoomUserIds(room.getId());
        if (ids == null || ids.size() == 0) {
            return;
        }
        for (Integer id : ids) {
            Session userSession = GameData.getSessionManager().getSessionByUserId(id);
            if (userSession != null) {
                Battle.ApplyDissolveRoomS2C.Builder builder = Battle.ApplyDissolveRoomS2C.newBuilder();
                builder.setUserId(userId);
                userSession.sendClient(Cmd.S2C_ROOM_APPLY_DISSOLVE, builder.build().toByteArray());
            }
        }
    }
}
