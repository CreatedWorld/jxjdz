package avatar.module.mahjong.operate.scanner;

import avatar.entity.room.Room;
import avatar.entity.room.RoomPlayType;
import avatar.module.mahjong.Mahjong;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.operate.ActionType;
import avatar.module.mahjong.operate.CanDoOperate;
import avatar.module.mahjong.operate.Operate;
import avatar.module.mahjong.operate.picker.PutOutMahjongPicker;
import avatar.module.mahjong.operate.task.impl.ChiHuScanTask;
import avatar.module.mahjong.operate.task.impl.ChiScanTask;
import avatar.module.mahjong.operate.task.impl.PengScanTask;
import avatar.module.mahjong.operate.task.impl.ZhiGangScanTask;
import avatar.module.mahjong.service.BaseGameService;
import avatar.module.room.service.RoomDataService;

import java.util.ArrayList;
import java.util.List;

public class PlayACardScanner extends AbstractScanner {

    public static PlayACardScanner getInstance() {
        return new PlayACardScanner();
    }

    @Override
    protected void setScanTask(MahjongGameData mahjongGameData) {
        Room room = RoomDataService.getInstance().getRoomByRoomId(mahjongGameData.getRoomId());
        List<Integer> playTypeList = room.getPlayTypeList();

        scanTasks = new ArrayList<>();

        if (!playTypeList.contains(RoomPlayType.NO_CHI_HU.getId())) {
            // 吃胡
            scanTasks.add(ChiHuScanTask.class);
        }

        // 直杠（明杠）
        scanTasks.add(ZhiGangScanTask.class);

        // 碰
        scanTasks.add(PengScanTask.class);


        //吃
        if (playTypeList.contains(RoomPlayType.CAN_CHI.getId())) {
            scanTasks.add(ChiScanTask.class);
        }
    }

    @Override
    protected void setPersonalMahjongInfoPicker() {
        this.personalMahjongInfoPicker = PutOutMahjongPicker.getInstance();
    }

    @Override
    public List<CanDoOperate> scan(MahjongGameData mahjongGameData, Mahjong specialMahjong, int userId) throws Exception {
        List<CanDoOperate> canDoOperates = super.scan(mahjongGameData, specialMahjong, userId);

        int nextUserId = BaseGameService.getInstance().getNextTouchMahjongUserId(mahjongGameData, userId);
        for (CanDoOperate canDoOperate : canDoOperates) {
            if (canDoOperate.getUserId() != nextUserId) {
                canDoOperate.getOperates().removeIf(operate -> operate.getActionType() == ActionType.CHI);
            }
        }

        for (CanDoOperate canDoOperate : canDoOperates) {
            // 添加过操作
            canDoOperate.getOperates().add(new Operate(ActionType.PASS));
        }

        return canDoOperates;
    }
}
