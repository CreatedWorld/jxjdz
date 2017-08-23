package avatar.module.mahjong.service;

import avatar.entity.room.Room;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.dao.MahjongGameDataDao;
import avatar.module.mahjong.operate.ActionType;
import avatar.module.room.service.RoomDataService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class HuService {

    private static final HuService instance = new HuService();

    public static HuService getInstance() {
        return instance;
    }

    private static final BaseGameService baseGameService = BaseGameService.getInstance();

    private static final RoomDataService roomDao = RoomDataService.getInstance();

    private static final MahjongGameDataDao mahjongGameDataDao = MahjongGameDataDao.getInstance();

    private static final Logger logger = LoggerFactory.getLogger(HuService.class);

    /**
     * 抢直杠胡
     */
    public void qiangZhiGangHu(int userId) {
        Room room = roomDao.getInRoomByUserId(userId);
        MahjongGameData mahjongGameData = mahjongGameDataDao.get(room.getId());
        baseGameService.canOperate(mahjongGameData, userId, ActionType.QIANG_ZHI_GANG_HU);


    }


}
