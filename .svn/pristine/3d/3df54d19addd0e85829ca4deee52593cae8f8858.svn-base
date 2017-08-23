package avatar.module.mahjong.operate.scanner;

import avatar.module.mahjong.Mahjong;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.operate.CanDoOperate;
import avatar.module.mahjong.operate.picker.PersonalMahjongInfoPicker;
import avatar.module.mahjong.operate.task.abs.BaseScanTask;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

/**
 * 操作扫描管理器，
 * * 依次扫描scanTasks中的具体任务，得出所有玩家可以有的操作列表
 */
public abstract class AbstractScanner {

    protected static final Logger log = LoggerFactory.getLogger(AbstractScanner.class);

    /**
     * 已注册的扫描任务
     */
    protected List<Class<? extends BaseScanTask>> scanTasks;

    protected PersonalMahjongInfoPicker personalMahjongInfoPicker;

    protected abstract void setPersonalMahjongInfoPicker();

    AbstractScanner() {
        setPersonalMahjongInfoPicker();
    }

    protected abstract void setScanTask(MahjongGameData mahjongGameData);

    @SuppressWarnings("unchecked")
    public List<CanDoOperate> scan(MahjongGameData mahjongGameData, Mahjong specialMahjong, int userId)
            throws Exception {
        setScanTask(mahjongGameData);

        List<CanDoOperate> canDoOperates = new ArrayList<>();
        for (Class<? extends BaseScanTask> scanTask : scanTasks) {
            BaseScanTask task = scanTask.newInstance();
            task.setMahjongGameData(mahjongGameData);
            task.setSpecifiedMahjong(specialMahjong);
            task.setSpecifiedUserId(userId);
            task.setCanOperates(canDoOperates);
            task.setPersonalMahjongInfoPicker(personalMahjongInfoPicker);
            task.setScanner(this.getClass());
            task.scan();
        }
        canDoOperates.removeIf(next -> next.getOperates().size() == 0);
        if (canDoOperates.size() > 1) {
            Collections.sort(canDoOperates);
        }
        // for (CanDoOperate canDoOperate : canDoOperates) {
        //     canDoOperate.setScanner(this.getClass());
        // }
        log.info("最终扫描结果：{}", canDoOperates);
        return canDoOperates;
    }

}
