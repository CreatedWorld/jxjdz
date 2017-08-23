package avatar.module.mahjong.operate.scanner;

import avatar.module.mahjong.Mahjong;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.operate.ActionType;
import avatar.module.mahjong.operate.CanDoOperate;
import avatar.module.mahjong.operate.Operate;
import avatar.module.mahjong.operate.picker.PutOutMahjongPicker;
import avatar.module.mahjong.operate.task.impl.QiangAnGangHuScanTask;

import java.util.ArrayList;
import java.util.List;

public class QiangAnGangScanner extends AbstractScanner {

    public static QiangAnGangScanner getInstance() {
        return new QiangAnGangScanner();
    }

    @Override
    protected void setScanTask(MahjongGameData mahjongGameData) {
        scanTasks = new ArrayList<>();

        // 胡
        scanTasks.add(QiangAnGangHuScanTask.class);
    }

    @Override
    protected void setPersonalMahjongInfoPicker() {
        this.personalMahjongInfoPicker = PutOutMahjongPicker.getInstance();
    }

    @Override
    public List<CanDoOperate> scan(MahjongGameData mahjongGameData, Mahjong specialMahjong, int userId) throws Exception {
        List<CanDoOperate> canDoOperates = super.scan(mahjongGameData, specialMahjong, userId);

        for (CanDoOperate canDoOperate : canDoOperates) {
            // 添加过操作
            canDoOperate.getOperates().add(new Operate(ActionType.PASS));
        }

        return canDoOperates;
    }
}


