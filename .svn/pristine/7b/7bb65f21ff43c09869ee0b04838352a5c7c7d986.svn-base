package avatar.apiregister.battlerecord;

import avatar.entity.battlerecord.BattleReplayEntity;
import avatar.facade.SystemEventHandler;
import avatar.module.battleRecord.service.BattleRecordService;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import org.springframework.stereotype.Service;

/**
 * 获取回放记录的接口
 */
@Service
public class GetBattleReplayAPI extends SystemEventHandler<Hall.PlayVideoC2S, Session>{
    protected GetBattleReplayAPI(){ super(Cmd.C2S_Hall_Get_Replay_Data);}

    @Override
    public void method(Session session, Hall.PlayVideoC2S msg) throws Exception {
        if (session == null){
            return;
        }
        int roomId = msg.getRoomId();
        int round = msg.getRound();
        System.out.println("****************roomId = " + roomId);
        System.out.println("****************round = " + round);

        //根据房间id和局获取回放记录
        BattleReplayEntity entity = BattleRecordService.getInstance().getBattleReplayByRoomIdAndInnings(roomId, round);
        Hall.PlayVideoS2C.Builder builder = Hall.PlayVideoS2C.newBuilder();
        builder.setReport(entity.getReplayContent());
        session.sendClient(Cmd.S2C_Hall_Get_Replay_Data, builder.build().toByteArray());
    }
}