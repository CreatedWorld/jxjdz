package avatar.apiregister.room;

import avatar.entity.room.Room;
import avatar.entity.room.RoomPlayer;
import avatar.facade.SystemEventHandler;
import avatar.protobuf.Cmd;
import avatar.module.mahjong.offline.OfflineManager;
import avatar.module.mahjong.service.BaseGameService;
import avatar.module.room.dao.RoomPlayerDao;
import avatar.module.room.service.RoomDataService;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import avatar.util.GameData;
import org.apache.commons.collections.CollectionUtils;
import org.springframework.stereotype.Service;

import java.util.List;

/**
 * 设置用户是否在线
 */
@Service
public class SetOnlineApi extends SystemEventHandler<Battle.OnlineSettingC2S, Session> {
    protected SetOnlineApi() {
        super(Cmd.C2S_ROOM_ONLINESETTING);
    }

    private static final BaseGameService baseGameService = BaseGameService.getInstance();

    private static final RoomDataService roomDataService = RoomDataService.getInstance();

    @Override
    public void method(Session session, Battle.OnlineSettingC2S msg) throws Exception {
        int userId = session.getUserId();
        boolean isOnline = msg.getIsOnline();

        Room room = roomDataService.getInRoomByUserId(userId);
        if (room == null) {
            return;
        }

        if (isOnline) {
            OfflineManager.removeOfflineUser(userId);
        } else {
            OfflineManager.addOfflineUser(userId);
        }

        Battle.PushOnlineSettingS2C.Builder builder = Battle.PushOnlineSettingS2C.newBuilder()
                .setUserId(userId)
                .setIsOnline(isOnline);
        byte[] bytes = builder.build().toByteArray();

        List<RoomPlayer> roomPlayers = RoomPlayerDao.getInstance().loadRoomPlayersByRoomId(room.getId());
        if (CollectionUtils.isNotEmpty(roomPlayers)) {
            roomPlayers.forEach(
                    roomPlayer -> {
                        Session userSession = GameData.getSessionManager().getSessionByUserId(roomPlayer.getPlayerId());
                        if (userSession != null) {
                            userSession.sendClient(
                                    Cmd.S2C_ROOM_ONLINESETTING_BROADCAST,
                                    bytes
                            );
                        }
                    });
        }
    }

}
