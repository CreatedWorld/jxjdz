package avatar.entity.room;

import avatar.util.BaseEntity;
import avatar.util.utilDB.annotation.Column;
import avatar.util.utilDB.annotation.Pk;
import avatar.util.utilDB.annotation.Table;
import org.springframework.stereotype.Service;

import java.util.Date;

@Service
@Table(name = "RoomPlayer", comment = "房间玩家")
public class RoomPlayer extends BaseEntity implements Comparable {
    public RoomPlayer() {
        super(RoomPlayer.class);
    }

    @Pk(auto = true)
    @Column(name = "id", comment = "房间玩家id")
    protected int id;

    @Column(name = "roomId", comment = "房间id")
    protected int roomId;

    @Column(name = "playerId", comment = "玩家id")
    protected int playerId;

    @Column(name = "seat", comment = "玩家座位号，从1开始")
    protected int seat;

    @Column(name = "state", comment = "状态")
    protected int state;

    @Column(name = "time", comment = "记录时间。可用于查看玩家是什么时候加入、创建房间的")
    private Date time;

    public int getRoomId() {
        return roomId;
    }

    public void setRoomId(int roomId) {
        this.roomId = roomId;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public int getPlayerId() {
        return playerId;
    }

    public void setPlayerId(int playerId) {
        this.playerId = playerId;
    }

    public int getState() {
        return state;
    }

    public void setState(int state) {
        this.state = state;
    }

    public int getSeat() {
        return seat;
    }

    public void setSeat(int seat) {
        this.seat = seat;
    }

    public void setTime() {
        this.time = new Date();
    }

    public Date getTime() {
        return this.time;
    }

    @Override
    public int compareTo(Object o) {
        if (!(o instanceof RoomPlayer)) {
            throw new IllegalArgumentException("obj is not instanceof RoomPlayer");
        }
        return this.getSeat() - ((RoomPlayer) o).getSeat();
    }

    public enum State {
        UNREADY(1, "待准备"),
        READY(2, "已准备");

        public int getCode() {
            return code;
        }

        public void setCode(int code) {
            this.code = code;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        private int code;

        private String name;

        State(int code, String name) {
            this.code = code;
            this.name = name;
        }

        public static State parse(int code) {
            for (State s : State.values()) {
                if (s.getCode() == code) {
                    return s;
                }
            }
            throw new RuntimeException(String.format(
                    "无法解析code为[%s]的RoomPlayer.State",
                    code)
            );
        }
    }

}
