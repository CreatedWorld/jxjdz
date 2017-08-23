package avatar.module.mahjong.operate;

import avatar.module.express.Rule;
import avatar.module.mahjong.Combo;

import java.util.List;

/**
 * 扫描出来可以执行的一个操作
 */
public class Operate {
    /**
     * 可以进行操作对应的规则
     */
    private Rule rule;

    /**
     * 执行动作
     */
    private ActionType actionType;

    /**
     * 可以进行的操作的麻将
     * 例如可以回头暗杠1万和2万，则是[{1,1,1,1},{2,2,2,2}]
     * 如果可以碰1万，则是[{1,1,1}]
     */
    private List<Combo> combos;

    public Operate() {
    }

    public Operate(ActionType actionType) {
        this.actionType = actionType;
    }

    public Rule getRule() {
        return rule;
    }

    public void setRule(Rule rule) {
        this.rule = rule;
    }

    public List<Combo> getCombos() {
        return combos;
    }

    public void setCombos(List<Combo> combos) {
        this.combos = combos;
    }

    public ActionType getActionType() {
        return actionType;
    }

    public void setActionType(ActionType actionType) {
        this.actionType = actionType;
    }
}
