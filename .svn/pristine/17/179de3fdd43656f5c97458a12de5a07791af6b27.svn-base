package avatar.module.mahjong.operate.task.impl;

import avatar.module.express.XmlExpress;
import avatar.module.mahjong.operate.ActionType;
import avatar.module.mahjong.operate.task.abs.BaseScanTask;

/**
 * 吃牌的扫描任务
 */
public class ChiScanTask extends BaseScanTask {
    @Override
    protected void setActionType() {
        this.actionType = ActionType.CHI;
    }

    @Override
    protected void setRules() {
        this.rules = XmlExpress.getChis();
    }
}
