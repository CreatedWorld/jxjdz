package avatar.apiregister.exitroom;

import avatar.entity.battlerecord.BattleScoreEntity;
import avatar.facade.SystemEventHandler;
import avatar.global.ClientCode;
import avatar.global.InnerCmd;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.dao.MahjongGameDataDao;
import avatar.module.mahjong.service.SettlementService;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import avatar.protobuf.SysInner;
import avatar.util.GameData;
import avatar.util.MessageUtil;
import org.springframework.stereotype.Service;

import java.util.List;

/**
 * 监听到玩家解散房间请求
 */
@Service
public class DissolveRoomListeningApi extends SystemEventHandler<SysInner.InnerDissolveCenter2Room, Session> {
    protected DissolveRoomListeningApi() {
        super(InnerCmd.Center2Room_Inner_DISSOLVE);
    }

    @Override
    public void method(Session session, SysInner.InnerDissolveCenter2Room msg) throws Exception {
        int clientCode = msg.getClientCode();
        int userId = msg.getUserId();
        Battle.DissolveRoomS2C.Builder builder = Battle.DissolveRoomS2C.newBuilder();
        builder.setClientCode(clientCode);
        if (clientCode != ClientCode.SUCCESS) {
            Session userSession = GameData.getSessionManager().getSessionByUserId(userId);
            if (userSession != null) {
                userSession.sendClient(Cmd.S2C_ROOM_DISSOLVE_BROADCAST, builder.build().toByteArray());
            }
            return;
        } else {
            List<Integer> ids = msg.getMemberIdsList();

            // 解散房间，且房间已经开始游戏时，需要广播总结算
            if (msg.getIsStart()) {
                MahjongGameData mahjongGameData = MahjongGameDataDao.getInstance().get(msg.getRoomId());
                List<BattleScoreEntity> battleScores = SettlementService.getInstance().genTotalScores(mahjongGameData);
                MessageUtil.broadcastTotalScore(battleScores);
            }

            for (Integer id : ids) {
                Session userSession = GameData.getSessionManager().getSessionByUserId(id);
                if (userSession != null) {
                    userSession.sendClient(Cmd.S2C_ROOM_DISSOLVE_BROADCAST, builder.build().toByteArray());
                }
            }
            MahjongGameDataDao.getInstance().remove(msg.getRoomId());
        }
    }
}
