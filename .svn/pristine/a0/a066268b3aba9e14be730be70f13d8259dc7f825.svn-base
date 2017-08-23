package avatar.apiregister.center;

import avatar.entity.match.UserMatchData;
import avatar.facade.SystemEventHandler;
import avatar.global.ClientCode;
import avatar.global.InnerCmd;
import avatar.module.room.MatchRoomService;
import avatar.net.session.Session;
import avatar.protobuf.SysInner;
import org.springframework.stereotype.Service;

/**
 * 处理大厅发送过来的取消匹配操作
 */
@Service
public class HallCancelMatchApi extends SystemEventHandler<SysInner.InnerCancelMatchHall2Center , Session> {
    protected HallCancelMatchApi() {
        super(InnerCmd.Hall2Center_Inner_CancelMatchRoom);
    }

    @Override
    public void method(Session session, SysInner.InnerCancelMatchHall2Center msg) throws Exception {
        int userId = msg.getUserId();
        int roomType = msg.getRoomType();
        int roomRounds = msg.getRoomRounds();
        UserMatchData data = MatchRoomService.getInstance().cancelMatch(userId , roomType , roomRounds);
        int ret = ClientCode.FAILED;
        if(data != null){
            ret = ClientCode.SUCCESS;
        }
        SysInner.InnerCancelMatchCenter2Hall.Builder builder = SysInner.InnerCancelMatchCenter2Hall.newBuilder();
        builder.setUserId(userId);
        builder.setClientCode(ret);
        session.sendClient(InnerCmd.Center2Hall_Inner_CancelMatchRoom , builder.build().toByteArray());
    }
}
