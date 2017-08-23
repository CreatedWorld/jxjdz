package avatar.entity.arena;

import avatar.util.BaseEntity;
import avatar.util.utilDB.annotation.Column;
import avatar.util.utilDB.annotation.Pk;
import avatar.util.utilDB.annotation.Table;
import org.springframework.stereotype.Service;

import java.util.Date;


/**
 * 比赛场牌配置信息
 */
@Service
@Table(name="config_arena",comment = "比赛场配置信息表 只有一条数据'")
public class ArenaConfig extends BaseEntity{
    public ArenaConfig() {
        super(ArenaConfig.class);
    }

    @Pk
    @Column(name = "id")
    private int id;

    @Column(name="applyStartTime",comment = "报名开始时间")
    protected String applyStartTime;
    @Column(name="applyEndTime",comment = "报名结束时间")
    protected String applyEndTime;
    @Column(name="playStartTime",comment = "游戏开始时间")
    protected String playStartTime;
    @Column(name="playEndTime",comment = "游戏开始时间")
    protected String playEndTime;

    public int getId(){
        return id;
    }

    public String getApplyStartTime(){
        return applyStartTime;
    }

    public String getApplyEndTime(){
        return applyEndTime;
    }

    public String getPlayStartTime(){
        return playStartTime;
    }

    public String getPlayEndTime(){
        return playEndTime;
    }

}
