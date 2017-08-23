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
 * 处理中心服务器取消匹配操作
 */
@Service
public class CancelMatchCenter2HallApi extends SystemEventHandler<SysInner.InnerCancelMatchCenter2Hall , Session>{
    protected CancelMatchCenter2HallApi() {
        super(InnerCmd.Center2Hall_Inner_CancelMatchRoom);
    }

    @Override
    public void method(Session session, SysInner.InnerCancelMatchCenter2Hall msg) throws Exception {
        Hall.CancelMatchingS2C.Builder builder = Hall.CancelMatchingS2C.newBuilder();
        int userId = msg.getUserId();
        int ret = msg.getClientCode();
        Session userSession = GameData.getSessionManager().getSessionByUserId(userId);
        if(userSession != null){
            builder.setStatus(ret == ClientCode.SUCCESS ? 1 : 0);
            userSession.sendClient(Cmd.S2C_Hall_Cancel_Match , builder.build().toByteArray());
        }
    }
}
