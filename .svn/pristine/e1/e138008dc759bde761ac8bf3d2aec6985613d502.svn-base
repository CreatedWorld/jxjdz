package avatar.apiregister.room;

import avatar.facade.SystemEventHandler;
import avatar.global.ClientCode;
import avatar.global.InnerCmd;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import avatar.protobuf.SysInner;
import avatar.util.GameData;
import org.springframework.stereotype.Service;

/**
 * 处理中心服务器返回来的匹配信息
 */
@Service
public class MatchCenter2HallApi extends SystemEventHandler<SysInner.InnerMatchCenter2Hall , Session>{

    protected MatchCenter2HallApi() {
        super(InnerCmd.Center2Hall_Inner_MatchRoom);
    }

    @Override
    public void method(Session session, SysInner.InnerMatchCenter2Hall msg) throws Exception {

        Hall.ApplyMatchingS2C.Builder builder = Hall.ApplyMatchingS2C.newBuilder();
        int userId = msg.getUserId();
        int ret = msg.getClientCode();

        Session userSession = GameData.getSessionManager().getSessionByUserId(userId);
        if(userSession != null){
            builder.setStatus(ret == ClientCode.SUCCESS ? 1 : 0);
            userSession.sendClient(Cmd.S2C_Hall_Match , builder.build().toByteArray());
        }
    }
}
