package avatar.apiregister.room;

import avatar.entity.room.Room;
import avatar.facade.SystemEventHandler;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.MultipleChiHu;
import avatar.module.mahjong.action.Operated;
import avatar.module.mahjong.dao.MahjongGameDataDao;
import avatar.module.mahjong.operate.ActionType;
import avatar.module.mahjong.operate.CanDoOperate;
import avatar.module.mahjong.operate.Operate;
import avatar.module.mahjong.operate.scanner.GetACardScanner;
import avatar.module.mahjong.operate.scanner.PlayACardScanner;
import avatar.module.mahjong.service.BaseGameService;
import avatar.module.mahjong.service.SettlementService;
import avatar.module.room.service.RoomDataService;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import avatar.util.MessageUtil;
import org.apache.commons.collections.CollectionUtils;
import org.springframework.stereotype.Service;

import java.util.Iterator;
import java.util.List;

/**
 * 过
 */
@Service
public class PassApi extends SystemEventHandler<Battle.GuoC2S, Session> {
    protected PassApi() {
        super(Cmd.C2S_ROOM_PASS);
    }

    private static final BaseGameService baseGameService = BaseGameService.getInstance();

    @Override
    public void method(Session session, Battle.GuoC2S msg) throws Exception {
        int userId = session.getUserId();
        Room room = RoomDataService.getInstance().getInRoomByUserId(userId);
        // 取出麻将数据对象
        MahjongGameData mahjongGameData = MahjongGameDataDao.getInstance().get(room.getId());

        baseGameService.canOperate(mahjongGameData, userId, ActionType.PASS);

        // 响应用户过
        session.sendClient(
                Cmd.S2C_ROOM_PASS,
                Battle.GuoS2C.newBuilder().build().toByteArray());

        // 广播玩家执行“过”
        MessageUtil.broadcastPlayerAction(mahjongGameData, userId, 0, ActionType.PASS);
        mahjongGameData.addOperated(Operated.newPass(userId));

        CanDoOperate myCanDoOperate = CanDoOperate.findMyCanDoOperate(mahjongGameData.getCanDoOperates(), userId);

        if (myCanDoOperate.getScanner() == GetACardScanner.class) {
            // 如果是玩家摸到一张牌，可以自摸胡、普通暗杠、回头暗杠时，选择过后，下一步操作是 打出一张牌
            Operate operate = new Operate();
            operate.setActionType(ActionType.PLAY_A_MAHJONG);

            myCanDoOperate.getOperates().clear();
            myCanDoOperate.getOperates().add(operate);

            // 推送用户操作提示
            MessageUtil.pushPlayerActionTips(mahjongGameData);
        } else if (myCanDoOperate.getScanner() == PlayACardScanner.class) {
            handlePlayACardPass(mahjongGameData, myCanDoOperate);
        }
    }

    /**
     * 处理“上一个用户打出一张牌，其他用户选择过”的情况
     */
    private void handlePlayACardPass(MahjongGameData mahjongGameData, CanDoOperate myCanDoOperate) throws Exception {
        List<CanDoOperate> canDoOperates = mahjongGameData.getCanDoOperates();
        // 如果是一炮多响，则其他也能吃胡的玩家都选择过，才能下一个人执行操作,如果其中一个人选择胡，且全部人已经选择完成，则调用吃胡逻辑
        if (CollectionUtils.isNotEmpty(mahjongGameData.getMultipleChiHus())) {
            Iterator<CanDoOperate> iterator = canDoOperates.iterator();
            while (iterator.hasNext()) {
                CanDoOperate next = iterator.next();
                if (next.getUserId() == myCanDoOperate.getUserId()) {

                    // 记录当前用户的选择
                    MultipleChiHu.select(mahjongGameData.getMultipleChiHus(), next.getUserId(), MultipleChiHu.Select.PASS);

                    iterator.remove();
                    break;
                }
            }

            // 判断是否全部人都执行了选择
            if (MultipleChiHu.isAllSelected(mahjongGameData.getMultipleChiHus())) {
                // 如果全部选择过，则下一个人可以执行操作，如果有一人个选择胡，则胡
                if (MultipleChiHu.hasSelectChiHu(mahjongGameData.getMultipleChiHus())) {
                    List<CanDoOperate> winners = CanDoOperate.findChiHuCanDoOperates(mahjongGameData.getMultipleChiHus());

                    // 单局结算  mahjongGameData的CanDoOperate都被移除了
                    SettlementService.getInstance().settlement(mahjongGameData, winners);
                } else {
                    if (canDoOperates.isEmpty()) {
                        mahjongGameData.getMultipleChiHus().clear();
                        // 全部人都过了，就轮到上一次打出牌的下家摸牌
                        baseGameService.handleCommonNextUserTouchAMahjong(mahjongGameData, myCanDoOperate.getSpecialUserId());
                    } else {
                        // 推送用户操作提示
                        MessageUtil.pushPlayerActionTips(mahjongGameData);
                    }
                }
            }
        } else {
            // 如果可以玩家 吃胡、直杠、碰  然后选择过时，下一个人可以执行操作
            canDoOperates.remove(0);
            if (canDoOperates.isEmpty()) {
                // 全部人都过了，就轮到上一次打出牌的下家摸牌
                baseGameService.handleCommonNextUserTouchAMahjong(mahjongGameData, myCanDoOperate.getSpecialUserId());
            } else {
                // 推送用户操作提示
                MessageUtil.pushPlayerActionTips(mahjongGameData);
            }
        }
    }

    /**
     * 托管执行“过”
     */
    public void method(int userId, Battle.GuoC2S msg) throws Exception {

    }
}
