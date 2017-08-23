package avatar.apiregister.room.gang;

import avatar.entity.room.Room;
import avatar.entity.room.RoomPlayType;
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
 * 直杠
 */
@Service
public class ZhiGangApi extends SystemEventHandler<Battle.ZhiGangC2S, Session> {
    protected ZhiGangApi() {
        super(Cmd.C2S_ROOM_ZHI_GANG);
    }

    @Override
    public void method(Session session, Battle.ZhiGangC2S msg) throws Exception {
        int userId = session.getUserId();
        int mahjongCode = msg.getMahjongCode();
        Mahjong mahjong = Mahjong.parseFromCode(mahjongCode);


        Room room = RoomDataService.getInstance().getInRoomByUserId(userId);
        MahjongGameData mahjongGameData = MahjongGameDataDao.getInstance().get(room.getId());

        BaseGameService.getInstance().canOperate(mahjongGameData, userId, ActionType.ZHI_GANG);

        CanDoOperate canDoOperate = mahjongGameData.getCanDoOperates().get(0);
        if (canDoOperate.getSpecialMahjong() != mahjong) {
            throw new RuntimeException(String.format("玩家[specifiedUserId=%s]不能直杠%s", userId, mahjong));
        }

        PersonalMahjongInfo myInfo = PersonalMahjongInfo.getMyInfo(mahjongGameData.getPersonalMahjongInfos(), userId);

        // 玩家的个人卡信息的手牌中移除3只直杠的麻将
        myInfo.getHandCards().remove(mahjong);
        myInfo.getHandCards().remove(mahjong);
        myInfo.getHandCards().remove(mahjong);

        // 添加直杠combo
        myInfo.getGangs().add(Combo.newZhiGang(mahjong, canDoOperate.getSpecialUserId()));

        // 移除被杠玩家打出的牌
        PersonalMahjongInfo gangedUserInfo = PersonalMahjongInfo.getMyInfo(
                mahjongGameData.getPersonalMahjongInfos(),
                canDoOperate.getSpecialUserId()
        );
        gangedUserInfo.getOutMahjong().remove(gangedUserInfo.getOutMahjong().size() - 1);

        mahjongGameData.addOperated(
                Operated.newZhiGang(
                        userId,
                        canDoOperate.getSpecialUserId(),
                        mahjong
                ));

        // 响应用户直杠
        session.sendClient(
                Cmd.S2C_ROOM_ZHI_GANG,
                Battle.ZhiGangS2C.newBuilder().setMahjongCode(mahjongCode).build().toByteArray());

        // 广播直杠
        MessageUtil.broadcastPlayerAction(
                mahjongGameData,
                userId,
                canDoOperate.getSpecialUserId(),
                mahjongCode,
                ActionType.ZHI_GANG
        );

        if (room.getPlayTypeList().contains(RoomPlayType.CAN_QIANG_ZHI_GANG.getId())) {
            // 判断其他玩家有没有抢杠
            BaseGameService.getInstance().scanAnyUserQiangGangHandler(mahjongGameData, mahjong, userId);
        } else {
            // 给杠的用户摸一张牌
            BaseGameService.getInstance().handleCommonUserTouchAMahjong(mahjongGameData, userId);
        }

    }
}
