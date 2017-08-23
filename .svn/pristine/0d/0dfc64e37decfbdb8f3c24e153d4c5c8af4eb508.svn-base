package avatar.entity.arena;

import avatar.util.BaseEntity;
import avatar.util.utilDB.annotation.Column;
import avatar.util.utilDB.annotation.Pk;
import avatar.util.utilDB.annotation.Table;
import org.springframework.stereotype.Service;

import java.util.Date;

/**
 * 比赛场排名
 */
@Service
@Table(name="arenaranking",comment = "比赛场排名")
public class ArenaRankingEntity extends BaseEntity{

    public ArenaRankingEntity() {
        super(ArenaRankingEntity.class);
    }
  @Pk
  @Column(name="id",comment = "主键")
   protected  int id;
    @Column(name="userId",comment = "用户id")
    protected int userId;
    @Column(name = "userName",comment = "用户名称")
    protected String userName;
    @Column(name="score",comment = "比赛分数")
    protected int score;
    @Column(name="arenaStartTime",comment = "游戏开始时间")
    protected Date arenaStartTime;
    @Column(name="arenaEndTime",comment = "游戏结束时间")
    protected Date arenaEndTime;
    @Column(name="isNew",comment = "是否最近的比赛记录 0 否 1 是")
    protected int isNew;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public int getUserId() {
        return userId;
    }

    public void setUserId(int userId) {
        this.userId = userId;
    }

    public String getUserName() {
        return userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public int getScore() {
        return score;
    }

    public void setScore(int score) {
        this.score = score;
    }

    public Date getArenaStartTime() {
        return arenaStartTime;
    }

    public void setArenaStartTime(Date arenaStartTime) {
        this.arenaStartTime = arenaStartTime;
    }

    public Date getArenaEndTime() {
        return arenaEndTime;
    }

    public void setArenaEndTime(Date arenaEndTime) {
        this.arenaEndTime = arenaEndTime;
    }

    public int getIsNew() {
        return isNew;
    }

    public void setIsNew(int isNew) {
        this.isNew = isNew;
    }

    public enum Status{
        NEW(1,"新的数据"),
        OLD(0,"旧的数据");
        private int id;
        private String desc;
        Status(int id,String desc){
            this.id=id;
            this.desc=desc;
        }
        public int getId(){
            return id;
        }
    }
}
