package avatar.apiregister.center;

import avatar.facade.SystemEventHandler;
import avatar.global.InnerCmd;
import avatar.module.room.MatchRoomService;
import avatar.net.session.Session;
import avatar.protobuf.SysInner;
import org.springframework.stereotype.Service;

/**
 * 处理大厅发送的匹配请求
 */
@Service
public class HallMatchApi extends SystemEventHandler<SysInner.InnerMatchHall2Center , Session>{


    protected HallMatchApi() {
        super(InnerCmd.Hall2Center_Inner_MatchRoom);
    }

    @Override
    public void method(Session session, SysInner.InnerMatchHall2Center msg) throws Exception {
        int userId = msg.getUserId();
        int roomType = msg.getRoomType();
        int roomRoundes = msg.getRoomRounds();
        String hallServerLocalName = msg.getHallServerLocalName();
        int ret = MatchRoomService.getInstance().putUserIntoMatchList(userId , roomType , roomRoundes , hallServerLocalName);
        SysInner.InnerMatchCenter2Hall.Builder builder =SysInner.InnerMatchCenter2Hall.newBuilder();
        builder.setClientCode(ret);
        builder.setUserId(userId);
        session.sendClient(InnerCmd.Center2Hall_Inner_MatchRoom , builder.build().toByteArray());
    }
}
