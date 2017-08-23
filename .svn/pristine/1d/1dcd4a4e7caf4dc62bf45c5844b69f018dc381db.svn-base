package avatar.apiregister.battlerecord;

import avatar.entity.battlerecord.BattleReplayEntity;
import avatar.entity.battlerecord.BattleScoreEntity;
import avatar.facade.SystemEventHandler;
import avatar.module.battleRecord.service.BattleRecordService;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import org.springframework.stereotype.Service;

import java.util.List;

/**
 * 获取战绩明细接口
 */
@Service
public class GetBattleRecordDetailsAPI extends SystemEventHandler<Hall.GetRoundInfoC2S, Session> {
    protected GetBattleRecordDetailsAPI() {
        super(Cmd.C2S_Hall_GET_BATTLE_DETAIL);
    }

    @Override
    public void method(Session session, Hall.GetRoundInfoC2S msg) throws Exception {
        if (session == null){
            return;
        }
        int roomId = msg.getRoomID();
        System.out.println("===============roomId = " + roomId);

        //===获取战绩明细逻辑===//
        //一级builder
        Hall.GetRoundInfoS2C.Builder builder = Hall.GetRoundInfoS2C.newBuilder();
        //1.获取该房间下所有局的回放记录
        List<BattleReplayEntity> battleReplays = BattleRecordService.getInstance().getBattleReplaysByRoomId(roomId);
        for (int i = 0; i < battleReplays.size(); i++){
            //二级builder
            Hall.RoundData.Builder roundDataBuilder = Hall.RoundData.newBuilder();
            roundDataBuilder.setRoomCode(battleReplays.get(i).getRoomCode());
            roundDataBuilder.setTime(battleReplays.get(i).getGamesTime().getTime());
            roundDataBuilder.setRoomID(roomId);
            //2.获取该条回放记录对应的各个玩家得分记录
            List<BattleScoreEntity> battleScores = BattleRecordService.getInstance().getBattleScoreByBattleReplayId(battleReplays.get(i).getId());
            for (int j = 0; j < battleScores.size(); j++){
                //三级builder
                Hall.UsersInfoS2C.Builder userInfoBuilder = Hall.UsersInfoS2C.newBuilder();
                userInfoBuilder.setScore(battleScores.get(j).getScore());
                userInfoBuilder.setUserName(battleScores.get(j).getPlayerNickName());
                roundDataBuilder.addUsersInfo(j, userInfoBuilder);
            }
            builder.addRoundData(i , roundDataBuilder);
        }
        session.sendClient(Cmd.S2C_Hall_GET_BATTLE_DETAIL, builder.build().toByteArray());
    }
}
