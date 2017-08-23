package avatar.apiregister.room;

import avatar.facade.SystemEventHandler;
import avatar.global.InnerCmd;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import avatar.protobuf.SysInner;
import avatar.util.GameData;
import org.springframework.stereotype.Service;

/**
 * 接收到中心服务器对创建房间事件的响应
 */
@Service
public class CreateRoomCenter2HallApi extends SystemEventHandler<SysInner.InnerCreateRoomCenter2Hall , Session>{
    protected CreateRoomCenter2HallApi() {
        super(InnerCmd.Center2Hall_Inner_CreateRoom);
    }

    @Override
    public void method(Session session, SysInner.InnerCreateRoomCenter2Hall msg) throws Exception {
        int clientCode = msg.getClientCode();
        // if(clientCode == ClientCode.SUCCESS){
        //     //消耗资源
        //     RoomType roomType = RoomTypeService.getInstance().getRoomTypeByType(msg.getRoomType());
        //     if(roomType.getIsCost() == 1){
        //         boolean ret = ItemService.getInstance().costItem(msg.getUserId() , roomType.getItemTypeId() ,
        //                 roomType.getNeedScore());
        //         if(!ret){
        //             return;
        //         }
        //     }
        // }
        int seat = msg.getSeat();
        String ip = msg.getRoomServerIp();
        int port = msg.getRoomServerPort();
        String roomCode = msg.getRoomCode();
        Session userSession = GameData.getSessionManager().getSessionByUserId(msg.getUserId());

        Hall.CheckCreateRoomS2C.Builder builder = Hall.CheckCreateRoomS2C.newBuilder();

        builder.setClientCode(clientCode);
        builder.setRoomServerIp(ip);
        builder.setRoomServerPort(port);
        builder.setSeat(seat);
        builder.setRoomCode(roomCode);
        userSession.sendClient(Cmd.S2C_Hall_CREATE_ROOM , builder.build().toByteArray());
    }
}
