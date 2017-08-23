package avatar.apiregister.room.hu;

import avatar.entity.room.Room;
import avatar.facade.SystemEventHandler;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.PersonalMahjongInfo;
import avatar.module.mahjong.dao.MahjongGameDataDao;
import avatar.module.mahjong.operate.ActionType;
import avatar.module.mahjong.service.BaseGameService;
import avatar.module.mahjong.service.HuService;
import avatar.module.mahjong.service.SettlementService;
import avatar.module.room.service.RoomDataService;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import avatar.util.MessageUtil;
import org.springframework.stereotype.Service;

/**
 * 自摸
 */
@Service
public class ZiMoHuApi extends SystemEventHandler<Battle.ZiMoHuC2S, Session> {
    protected ZiMoHuApi() {
        super(Cmd.C2S_ROOM_ZI_MO_HU);
    }

    private static final HuService huService = HuService.getInstance();

    @Override
    public void method(Session session, Battle.ZiMoHuC2S msg) throws Exception {
        int userId = session.getUserId();

        Room room = RoomDataService.getInstance().getInRoomByUserId(userId);
        MahjongGameData mahjongGameData = MahjongGameDataDao.getInstance().get(room.getId());
        BaseGameService.getInstance().canOperate(mahjongGameData, userId, ActionType.ZI_MO);

        PersonalMahjongInfo myInfo = PersonalMahjongInfo.getMyInfo(mahjongGameData.getPersonalMahjongInfos(), userId);

        // 响应用户自摸
        session.sendClient(
                Cmd.S2C_ROOM_ZI_MO_HU,
                Battle.ZiMoHuS2C.newBuilder().build().toByteArray());

        // 广播自摸
        MessageUtil.broadcastPlayerAction(
                mahjongGameData,
                userId,
                mahjongGameData.getCanDoOperates().get(0).getSpecialMahjong().getCode(),
                ActionType.ZI_MO
        );

        SettlementService.getInstance().settlement(mahjongGameData, mahjongGameData.getCanDoOperates());
    }
}
