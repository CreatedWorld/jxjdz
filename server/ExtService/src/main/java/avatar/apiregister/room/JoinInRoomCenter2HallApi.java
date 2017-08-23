package avatar.apiregister.room;

import avatar.facade.SystemEventHandler;
import avatar.global.ClientCode;
import avatar.global.InnerCmd;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import avatar.protobuf.SysInner;
import avatar.util.GameData;
import avatar.util.LogUtil;
import org.springframework.stereotype.Service;

/**
 * 接收到中心服务器的加入房间的响应
 */
@Service
public class JoinInRoomCenter2HallApi extends SystemEventHandler<SysInner.InnerJoinInRoomCenter2Hall, Session> {
    protected JoinInRoomCenter2HallApi() {
        super(InnerCmd.Center2Hall_Inner_JoinInRoom);
    }

    @Override
    public void method(Session session, SysInner.InnerJoinInRoomCenter2Hall msg) throws Exception {
        if (session == null) {
            return;
        }
        int roomPort = msg.getRoomServerPort();
        String roomIp = msg.getRoomServerIp();
        int roomType = msg.getRoomType();
        int userId = msg.getUserId();
        int clientCode = msg.getClientCode();
        String roomCode = msg.getRoomCode();
        int seat = msg.getSeat();
        int round = msg.getRounds();

        Session userSession = GameData.getSessionManager().getSessionByUserId(userId);
        if (userSession == null) {
            return;
        }

        Hall.JoinInRoomS2C.Builder builder = Hall.JoinInRoomS2C.newBuilder();
        builder.setClientCode(clientCode);
        if (clientCode == ClientCode.SUCCESS) {
            builder.setRoomServerIp(roomIp);
            builder.setRoomServerPort(roomPort);
            builder.setRoomRule(roomType);
            builder.setRoomRounds(round);
            builder.setSeat(seat);
        }

        LogUtil.getLogger().debug("S2C_Hall_JOIN_IN_ROOM:{} receiveUserId={}\n{}"
                , Cmd.S2C_Hall_JOIN_IN_ROOM
                , userId
                , builder.build().toString()
        );
        userSession.sendClient(Cmd.S2C_Hall_JOIN_IN_ROOM, builder.build().toByteArray());
    }
}
