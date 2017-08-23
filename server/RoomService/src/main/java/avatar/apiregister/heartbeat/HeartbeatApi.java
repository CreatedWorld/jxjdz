package avatar.apiregister.heartbeat;

import avatar.facade.SystemEventHandler;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import org.springframework.stereotype.Service;

/**
 * 心跳
 */
@Service
public class HeartbeatApi extends SystemEventHandler<Battle.BattleBeatC2S, Session> {
    protected HeartbeatApi() {
        super(Cmd.C2S_ROOM_BATTLEBEAT);
    }

    @Override
    public void method(Session session, Battle.BattleBeatC2S msg) throws Exception {
        session.sendClient(Cmd.S2C_ROOM_BATTLEBEAT, Battle.BattleBeatS2C.newBuilder().build().toByteArray());
    }
}
