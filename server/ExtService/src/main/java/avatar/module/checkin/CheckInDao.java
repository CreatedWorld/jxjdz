package avatar.module.checkin;

import avatar.entity.checkin.CheckIn;
import avatar.util.GameData;

/**
 * 玩家登录数据
 */
public class CheckInDao {
    private final static CheckInDao instance = new CheckInDao();
    public static final CheckInDao getInstance(){
        return instance;
    }

    private final String key = "checkIn_%d";

    private String getKey(int userId){
        return String.format(key , userId);
    }

    public CheckIn loadByPlayerId(int playerId){
        CheckIn checkIn = loadCache(playerId);
        if(checkIn != null){
            return checkIn;
        }
        checkIn = loadDB(playerId);
        if(checkIn != null){
            setCache(playerId , checkIn);
        }
        return checkIn;
    }

    public boolean update(CheckIn checkIn){
        removeCache(checkIn.getPlayerId());
        boolean ret = updateDB(checkIn);
        return ret;
    }

    public boolean insert(CheckIn checkIn){
        removeCache(checkIn.getPlayerId());
        return insertDB(checkIn);
    }


    private CheckIn loadCache(int playerId){
        return (CheckIn) GameData.getCache().get(getKey(playerId));
    }

    private void setCache(int playerId , CheckIn checkIn){
        GameData.getCache().set(getKey(playerId) , checkIn);
    }

    private void removeCache(int playerId ){
        GameData.getCache().removeCache(getKey(playerId));
    }

    private CheckIn loadDB(int playerId){
        CheckIn checkIn = GameData.getDB().get(CheckIn.class , playerId);
        return checkIn;
    }

    private boolean insertDB(CheckIn checkIn){
        boolean ret = GameData.getDB().insert(checkIn);
        if(!ret){
            //log
        }
        return ret;
    }

    private boolean updateDB(CheckIn checkIn){
        boolean ret = GameData.getDB().update(checkIn);
        if(!ret){
            //log
        }
        return ret;
    }
}
