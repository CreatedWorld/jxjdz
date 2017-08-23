package avatar.apiregister.center;

import avatar.facade.SystemEventHandler;
import avatar.global.ClientCode;
import avatar.global.InnerCmd;
import avatar.module.room.CreateRoomService;
import avatar.net.session.Session;
import avatar.protobuf.SysInner;
import avatar.util.GameData;
import org.springframework.stereotype.Service;

/**
 * 处理大厅加入
 */
@Service
public class HallCheckJoinInRoomApi extends SystemEventHandler<SysInner.InnerJoinInRoomHall2Center , Session>{
    protected HallCheckJoinInRoomApi() {
        super(InnerCmd.Hall2Center_Inner_JoinInRoom);
    }

    @Override
    public void method(Session session, SysInner.InnerJoinInRoomHall2Center msg) throws Exception {
        String roomCode = msg.getRoomCode();
        int userId = msg.getUserId();
        int seat = msg.getSeat();

        SysInner.InnerJoinInRoomCenter2Hall.Builder builder = SysInner.InnerJoinInRoomCenter2Hall.newBuilder();
        SysInner.InnerJoinInRoomCenter2Room.Builder roomBuild = SysInner.InnerJoinInRoomCenter2Room.newBuilder();
        int ret = CreateRoomService.getInstance().joinRoom(userId , seat , roomCode , builder , roomBuild);
        if(ret != ClientCode.SUCCESS){
            builder.setRoomCode(roomCode);
            builder.setRoomServerIp("");
            builder.setRoomServerPort(0);
            builder.setUserId(userId);
            builder.setRoomType(1);
            builder.setRounds(0);
            builder.setSeat(seat);
        }
        builder.setClientCode(ret);

        session.sendClient(InnerCmd.Center2Hall_Inner_JoinInRoom , builder.build().toByteArray());
        //发送命令给房间服务器。通知当前房间所有的用户
        if (ret == ClientCode.SUCCESS){
            Session roomSession = GameData.getSessionManager().getSessionByRemoteServerName(
                    roomBuild.getRoomServerLocalName());
            if(roomSession == null){
                return;
            }
            roomSession.sendClient(InnerCmd.Center2Room_Inner_JoinInRoom , roomBuild.build().toByteArray());
        }
    }
}
