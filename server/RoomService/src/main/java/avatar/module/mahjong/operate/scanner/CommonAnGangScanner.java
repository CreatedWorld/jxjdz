package avatar.module.mahjong.operate.scanner;

import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.operate.picker.GetAMahjongPicker;
import avatar.module.mahjong.operate.task.impl.CommonAnGangScanTask;

import java.util.ArrayList;

public class CommonAnGangScanner extends AbstractScanner {

    public static CommonAnGangScanner getInstance() {
        return new CommonAnGangScanner();
    }

    @Override
    protected void setScanTask(MahjongGameData mahjongGameData) {

        scanTasks = new ArrayList<>();

        scanTasks.add(CommonAnGangScanTask.class);
    }

    @Override
    protected void setPersonalMahjongInfoPicker() {
        this.personalMahjongInfoPicker = GetAMahjongPicker.getInstance();
    }
}


