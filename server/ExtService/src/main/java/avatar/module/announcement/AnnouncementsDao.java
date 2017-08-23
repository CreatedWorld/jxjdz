package avatar.module.announcement;


import avatar.entity.announcements.Announcements;
import avatar.util.GameData;

import java.util.Date;
import java.util.List;

/**
 * 系统公告数据接口
 */
public class AnnouncementsDao {
    private static final AnnouncementsDao instance = new AnnouncementsDao();
    public static final AnnouncementsDao getInstance(){
        return instance;
    }

    public List<Announcements> loadAnnouncement(){
        return loadDB();
    }

    private List<Announcements> loadDB(){
        String sql = "select * from config_announcements where startTime < ? and endTime > ? ";
        List<Announcements> list = GameData.getDB().list(Announcements.class , sql ,
                new Object[]{new Date() , new Date()});
        return list;
    }
}
