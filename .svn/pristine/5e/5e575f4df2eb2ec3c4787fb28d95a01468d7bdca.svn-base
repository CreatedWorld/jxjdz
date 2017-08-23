package avatar.apiregister.exitroom;

import avatar.entity.room.Room;
import avatar.facade.SystemEventHandler;
import avatar.global.Config;
import avatar.global.InnerCmd;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.dao.MahjongGameDataDao;
import avatar.module.room.service.RoomDataService;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import avatar.protobuf.SysInner;
import avatar.util.GameData;
import org.springframework.stereotype.Service;

import java.util.List;

/**
 *确认同意解散房间操作<br></br>
 * 如果有半数以上的人申请了解散房间，那么执行解散房间操作。<br></br>
 * 否则广播申请解散房间的操作给其他玩家
 */
@Service
public class DissolveComfirmApi extends SystemEventHandler<Battle.DissloveRoomConfirmC2S , Session>{
    protected DissolveComfirmApi() {
        super(Cmd.C2S_ROOM_Comfirm_Dissolve);
    }

    @Override
    public void method(Session session, Battle.DissloveRoomConfirmC2S msg) throws Exception {
        int userId = session.getUserId();
        Room room = RoomDataService.getInstance().getInRoomByUserId(userId);
        if(room == null){
            return;
        }
        MahjongGameData mahjongGameData = MahjongGameDataDao.getInstance().get(room.getId());
        if(mahjongGameData == null){
            return;
        }
        mahjongGameData.putApplyDissolve(userId);
        List<MahjongGameData.ApplyDissolveData> list = mahjongGameData.getApplyDissolveList();
        if(list.size() == 0){
            return;
        }

        //检查是否已经有半数以上的人申请解散了，如果是，直接解散房间
        int size = list.size();
        int roomNum = room.getNeedPlayerNum();
        boolean dissolve = false;
        if(size > (roomNum / 2)){
            dissolve = true;
        }
        if(dissolve){//如果是解散房间，那么发送给中心服务器删除房间消息，之后的处理在监听解散房间消息
            Session roomSession = GameData.getSessionManager().getSessionByRemoteServerName(
                    Config.getInstance().getCenterServerName());
            if(roomSession != null){
                SysInner.InnerDissolveRoom2Center.Builder builder = SysInner.InnerDissolveRoom2Center.newBuilder();
                builder.setUserId(room.getCreateUserId());
                roomSession.sendClient(InnerCmd.Room2Center_Inner_DISSOLVE , builder.build().toByteArray());
            }
        }else {
            //如果没达到解散条件，那么广播申请解散房间消息给其他玩家
            List<Integer> ids = RoomDataService.getInstance().getRoomUserIds(room.getId());
            if(ids == null || ids.size() == 0){
                return;
            }
            for(Integer id : ids){
                Session userSession = GameData.getSessionManager().getSessionByUserId(id);
                if(userSession != null){
                    Battle.DissloveRoomConfirmS2C.Builder builder = Battle.DissloveRoomConfirmS2C.newBuilder();
                    builder.setUserId(userId);
                    userSession.sendClient(Cmd.S2C_ROOM_Comfirm_Dissolve , builder.build().toByteArray());
                }
            }
        }
    }
}
