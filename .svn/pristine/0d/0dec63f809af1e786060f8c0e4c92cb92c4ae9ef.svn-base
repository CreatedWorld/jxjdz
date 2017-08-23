package avatar.apiregister.exitroom;

import avatar.facade.SystemEventHandler;
import avatar.global.Config;
import avatar.global.InnerCmd;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import avatar.protobuf.SysInner;
import avatar.util.GameData;
import org.springframework.stereotype.Service;

/**
 * 处理玩家解散房间请求
 */
@Service
public class DissolveRoomApi extends SystemEventHandler<Battle.DissolveRoomC2S , Session>{
    protected DissolveRoomApi() {
        super(Cmd.C2S_ROOM_DISSOLVE);
    }

    @Override
    public void method(Session session, Battle.DissolveRoomC2S msg) throws Exception {
        int userId = session.getUserId();
        Session roomSession = GameData.getSessionManager().getSessionByRemoteServerName(
                Config.getInstance().getCenterServerName());
        if(roomSession != null){
            SysInner.InnerDissolveRoom2Center.Builder builder = SysInner.InnerDissolveRoom2Center.newBuilder();
            builder.setUserId(userId);
            roomSession.sendClient(InnerCmd.Room2Center_Inner_DISSOLVE , builder.build().toByteArray());

        }
    }
}
