package avatar.apiregister.checkin;

import avatar.entity.checkin.CheckIn;
import avatar.facade.SystemEventHandler;
import avatar.module.checkin.CheckInService;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import org.springframework.stereotype.Service;

/**
 * 获取签到信息接口
 */
@Service
public class GetCheckInInfoApi extends SystemEventHandler<Hall.GetCheckInInfoC2S , Session>{
    protected GetCheckInInfoApi() {
        super(Cmd.C2S_Hall_GET_CHECKIN_INFO);
    }

    @Override
    public void method(Session session, Hall.GetCheckInInfoC2S msg) throws Exception {
        if(session == null){
            return;
        }
        int userId = session.getUserId();
        if(userId == 0){
            return;
        }
        CheckIn checkIn = CheckInService.getInstance().getCheckInInfo(userId);
        
        Hall.GetCheckInInfoS2C.Builder builder = Hall.GetCheckInInfoS2C.newBuilder();
        builder.setDays(checkIn.getCheckInDays());
        builder.setStatus(checkIn.getStatus());

        session.sendClient(Cmd.S2C_Hall_GET_CHECKIN_INFO , builder.build().toByteArray());
    }
}
