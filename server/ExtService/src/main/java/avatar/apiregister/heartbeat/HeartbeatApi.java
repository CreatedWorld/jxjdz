package avatar.apiregister.heartbeat;

import avatar.facade.SystemEventHandler;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import org.springframework.stereotype.Service;

/**
 * 心跳
 */
@Service
public class HeartbeatApi extends SystemEventHandler<Hall.HallBeatC2S, Session> {
    protected HeartbeatApi() {
        super(Cmd.C2S_HALL_BATTLEBEAT);
    }

    @Override
    public void method(Session session, Hall.HallBeatC2S msg) throws Exception {
        session.sendClient(Cmd.S2C_HALL_BATTLEBEAT, Hall.HallBeatS2C.newBuilder().build().toByteArray());
    }
}
