package avatar.entity.battlerecord;

import avatar.util.BaseEntity;
import avatar.util.utilDB.annotation.Column;
import avatar.util.utilDB.annotation.Pk;
import avatar.util.utilDB.annotation.Table;
import org.springframework.stereotype.Service;

import java.util.Date;

/**
 * 记录玩家在一局游戏中的得分
 */
@Service
@Table(name = "battlescore", comment = "记录玩家每一局游戏中的得分")
public class BattleScoreEntity extends BaseEntity implements Comparable {
    public BattleScoreEntity() {
        super(BattleScoreEntity.class);
    }

    @Pk
    @Column(name = "id", comment = "每一条得分记录的id")
    private int id;

    /**
     * 房间id
     */
    @Column(name = "roomId", comment = "房间id")
    private int roomId;

    /**
     * 当前局数
     */
    @Column(name = "currentTimes", comment = "当前局数")
    private int currentTimes;

    @Column(name = "battleReplayId", comment = "每一条回放记录的id")
    private int battleReplayId;

    @Column(name = "playerId", comment = "玩家的id")
    private int playerId;

    @Column(name = "playerNickName", comment = "玩家名称")
    private String playerNickName;


    @Column(name = "score", comment = "玩家的得分")
    private int score;

    /**
     * 暗杠次数
     */
    @Column(name = "anGangTimes", comment = "暗杠次数")
    private int anGangTimes;

    /**
     * 明杠次数
     */
    @Column(name = "mingGangTimes", comment = "明杠次数")
    private int mingGangTimes;

    /**
     * 点炮的用户id
     */
    @Column(name = "dianPaoUserId", comment = "点炮用户的id")
    private int dianPaoUserId;

    /**
     * 接炮的用户id
     */
    @Column(name = "jiePaoUserId", comment = "接炮用户id")
    private int jiePaoUserId;

    /**
     * 炮数
     */
    @Column(name = "paoNum", comment = "炮数")
    private int paoNum;

    /**
     * 是否自摸
     */
    @Column(name = "isZiMO", comment = "是否自摸")
    private int isZiMo;

    /**
     * 胡牌名称
     */
    @Column(name = "huName", comment = "胡牌名称")
    private String huName;

    /**
     * 输或赢的情况
     */
    @Column(name = "winOrLose", comment = "输或赢的情况")
    private int winOrLose;

    @Column(name = "createTime", comment = "胡牌时间")
    private Date huTime;

    @Override
    public int compareTo(Object o) {
        if (!(o instanceof BattleScoreEntity)) {
            return 0;
        }
        BattleScoreEntity otherScore = (BattleScoreEntity) o;
        if (this.roomId == otherScore.getRoomId()) {
            return this.getCurrentTimes() - otherScore.getCurrentTimes();
        } else {
            return this.roomId - otherScore.getRoomId();
        }
    }

    public enum WinOrLose {
        NONE(1, "没有胡牌,不用输分"),
        ZI_MO(2, "自摸"),
        DIAN_PAO(3, "点炮"),
        JIE_PAO(4, "接炮"),
        OTHER_USER_ZI_MO(5, "别家自摸");

        private int id;
        private String name;

        WinOrLose(int id, String name) {
            this.id = id;
            this.name = name;
        }

        public int getId() {
            return id;
        }

        public String getName() {
            return name;
        }
    }

    public enum IsZiMo {
        ZI_MO(1, "自摸"),
        NOT_ZI_MO(2, "非自摸");

        private int id;
        private String name;

        IsZiMo(int id, String name) {
            this.id = id;
            this.name = name;
        }

        public int getId() {
            return id;
        }

        public String getName() {
            return name;
        }
    }

    /**
     * 我是否赢
     */
    public boolean isWinner() {
        WinOrLose[] values = WinOrLose.values();
        WinOrLose w = null;
        for (WinOrLose value : values) {
            if (value.getId() == this.winOrLose) {
                w = value;
                break;
            }
        }
        if (w == null) {
            return false;
        }
        switch (w) {
            case NONE:
                return false;
            case ZI_MO:
                return true;
            case DIAN_PAO:
                return false;
            case JIE_PAO:
                return true;
            case OTHER_USER_ZI_MO:
                return false;
        }
        return false;

    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public int getBattleReplayId() {
        return battleReplayId;
    }

    public void setBattleReplayId(int battleReplayId) {
        this.battleReplayId = battleReplayId;
    }

    public int getPlayerId() {
        return playerId;
    }

    public void setPlayerId(int playerId) {
        this.playerId = playerId;
    }

    public int getScore() {
        return score;
    }

    public void setScore(int score) {
        this.score = score;
    }

    public String getPlayerNickName() {
        return playerNickName;
    }

    public void setPlayerNickName(String playerNickName) {
        this.playerNickName = playerNickName;
    }

    public int getAnGangTimes() {
        return anGangTimes;
    }

    public void setAnGangTimes(int anGangTimes) {
        this.anGangTimes = anGangTimes;
    }

    public int getDianPaoUserId() {
        return dianPaoUserId;
    }

    public void setDianPaoUserId(int dianPaoUserId) {
        this.dianPaoUserId = dianPaoUserId;
    }

    public int getIsZiMo() {
        return isZiMo;
    }

    public void setIsZiMo(int isZiMo) {
        this.isZiMo = isZiMo;
    }

    public int getJiePaoUserId() {
        return jiePaoUserId;
    }

    public void setJiePaoUserId(int jiePaoUserId) {
        this.jiePaoUserId = jiePaoUserId;
    }

    public int getMingGangTimes() {
        return mingGangTimes;
    }

    public void setMingGangTimes(int mingGangTimes) {
        this.mingGangTimes = mingGangTimes;
    }

    public int getRoomId() {
        return roomId;
    }

    public void setRoomId(int roomId) {
        this.roomId = roomId;
    }

    public int getPaoNum() {
        return paoNum;
    }

    public void setPaoNum(int paoNum) {
        this.paoNum = paoNum;
    }

    public int getCurrentTimes() {
        return currentTimes;
    }

    public void setCurrentTimes(int currentTimes) {
        this.currentTimes = currentTimes;
    }

    public int getWinOrLose() {
        return winOrLose;
    }

    public void setWinOrLose(int winOrLose) {
        this.winOrLose = winOrLose;
    }

    public String getHuName() {
        return huName;
    }

    public void setHuName(String huName) {
        this.huName = huName;
    }

    public Date getHuTime() {
        return huTime;
    }

    public void setHuTime(Date huTime) {
        this.huTime = huTime;
    }
}
