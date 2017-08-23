package avatar.module.mahjong.operate.task.impl;


import avatar.module.express.XmlExpress;
import avatar.module.mahjong.operate.ActionType;
import avatar.module.mahjong.operate.task.abs.BaseScanTask;

public class PengScanTask extends BaseScanTask {
    @Override
    protected void setActionType() {
        this.actionType = ActionType.PENG;
    }

    @Override
    protected void setRules() {
        this.rules = XmlExpress.getPengs();
    }

}
