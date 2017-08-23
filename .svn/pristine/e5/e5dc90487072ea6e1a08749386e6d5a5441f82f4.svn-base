package avatar.module.mahjong.replay;

import avatar.module.mahjong.Combo;
import avatar.module.mahjong.Mahjong;
import avatar.module.mahjong.MahjongConfig;
import avatar.module.mahjong.operate.ActionType;
import avatar.module.mahjong.operate.CanDoOperate;
import avatar.module.mahjong.operate.Operate;
import avatar.util.ActionTypeMapping;
import org.apache.commons.collections.CollectionUtils;

import java.util.HashMap;
import java.util.Map;

public class Action {
    private String isActionTip;
    private String actionTime;

    private ActTip actTip;

    private Act act;

    public static Action newAct(int userId, int target, int mahjongCode, ActionType actionType) {
        Action action = new Action();
        action.isActionTip = "false";
        action.actionTime = System.currentTimeMillis() + "";

        Act act = new Act();
        act.setUserId(userId + "");
        act.setAct(ActionTypeMapping.parse(actionType).getNumber() + "");

        act.setActCard(mahjongCode + "");
        act.setTargetUserId(target + "");

        action.act = act;

        action.actTip = new ActTip();
        return action;
    }

    public static Action newActTips(CanDoOperate canDoOperate) {
        Action action = new Action();
        action.isActionTip = "true";
        action.actionTime = System.currentTimeMillis() + "";

        ActTip actTip = new ActTip();
        actTip.setOptUserId(canDoOperate.getUserId() + "");
        actTip.setTipRemainTime(MahjongConfig.TIP_REMAIN_TIME + "");
        actTip.setTipRemainUT(System.currentTimeMillis() + "");
        Map<String, String> acts = new HashMap<>();
        Map<String, String> actCards = new HashMap<>();
        for (Operate operate : canDoOperate.getOperates()) {
            if (CollectionUtils.isEmpty(operate.getCombos())) {
                // 如果操作牌为空，则默认给一只无关的牌
                acts.put(acts.size() + 1 + "", ActionTypeMapping.parse(operate.getActionType()).getNumber() + "");
                actCards.put(acts.size() + 1 + "", Mahjong.ONE_WANG.getCode() + "");
                continue;
            }
            // 如果是胡牌，则只传一个combo
            // 暂时不考虑听牌。将胡的操作的combo 传给actCards时，只传一张拍牌
            if (operate.getActionType() == ActionType.ZI_MO
                    || operate.getActionType() == ActionType.QIANG_PENG_GANG_HU
                    || operate.getActionType() == ActionType.QIANG_AN_GANG_HU
                    || operate.getActionType() == ActionType.QIANG_ZHI_GANG_HU
                    || operate.getActionType() == ActionType.CHI_HU) {
                acts.put(acts.size() + 1 + "", ActionTypeMapping.parse(operate.getActionType()).getNumber() + "");
                actCards.put(acts.size() + 1 + "", operate.getCombos().get(0).getMahjongs().get(0).getCode() + "");
                continue;
            }

            for (Combo combo : operate.getCombos()) {
                acts.put(acts.size() + 1 + "", ActionTypeMapping.parse(operate.getActionType()).getNumber() + "");
                actCards.put(acts.size() + 1 + "", combo.getMahjongs().get(0).getCode() + "");
            }
        }
        actTip.setActs(acts);
        actTip.setActCards(actCards);

        action.actTip = actTip;
        action.act = new Act();
        return action;
    }

    public String getIsActionTip() {
        return isActionTip;
    }

    public String getActionTime() {
        return actionTime;
    }

    public ActTip getActTip() {
        return actTip;
    }

    public Act getAct() {
        return act;
    }

}
