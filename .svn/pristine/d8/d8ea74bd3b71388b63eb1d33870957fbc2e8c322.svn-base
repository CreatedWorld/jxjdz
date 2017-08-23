package avatar.apiregister.checkin;

import avatar.entity.item.ItemEntity;
import avatar.facade.SystemEventHandler;
import avatar.module.checkin.CheckInService;
import avatar.module.item.ItemService;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import org.springframework.stereotype.Service;

import java.util.List;

/**
 * 点击领取签到奖励
 */
@Service
public class GetCheckInRewardApi extends SystemEventHandler<Hall.GetCheckInRewardC2S , Session>{
    protected GetCheckInRewardApi() {
        super(Cmd.C2S_Hall_GET_CHECKIN_REWARD);
    }

    @Override
    public void method(Session session, Hall.GetCheckInRewardC2S msg) throws Exception {
        if(session == null){
            return;
        }
        int userId = session.getUserId();
        Hall.GetCheckInRewardS2C.Builder builder = Hall.GetCheckInRewardS2C.newBuilder();
        int ret = CheckInService.getInstance().getCheckInReward(userId);
        builder.setStatus(ret);

        List<ItemEntity> list = ItemService.getInstance().getAllUserItems(userId);
        if(list != null && list.size() > 0){
            for(int i = 0 ; i < list.size() ; i ++){
                ItemEntity item = list.get(i);
                Hall.UserItem.Builder itemBuild = ItemService.getInstance().buildUserItem(item);
                builder.addUserItems(i , itemBuild);
            }
        }

        session.sendClient(Cmd.S2C_Hall_GET_CHEKCIN_REWARD , builder.build().toByteArray());
    }
}
