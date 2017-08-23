package avatar.apiregister.reconnect;

import avatar.entity.room.Room;
import avatar.entity.room.RoomPlayer;
import avatar.facade.SystemEventHandler;
import avatar.module.room.dao.RoomPlayerDao;
import avatar.module.room.service.RoomDataService;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import org.springframework.stereotype.Service;

/**
 * 处理玩家重连消息。检查玩家是否在房间内
 */
@Service
public class CheckReConnectApi extends SystemEventHandler<Hall.ReConnectC2S, Session> {
    protected CheckReConnectApi() {
        super(Cmd.C2S_Hall_Reconnect);
    }

    @Override
    public void method(Session session, Hall.ReConnectC2S msg) throws Exception {
        int userId = session.getUserId();

        Hall.ReConnectS2C.Builder builder = Hall.ReConnectS2C.newBuilder();
        Room room = RoomDataService.getInstance().getInRoomByUserId(userId);

        if (room != null) {
            RoomPlayer roomPlayer = RoomPlayerDao.getInstance().loadRoomPlayerByPlayerId(userId);
            if (roomPlayer != null) {
                builder.setRoomCode(room.getRoomCode());
                builder.setRoomId(room.getId());
                builder.setRoomIp(room.getRoomIp());
                builder.setRoomPort(room.getPort());
                session.sendClient(Cmd.S2C_Hall_Reconnect, builder.build().toByteArray());
                return;
            }
        }
        builder.setRoomId(0);
        session.sendClient(Cmd.S2C_Hall_Reconnect, builder.build().toByteArray());
    }
}
