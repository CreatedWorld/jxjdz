package avatar.apiregister.room;

import avatar.facade.SystemEventHandler;
import avatar.module.room.HallRoomService;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import org.springframework.stereotype.Service;

/**
 * 大厅处理玩家加入房间的请求
 */
@Service
public class JoinInRoomApi extends SystemEventHandler<Hall.JoinInRoomC2S , Session>{
    protected JoinInRoomApi() {
        super(Cmd.C2S_Hall_JOIN_IN_ROOM);
    }

    @Override
    public void method(Session session, Hall.JoinInRoomC2S msg) throws Exception {
        if(session == null){
            return;
        }
        int userId = session.getUserId();
        if (userId == 0){
            return;
        }
        String  roomCode = msg.getRoomCode();
        int seat = msg.getSeat();
        System.out.println("================seat = " + seat);
        HallRoomService.getInstance().joinRoom(userId , roomCode , seat);
    }
}
