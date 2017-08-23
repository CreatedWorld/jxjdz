package avatar.apiregister.checkin;

import avatar.facade.SystemEventHandler;
import avatar.module.checkin.CheckInService;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import org.springframework.stereotype.Service;

/**
 *点击签到的接口
 */
@Service
public class CheckInApi extends SystemEventHandler<Hall.CheckInC2S , Session>{
    protected CheckInApi() {
        super(Cmd.C2S_Hall_CHECKIN);
    }

    @Override
    public void method(Session session, Hall.CheckInC2S msg) throws Exception {
        if(session == null){
            return;
        }
        int userId = session.getUserId();
        if(userId == 0){
            return;
        }
        Hall.CheckInS2C.Builder builder = Hall.CheckInS2C.newBuilder();
        int ret = CheckInService.getInstance().checkIn(userId , builder);
        builder.setClientCode(ret);

        session.sendClient(Cmd.S2C_Hall_CHECKIN , builder.build().toByteArray());
    }
}
