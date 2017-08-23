package avatar.apiregister.exitroom;

import avatar.entity.room.Room;
import avatar.facade.SystemEventHandler;
import avatar.global.ClientCode;
import avatar.global.Config;
import avatar.global.InnerCmd;
import avatar.module.room.service.RoomDataService;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import avatar.protobuf.SysInner;
import avatar.util.GameData;
import org.springframework.stereotype.Service;

/**
 * 玩家退出房间请求
 */
@Service
public class ExitRoomApi extends SystemEventHandler<Battle.ExitRoomC2S, Session> {
    protected ExitRoomApi() {
        super(Cmd.C2S_ROOM_EXIT);
    }

    @Override
    public void method(Session session, Battle.ExitRoomC2S msg) throws Exception {
        int userId = session.getUserId();
        Room room = RoomDataService.getInstance().getInRoomByUserId(userId);
        if (room == null) {
            return;
        }

        if (room.getState() == Room.State.STARTED.getCode()) {
            Battle.ExitRoomS2C exitRoomS2C = Battle.ExitRoomS2C.newBuilder()
                    .setClientCode(ClientCode.NO_EXIT_BY_ROOM_START)
                    .setUserId(userId)
                    .build();
            session.sendClient(Cmd.S2C_ROOM_EXIT_BROADCAST, exitRoomS2C.toByteArray());
            return;
        }

        //发送给中心服务器处理退出事件
        Session roomSession = GameData.getSessionManager().getSessionByRemoteServerName(
                Config.getInstance().getCenterServerName());
        if (roomSession != null) {
            SysInner.InnerExitRoom2Center.Builder builder = SysInner.InnerExitRoom2Center.newBuilder();
            builder.setUserId(session.getUserId());
            roomSession.sendClient(InnerCmd.Room2Center_Inner_EXIT, builder.build().toByteArray());
        }
    }
}
