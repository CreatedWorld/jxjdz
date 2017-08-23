package avatar.entity.announcements;

import avatar.util.BaseEntity;
import avatar.util.utilDB.annotation.Column;
import avatar.util.utilDB.annotation.Pk;
import avatar.util.utilDB.annotation.Table;
import org.springframework.stereotype.Service;

import java.util.Date;

/**
 * 公告实体类
 */
@Service
@Table(name = "config_announcements" ,comment = "系统公告")
public class Announcements extends BaseEntity{
    public Announcements() {
        super(Announcements.class);
    }

    @Pk
    @Column(name = "id" , comment = "表id")
    private int id;

    @Column(name = "content" ,comment = "内容")
    private String content;

    @Column(name = "status" , comment = "状态")
    private int status;

    @Column(name = "startTime" , comment = "公告生效的开始时间")
    private Date startTime;

    @Column(name = "endTime" , comment = "公告生效的结束时间")
    private Date endTime;

    @Column(name = "rounds" , comment = "轮询播放次数")
    private int rounds;

    public int getId(){
        return id;
    }

    public String getContent(){
        return content;
    }

    public int getStatus(){
        return status;
    }

    public Date getStartTime(){
        return startTime;
    }

    public Date getEndTime(){
        return endTime;
    }


    public void setStatus(AnnounceStatus status){
        this.status = status.getId();
    }

    public int getRounds(){
        return rounds;
    }

    public void setRounds(int rounds){
        this.rounds = rounds;
    }

    enum AnnounceStatus{
        NORMAL(0 , "未发送状态"),
        SEND(1 , "已经发送成功");

        private int id;
        private String desc;

        AnnounceStatus(int id , String desc){
            this.id = id;
            this.desc = desc;
        }

        public int getId(){
            return id;
        }

        public String getDesc(){
            return desc;
        }
    }
}
