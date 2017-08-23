package avatar.apiregister.enterroom;

import avatar.entity.room.RoomPlayer;
import avatar.entity.userinfo.UserEntity;
import avatar.facade.SystemEventHandler;
import avatar.global.InnerCmd;
import avatar.module.mahjong.offline.OfflineManager;
import avatar.module.room.service.RoomDataService;
import avatar.module.user.UserDao;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import avatar.protobuf.SysInner;
import avatar.util.GameData;
import avatar.util.LogUtil;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;

import java.util.List;

/**
 * 房间服务器监听到玩家加入房间消息，需要广播给其他用户
 */
@Service
public class JoinInRoomListeningApi extends SystemEventHandler<SysInner.InnerJoinInRoomCenter2Room, Session> {

    private static final Logger logger = LoggerFactory.getLogger(JoinInRoomListeningApi.class);

    protected JoinInRoomListeningApi() {
        super(InnerCmd.Center2Room_Inner_JoinInRoom);
    }

    @Override
    public void method(Session session, SysInner.InnerJoinInRoomCenter2Room msg) throws Exception {
        int userId = msg.getUserId();
        int seat = msg.getSeat();
        int roomId = msg.getRoomId();

        List<RoomPlayer> roomPlayers = RoomDataService.getInstance().loadAllInRoom(roomId);
        if (roomPlayers == null || roomPlayers.size() == 0) {
            return;
        }

        RoomPlayer joined = null;
        for (RoomPlayer roomPlayer : roomPlayers) {
            if (roomPlayer.getPlayerId() == userId) {
                joined = roomPlayer;
                break;
            }
        }

        for (RoomPlayer roomPlayer : roomPlayers) {
            if (roomPlayer.getPlayerId() == userId) {
                continue;
            }

            Battle.PushJoinS2C.Builder builder = Battle.PushJoinS2C.newBuilder();
            Battle.PlayerInfoVOS2C.Builder playerInfo = Battle.PlayerInfoVOS2C.newBuilder();
            playerInfo.setUserId(userId);
            UserEntity userEntity = UserDao.getInstance().loadUserEntityByUserId(userId);
            String name = "";
            if (userEntity != null) {
                name = userEntity.getNickName();
            }
            playerInfo.setName(name);
            playerInfo.setSit(seat);
            playerInfo.setIsMaster(roomPlayer.getSeat() == 1);
            playerInfo.setHeadIcon(userEntity == null ? "" : userEntity.getImageUrl());
            playerInfo.setSex(userEntity == null ? 0 : userEntity.getSex());
            playerInfo.setIsReady(joined.getState() == RoomPlayer.State.READY.getCode());
            playerInfo.setIsOnline(!OfflineManager.isOffline(userId));

            builder.setPlayerInfo(playerInfo);

            Session userSession = GameData.getSessionManager().getSessionByUserId(roomPlayer.getPlayerId());
            if (userSession != null) {
                LogUtil.getLogger().debug("S2C_ROOM_JOIN_ROOM_BROADCAST:{} receiveUserId={}\n{}"
                        , Cmd.S2C_ROOM_JOIN_ROOM_BROADCAST
                        , roomPlayer.getPlayerId()
                        , builder.build().toString()
                );
                userSession.sendClient(Cmd.S2C_ROOM_JOIN_ROOM_BROADCAST, builder.build().toByteArray());
            }
        }
    }
}
