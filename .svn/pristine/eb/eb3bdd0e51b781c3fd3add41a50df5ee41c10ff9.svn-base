package avatar.apiregister.room;

import avatar.facade.SystemEventHandler;
import avatar.global.Config;
import avatar.global.InnerCmd;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import avatar.protobuf.SysInner;
import avatar.util.GameData;
import org.springframework.stereotype.Service;

/**
 * 处理大厅玩家处理匹配玩家接口
 */
@Service
public class MatchApi extends SystemEventHandler<Hall.ApplyMatchingC2S , Session>{
    protected MatchApi() {
        super(Cmd.C2S_Hall_Match);
    }

    @Override
    public void method(Session session, Hall.ApplyMatchingC2S msg) throws Exception {
        int userId = session.getUserId();
        int roomType = msg.getRoomType();
        int roomRounds = msg.getRoomRounds();
        String name = Config.getInstance().getLocalServerName();

        Session center = GameData.getSessionManager().getSessionByRemoteServerName(Config.getInstance().getCenterServerName());
        if(center !=null){
            SysInner.InnerMatchHall2Center.Builder builder = SysInner.InnerMatchHall2Center.newBuilder();
            builder.setUserId(userId);
            builder.setRoomType(roomType);
            builder.setRoomRounds(roomRounds);
            builder.setHallServerLocalName(name);
            center.sendClient(InnerCmd.Hall2Center_Inner_MatchRoom , builder.build().toByteArray());
        }
    }
}
