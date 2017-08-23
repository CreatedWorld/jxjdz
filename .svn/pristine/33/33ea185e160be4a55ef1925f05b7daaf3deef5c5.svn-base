package avatar.task;

import avatar.apiregister.room.PlayACardApi;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.PersonalMahjongInfo;
import avatar.module.trusteeship.TrusteeshipService;
import avatar.module.trusteeship.TrusteeshipTask;
import avatar.protobuf.Battle;
import avatar.util.LogUtil;
import avatar.util.SpringApplicationContext;

/**
 * 自动打一张牌的任务
 */
public class AutoPlayACardTask extends TrusteeshipTask {

    private MahjongGameData mahjongGameData;

    public AutoPlayACardTask() {
        super("auto play a card task");
    }

    public AutoPlayACardTask(int userId, MahjongGameData mahjongGameData) {
        super("auto play a card task");
        this.userId = userId;
        this.mahjongGameData = mahjongGameData;
    }

    @Override
    public void run() {
        if (!TrusteeshipService.isTrusteeship(userId)) {
            return;
        }

        PlayACardApi playACardApi = (PlayACardApi) SpringApplicationContext.getBean("playACardApi");

        PersonalMahjongInfo myInfo = PersonalMahjongInfo.getMyInfo(mahjongGameData.getPersonalMahjongInfos(), userId);
        if (myInfo == null || myInfo.getTouchMahjong() == null) {
            return;
        }

        Battle.PlayAMahjongC2S msg = Battle.PlayAMahjongC2S
                .newBuilder()
                .setMahjongCode(myInfo.getTouchMahjong().getCode())
                .build();
        try {
            playACardApi.method(userId, msg);
        } catch (Exception e) {
            LogUtil.getLogger().error(e.getMessage(), e);
        }
    }


}
