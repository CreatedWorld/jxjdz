package avatar.module.mahjong.operate.task.impl;


import avatar.module.express.XmlExpress;
import avatar.module.mahjong.operate.ActionType;
import avatar.module.mahjong.operate.task.abs.BaseScanTask;

public class BackPengGangScanTask extends BaseScanTask {

    @Override
    protected void setActionType() {
        this.actionType = ActionType.BACK_PENG_GANG;
    }

    @Override
    protected void setRules() {
        this.rules = XmlExpress.getBackPengGangs();
    }

}
