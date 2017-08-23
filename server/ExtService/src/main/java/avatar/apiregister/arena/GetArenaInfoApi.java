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
 * 检查用户报名比赛状态api
 */
@Service
public class GetArenaInfoApi extends SystemEventHandler<Hall.CheckApplyStatusC2S,Session>{

    private static final Logger logger= LoggerFactory.getLogger(GetArenaInfoApi.class);
    public GetArenaInfoApi() {
        super(Cmd.C2S_Hall_CHECK_APPLY_STATUS);
    }
    @Override
    public void method(Session session, Hall.CheckApplyStatusC2S msg) throws Exception {
        logger.info("进入检查用户报名比赛状态api成功****************");
        int userId = session.getUserId();
        Hall.CheckApplyStatusS2C.Builder builder=Hall.CheckApplyStatusS2C.newBuilder();
        ArenaService.getInstance().getArenaInfo(userId ,builder);
        session.sendClient(Cmd.S2C_Hall_CHECK_APPLY_STATUS,builder.build().toByteArray());
    }
}
