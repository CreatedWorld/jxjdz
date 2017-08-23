package avatar.entity.battlerecord;

import avatar.util.BaseEntity;
import avatar.util.utilDB.annotation.Column;
import avatar.util.utilDB.annotation.Pk;
import avatar.util.utilDB.annotation.Table;
import org.springframework.stereotype.Service;

import java.util.Date;

/**
 * 玩家每一回合的总战绩
 */
@Service
@Table(name = "userbattlescore", comment = "玩家每一回合的总战绩")
public class UserBattleScoreEntity extends BaseEntity {
    public UserBattleScoreEntity() {
        super(UserBattleScoreEntity.class);
    }

    @Pk
    @Column(name = "id", comment = "战绩id")
    private int id;

    @Column(name = "userId", comment = "玩家id")
    private int userId;

    @Column(name = "roomId", comment = "房间id")
    private int roomId;

    @Column(name = "roomCode", comment = "房间号")
    private String roomCode;

    @Column(name = "userName", comment = "玩家名称")
    private String userName;

    @Column(name = "allScore", comment = "玩家在该回合的总积分")
    private int allScore;

    @Column(name = "time", comment = "游戏时间")
    private Date time;

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

    public String getRoomCode() {
        return roomCode;
    }

    public void setRoomCode(String roomCode) {
        this.roomCode = roomCode;
    }

    public String getUserName() {
        return userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public int getAllScore() {
        return allScore;
    }

    public void setAllScore(int allScore) {
        this.allScore = allScore;
    }

    public Date getTime() {
        return time;
    }

    public void setTime(Date time) {
        this.time = time;
    }

    public int getRoomId() {
        return roomId;
    }

    public void setRoomId(int roomId) {
        this.roomId = roomId;
    }
}
