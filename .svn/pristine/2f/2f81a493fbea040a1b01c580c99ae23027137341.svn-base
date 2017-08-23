package avatar.apiregister.userinfo;

import avatar.entity.noticeconfig.NoticeConfig;
import avatar.facade.SystemEventHandler;
import avatar.module.noticeconfig.NoticeConfigService;
import avatar.module.user.UserService;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import org.springframework.stereotype.Service;

import java.util.List;

/**
 * 获取用户数据接口
 */
@Service
public class GetUserInfoApi extends SystemEventHandler<Hall.GetUserInfoC2S , Session>{
    protected GetUserInfoApi() {
        super(Cmd.C2S_Hall_GET_USERINFO);
    }

    @Override
    public void method(Session session, Hall.GetUserInfoC2S msg) throws Exception {
        if(session == null){
            return;
        }
        //1.用户信息推送
        int userId = session.getUserId();
        if(userId == 0){
            return;
        }
        Hall.GetUserInfoS2C.Builder builder = Hall.GetUserInfoS2C.newBuilder();
        UserService.getInstance().getUserInfo(userId , builder);
        session.sendClient(Cmd.S2C_Hall_GET_USERINFO , builder.build().toByteArray());

        //2.大厅公告配置信息的推送（公告、喇叭、客服）
        Hall.NoticeConfigS2C.Builder noticeConfigBuilder = Hall.NoticeConfigS2C.newBuilder();
        List<NoticeConfig> allNoticeConfigs = NoticeConfigService.getInstance().getAllNoticeConfigData();
        for (int i = 0; i < allNoticeConfigs.size(); i++){
            Hall.NoticeConfigDataS2C.Builder noticeConfigDataBuilder = Hall.NoticeConfigDataS2C.newBuilder();
            noticeConfigDataBuilder.setType(allNoticeConfigs.get(i).getType());
            noticeConfigDataBuilder.setContent(allNoticeConfigs.get(i).getContent());
            noticeConfigBuilder.addNoticeConfigData(i, noticeConfigDataBuilder);
            System.out.println(allNoticeConfigs.get(i).getContent());
        }

        System.out.println(noticeConfigBuilder.toString());
        session.sendClient(Cmd.S2C_Hall_Push_Notice, noticeConfigBuilder.build().toByteArray());

        //玩家登录大厅成功后，更新玩家所在的大厅服务器别名
        UserService.getInstance().updateHallAddr(userId);
    }
}
