package avatar.module.announcement;

import avatar.entity.announcements.AnnounceUser;
import avatar.util.GameData;

import java.util.ArrayList;
import java.util.List;

/**
 * 玩家已经发送的公告的列表
 */
public class AnnounceUserDao {
    private static final AnnounceUserDao instance = new AnnounceUserDao();
    public static final AnnounceUserDao getInstance(){
        return instance;
    }

    private final String key = "announce_%d";
    private String getKey(int announceId){
        return String.format(key , announceId);
    }


    public List<Integer> loadUserIds(int announceId){
        List<AnnounceUser> list = loadList(announceId);
        if(list == null || list.size() == 0){
            return null;
        }
        List<Integer> userIds = new ArrayList<>();
        for(AnnounceUser announceUser : list){
            userIds.add(announceUser.getUserId());
        }
        return userIds;
    }

    public List<AnnounceUser> loadList(int announceId){
        List<AnnounceUser> list = loadCache(announceId);
        if(list != null && list.size() > 0){
            return list;
        }
        list = loadDB(announceId);
        if(list != null && list.size() > 0){
            setCache(announceId , list);
        }
        return list;
    }

    private List<AnnounceUser> loadCache(int announceId){
        return GameData.getCache().getList(getKey(announceId));
    }

    private void setCache(int announceId , List<AnnounceUser> list){
        GameData.getCache().setList(getKey(announceId) , list);
    }

    private void removeCache(int announceId){
        GameData.getCache().removeCache(getKey(announceId));
    }

    public boolean insert(List<AnnounceUser> list){
        removeCache(list.get(0).getAnnounceId());
        return insertDB(list);
    }

    private List<AnnounceUser> loadDB(int announceId){
        String sql = "select * from AnnounceUser where announceId = ?;";
        List<AnnounceUser> list = GameData.getDB().list(AnnounceUser.class ,
                sql , new Object[]{announceId});
        return list;
    }

    private boolean insertDB(List<AnnounceUser> list){
        return GameData.getDB().insert(list);
    }
}
