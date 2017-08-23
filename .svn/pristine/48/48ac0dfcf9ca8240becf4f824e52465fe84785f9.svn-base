package avatar.module.mahjong.operate.picker;

import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.PersonalMahjongInfo;

import java.util.ArrayList;
import java.util.List;

/**
 * 从mahjongGameData中提取出非打出牌的玩家的PersonalCardInfo
 */
public class PutOutMahjongPicker implements PersonalMahjongInfoPicker {
    private static final PutOutMahjongPicker instance = new PutOutMahjongPicker();

    public static PutOutMahjongPicker getInstance() {
        return instance;
    }

    @Override
    public List<PersonalMahjongInfo> pick(MahjongGameData mahjongGameData, int userId) {
        List<PersonalMahjongInfo> personalMahjongInfos = mahjongGameData.getPersonalMahjongInfos();

        List<PersonalMahjongInfo> toBeScanCardInfos = new ArrayList<>(personalMahjongInfos.size() - 1);

        // 循环除了出牌的玩家，判断能不能有一些操作
        for (PersonalMahjongInfo personalMahjongInfo : personalMahjongInfos) {
            if (userId != personalMahjongInfo.getUserId()) {
                toBeScanCardInfos.add(personalMahjongInfo);
            }
        }

        return toBeScanCardInfos;
    }
}
