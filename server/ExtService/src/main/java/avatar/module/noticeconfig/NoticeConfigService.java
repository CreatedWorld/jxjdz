package avatar.module.noticeconfig;

import avatar.entity.noticeconfig.NoticeConfig;

import java.util.List;

/**
 * 大厅配置接口（公告、喇叭、客服）
 */
public class NoticeConfigService {
    private static final NoticeConfigService instance = new NoticeConfigService();
    public static final NoticeConfigService getInstance(){ return instance;}

    /**
     * 获取大厅公告配置：公告、喇叭、客服
     * @return 大厅公告配置
     */
    public List<NoticeConfig> getAllNoticeConfigData(){
        return NoticeConfigDao.getInstance().getAllNoticeConfigData();
    }
}
