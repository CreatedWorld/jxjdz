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
 * 回头碰杠（明杠）
 */
@Service
public class BackPengGangApi extends SystemEventHandler<Battle.BackPengGangC2S, Session> {
    protected BackPengGangApi() {
        super(Cmd.C2S_ROOM_BACK_PENG_GANG);
    }

    @Override
    public void method(Session session, Battle.BackPengGangC2S msg) throws Exception {
        int userId = session.getUserId();
        int mahjongCode = msg.getMahjongCode();
        Mahjong mahjong = Mahjong.parseFromCode(mahjongCode);


        Room room = RoomDataService.getInstance().getInRoomByUserId(userId);
        MahjongGameData mahjongGameData = MahjongGameDataDao.getInstance().get(room.getId());

        BaseGameService.getInstance().canOperate(mahjongGameData, userId, ActionType.BACK_PENG_GANG);

        PersonalMahjongInfo myInfo = PersonalMahjongInfo.getMyInfo(mahjongGameData.getPersonalMahjongInfos(), userId);

        if (!myInfo.getHandCards().contains(mahjong)) {
            throw new RuntimeException(String.format("手牌无%s，不能回头碰杠", mahjong.getName()));
        }

        boolean hasPeng = false;
        for (Combo combo : myInfo.getPengs()) {
            if (combo.getMahjongs().contains(mahjong)) {
                hasPeng = true;
                break;
            }
        }
        if (!hasPeng) {
            throw new RuntimeException(String.format("没有已碰的%s，不能回头碰杠", mahjong.getName()));
        }

        // 玩家的个人卡信息的手牌中移除普通碰杠麻将
        myInfo.getHandCards().add(myInfo.getTouchMahjong());
        myInfo.getHandCards().remove(mahjong);
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
        myInfo.getGangs().add(Combo.newBackPengGang(mahjong, pengTargetUserId));

        mahjongGameData.addOperated(
                Operated.newBackPengGang(
                        userId,
                        mahjong
                ));

        // 响应用户回头碰杠
        session.sendClient(
                Cmd.S2C_ROOM_BACK_PENG_GANG,
                Battle.BackPengGangS2C.newBuilder().setMahjongCode(mahjongCode).build().toByteArray());

        // 广播回头碰杠
        MessageUtil.broadcastPlayerAction(
                mahjongGameData,
                userId,
                pengTargetUserId,
                mahjongCode,
                ActionType.BACK_PENG_GANG
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
