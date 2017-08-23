package avatar.module.mahjong.replay;

import java.util.Map;

public class Act {
    private String userId;

    private String act;

    private String actCard;

    private String targetUserId;

    private Map<String, String> flowerCards;

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public String getAct() {
        return act;
    }

    public void setAct(String act) {
        this.act = act;
    }

    public String getActCard() {
        return actCard;
    }

    public void setActCard(String actCard) {
        this.actCard = actCard;
    }

    public String getTargetUserId() {
        return targetUserId;
    }

    public void setTargetUserId(String targetUserId) {
        this.targetUserId = targetUserId;
    }

    public Map<String, String> getFlowerCards() {
        return flowerCards;
    }

    public void setFlowerCards(Map<String, String> flowerCards) {
        this.flowerCards = flowerCards;
    }

}
