package avatar.apiregister.room.hu;

import avatar.entity.room.Room;
import avatar.facade.SystemEventHandler;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.MultipleChiHu;
import avatar.module.mahjong.PersonalMahjongInfo;
import avatar.module.mahjong.dao.MahjongGameDataDao;
import avatar.module.mahjong.operate.ActionType;
import avatar.module.mahjong.operate.CanDoOperate;
import avatar.module.mahjong.service.BaseGameService;
import avatar.module.mahjong.service.SettlementService;
import avatar.module.room.service.RoomDataService;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import avatar.util.MessageUtil;
import org.apache.commons.collections.CollectionUtils;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

/**
 * 吃胡
 */
@Service
public class ChiHuApi extends SystemEventHandler<Battle.ChiHuC2S, Session> {
    protected ChiHuApi() {
        super(Cmd.C2S_ROOM_CHI_HU);
    }

    @Override
    public void method(Session session, Battle.ChiHuC2S msg) throws Exception {
        int userId = session.getUserId();

        Room room = RoomDataService.getInstance().getInRoomByUserId(userId);
        MahjongGameData mahjongGameData = MahjongGameDataDao.getInstance().get(room.getId());
        BaseGameService.getInstance().canOperate(mahjongGameData, userId, ActionType.CHI_HU);

        PersonalMahjongInfo myInfo = PersonalMahjongInfo.getMyInfo(mahjongGameData.getPersonalMahjongInfos(), userId);

        CanDoOperate myCanDoOperate = CanDoOperate.findMyCanDoOperate(mahjongGameData.getCanDoOperates(), userId);

        // 响应用户吃胡
        session.sendClient(
                Cmd.S2C_ROOM_CHI_HU,
                Battle.ChiHuS2C.newBuilder().build().toByteArray());

        // 广播吃胡
        MessageUtil.broadcastPlayerAction(
                mahjongGameData,
                myCanDoOperate.getUserId(),
                myCanDoOperate.getSpecialUserId(),
                myCanDoOperate.getSpecialMahjong().getCode(),
                ActionType.CHI_HU
        );

        // 一炮多响时，移除当前玩家的可行操作
        if (CollectionUtils.isNotEmpty(mahjongGameData.getMultipleChiHus())) {
            Iterator<CanDoOperate> iterator = mahjongGameData.getCanDoOperates().iterator();
            while (iterator.hasNext()) {
                CanDoOperate next = iterator.next();
                if (next.getUserId() == userId) {
                    // 记录当前用户的选择
                    MultipleChiHu.select(mahjongGameData.getMultipleChiHus(), next.getUserId(), MultipleChiHu.Select.CHI_HU);
                    iterator.remove();
                    break;
                }
            }

            // 如果是一炮多响，则要等到所有人做了选择，才能结算
            if (MultipleChiHu.isAllSelected(mahjongGameData.getMultipleChiHus())) {
                List<CanDoOperate> winners =
                        CanDoOperate.findChiHuCanDoOperates(mahjongGameData.getMultipleChiHus());

                // 单局结算  mahjongGameData的CanDoOperate都被移除了
                SettlementService.getInstance().settlement(mahjongGameData, winners);
            }
        } else {
            List<CanDoOperate> winners = new ArrayList<>(1);
            winners.add(myCanDoOperate);
            SettlementService.getInstance().settlement(mahjongGameData, winners);
        }
    }


}