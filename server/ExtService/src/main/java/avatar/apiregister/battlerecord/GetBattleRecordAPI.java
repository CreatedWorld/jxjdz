package avatar.apiregister.battlerecord;

import avatar.entity.battlerecord.UserBattleScoreEntity;
import avatar.facade.SystemEventHandler;
import avatar.module.battleRecord.service.BattleRecordService;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import org.springframework.stereotype.Service;

import java.util.List;

/**
 * 获取战绩列表
 */
@Service
public class GetBattleRecordAPI extends SystemEventHandler<Hall.GetGradeInfoC2S , Session> {
    protected GetBattleRecordAPI() {
        super(Cmd.C2S_Hall_GET_BATTLE_HISTORY);
    }

    @Override
    public void method(Session session, Hall.GetGradeInfoC2S msg) throws Exception {
        if(session == null){
            return;
        }
        int userId = session.getUserId();
        int count = 10;

        //==获取数据的逻辑==//
        //1.获取请求玩家最近的战绩记录
        List< UserBattleScoreEntity > userBattleScores = BattleRecordService.getInstance().
                getUserBattleScoreByPlayerId(userId , count);

        if(userBattleScores == null || userBattleScores.size() == 0){//没有战绩记录
        }else{//有战绩记录
            //一级builder
            Hall.GetGradeInfoS2C.Builder builder = Hall.GetGradeInfoS2C.newBuilder();
            for (int i = 0; i < userBattleScores.size(); i++){
                //二级builder
                Hall.GradeDataS2C.Builder gradeDataBuilder = Hall.GradeDataS2C.newBuilder();
                gradeDataBuilder.setRoomID(userBattleScores.get(i).getRoomId());
                gradeDataBuilder.setTime(userBattleScores.get(i).getTime().getTime());
                gradeDataBuilder.setRoomCode(userBattleScores.get(i).getRoomCode());
                //2.根据房间id获取该房间中各个玩家的战绩
                List<UserBattleScoreEntity> singleRoomUserBattleScoreList = BattleRecordService.getInstance().getUserBattleScoreByRoomId(userBattleScores.get(i).getRoomId());
                for (int j = 0; j < singleRoomUserBattleScoreList.size(); j++){
                    //三级builder
                    Hall.UsersInfoS2C.Builder userInfoBuilder = Hall.UsersInfoS2C.newBuilder();
                    userInfoBuilder.setUserName(singleRoomUserBattleScoreList.get(j).getUserName());
                    userInfoBuilder.setScore(singleRoomUserBattleScoreList.get(j).getAllScore());
                    gradeDataBuilder.addUsersInfo(j, userInfoBuilder);
                }
                builder.addGradeDataS2C(i, gradeDataBuilder);
            }
            session.sendClient(Cmd.S2C_Hall_GET_BATTLE_HISTORY , builder.build().toByteArray());
        }
    }
}
