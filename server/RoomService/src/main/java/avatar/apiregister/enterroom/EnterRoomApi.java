package avatar.apiregister.enterroom;

import avatar.facade.SystemEventHandler;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import org.springframework.stereotype.Service;

/**
 * 响应玩家登录到房间服务器
 */
@Service
public class EnterRoomApi extends SystemEventHandler<Battle.EnterRoomServerC2S , Session>{
    protected EnterRoomApi() {
        super(Cmd.C2S_ENTER_ROOM);
    }

    @Override
    public void method(Session session, Battle.EnterRoomServerC2S msg) throws Exception {
        if(session != null){
            Battle.EnterRoomServerS2C.Builder builder = Battle.EnterRoomServerS2C.newBuilder();
            session.sendClient(Cmd.S2C_ENTER_ROOM , builder.build().toByteArray());
        }
    }
}
