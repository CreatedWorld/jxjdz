package avatar.apiregister.login;

import avatar.entity.userinfo.UserEntity;
import avatar.facade.SystemEventHandler;
import avatar.global.Config;
import avatar.global.ServerCenterManager;
import avatar.global.ServerUpdate;
import avatar.module.login.LoginService;
import avatar.net.session.Session;
import avatar.net.session.TcpTransport;
import avatar.protobuf.Cmd;
import avatar.protobuf.Login;
import avatar.util.LogUtil;
import org.springframework.stereotype.Service;


/**
 * 客户端登录接口
 */
@Service
public class LoginApi extends SystemEventHandler<Login.LoginC2S , Session> {
    public LoginApi() {
        super(Cmd.C2S_LOGIN);
    }

    private static final int STATUS_SUC = 1;
    private static final int STATUS_FAIL = 0;

    @Override
    public void method(Session session, Login.LoginC2S msg) throws Exception {
        String name = msg.getName();
        String mac = msg.getMac();
        String psw = msg.getPsw();
        String imageUrl = msg.getImageUrl();
        int sex = msg.getSex();
        long  phone = 11111111111L;
        System.out.println("key ======= " + name + ", mac = " + mac);

        TcpTransport tcpTransport = (TcpTransport) session;
        String clientIp = tcpTransport.getRemoteIP();

        UserEntity userInfo = LoginService.getInstance().login(mac,name , psw , clientIp ,imageUrl , sex , phone);
        ServerUpdate serverUpdate = ServerCenterManager.getInstance().randomServerByName(
                Config.getInstance().getHallServerName());
        String ip = "";
        int port = 0;
        if(serverUpdate != null){
            ip = serverUpdate.getClientIp();
            port = serverUpdate.getClientPort();
        }

//        System.out.println("登录成功，userId = " + userInfo.getPlayerId()+",服务器ip="
//                +ip+",服务器端口 = " + port);
        LogUtil.getLogger().debug("登录成功，userId = " + userInfo.getPlayerId()+",服务器ip="
                +ip+",服务器端口 = " + port);

        Login.LoginS2C.Builder builder = Login.LoginS2C.newBuilder();
        if(userInfo == null){
            builder.setStatus(STATUS_FAIL);
        }else{
            builder.setStatus(STATUS_SUC);
        }
        builder.setUserId(userInfo.getPlayerId());
        builder.setServerIp(ip);
        builder.setPort(port);
        builder.setTime(System.currentTimeMillis());
        session.sendClient(Cmd.S2C_LOGIN , builder.build().toByteArray());
    }
}