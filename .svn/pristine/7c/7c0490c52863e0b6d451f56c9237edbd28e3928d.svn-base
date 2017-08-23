package avatar.module.roomType;

import avatar.entity.roomType.RoomType;

import java.util.ArrayList;
import java.util.List;

/**
 * 房间列表接口
 */
public class RoomTypeService {
    private static final RoomTypeService instance = new RoomTypeService();
    public static final RoomTypeService getInstance(){
        return instance;
    }

    private static volatile List<RoomType> list = new ArrayList<>();

    /**
     *初始化房间配置
     */
    public void init(){
        list = RoomTypeDao.getInstance().loadAll();
    }

    /**
     * 得到房间类型
     * @return
     */
    public RoomType getRoomTypeByType(int roomType){
        for(RoomType temp : list){
            if(temp.getRoomType() == roomType){
                return temp;
            }
        }
        return null;
    }

    public int getSeatNumByRoomType(int roomType){
        RoomType roomType1 = getRoomTypeByType(roomType);
        if(roomType1 == null){
            return 4;
        }
        return roomType1.getSeatNum();
    }

}
