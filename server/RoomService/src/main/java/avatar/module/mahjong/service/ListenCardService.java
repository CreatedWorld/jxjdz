package avatar.module.mahjong.service;

import avatar.entity.room.Room;
import avatar.module.mahjong.Mahjong;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.operate.CanDoOperate;
import avatar.module.mahjong.operate.scanner.HuScanner;
import avatar.module.room.service.RoomDataService;
import avatar.util.RoomPlayTypeUtil;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 听牌服务，听牌本系统的专门指：已有手牌，再加上一只摸到的牌或者别人打出的牌，就可以胡牌。听牌服务不应包含托管功能！
 */
public class ListenCardService {

    private static final ListenCardService instance = new ListenCardService();

    public static ListenCardService getInstance() {
        return instance;
    }

    private static HuScanner huScanner = HuScanner.getInstance();

    private static RoomDataService roomDataService = RoomDataService.getInstance();

    /**
     * 查找可以胡牌的牌
     *
     * @param userId 需要判断是否听牌的userId
     */
    public List<Mahjong> findListenCards(MahjongGameData mahjongGameData, int userId) throws Exception {
        // 获取所有麻将牌
        List<Mahjong> allMahjongs = Mahjong.getAllMahjongs();

        // 根据每个项目的房间类型的玩法不同，来过滤掉某些麻将
        Room room = roomDataService.getRoomByRoomId(mahjongGameData.getRoomId());
        RoomPlayTypeUtil.filterMahjongs(room, allMahjongs);

        Map<Integer, Boolean> map = new HashMap<>();
        List<Mahjong> listenCards = new ArrayList<>(3);

        for (Mahjong mahjong : allMahjongs) {
            Boolean b = map.get(mahjong.getCode());
            if (b == null) {
                List<CanDoOperate> canDoOperates = huScanner.scan(mahjongGameData, mahjong, userId);
                if (!canDoOperates.isEmpty()) {
                    listenCards.add(mahjong);
                }

                map.put(mahjong.getCode(), true);
            }
        }

        return listenCards;
    }

}
