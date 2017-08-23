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
public class BackAnGangApi extends SystemEventHandler<Battle.BackAnGangC2S, Session> {
    protected BackAnGangApi() {
        super(Cmd.C2S_ROOM_BACK_AN_GANG);
    }

    @Override
    public void method(Session session, Battle.BackAnGangC2S msg) throws Exception {
        int userId = session.getUserId();
        int mahjongCode = msg.getMahjongCode();
        Mahjong mahjong = Mahjong.parseFromCode(mahjongCode);

        Room room = RoomDataService.getInstance().getInRoomByUserId(userId);
        MahjongGameData mahjongGameData = MahjongGameDataDao.getInstance().get(room.getId());

        BaseGameService.getInstance().canOperate(mahjongGameData, userId, ActionType.BACK_AN_GANG);

        PersonalMahjongInfo myInfo = PersonalMahjongInfo.getMyInfo(mahjongGameData.getPersonalMahjongInfos(), userId);

        // 手牌是否有4只mahjong
        int count = 0;
        for (Mahjong m : myInfo.getHandCards()) {
            if (m == mahjong) {
                count++;
            }
        }
        if (count != 4) {
            throw new RuntimeException(String.format("手牌无4只%s，不能回头暗杠", mahjong.getName()));
        }

        myInfo.getHandCards().add(myInfo.getTouchMahjong());
        myInfo.setTouchMahjong(null);

        // 玩家的个人卡信息的手牌中移除4只杠的麻将
        myInfo.getHandCards().remove(mahjong);
        myInfo.getHandCards().remove(mahjong);
        myInfo.getHandCards().remove(mahjong);
        myInfo.getHandCards().remove(mahjong);

        // 添加暗杠combo
        myInfo.getGangs().add(Combo.newBackAnGang(mahjong, mahjongGameData.getCanDoOperates().get(0).getSpecialUserId()));

        mahjongGameData.addOperated(
                Operated.newBackAnGang(
                        userId,
                        mahjong
                ));

        // 响应用户回头暗杠
        session.sendClient(
                Cmd.S2C_ROOM_BACK_AN_GANG,
                Battle.BackAnGangS2C.newBuilder().setMahjongCode(mahjongCode).build().toByteArray());

        // 广播回头暗杠
        MessageUtil.broadcastPlayerAction(
                mahjongGameData,
                userId,
                userId,
                mahjongCode,
                ActionType.BACK_AN_GANG
        );


        if (room.getPlayTypeList().contains(RoomPlayType.CAN_QIANG_AN_GANG.getId())) {
            // 判断其他玩家有没有抢杠
            BaseGameService.getInstance().scanAnyUserQiangGangHandler(mahjongGameData, mahjong, userId);
        } else {
            // 给杠的用户摸一张牌
            BaseGameService.getInstance().handleCommonUserTouchAMahjong(mahjongGameData, userId);
        }

    }
}
