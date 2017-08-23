package avatar.entity.arena;

import avatar.util.BaseEntity;
import avatar.util.utilDB.annotation.Column;
import avatar.util.utilDB.annotation.Pk;
import avatar.util.utilDB.annotation.Table;
import org.springframework.stereotype.Service;

import java.util.Date;

/**
 * 比赛场报名
 */
@Service
@Table(name="ArenaApplyUser",comment = "玩家比赛场报名表")
public class ArenaApplyUserEntity extends BaseEntity{
    public ArenaApplyUserEntity() {
        super(ArenaApplyUserEntity.class);
    }

    @Pk(auto = false)
    @Column(name="userId",comment = "参加比赛用户的id")
    protected int userId;
    @Column(name="isApply",comment ="是否参加比赛 0 未参加 1 参加")
    protected int isApply;
    @Column(name="applyTime",comment = "报名时间")
    protected Date applyTime;



    public int getUserId() {
        return userId;
    }

    public void setUserId(int userId) {
        this.userId = userId;
    }

    public int getIsApply() {
        return isApply;
    }

    public void setIsApply(int isApply) {
        this.isApply = isApply;
    }

    public Date getApplyTime() {
        return applyTime;
    }

    public void setApplyTime(Date applyTime) {
        this.applyTime = applyTime;
    }


    public enum Status{
        NORMAL(0 , "未报名状态"),
        NOBETWEEN(2,"不在报名时间内"),
        APPLY(1 , "已经报名");

        private int id;
        private String desc;

        Status(int id , String desc){
            this.id = id;
            this.desc = desc;
        }

        public int getId(){
            return id;
        }
    }
}
