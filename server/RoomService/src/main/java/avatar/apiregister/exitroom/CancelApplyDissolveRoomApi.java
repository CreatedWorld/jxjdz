package avatar.apiregister.exitroom;

import avatar.entity.room.Room;
import avatar.entity.userinfo.UserEntity;
import avatar.facade.SystemEventHandler;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.dao.MahjongGameDataDao;
import avatar.module.room.service.RoomDataService;
import avatar.module.user.UserDao;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import avatar.util.GameData;
import org.apache.commons.collections.CollectionUtils;
import org.springframework.stereotype.Service;

import java.util.List;

/**
 * 处理玩家取消申请解散房间操作<br></br>
 * 有一个玩家执行取消，那么该房间的所有申请解散的操作作废
 */
@Service
public class CancelApplyDissolveRoomApi extends SystemEventHandler<Battle.CancelDissolveRoomC2S, Session> {
    protected CancelApplyDissolveRoomApi() {
        super(Cmd.C2S_ROOM_CANCEL_APPLY_DISSOLVE);
    }

    @Override
    public void method(Session session, Battle.CancelDissolveRoomC2S msg) throws Exception {
        int userId = session.getUserId();
        //删除申请解散队列

        Room room = RoomDataService.getInstance().getInRoomByUserId(userId);
        List<Integer> roomPlayers = RoomDataService.getInstance().getRoomUserIds(room.getId());

        Battle.CancelDissolveRoomS2C.Builder builder;

        // 有一个玩家执行取消，那么该房间的所有申请解散的操作作废
        if (CollectionUtils.isNotEmpty(roomPlayers)) {
            MahjongGameData mahjongGameData = MahjongGameDataDao.getInstance().get(room.getId());
            if (mahjongGameData != null && CollectionUtils.isNotEmpty(mahjongGameData.getApplyDissolveList())) {
                mahjongGameData.getApplyDissolveList().clear();
            }
        }

        for (Integer id : roomPlayers) {
            Session userSession = GameData.getSessionManager().getSessionByUserId(id);
            if (userSession != null) {
                UserEntity userEntity = UserDao.getInstance().loadUserEntityByUserId(id);
                if (userEntity != null) {
                    builder = Battle.CancelDissolveRoomS2C.newBuilder();
                    builder.setUserId(userId);
                    // builder.setUserName(userEntity.getNickName());
                    userSession.sendClient(Cmd.S2C_ROOM_CANCEL_APPLY_DISSOLVE, builder.build().toByteArray());
                }
            }
        }
    }
}
