package avatar.apiregister.exitroom;

import avatar.facade.SystemEventHandler;
import avatar.global.ClientCode;
import avatar.global.InnerCmd;
import avatar.module.room.service.RoomDataService;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import avatar.protobuf.SysInner;
import avatar.util.GameData;
import org.springframework.stereotype.Service;

import java.util.Arrays;
import java.util.List;

/**
 * 监听到中心服务器对玩家退出房间事件的处理
 */
@Service
public class ExitRoomListeningApi extends SystemEventHandler<SysInner.InnerExitCenter2Room , Session>{
    protected ExitRoomListeningApi() {
        super(InnerCmd.Center2Room_Inner_EXIT);
    }

    @Override
    public void method(Session session, SysInner.InnerExitCenter2Room msg) throws Exception {
        int userId = msg.getUserId();
        int ret = msg.getClientCode();
        Battle.ExitRoomS2C.Builder builder = Battle.ExitRoomS2C.newBuilder();
        builder.setClientCode(ret);
        builder.setUserId(userId);
        if(ret != ClientCode.SUCCESS){
            Session userSession = GameData.getSessionManager().getSessionByUserId(userId);
            if(userSession != null){
                userSession.sendClient(Cmd.S2C_ROOM_EXIT_BROADCAST , builder.build().toByteArray());
            }
        }else {
            //广播玩家所在的房间其他用户
            List<Integer> ids = msg.getMemberIdsList();
            for (Integer id : ids){
                Session userSession = GameData.getSessionManager().getSessionByUserId(id);
                if(userSession != null){
                    userSession.sendClient(Cmd.S2C_ROOM_EXIT_BROADCAST , builder.build().toByteArray());
                }
            }

            //如果是最后一个退出的，应该要删除房间
            boolean allExit = msg.getAllExit();
            if(allExit) {
                int roomId = msg.getRoomId();
                List<Integer> roomIds = Arrays.asList(roomId);
                RoomDataService.getInstance().deleteBatchRoom(roomIds , roomIds.size() , 0);
            }
        }
    }
}
