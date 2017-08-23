package avatar.apiregister.center;

import avatar.facade.SystemEventHandler;
import avatar.global.InnerCmd;
import avatar.module.room.ExitRoomService;
import avatar.net.session.Session;
import avatar.protobuf.SysInner;
import org.springframework.stereotype.Service;

/**
 * 处理房间服务器发送过来的解散房间的请求
 */
@Service
public class HandleRoomDissolveRoomApi extends SystemEventHandler<SysInner.InnerDissolveRoom2Center , Session>{
    protected HandleRoomDissolveRoomApi() {
        super(InnerCmd.Room2Center_Inner_DISSOLVE);
    }

    @Override
    public void method(Session session, SysInner.InnerDissolveRoom2Center msg) throws Exception {
        int userId = msg.getUserId();

        SysInner.InnerDissolveCenter2Room.Builder builder =
                SysInner.InnerDissolveCenter2Room.newBuilder();

        int ret = ExitRoomService.getInstance().dissolveRoom(userId , builder);
        builder.setClientCode(ret);
        builder.setUserId(userId);

        session.sendClient(InnerCmd.Center2Room_Inner_DISSOLVE , builder.build().toByteArray());
    }
}
