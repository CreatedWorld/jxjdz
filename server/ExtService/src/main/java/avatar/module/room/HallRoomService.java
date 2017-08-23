package avatar.module.room;

import avatar.entity.roomType.RoomType;
import avatar.global.Config;
import avatar.global.InnerCmd;
import avatar.module.item.ItemService;
import avatar.module.roomType.RoomTypeService;
import avatar.net.session.Session;
import avatar.protobuf.SysInner;
import avatar.util.GameData;

import java.util.List;

/**
 * 大厅中处理玩家房间请求
 */
public class HallRoomService {
    private static final HallRoomService instance = new HallRoomService();
    public static final HallRoomService getInstance(){
        return instance;
    }

    public void createRoom(int userId , int roomType , int roomRounds ,int seat ,List<Integer> playType){
        //检查是否存在房间中
        Session session = GameData.getSessionManager().getSessionByRemoteServerName(
                Config.getInstance().getCenterServerName());
        if(session != null){
            //TODO 这里的服务器间不是同步操作，以后需要考虑同步获取远程信息

            RoomType type = RoomTypeService.getInstance().getRoomTypeByType(roomType);
            if(type == null){
                return;
            }
            if(type.getIsCost() == 1){
                if(!ItemService.getInstance().isEnoughItem(userId , type.getItemTypeId() , type.getNeedScore())){
                    //客户端已经验证过一次，这里不做其他验证
                    return;
                }
            }

            SysInner.InnerCreateRoomHall2Center.Builder builder =
                    SysInner.InnerCreateRoomHall2Center.newBuilder();
            builder.setUserId(userId);
            builder.setRoomType(roomType);
            builder.setRoomRounds(roomRounds);
            builder.addAllPlayType(playType);
            builder.setSeat(seat);//如果有带上座位号，那么直接创建一个房间，并且指定座位号

            session.sendClient(InnerCmd.Hall2Center_Inner_CreateRoom ,
                    builder.build().toByteArray());
        }
    }

    public void joinRoom(int userId , String roomCode , int seat){
        Session session = GameData.getSessionManager().getSessionByRemoteServerName(
                Config.getInstance().getCenterServerName());
        if(session !=
                null){

            SysInner.InnerJoinInRoomHall2Center.Builder builder =
                    SysInner.InnerJoinInRoomHall2Center.newBuilder();

            builder.setUserId(userId);
            builder.setRoomCode(roomCode);
            builder.setSeat(seat);
            session.sendClient(InnerCmd.Hall2Center_Inner_JoinInRoom ,
                    builder.build().toByteArray());
        }
    }
}
