package avatar.module.mahjong;

import java.util.ArrayList;
import java.util.List;

public class PersonalMahjongInfo {
    /**
     * 手上的麻将，不包含碰了和杠了的麻将
     */
    private List<Mahjong> handCards;

    /**
     * 碰了的麻将
     */
    private List<Combo> pengs;

    /**
     * 杠了的麻将
     */
    private List<Combo> gangs;

    /**
     * 吃了的麻将
     */
    private List<Combo> chi;

    // todo 花牌

    /**
     * 最新摸到的麻将
     */
    private Mahjong touchMahjong;

    /**
     * 自己打出的麻将，被别人碰或杠了的麻将，不会存放在此集合。每次打出的麻将，加在队尾
     */
    private List<Mahjong> outMahjong;

    /**
     * 用户id
     */
    private Integer userId;

    public List<Mahjong> getHandCards() {
        if (this.handCards == null) {
            this.handCards = new ArrayList<>();
        }
        return handCards;
    }

    public void setHandCards(List<Mahjong> handCards) {
        this.handCards = handCards;
    }

    public List<Combo> getPengs() {
        return pengs;
    }

    public void setPengs(List<Combo> pengs) {
        this.pengs = pengs;
    }

    public List<Combo> getGangs() {
        return gangs;
    }

    public void setGangs(List<Combo> gangs) {
        this.gangs = gangs;
    }

    public Mahjong getTouchMahjong() {
        return touchMahjong;
    }

    public void setTouchMahjong(Mahjong touchMahjong) {
        this.touchMahjong = touchMahjong;
    }

    public List<Mahjong> getOutMahjong() {
        if (this.outMahjong == null) {
            this.outMahjong = new ArrayList<>();
        }
        return outMahjong;
    }

    public void setOutMahjong(List<Mahjong> outMahjong) {
        this.outMahjong = outMahjong;
    }

    public Integer getUserId() {
        return userId;
    }

    public void setUserId(Integer userId) {
        this.userId = userId;
    }

    public List<Combo> getChi() {
        if (this.chi == null) {
            this.chi = new ArrayList<>(4);
        }
        return chi;
    }

    public void setChi(List<Combo> chi) {
        this.chi = chi;
    }

    public static PersonalMahjongInfo getMyInfo(List<PersonalMahjongInfo> personalMahjongInfos, int userId) {
        for (PersonalMahjongInfo personalMahjongInfo : personalMahjongInfos) {
            if (personalMahjongInfo.getUserId() == userId) {
                return personalMahjongInfo;
            }
        }
        return null;
    }

    /**
     * 判断用户的手牌和摸到的牌有没有包含指定的牌
     */
    public boolean hasMahjong(Mahjong mahjong) {
        for (Mahjong temp : this.handCards) {
            if (temp == mahjong) {
                return true;
            }
        }
        return mahjong == this.touchMahjong;
    }

    /**
     * 移除打出的牌
     */
    public void removePlayedMahjong(Mahjong playedMahjong) {
        if (this.touchMahjong == null) {
            this.handCards.remove(playedMahjong);
        } else {
            if (this.touchMahjong != playedMahjong) {
                this.handCards.add(this.touchMahjong);
                this.handCards.remove(playedMahjong);
            }
            this.touchMahjong = null;
        }
    }
}
