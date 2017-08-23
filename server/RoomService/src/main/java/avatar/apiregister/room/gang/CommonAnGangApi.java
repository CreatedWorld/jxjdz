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
import avatar.module.mahjong.operate.scanner.CommonAnGangScanner;
import avatar.module.mahjong.service.BaseGameService;
import avatar.module.room.service.RoomDataService;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import avatar.util.MessageUtil;
import org.springframework.stereotype.Service;

import java.util.List;

/**
 * 普通暗杠
 */
@Service
public class CommonAnGangApi extends SystemEventHandler<Battle.CommonAnGangC2S, Session> {
    protected CommonAnGangApi() {
        super(Cmd.C2S_ROOM_COMMON_AN_GANG);
    }

    @Override
    public void method(Session session, Battle.CommonAnGangC2S msg) throws Exception {
        int userId = session.getUserId();
        int mahjongCode = msg.getMahjongCode();
        Mahjong mahjong = Mahjong.parseFromCode(mahjongCode);


        Room room = RoomDataService.getInstance().getInRoomByUserId(userId);
        MahjongGameData mahjongGameData = MahjongGameDataDao.getInstance().get(room.getId());

        BaseGameService.getInstance().canOperate(mahjongGameData, userId, ActionType.COMMON_AN_GANG);

        int specialUserId = mahjongGameData.getCanDoOperates().get(0).getSpecialUserId();

        List<CanDoOperate> canDoOperate = CommonAnGangScanner.getInstance().scan(mahjongGameData, mahjong, userId);

        if (canDoOperate.isEmpty()) {
            throw new RuntimeException(String.format("玩家[specifiedUserId=%s]不能暗杠%s", userId, mahjong));
        }

        PersonalMahjongInfo myInfo = PersonalMahjongInfo.getMyInfo(mahjongGameData.getPersonalMahjongInfos(), userId);

        myInfo.setTouchMahjong(null);

        // 玩家的个人卡信息的手牌中移除3只杠的麻将
        myInfo.getHandCards().remove(mahjong);
        myInfo.getHandCards().remove(mahjong);
        myInfo.getHandCards().remove(mahjong);

        // 添加暗杠combo
        myInfo.getGangs().add(Combo.newCommonAnGang(mahjong, specialUserId));

        mahjongGameData.addOperated(
                Operated.newCommonAnGang(
                        userId,
                        mahjong
                ));

        // 响应用户普通暗杠
        session.sendClient(
                Cmd.S2C_ROOM_COMMON_AN_GANG,
                Battle.CommonAnGangS2C.newBuilder().setMahjongCode(mahjongCode).build().toByteArray());

        // 广播暗杠
        MessageUtil.broadcastPlayerAction(
                mahjongGameData,
                userId,
                userId,
                mahjongCode,
                ActionType.COMMON_AN_GANG
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
