package avatar.module.mahjong.operate.scanner;

import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.operate.picker.GetAMahjongPicker;
import avatar.module.mahjong.operate.task.impl.ZiMoScanTask;

import java.util.ArrayList;

public class HuScanner extends AbstractScanner {

    public static HuScanner getInstance() {
        return new HuScanner();
    }

    @Override
    protected void setScanTask(MahjongGameData mahjongGameData) {
        scanTasks = new ArrayList<>();

        // 胡
        scanTasks.add(ZiMoScanTask.class);
    }

    @Override
    protected void setPersonalMahjongInfoPicker() {
        this.personalMahjongInfoPicker = GetAMahjongPicker.getInstance();
    }
}
