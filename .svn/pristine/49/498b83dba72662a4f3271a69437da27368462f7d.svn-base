package avatar.module.mahjong;

import avatar.module.mahjong.action.Operated;
import avatar.module.mahjong.operate.CanDoOperate;
import avatar.module.mahjong.replay.Action;
import avatar.module.mahjong.replay.Replay;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Date;
import java.util.List;

public class MahjongGameData {

    // 房间号
    private int roomId;

    // 庄家的座位号，从1开始
    private int bankerSite;

    // 下一局游戏的庄家的座位号，在单局结算时，赋值。默认为1
    private int nextBankerSite = 1;

    // 骰子
    private Integer[] dices;

    // 当前局数
    private int currentTimes;

    // 总局数
    private int totalTimes;

    // n个人的手牌，玩家的座位与list下标对应，第一个玩家下标为0
    private List<PersonalMahjongInfo> personalMahjongInfos;

    // 剩下可以被摸牌的麻将，有先后顺序
    private List<Mahjong> leftCards;

    // 当前游戏状态中，玩家可以进行的操作
    private List<CanDoOperate> canDoOperates;

    //记录房间玩家申请解散房间队列列表
    private List<ApplyDissolveData> applyDissolveList;

    // 记录当前局的回放数据，在单局结算时，将数据存进数据库
    private Replay replay;

    // 出现一炮多响时，记录每个玩家选择过或胡的情况
    private List<MultipleChiHu> multipleChiHus;

    // 上一局赢的人的操作列表
    private List<CanDoOperate> lastWinCanDoOperate;

    // 记录所有玩家执行过的操作
    private List<Operated> operated;

    // 是否消耗了创建房间的资源
    private boolean isSpend;

    //宝牌 需要在BaseGameService.initGameData时，给本变量赋值
    private Mahjong treasureCard;

    // 宝牌能变成的麻将
    private Collection<Mahjong> makeUpMahjongs;

    public Mahjong getTreasureCard() {
        return treasureCard;
    }

    public void setTreasureCard(Mahjong treasureCard) {
        this.treasureCard = treasureCard;
    }

    public Collection<Mahjong> getMakeUpMahjongs() {
        return makeUpMahjongs;
    }

    public void setMakeUpMahjongs(Collection<Mahjong> makeUpMahjongs) {
        this.makeUpMahjongs = makeUpMahjongs;
    }

    public boolean isSpend() {
        return isSpend;
    }

    public void setSpend(boolean spend) {
        isSpend = spend;
    }

    public List<CanDoOperate> getLastWinCanDoOperate() {
        return lastWinCanDoOperate;
    }

    public void setLastWinCanDoOperate(List<CanDoOperate> lastWinCanDoOperate) {
        this.lastWinCanDoOperate = lastWinCanDoOperate;
    }

    public Replay getReplay() {
        return replay;
    }

    public List<Operated> getOperated() {
        return operated;
    }

    public void setOperated(List<Operated> operated) {
        this.operated = operated;
    }

    public void setReplay(Replay replay) {
        this.replay = replay;
    }

    public List<CanDoOperate> getCanDoOperates() {
        return canDoOperates;
    }

    public void setCanDoOperates(List<CanDoOperate> canDoOperates) {
        this.canDoOperates = canDoOperates;
    }

    public int getBankerSite() {
        return bankerSite;
    }

    public void setBankerSite(int bankerSite) {
        this.bankerSite = bankerSite;
    }

    public Integer[] getDices() {
        return dices;
    }

    public void setDices(Integer[] dices) {
        this.dices = dices;
    }

    public int getCurrentTimes() {
        return currentTimes;
    }

    public void setCurrentTimes(int currentTimes) {
        this.currentTimes = currentTimes;
    }

    public int getTotalTimes() {
        return totalTimes;
    }

    public void setTotalTimes(int totalTimes) {
        this.totalTimes = totalTimes;
    }

    public List<PersonalMahjongInfo> getPersonalMahjongInfos() {
        return personalMahjongInfos;
    }

    public void setPersonalMahjongInfos(List<PersonalMahjongInfo> personalMahjongInfos) {
        this.personalMahjongInfos = personalMahjongInfos;
    }

    public List<Mahjong> getLeftCards() {
        return leftCards;
    }

    public void setLeftCards(List<Mahjong> leftCards) {
        this.leftCards = leftCards;
    }

    public int getRoomId() {
        return roomId;
    }

    public void setRoomId(int roomId) {
        this.roomId = roomId;
    }

    public int getNextBankerSite() {
        return nextBankerSite;
    }

    public void setNextBankerSite(int nextBankerSite) {
        this.nextBankerSite = nextBankerSite;
    }

    public List<ApplyDissolveData> getApplyDissolveList() {
        return applyDissolveList;
    }

    public List<MultipleChiHu> getMultipleChiHus() {
        return multipleChiHus;
    }

    public void putApplyDissolve(int userId) {
        if (applyDissolveList == null) {
            applyDissolveList = new ArrayList<>();
        }
        if (applyDissolveList.size() > 0) {
            for (ApplyDissolveData data : applyDissolveList) {
                if (data.userId == userId) {
                    return;
                }
            }
        }
        ApplyDissolveData applyDissolveData = new ApplyDissolveData(userId, new Date());
        applyDissolveList.add(applyDissolveData);
    }

    public void resetApplyDissolve() {
        applyDissolveList.clear();
    }

    public void addAction(Action action) {
        if (this.replay == null) {
            this.replay = new Replay(this);
        }
        this.replay.getActions().put(this.replay.getActions().size() + 1 + "", action);
    }

    public void addMultipleChiHu(MultipleChiHu multipleChiHu) {
        if (this.multipleChiHus == null) {
            this.multipleChiHus = new ArrayList<>(3);
        }
        this.multipleChiHus.add(multipleChiHu);
    }

    public void addOperated(Operated operated) {
        if (this.operated == null) {
            this.operated = new ArrayList<>(20);
        }
        this.operated.add(operated);
    }

    public class ApplyDissolveData {
        private int userId;
        private Date startTime;

        ApplyDissolveData(int userId, Date startTime) {
            this.userId = userId;
            this.startTime = startTime;
        }

        public Date getStartTime() {
            return startTime;
        }

        public int getUserId() {
            return userId;
        }

        @Override
        public String toString() {
            return "ApplyDissolveData{" +
                    "userId=" + userId +
                    ", startTime=" + startTime +
                    '}';
        }
    }
}
