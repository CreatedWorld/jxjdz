package avatar.module.mahjong.operate.scanner;

import avatar.module.mahjong.Mahjong;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.operate.ActionType;
import avatar.module.mahjong.operate.CanDoOperate;
import avatar.module.mahjong.operate.Operate;
import avatar.module.mahjong.operate.picker.GetAMahjongPicker;
import avatar.module.mahjong.operate.task.impl.*;

import java.util.ArrayList;
import java.util.List;

public class GetACardScanner extends AbstractScanner {

    public static GetACardScanner getInstance() {
        return new GetACardScanner();
    }

    @Override
    protected void setScanTask(MahjongGameData mahjongGameData) {
        scanTasks = new ArrayList<>();

        // 胡
        scanTasks.add(ZiMoScanTask.class);

        // 普通暗杠
        scanTasks.add(CommonAnGangScanTask.class);

        // 回头暗杠
        scanTasks.add(BackAnGangScanTask.class);

        // 普通碰杠（明杠）
        scanTasks.add(CommonPengGangScanTask.class);

        // 回头碰杠（明杠）
        scanTasks.add(BackPengGangScanTask.class);
    }

    @Override
    protected void setPersonalMahjongInfoPicker() {
        this.personalMahjongInfoPicker = GetAMahjongPicker.getInstance();
    }

    @Override
    public List<CanDoOperate> scan(MahjongGameData mahjongGameData, Mahjong specialMahjong, int userId) throws Exception {

        List<CanDoOperate> canDoOperates = super.scan(mahjongGameData, specialMahjong, userId);

        if (canDoOperates.isEmpty()) {
            // 添加出牌操作
            CanDoOperate canDoOperate = CanDoOperate.newPlayAMahjongOperate(userId, specialMahjong);
            canDoOperate.setScanner(this.getClass());
            canDoOperates.add(canDoOperate);
        } else {
            // 添加过操作
            CanDoOperate canDoOperate = canDoOperates.get(0);
            canDoOperate.setScanner(this.getClass());
            canDoOperate.getOperates().add(new Operate(ActionType.PASS));
        }


        return canDoOperates;
    }
}
