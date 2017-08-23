package avatar.module.roomType;

/**
 * Created by Administrator on 2017/5/17.
 */

import avatar.entity.roomType.RoomType;
import avatar.util.GameData;

import java.util.List;

/**
 * 房间列表相关
 */
public class RoomTypeDao {
    private static final RoomTypeDao instance = new RoomTypeDao();
    static final RoomTypeDao getInstance(){
        return instance;
    }
    private RoomTypeDao(){}

    /**
     * 存储所有分场列表
     */
    public List<RoomType> loadAll(){
        return loadDb();
    }


    private List<RoomType> loadDb() {
        List<RoomType> roomType = GameData.getDB().list(RoomType.class , "select * from config_roomtype;" , new Object[]{});
        return roomType;
    }
}
