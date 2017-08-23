package avatar.util;

import avatar.entity.room.Room;
import avatar.entity.room.RoomPlayType;
import avatar.module.mahjong.Mahjong;

import java.util.List;

public class RoomPlayTypeUtil {

    /**
     * 根据每个项目的房间类型的玩法不同，来过滤掉某些麻将，不用发牌
     * 实现此方法的思路是：去掉某些不需要的麻将，而不是添加需要的麻将
     */
    public static void filterMahjongs(Room room, List<Mahjong> allMahjongs) {
        // 有风无风(有东南西北中发白)
        if (room.getPlayTypeList().contains(RoomPlayType.NO_WIND.getId())) {
            allMahjongs.removeIf(
                    mahjong ->
                            mahjong.getType() == Mahjong.Type.FENG.getId()
                                    || mahjong.getType() == Mahjong.Type.JIAN.getId()
            );
        }

        // 去掉4张风牌
        if (room.getPlayTypeList().contains(RoomPlayType.NO_WIND_1.getId())) {
            allMahjongs.removeIf(
                    mahjong ->
                            mahjong.getType() == Mahjong.Type.FENG.getId()
            );
        }

        // 去掉发白
        if (room.getPlayTypeList().contains(RoomPlayType.NO_FA_BAI.getId())) {
            allMahjongs.removeIf(
                    mahjong -> mahjong == Mahjong.FA_CAI || mahjong == Mahjong.BAI_BAN
            );
        }
    }
}
