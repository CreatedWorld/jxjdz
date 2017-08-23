package avatar.apiregister.chat;

import avatar.entity.room.Room;
import avatar.entity.room.RoomPlayer;
import avatar.facade.SystemEventHandler;
import avatar.module.room.service.RoomDataService;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import avatar.util.GameData;
import com.google.protobuf.ByteString;
import org.springframework.stereotype.Service;

import java.util.List;

/**
 * 转发文字聊天
 */
@Service
public class VoiceChatApi extends SystemEventHandler<Battle.SendVoiceC2S, Session> {
    protected VoiceChatApi() {
        super(Cmd.C2S_ROOM_VOICE_CHAT);
    }

    private RoomDataService roomDataService = RoomDataService.getInstance();

    @Override
    public void method(Session session, Battle.SendVoiceC2S msg) throws Exception {
        int userId = session.getUserId();

        ByteString content = msg.getContent();
        int flag = msg.getFlag();

        Battle.PushVoiceS2C voiceChat = Battle.PushVoiceS2C.newBuilder()
                .setSenderUserId(userId)
                .setContent(content)
                .setFlag(flag)
                .build();
        byte[] bytes = voiceChat.toByteArray();

        Room room = roomDataService.getInRoomByUserId(userId);
        List<RoomPlayer> unReadyPlayers = roomDataService.loadRoomPlayerListByState(room.getId(), RoomPlayer.State.UNREADY.getCode());
        List<RoomPlayer> readyPlayers = roomDataService.loadRoomPlayerListByState(room.getId(), RoomPlayer.State.READY.getCode());
        for (RoomPlayer unReadyPlayer : unReadyPlayers) {
            if (readyPlayers.stream().noneMatch(roomPlayer -> roomPlayer.getPlayerId() == unReadyPlayer.getPlayerId())) {
                readyPlayers.add(unReadyPlayer);
            }
        }

        for (RoomPlayer readyPlayer : readyPlayers) {
            Session playerSession = GameData.getSessionManager().getSessionByUserId(readyPlayer.getPlayerId());
            if (playerSession != null) {
                playerSession.sendClient(Cmd.S2C_ROOM_VOICE_CHAT, bytes);
            }
        }
    }
}
