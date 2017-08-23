package avatar;

import avatar.module.roomType.RoomTypeService;

/**
 * Created by Administrator on 2017/5/19.
 */
public class InitRoomDBConfig extends InitDBConfig{
    @Override
    public void loadConfig() {
        RoomTypeService.getInstance().init();
    }
}
