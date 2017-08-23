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
 * 处理玩家取消匹配操作
 */
@Service
public class CancelMatchApi extends SystemEventHandler<Hall.CancelMatchingC2S ,Session>{
    protected CancelMatchApi() {
        super(Cmd.C2S_Hall_Cancel_Match);
    }

    @Override
    public void method(Session session, Hall.CancelMatchingC2S msg) throws Exception {
        int userId = session.getUserId();
        int roomType = msg.getRoomType();
        int roomRounds = msg.getRoomRounds();
        String name = Config.getInstance().getLocalServerName();
        Session center = GameData.getSessionManager().getSessionByRemoteServerName(Config.getInstance().getCenterServerName());
        if(center !=null){
            SysInner.InnerCancelMatchHall2Center.Builder builder = SysInner.InnerCancelMatchHall2Center.newBuilder();
            builder.setUserId(userId);
            builder.setRoomType(roomType);
            builder.setRoomRounds(roomRounds);
            builder.setHallServerLocalName(name);
            center.sendClient(InnerCmd.Hall2Center_Inner_CancelMatchRoom , builder.build().toByteArray());
        }
    }
}
