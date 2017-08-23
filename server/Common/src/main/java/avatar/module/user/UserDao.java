package avatar.module.user;

import avatar.entity.userinfo.UserEntity;
import avatar.util.GameData;
import avatar.util.IdGenerator;

/**
 * 与登录的数据相关的操作
 */
public class UserDao {

    private static final UserDao instance = new UserDao();
    public static final UserDao getInstance(){
        return instance;
    }

    private final String key = "user_%d";
    private String getKey(int userId){
        return String.format(key , userId);
    }

    public UserEntity loadUserEntityByUserId(int userId){
        //load cache
        UserEntity userEntity = loadCache(userId);
        if(userEntity != null){
            return userEntity;
        }
        userEntity = loadDB(userId);
        if(userEntity == null){
            return null;
        }
        //set cache
        setCache(userId , userEntity);
        return userEntity;
    }


    /**
     * 根据玩家mac地址获取玩家数据
     */
    public UserEntity loadLoginEntityByMac(String mac){
        UserEntity user = loadByMac(mac);//这里直接读数据库。因为每次每个玩家登录的频率不会太高
        if(user != null){
            //setCache
            setCache(user.getPlayerId(), user);
        }
        return user;
    }

    public boolean insert(String mac , String name , String psw , String ip , String imageUrl , int sex , long phone){
        UserEntity entity = new UserEntity();
        entity.setMac(mac);
        entity.setNickName(name);
        entity.setPsw(psw);
        entity.setIp(ip);
        entity.setLoginTime();
        entity.setOpenId(String.valueOf(1111));
        entity.setShowId(String.valueOf(0));
        entity.setImageUrl(imageUrl);
        entity.setSex(sex);
        entity.setPhone(phone);
        entity.setHallLocalName("");
        boolean ret = insertDB(entity);
        return ret;
    }

    public boolean update(UserEntity entity){
        //先移除缓存中的数据
        removeCache(entity.getPlayerId());
        boolean ret = updateDB(entity);
        return ret;
    }

    //=========================cache========================

    /**
     * 从缓存中加载玩家数据
     */
    private UserEntity loadCache(int userId){
        UserEntity userEntity = (UserEntity) GameData.getCache().get(getKey(userId));
        return userEntity;
    }

    /**
     * 插入玩家数据到缓存中
     * @param userEntity    玩家数据
     */
    private void setCache(int userId , UserEntity userEntity){
        GameData.getCache().set(getKey(userId) , userEntity);
    }

    /**
     * 从缓存中移除玩家数据
     */
    private void removeCache(int userId){
        GameData.getCache().removeCache(getKey(userId));
    }

    //=========================db===========================
    private final String selectSql = "select * from Player where mac = ?";
    private UserEntity loadByMac(String mac){
        UserEntity entity = GameData.getDB().get(UserEntity.class , selectSql , new Object[]{mac});
        return entity;
    }

    private UserEntity loadDB(int id){
        UserEntity entity = GameData.getDB().get(UserEntity.class , id);
        return entity;
    }

    private boolean insertDB(UserEntity entity){
        boolean ret = GameData.getDB().insert(entity);
        if(!ret){
            //log
        }
        return ret;
    }

    private boolean updateDB(UserEntity entity){
        boolean ret = GameData.getDB().update(entity);
        if(!ret){

        }
        return ret;
    }
}
