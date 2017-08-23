package avatar.apiregister.room.hu;

import avatar.facade.SystemEventHandler;
import avatar.module.mahjong.service.HuService;
import avatar.net.session.Session;
import avatar.protobuf.Battle;
import avatar.protobuf.Cmd;
import org.springframework.stereotype.Service;

/**
 * 自摸
 */
@Service
public class QiangZhiGangHuApi extends SystemEventHandler<Battle.QiangZhiGangHuC2S, Session> {
    protected QiangZhiGangHuApi() {
        super(Cmd.C2S_ROOM_QIANG_ZHI_GANG_HU);
    }

    private static final HuService huService = HuService.getInstance();

    @Override
    public void method(Session session, Battle.QiangZhiGangHuC2S msg) throws Exception {
        int userId = 13791;

        huService.qiangZhiGangHu(userId);

        // todo 广播玩家执行直杠
    }
}
