package avatar.apiregister.room;

import avatar.entity.room.Room;
import avatar.facade.SystemEventHandler;
import avatar.module.mahjong.Combo;
import avatar.module.mahjong.Mahjong;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.PersonalMahjongInfo;
import avatar.module.mahjong.action.Operated;
import avatar.module.mahjong.dao.MahjongGameDataDao;
import avatar.module.mahjong.operate.ActionType;
import avatar.module.mahjong.operate.CanDoOperate;
import avatar.module.mahjong.service.BaseGameService;
import avatar.module.room.service.RoomDataService;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import avatar.util.MessageUtil;
import org.springframework.stereotype.Service;

/**
 * 碰牌
 */
@Service
public class PengApi extends SystemEventHandler<Battle.PengC2S, Session> {
    protected PengApi() {
        super(Cmd.C2S_ROOM_PENG);
    }

    private static final BaseGameService baseGameService = BaseGameService.getInstance();

    @Override
    public void method(Session session, Battle.PengC2S msg) throws Exception {
        int userId = session.getUserId();
        int mahjongCode = msg.getMahjongCode();
        Mahjong mahjong = Mahjong.parseFromCode(mahjongCode);


        Room room = RoomDataService.getInstance().getInRoomByUserId(userId);
        MahjongGameData mahjongGameData = MahjongGameDataDao.getInstance().get(room.getId());

        baseGameService.canOperate(mahjongGameData, userId, ActionType.PENG);

        CanDoOperate canDoOperate = mahjongGameData.getCanDoOperates().get(0);
        int specialUserId = canDoOperate.getSpecialUserId();

        if (canDoOperate.getSpecialMahjong() != mahjong) {
            throw new RuntimeException(String.format("玩家[specifiedUserId=%s]不能碰%s", userId, mahjong));
        }

        PersonalMahjongInfo myInfo = PersonalMahjongInfo.getMyInfo(mahjongGameData.getPersonalMahjongInfos(), userId);

        // 玩家的个人卡信息的手牌中移除2只已碰的麻将
        myInfo.getHandCards().remove(mahjong);
        myInfo.getHandCards().remove(mahjong);

        // 添加碰combo
        myInfo.getPengs().add(Combo.newPeng(mahjong, specialUserId));

        // mahjongGameData.addAction(Action.newAct(mahjongGameData.getCanDoOperates().get(0)));

        // 添加可以打牌操作
        mahjongGameData.getCanDoOperates().clear();
        mahjongGameData.getCanDoOperates().add(CanDoOperate.newPlayAMahjongOperate(userId, mahjong));

        // 移除被碰玩家打出的牌
        PersonalMahjongInfo pengedUserInfo = PersonalMahjongInfo.getMyInfo(mahjongGameData.getPersonalMahjongInfos(), specialUserId);
        pengedUserInfo.getOutMahjong().remove(pengedUserInfo.getOutMahjong().size() - 1);

        mahjongGameData.addOperated(Operated.newPeng(userId, specialUserId, mahjong));

        MahjongGameDataDao.getInstance().save(mahjongGameData);

        // 响应用户碰
        session.sendClient(
                Cmd.S2C_ROOM_PENG,
                Battle.PengS2C.newBuilder().setMahjongCode(mahjongCode).build().toByteArray());

        // 广播玩家执行碰
        MessageUtil.broadcastPlayerAction(mahjongGameData, userId, specialUserId, mahjongCode, ActionType.PENG);

        // 通知玩家打出一张牌
        MessageUtil.pushPlayerActionTips(mahjongGameData);

    }
}
