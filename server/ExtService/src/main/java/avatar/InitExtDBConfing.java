package avatar;

import avatar.module.arena.ArenaConfigDataService;
import avatar.module.roomType.RoomTypeService;

/**
 * 初始化大厅所有的配置
 */
public class InitExtDBConfing extends InitDBConfig{
    @Override
    public void loadConfig() {
        initAllConfig();
    }


    public static void initAllConfig(){
        RoomTypeService.getInstance().init();
        ArenaConfigDataService.getInstance().loadArenaConfig();
    }
}
