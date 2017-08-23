package avatar.task;

import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.dao.MahjongGameDataDao;
import avatar.module.room.service.RoomDataService;
import com.yaowan.game.common.scheduler.ScheduledTask;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * 检查所有房间的玩家的申请解散房间定时器
 */
public class CheckApplyDissolveRoomTask extends ScheduledTask{
    public CheckApplyDissolveRoomTask() {
        super("check apply dissolve room task");
    }

    private final long time = 5*60*1000;

    @Override
    public void run() {
        Map<String, MahjongGameData> map =  MahjongGameDataDao.getInstance().getMahjongGameDataMap();
        if(map.size() == 0){
            return;
        }
        Set<String> keys = map.keySet();
        List<Integer> roomIds = new ArrayList<Integer>();
        for(String key : keys){
            MahjongGameData data = map.get(key);
            List<MahjongGameData.ApplyDissolveData> list = data.getApplyDissolveList();
            if(list == null || list.size() == 0){
                continue;
            }
            MahjongGameData.ApplyDissolveData applyDissolveData = list.get(0);
            if(System.currentTimeMillis() - applyDissolveData.getStartTime().getTime() > time){
                roomIds.add(data.getRoomId());
            }
        }
        if(roomIds.size() > 0) {
            //这里需要做推送结算当前局的分数结算给客户端

            RoomDataService.getInstance().deleteBatchRoom(roomIds, roomIds.size(), 0);
        }
    }
}
