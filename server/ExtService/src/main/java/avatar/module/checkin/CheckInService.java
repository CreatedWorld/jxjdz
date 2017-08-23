package avatar.module.checkin;

import avatar.entity.checkin.CheckIn;
import avatar.global.ClientCode;
import avatar.global.ItemType;
import avatar.module.item.ItemService;
import avatar.protobuf.Hall;
import avatar.util.DateUtil;
import avatar.util.LogUtil;

import java.util.Date;

/**
 * 签到接口
 */
public class CheckInService {

    private static final CheckInService instance = new CheckInService();

    private static final int checkDays = 7;//连续登录7天可以领取奖励
    private static final int cardNum = 1;//签到可以送n张房卡

    public static final CheckInService getInstance(){
        return instance;
    }

    public CheckIn getCheckInInfo(int userId){
        CheckIn checkIn = CheckInDao.getInstance().loadByPlayerId(userId);
        if(checkIn == null){
            checkIn = new CheckIn();
            checkIn.resetCheckInDays();
            checkIn.setStatus(CheckIn.CheckInStatus.No);
        }else {
            //检查同一天是否已经签到过
            if(isCheckIn(checkIn)){
                //是
            }else{
                //否
                if(checkIn.getStatus() == CheckIn.CheckInStatus.CanGet.getId()){
                    checkIn.setStatus(CheckIn.CheckInStatus.No);
                    checkIn.resetCheckInDays();
                    checkIn.setUpdateTime();
                    CheckInDao.getInstance().update(checkIn);
                }
            }
        }
        return checkIn;
    }

    //是否已经签到
    private boolean isCheckIn(CheckIn checkIn){
        if(checkIn == null){
            return false;
        }
        if(checkIsSameDay(checkIn.getCheckInTime())){
            return true;
        }
        return false;
    }

    private boolean checkIsSameDay(Date date){
        return DateUtil.isSameDay(date);
    }

    public int checkIn(int userId , Hall.CheckInS2C.Builder builder){
        CheckIn checkIn = CheckInDao.getInstance().loadByPlayerId(userId);

        if(checkIn == null){
            checkIn = new CheckIn();
            checkIn.setCheckInTime(new Date());
            checkIn.setStatus(CheckIn.CheckInStatus.No);
            checkIn.setUpdateTime();
            checkIn.setCheckInDays(1);
            checkIn.setPlayerId(userId);
            boolean ret = CheckInDao.getInstance().insert(checkIn);
            if(!ret){
                builder.setStatus(0);
                builder.setDays(0);
                return ClientCode.FAILED;
            }
            builder.setStatus(1);
            builder.setDays(checkIn.getCheckInDays());
            return ClientCode.SUCCESS;
        }else {
            //今天是否已经签到
            if(isCheckIn(checkIn)){
                builder.setStatus(0);
                builder.setDays(checkIn.getCheckInDays());
                return ClientCode.FAILED;
            }
            checkIn.setCheckInTime(new Date());
            checkIn.setUpdateTime();
            int oldCheckDays = checkIn.getCheckInDays();
            checkIn.setCheckInDays(oldCheckDays + 1);
            if(checkIn.getCheckInDays() == checkDays){
                checkIn.setStatus(CheckIn.CheckInStatus.CanGet);
            }
            boolean ret = CheckInDao.getInstance().update(checkIn);
            if(!ret){
                //log
                builder.setStatus(0);//暂时先写这里--客户端要求
                builder.setDays(oldCheckDays);
                return ClientCode.FAILED;
            }
            builder.setStatus(1);//暂时先写这里--客户端要求
            builder.setDays(checkIn.getCheckInDays());
            return ClientCode.SUCCESS;
        }
    }

    public int getCheckInReward(int userId){
        CheckIn checkIn = CheckInDao.getInstance().loadByPlayerId(userId);
        if(checkIn == null){
            return ClientCode.FAILED;
        }
        if(checkIn.getStatus() != CheckIn.CheckInStatus.CanGet.getId()){
            return ClientCode.FAILED;
        }
        //添加房卡
        boolean add = ItemService.getInstance().addItem(userId , ItemType.CARD.getItemType() , cardNum);
        if(!add){
            //TODO log
            LogUtil.getLogger().error("[CheckInService]getCheckInReward , add item failed , userId = " + userId);
            return ClientCode.FAILED;
        }
        //设置已经领取状态
        checkIn.setStatus(CheckIn.CheckInStatus.Got);
        checkIn.resetCheckInDays();
        checkIn.setUpdateTime();
        boolean ret = CheckInDao.getInstance().update(checkIn);
        return ret ? ClientCode.SUCCESS : ClientCode.FAILED;
    }
}
