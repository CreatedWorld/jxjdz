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

import java.util.Iterator;

/**
 * 普通碰杠（明杠）
 */
@Service
public class CommonPengGangApi extends SystemEventHandler<Battle.CommonPengGangC2S, Session> {
    protected CommonPengGangApi() {
        super(Cmd.C2S_ROOM_COMMON_PENG_GANG);
    }

    @Override
    public void method(Session session, Battle.CommonPengGangC2S msg) throws Exception {
        int userId = session.getUserId();
        int mahjongCode = msg.getMahjongCode();
        Mahjong mahjong = Mahjong.parseFromCode(mahjongCode);

        Room room = RoomDataService.getInstance().getInRoomByUserId(userId);
        MahjongGameData mahjongGameData = MahjongGameDataDao.getInstance().get(room.getId());

        BaseGameService.getInstance().canOperate(mahjongGameData, userId, ActionType.COMMON_PENG_GANG);

        PersonalMahjongInfo myInfo = PersonalMahjongInfo.getMyInfo(mahjongGameData.getPersonalMahjongInfos(), userId);

        // 玩家的个人卡信息的手牌中移除普通碰杠麻将
        myInfo.setTouchMahjong(null);

        int pengTargetUserId = 0;

        // 删除已碰的combo
        Iterator<Combo> iterator = myInfo.getPengs().iterator();
        while (iterator.hasNext()) {
            Combo next = iterator.next();
            if (next.getMahjongs().contains(mahjong)) {
                pengTargetUserId = next.getTargetUserId();
                iterator.remove();
                break;
            }
        }

        // 添加杠combo
        myInfo.getGangs().add(Combo.newCommonPengGang(mahjong, pengTargetUserId));

        mahjongGameData.addOperated(
                Operated.newCommonPengGang(
                        userId,
                        mahjong
                ));

        // 响应用户普通碰杠
        session.sendClient(
                Cmd.S2C_ROOM_COMMON_PENG_GANG,
                Battle.CommonPengGangS2C.newBuilder().setMahjongCode(mahjongCode).build().toByteArray());

        // 广播碰杠
        MessageUtil.broadcastPlayerAction(
                mahjongGameData,
                userId,
                pengTargetUserId,
                mahjongCode,
                ActionType.COMMON_PENG_GANG
        );

        if (room.getPlayTypeList().contains(RoomPlayType.CAN_QIANG_PENG_GANG.getId())) {
            // 判断其他玩家有没有抢杠
            BaseGameService.getInstance().scanAnyUserQiangGangHandler(mahjongGameData, mahjong, userId);
        } else {
            // 给杠的用户摸一张牌
            BaseGameService.getInstance().handleCommonUserTouchAMahjong(mahjongGameData, userId);
        }

    }
}
