package avatar.module.user;

import avatar.entity.item.ItemEntity;
import avatar.entity.shop.PlayerAgent;
import avatar.entity.userinfo.UserEntity;
import avatar.event.InternalEventDispatcher;
import avatar.event.ListenInternalEvent;
import avatar.global.Config;
import avatar.global.ItemType;
import avatar.module.item.ItemService;
import avatar.module.shop.PlayerAgentDao;
import avatar.protobuf.Hall;
import avatar.util.StringUtil;

import java.util.List;

/**
 * 大厅用户接口
 */
public class UserService {
    private static volatile UserService instance = null;

    public static final UserService getInstance() {
        if(instance != null){
            return instance;
        }
        return new UserService();
    }


    public UserService(){
        InternalEventDispatcher.getInstance().addEventListener(UserOnLineEventType.Type, this.getClass());
    }

    /**
     * 获取玩家信息接口
     *
     * @param userId 用户id
     */
    public void getUserInfo(int userId, Hall.GetUserInfoS2C.Builder builder) {
        UserEntity entity = UserDao.getInstance().loadUserEntityByUserId(userId);
        if (entity == null) {
            return;
        }
        List<ItemEntity> list = ItemService.getInstance().getAllUserItems(userId);
        if (list != null && list.size() > 0) {
            for (int i = 0; i < list.size(); i++) {
                ItemEntity item = list.get(i);
                Hall.UserItem.Builder itemBuild = ItemService.getInstance().buildUserItem(item);
                builder.addUserItems(i, itemBuild);
            }
        }

        builder.setUserName(entity.getNickName());
        builder.setShowId(entity.getShowId());
        builder.setIp(getUserIP(entity.getIp()));
        builder.setImageUrl(entity.getImageUrl());
        builder.setSex(entity.getSex());
        PlayerAgent playerAgent = PlayerAgentDao.getInstance().loadPlayerAgentByPlayerId(userId);
        if (playerAgent == null) {
            builder.setBoundAgency(0);
        } else {
            builder.setBoundAgency(1);
        }
    }

    /**
     * 获取其他玩家信息接口
     *
     * @param otherId 其他用户的用户id
     */
    public boolean getOtherUserInfo(int otherId, Hall.GetUserInfoByIdS2C.Builder builder) {
        UserEntity userEntity = UserDao.getInstance().loadUserEntityByUserId(otherId);
        if (userEntity == null) {
            return false;
        }
        List<ItemEntity> list = ItemService.getInstance().getAllUserItems(otherId);
        if (list != null && list.size() > 0) {
            for (int i = 0; i < list.size(); i++) {
                ItemEntity item = list.get(i);
                if (item.getItemType() != ItemType.CARD.getItemType()) {
                    continue;
                }
                Hall.UserItem.Builder itemBuild = ItemService.getInstance().buildUserItem(item);
                builder.addUserItems(i, itemBuild);
            }
        }
        builder.setUserName(userEntity.getNickName());
        builder.setShowId(userEntity.getShowId());
        builder.setIp(getUserIP(userEntity.getIp()));
        builder.setImageUrl(userEntity.getImageUrl());
        return true;
    }

    private String getUserIP(String ip){
        if(StringUtil.isNullOrEmpty(ip)){
           return "";
        }
        ip = ip.replace("/" , "");
        String[] arr = ip.split(":");
        if(arr.length != 2){
            return "";
        }
        return arr[0];
    }

    /**
     * 每次登录大厅成功，更新用户的大厅信息
     */
    public boolean updateHallAddr(int userId) {
        UserEntity entity = UserDao.getInstance().loadUserEntityByUserId(userId);
        if (entity != null) {
            entity.setHallLocalName(Config.getInstance().getLocalServerName());
            return UserDao.getInstance().update(entity);
        }
        return false;
    }

    @ListenInternalEvent(UserOnLineEventType.Type)
    public void handleOnLine(UserOnLineEventType event){
        int userId = event.getUserId();
    }
}
