package avatar.apiregister.arena;

import avatar.facade.SystemEventHandler;
import avatar.module.arena.service.ArenaService;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

/**
 *申请参加比赛 api
 */
public class JoinArenaApi extends SystemEventHandler<Hall.JoinCompetitionC2S,Session>{
    private static final Logger logger= LoggerFactory.getLogger(JoinArenaApi.class);
    public JoinArenaApi() {
        super(Cmd.C2S_Hall_JOIN_COMPETITION);
    }

    @Override
    public void method(Session session, Hall.JoinCompetitionC2S msg) throws Exception {
        logger.info("进入参加比赛api成功**********************");
        int userId = session.getUserId();
        Hall.JoinCompetitionS2C.Builder builder=Hall.JoinCompetitionS2C.newBuilder();//0 不可以进入比赛 1 可以进入比赛
        ArenaService.getInstance().JoinArena(userId,builder);
        session.sendClient(Cmd.S2C_Hall_JOIN_COMPETITION,builder.build().toByteArray());
    }
}
