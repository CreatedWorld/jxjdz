package avatar.module.noticeconfig;

import avatar.entity.noticeconfig.NoticeConfig;
import avatar.util.GameData;

import java.util.List;

/**
 * 大厅配置接口（包括公告、喇叭、客服）
 */
public class NoticeConfigDao {
    private NoticeConfigDao() {
    }
    private static final NoticeConfigDao instance = new NoticeConfigDao();
    public static final NoticeConfigDao getInstance(){ return  instance; }

    //=========接口=========//
    /**
     * 获取大厅所有配置：公告、喇叭、客服
     * @return 大厅所有配置信息集合
     */
    public List<NoticeConfig> getAllNoticeConfigData(){
        return  getAllNoticeConfigDataDB();
    }

    //=========cache=========//

    //=========DB=========//
    //查询
    /**
     * 查询大厅所有配置：公告、喇叭、客服
     * @return 大厅所有配置信息集合
     */
    private List<NoticeConfig> getAllNoticeConfigDataDB(){
        List<NoticeConfig> noticeConfigs = GameData.getDB().list(NoticeConfig.class, "SELECT * FROM config_notice", new Object[]{});
        return noticeConfigs;
    }
}
