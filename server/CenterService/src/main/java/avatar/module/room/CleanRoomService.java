package avatar.module.room;

import avatar.entity.room.Room;
import avatar.entity.room.RoomPlayer;
import avatar.module.room.dao.RoomDao;
import avatar.module.room.dao.RoomPlayerDao;
import avatar.module.room.service.RoomDataService;

import java.util.ArrayList;
import java.util.List;

/**
 * 处理房间回收工作
 */
public class CleanRoomService {
    private static final CleanRoomService instance = new CleanRoomService();
    public static final CleanRoomService getInstance(){
        return instance;
    }

    private final long limitTime = 30 * 60 * 1000;
    private final int deleteNum = 100;//每次清理的房间个数


    /**
     * 房间清理工作，每次只清理n个房间
     */
    public void recycleRoom(){
        List<Integer> list = RoomDao.getInstance().loadAllRoomId();
        if(list == null || list.size() == 0){
            return;
        }
        //每次只做前面n个房间的清理操作
        RoomDataService.getInstance().deleteBatchRoom(list , deleteNum , limitTime);
    }


}
