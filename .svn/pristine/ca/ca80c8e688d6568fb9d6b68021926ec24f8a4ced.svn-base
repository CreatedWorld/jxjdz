package avatar.apiregister.center;

import avatar.facade.SystemEventHandler;
import avatar.global.InnerCmd;
import avatar.module.room.CreateRoomService;
import avatar.net.session.Session;
import avatar.protobuf.SysInner;
import org.springframework.stereotype.Service;

import java.util.List;

/**
 * 处理大厅服务器创建房间的接口
 */
@Service
public class HallCheckCreateRoomApi extends SystemEventHandler<SysInner.InnerCreateRoomHall2Center, Session> {
    protected HallCheckCreateRoomApi() {
        super(InnerCmd.Hall2Center_Inner_CreateRoom);
    }

    @Override
    public void method(Session session, SysInner.InnerCreateRoomHall2Center msg) throws Exception {
        int userId = msg.getUserId();
        int roomType = msg.getRoomType();
        int rounds = msg.getRoomRounds();
        int seat = msg.getSeat();
        List<Integer> playType = msg.getPlayTypeList();
        SysInner.InnerCreateRoomCenter2Hall.Builder builder = SysInner.InnerCreateRoomCenter2Hall.newBuilder();
        builder.setRoomType(roomType);
        builder.setUserId(userId);

        if (rounds == 0) {
            rounds = 10;
        }

        int ret = CreateRoomService.getInstance().createRoom(userId, roomType, rounds, userId,
                playType,seat ,builder);
        builder.setClientCode(ret);
        builder.setRoomRounds(rounds);
        builder.setRoomType(roomType);
        session.sendClient(InnerCmd.Center2Hall_Inner_CreateRoom, builder.build().toByteArray());
    }
}
