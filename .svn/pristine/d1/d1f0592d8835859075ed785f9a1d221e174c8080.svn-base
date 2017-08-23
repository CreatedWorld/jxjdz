package avatar.apiregister.arena;

import avatar.facade.SystemEventHandler;
import avatar.module.arena.service.ArenaService;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;

/**
 * 申请报名接口
 */
@Service
public class ApplyArenaApi extends SystemEventHandler<Hall.ApplyCompetitionC2S,Session>{
    private static final Logger logger= LoggerFactory.getLogger(ApplyArenaApi.class);
    public ApplyArenaApi() {
        super(Cmd.C2S_Hall_APPLY_COMPETITION);
    }

    @Override
    public void method(Session session, Hall.ApplyCompetitionC2S msg) throws Exception {

        logger.info("进入申请报名接口API成功********************8");
        int userId = session.getUserId();
        Hall.ApplyCompetitionS2C.Builder builder= Hall.ApplyCompetitionS2C.newBuilder();
        ArenaService.getInstance().applyArena(userId,builder);  // 0 报名失败  1 报名成功
        session.sendClient(Cmd.S2C_Hall_APPLY_COMPETITION,builder.build().toByteArray());
    }
}
