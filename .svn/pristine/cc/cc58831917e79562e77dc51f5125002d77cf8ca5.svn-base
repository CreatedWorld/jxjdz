package avatar.apiregister.room;

import avatar.facade.SystemEventHandler;
import avatar.global.InnerCmd;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import avatar.protobuf.SysInner;
import avatar.util.GameData;
import org.springframework.stereotype.Service;

/**
 * 处理中心服务器匹配成功的操作
 */
@Service
public class MatchSucCenter2HallApi extends SystemEventHandler<SysInner.InnerMatchSucCenter2Hall , Session>{
    protected MatchSucCenter2HallApi() {
        super(InnerCmd.Center2Hall_Inner_MatchSuc);
    }

    @Override
    public void method(Session session, SysInner.InnerMatchSucCenter2Hall msg) throws Exception {
        int userId = msg.getUserId();
        int roomId = msg.getRoomId();
        String roomCode = msg.getRoomCode();
        int roomType = msg.getRoomType();
        int roomRounds = msg.getRoomRounds();
        String ip = msg.getRoomServerIp();
        int port = msg.getRoomServerPort();
        int seat = msg.getSeat();

        Session userSession = GameData.getSessionManager().getSessionByUserId(userId);
        if(userSession != null){
            Hall.MatchingSucceedS2C.Builder b = Hall.MatchingSucceedS2C.newBuilder();
            b.setRoomCode(roomCode);
            b.setRoomRounds(roomRounds);
            b.setRoomRule(roomType);
            b.setRoomServerIp(ip);
            b.setRoomServerPort(port);
            b.setSeat(seat);
            userSession.sendClient(Cmd.S2C_Hall_Push_Match_Suc , b.build().toByteArray());
        }
    }
}
