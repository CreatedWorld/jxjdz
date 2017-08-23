package avatar.apiregister.center;

import avatar.facade.SystemEventHandler;
import avatar.global.InnerCmd;
import avatar.module.room.ExitRoomService;
import avatar.net.session.Session;
import avatar.protobuf.SysInner;
import org.springframework.stereotype.Service;

/**
 * 处理房间玩家退出房间消息
 */
@Service
public class HandleRoomExitRoomApi extends SystemEventHandler<SysInner.InnerExitRoom2Center , Session>{
    protected HandleRoomExitRoomApi() {
        super(InnerCmd.Room2Center_Inner_EXIT);
    }

    @Override
    public void method(Session session, SysInner.InnerExitRoom2Center msg) throws Exception {
        int userId = msg.getUserId();
        SysInner.InnerExitCenter2Room.Builder builder = SysInner.InnerExitCenter2Room.newBuilder();
        int ret = ExitRoomService.getInstance().exitRoom(userId , builder);

        builder.setUserId(userId);
        builder.setClientCode(ret);
        session.sendClient(InnerCmd.Center2Room_Inner_EXIT , builder.build().toByteArray());
    }
}
