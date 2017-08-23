package avatar.entity.shop;

import avatar.util.BaseEntity;
import avatar.util.utilDB.annotation.Column;
import avatar.util.utilDB.annotation.Pk;
import avatar.util.utilDB.annotation.Table;
import org.springframework.stereotype.Service;

/**
 * 玩家与代理的绑定关系
 */
@Service
@Table(name = "playeragent", comment = "玩家与代理的绑定关系")
public class PlayerAgent extends BaseEntity {
    public PlayerAgent() {
        super(PlayerAgent.class);
    }

    @Pk
    @Column(name = "id", comment = "id")
    private int id;

    @Column(name = "playerId", comment = "玩家id")
    private int playerId;

    @Column(name = "adminId", comment = "管理员id，即代理id，对应player表的id")
    private int adminId;

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

    public int getAdminId() {
        return adminId;
    }

    public void setAdminId(int adminId) {
        this.adminId = adminId;
    }
}
