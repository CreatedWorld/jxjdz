package avatar;

import avatar.module.arena.ArenaConfigDataService;
import avatar.module.roomType.RoomTypeService;

/**
 * 中心服务器初始化所有配置
 */
public class InitCenterDBConfig extends InitDBConfig{
    @Override
    public void loadConfig() {
        initAllConfig();
    }

    public static void initAllConfig(){
        RoomTypeService.getInstance().init();
        ArenaConfigDataService.getInstance().loadArenaConfig();
    }
}
