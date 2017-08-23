package avatar.entity.checkin;

import avatar.util.BaseEntity;
import avatar.util.utilDB.annotation.Column;
import avatar.util.utilDB.annotation.Pk;
import avatar.util.utilDB.annotation.Table;
import org.springframework.stereotype.Service;

import java.util.Date;

@Service
@Table(name = "CheckIn" , comment = "记录玩家登录签到表")
public class CheckIn extends BaseEntity{
    public CheckIn() {
        super(CheckIn.class);
    }

    @Pk(auto = false)
    @Column(name = "playerId" , comment = "玩家id")
    private int playerId;

    @Column(name = "updateTime" , comment = "记录玩家上次登录的日期")
    private Date updateTime;

    @Column(name = "checkInDays" , comment = "玩家累计登录次数")
    private int checkInDays;

    @Column(name = "status" , comment = "玩家领取奖励状态")
    private int status;

    @Column(name = "checkInTime" , comment = "签到日期")
    private Date checkInTime;


    public int getPlayerId(){
        return playerId;
    }

    public Date getUpdateTime(){
        return updateTime;
    }

    public int getCheckInDays(){
        return checkInDays;
    }

    public int getStatus(){
        return status;
    }

    public void setUpdateTime(){
        this.updateTime = new Date();
    }

    public void setCheckInDays(int days){
        this.checkInDays = days;
    }

    public void setPlayerId(int playerId){
        this.playerId = playerId;
    }

    public void resetCheckInDays(){
        this.checkInDays = 0;
    }

    public void setStatus(CheckInStatus status){
        this.status = status.getId();
    }

    public Date getCheckInTime(){
        return checkInTime;
    }

    public void setCheckInTime(Date time){
        this.checkInTime = time;
    }

    public enum CheckInStatus{
        No(-1 , "不能领取"),
        CanGet(0 , "可以领取"),
        Got(1 , "已经领取");

        private int id;
        private String desc;

        CheckInStatus(int id , String desc){
            this.id = id;
            this.desc= desc;
        }

        public int getId(){
            return id;
        }
    }
}
