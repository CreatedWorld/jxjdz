package avatar.apiregister.shop;

import avatar.entity.shop.Admin;
import avatar.entity.shop.PlayerAgent;
import avatar.facade.SystemEventHandler;
import avatar.module.shop.AdminDao;
import avatar.module.shop.PlayerAgentDao;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import org.springframework.stereotype.Service;

/**
 * 商城中验证用户输入的邀请码
 */
@Service
public class ValidateUserCodeApi extends SystemEventHandler<Hall.CheckInvitationCodeC2S, Session> {
    protected ValidateUserCodeApi() {
        super(Cmd.C2S_Hall_Check_Invitation_Code);
    }

    @Override
    public void method(Session session, Hall.CheckInvitationCodeC2S msg) throws Exception {
        int code = msg.getInvitationId();
        int userId = session.getUserId();

        // 邀请码是否正确,1----正确,0----不正确
        int status = 0;

        // 如果已存在代理，则不能更改代理
        PlayerAgent playerAgent = PlayerAgentDao.getInstance().loadPlayerAgentByPlayerId(userId);
        if (playerAgent == null) {
            Admin admin = AdminDao.getInstance().loadAdmin(code);
            if (admin != null && admin.getState() == Admin.State.ENABLE.getId()) {
                playerAgent = new PlayerAgent();
                playerAgent.setAdminId(admin.getAdminId());
                playerAgent.setPlayerId(userId);
                PlayerAgentDao.getInstance().insert(playerAgent);
                status = 1;
            }
        }

        Hall.CheckInvitationCodeS2C.Builder builder = Hall.CheckInvitationCodeS2C.newBuilder();
        builder.setStatus(status);
        session.sendClient(Cmd.S2C_Hall_Check_Invitation_Code, builder.build().toByteArray());
    }
}
