package avatar.module.login;

import avatar.entity.userinfo.UserEntity;
import avatar.global.ItemType;
import avatar.module.item.ItemService;
import avatar.module.user.UserDao;

/**
 * 登录具体操作
 */
public class LoginService {
    private static final LoginService instance = new LoginService();

    public static LoginService getInstance(){
        return instance;
    }

    /**
     * 验证登录接口
     * @param mac   客户端唯一mac地址
     * @param name  玩家昵称
     * @param psw
     */
    public UserEntity login(String mac , String name , String psw, String ip , String imageUrl , int sex , long phone){
        UserEntity user = UserDao.getInstance().loadLoginEntityByMac(mac);
        if(user != null){
            //验证信息.

            //更新登录时间
            user.setLoginTime();
            user.setImageUrl(imageUrl);
            user.setIp(ip);
            UserDao.getInstance().update(user);
        }else {
            UserDao.getInstance().insert(mac , name , psw , ip , imageUrl , sex , phone);
            user = UserDao.getInstance().loadLoginEntityByMac(mac);
            user.setShowId(String.valueOf(user.getPlayerId()));
            UserDao.getInstance().update(user);
            //test
            ItemService.getInstance().addItem(user.getPlayerId() , ItemType.CARD.getItemType() , 50);
        }

        return user;
    }
}
