package avatar.apiregister.room;

import avatar.entity.room.Room;
import avatar.facade.SystemEventHandler;
import avatar.module.mahjong.Mahjong;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.MultipleChiHu;
import avatar.module.mahjong.PersonalMahjongInfo;
import avatar.module.mahjong.action.Operated;
import avatar.module.mahjong.dao.MahjongGameDataDao;
import avatar.module.mahjong.operate.ActionType;
import avatar.module.mahjong.operate.CanDoOperate;
import avatar.module.mahjong.operate.scanner.PlayACardScanner;
import avatar.module.mahjong.replay.Action;
import avatar.module.mahjong.service.BaseGameService;
import avatar.module.mahjong.service.ListenCardService;
import avatar.module.room.service.RoomDataService;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import avatar.util.MessageUtil;
import org.springframework.stereotype.Service;

import java.util.List;

/**
 * 打出一张牌
 */
@Service
public class PlayACardApi extends SystemEventHandler<Battle.PlayAMahjongC2S, Session> {
    protected PlayACardApi() {
        super(Cmd.C2S_ROOM_PLAY_A_MAHJONG);
    }

    private static final BaseGameService baseGameService = BaseGameService.getInstance();

    @Override
    public void method(Session session, Battle.PlayAMahjongC2S msg) throws Exception {
        // RedisTemplate redisTemplate = GameData.getRedis().getRedisTemplate();
        // redisTemplate.opsForHash().put("a", "b", 33);
        // Integer o = (Integer) redisTemplate.opsForHash().get("a", "b");

        int userId = session.getUserId();
        int mahjongCode = msg.getMahjongCode();
        Mahjong playedMahjong = Mahjong.parseFromCode(mahjongCode);

        Room room = RoomDataService.getInstance().getInRoomByUserId(userId);
        MahjongGameData mahjongGameData = MahjongGameDataDao.getInstance().get(room.getId());
        baseGameService.canOperate(mahjongGameData, userId, ActionType.PLAY_A_MAHJONG);

        // 判断用户有没有打出的牌
        PersonalMahjongInfo myInfo = PersonalMahjongInfo.getMyInfo(mahjongGameData.getPersonalMahjongInfos(), userId);
        if (!myInfo.hasMahjong(playedMahjong)) {
            throw new RuntimeException(String.format("该玩家没有该牌：%s", playedMahjong.getName()));
        }

        // 用户麻将信息移除打出的牌
        myInfo.removePlayedMahjong(playedMahjong);

        // 用户麻将信息打出的牌集合添加打出的牌
        myInfo.getOutMahjong().add(playedMahjong);

        mahjongGameData.addOperated(Operated.newPlayAMahjong(userId, playedMahjong));

        // 保存游戏数据
        MahjongGameDataDao.getInstance().save(mahjongGameData);

        // 查找用户听的牌
        List<Mahjong> listenCards = ListenCardService.getInstance().findListenCards(mahjongGameData, userId);

        // 响应用户打牌
        session.sendClient(
                Cmd.S2C_ROOM_PLAY_A_MAHJONG,
                Battle.PlayAMahjongS2C.newBuilder()
                        .setMahjongCode(mahjongCode)
                        .addAllTingCards(Mahjong.parseToCodes(listenCards))
                        .build().toByteArray());

        // 广播用户打出的牌
        MessageUtil.broadcastPlayerAction(mahjongGameData, userId, mahjongCode, ActionType.PLAY_A_MAHJONG);

        // 用户打出一张牌后，处理下一步操作
        handleAfterPlayerAMahjong(mahjongGameData, userId, playedMahjong);
    }

    /**
     * 用户打出一张牌后，处理下一步操作
     *
     * @param mahjongGameData mahjongGameData
     * @param userId          打出麻将的userId
     * @param playedMahjong   打出的麻将
     */
    private void handleAfterPlayerAMahjong(MahjongGameData mahjongGameData, int userId, Mahjong playedMahjong) throws Exception {
        // 扫描其他用户是否有吃胡、直杠、碰的操作
        List<CanDoOperate> canDoOperates = PlayACardScanner.getInstance().scan(mahjongGameData, playedMahjong, userId);
        if (canDoOperates.isEmpty()) {
            // 其他用户没有吃胡、直杠、碰，下一个用户摸牌
            baseGameService.handleCommonNextUserTouchAMahjong(mahjongGameData, userId);
        } else {
            // 其他用户有吃胡、直杠、碰
            mahjongGameData.setCanDoOperates(canDoOperates);

            // 如果有一炮多响，则向每个吃胡的用户推送吃胡提示，并将该用户的canDoOperate加入mahjongGameData.multipleChiHus
            if (MultipleChiHu.hasMultipleChiHu(canDoOperates)) {
                for (CanDoOperate canDoOperate : mahjongGameData.getCanDoOperates()) {
                    if (canDoOperate.getOperates().stream().anyMatch(
                            operate -> operate.getActionType() == ActionType.CHI_HU)) {
                        MultipleChiHu multipleChiHu = new MultipleChiHu(canDoOperate);
                        mahjongGameData.addMultipleChiHu(multipleChiHu);
                        MessageUtil.pushPlayerActionTips(canDoOperate);
                        mahjongGameData.addAction(Action.newActTips(canDoOperate));
                    }
                }
            } else {
                MessageUtil.pushPlayerActionTips(mahjongGameData);
            }

            // todo 若项目有托管，则进行托管处理，判断用户是否在托管状态
        }
    }

    /**
     * 托管执行打牌
     */
    public void method(int userId, Battle.PlayAMahjongC2S msg) throws Exception {

    }
}
