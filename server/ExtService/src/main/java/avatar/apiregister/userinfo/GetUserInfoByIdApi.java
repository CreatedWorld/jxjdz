package avatar.apiregister.userinfo;

import avatar.facade.SystemEventHandler;
import avatar.module.user.UserService;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import org.springframework.stereotype.Service;

/**
 * 获取别的玩家的信息
 */
@Service
public class GetUserInfoByIdApi extends SystemEventHandler<Hall.GetUserInfoByIdC2S , Session>{
    protected GetUserInfoByIdApi() {
        super(Cmd.C2S_Hall_Get_UserInfo_By_Id);
    }

    @Override
    public void method(Session session, Hall.GetUserInfoByIdC2S msg) throws Exception {
        int id = msg.getUserId();
        Hall.GetUserInfoByIdS2C.Builder builder = Hall.GetUserInfoByIdS2C.newBuilder();
        boolean ret = UserService.getInstance().getOtherUserInfo(id , builder);
        builder.setUserId(id);
        if(ret){
            session.sendClient(Cmd.S2C_Hall_Get_UserInfo_By_Id , builder.build().toByteArray());
        }
    }
}
