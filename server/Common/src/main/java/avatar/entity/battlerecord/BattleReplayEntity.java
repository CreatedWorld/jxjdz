package avatar.entity.battlerecord;

import avatar.util.BaseEntity;
import avatar.util.utilDB.annotation.Column;
import avatar.util.utilDB.annotation.Pk;
import avatar.util.utilDB.annotation.Table;
import org.springframework.stereotype.Service;

import java.util.Date;

/**
 * 记录玩家每一局的回放记录表
 */
@Service
@Table(name = "battlereplay", comment = "记录玩家每一局的回放记录表")
public class BattleReplayEntity extends BaseEntity {
    public BattleReplayEntity() {
        super(BattleReplayEntity.class);
    }

    @Pk(auto = true)
    @Column(name = "id", comment = "每一条回放记录的id")
    private int id;

    @Column(name = "roomId", comment = "房间id")
    private int roomId;

    @Column(name = "roomCode", comment = "房间号")
    private String roomCode;

    @Column(name = "innings", comment = "该条回放记录是第几局")
    private int innings;

    @Column(name = "gamesTime", comment = "该局游戏的结束时间")
    private Date gamesTime;

    @Column(name = "replayContent", comment = "该局游戏的回放数据")
    private String replayContent;

    public void setId(int id) {
        this.id = id;
    }

    public int getId() {
        return id;
    }

    public int getInnings() {
        return innings;
    }

    public void setInnings(int innings) {
        this.innings = innings;
    }

    public Date getGamesTime() {
        return gamesTime;
    }

    public void setGamesTime(Date gamesTime) {
        this.gamesTime = gamesTime;
    }

    public String getReplayContent() {
        return replayContent;
    }

    public void setReplayContent(String replayContent) {
        this.replayContent = replayContent;
    }

    public String getRoomCode() {
        return roomCode;
    }

    public void setRoomCode(String roomCode) {
        this.roomCode = roomCode;
    }

    public int getRoomId() {
        return roomId;
    }

    public void setRoomId(int roomId) {
        this.roomId = roomId;
    }
}
