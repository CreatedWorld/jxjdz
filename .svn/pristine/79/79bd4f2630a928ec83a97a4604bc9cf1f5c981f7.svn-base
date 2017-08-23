package avatar.apiregister.room;

import avatar.facade.SystemEventHandler;
import avatar.module.room.HallRoomService;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import org.springframework.stereotype.Service;

import java.util.List;

/**
 * 大厅服务器点击创建房间操作
 */
@Service
public class CheckCreateRoomApi extends SystemEventHandler<Hall.CheckCreateRoomC2S , Session> {
    protected CheckCreateRoomApi() {
        super(Cmd.C2S_Hall_CREATE_ROOM);
    }

    @Override
    public void method(Session session, Hall.CheckCreateRoomC2S msg) throws Exception {
        int roomType = msg.getRoomRule();
        int roomRounds = msg.getRoomRounds();
        int userId = session.getUserId();
        int seat = 0;//默认没有座位号
        List<Integer> playType = msg.getPlayTypeList();

        HallRoomService.getInstance().createRoom(userId , roomType , roomRounds , seat,playType);
    }
}
