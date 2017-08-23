package avatar.task;

import avatar.apiregister.room.PassApi;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.PersonalMahjongInfo;
import avatar.module.trusteeship.TrusteeshipService;
import avatar.module.trusteeship.TrusteeshipTask;
import avatar.protobuf.Battle;
import avatar.util.LogUtil;
import avatar.util.SpringApplicationContext;

/**
 * 自动选择过
 */
public class AutoPassTask extends TrusteeshipTask {

    private MahjongGameData mahjongGameData;

    public AutoPassTask() {
        super("auto pass task");
    }

    public AutoPassTask(int userId, MahjongGameData mahjongGameData) {
        super("auto pass task");
        this.userId = userId;
        this.mahjongGameData = mahjongGameData;
    }

    @Override
    public void run() {
        if (!TrusteeshipService.isTrusteeship(userId)) {
            return;
        }

        PassApi passApi = (PassApi) SpringApplicationContext.getBean("passApi");

        PersonalMahjongInfo myInfo = PersonalMahjongInfo.getMyInfo(mahjongGameData.getPersonalMahjongInfos(), userId);
        if (myInfo == null || myInfo.getTouchMahjong() == null) {
            return;
        }
        Battle.GuoC2S msg = Battle.GuoC2S.newBuilder().build();
        try {
            passApi.method(userId, msg);
        } catch (Exception e) {
            LogUtil.getLogger().error(e.getMessage(), e);
        }
    }
}
