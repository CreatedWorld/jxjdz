package avatar.module.announcement;


import avatar.entity.announcements.AnnounceUser;
import avatar.entity.announcements.Announcements;
import avatar.net.session.Session;
import avatar.protobuf.Cmd;
import avatar.protobuf.Hall;
import avatar.util.GameData;

import java.util.*;

/**
 * 公告类
 */
public class AnnouncementService {
    private static final AnnouncementService instance = new AnnouncementService();
    public static final AnnouncementService getInstance(){
        return instance;
    }

    public Map<Session , List<Announcements>> getSendAnnounce(){
        List<Announcements> announcements = AnnouncementsDao.getInstance().loadAnnouncement();
        if(announcements == null || announcements.size() == 0){
            return null;
        }

        Map<Object , Session> map = GameData.getSessionManager().getSessionMap();
        Set<Object> objects = map.keySet();

        Map<Session , List<Announcements>> sessionListMap = new HashMap<>();

        for(Announcements announce : announcements) {
            List<Integer> announceUsers = AnnounceUserDao.getInstance().loadUserIds(announce.getId());

            for(Object o : objects){
                Session session = map.get(o);
                if(session == null){
                    continue;
                }
                int sessionUserId = session.getUserId();
                if(sessionUserId <= 0){
                    continue;
                }
                if(announceUsers != null && announcements.size() > 0 &&
                        announceUsers.indexOf(sessionUserId) > -1){
                    //已经发送过，无需再次处理
                }else{
                    List<Announcements> list = new ArrayList<>();
                    if(sessionListMap.containsKey(session)){
                        list = sessionListMap.get(session);
                    }
                    list.add(announce);
                    sessionListMap.put(session , list);
                }
            }
        }

        return sessionListMap;
    }

    /**
     * 推送公告<br></br>
     * 暂时推送给所有在线的用户，之后要改为分批处理
     */
    public void getAndSendAnnounce(){
        Map<Session , List<Announcements>> map =  getSendAnnounce();
        if(map == null || map.size() == 0){
            return;
        }

        Set<Session> keys = map.keySet();
        List<AnnounceUser> addList = new ArrayList<>();
        for(Session session : keys){
            if(session == null){
                continue;
            }
            List<Announcements> list = map.get(session);
            if(list == null || list.size() == 0){
                continue;
            }
            for(Announcements announcements : list) {
                AnnounceUser announceUser = new AnnounceUser();
                announceUser.setUserId(session.getUserId());
                announceUser.setAnnounceId(announcements.getId());
                addList.add(announceUser);

                Hall.PushAnnouncementS2C.Builder builder = Hall.PushAnnouncementS2C.newBuilder();
                builder.setContent(announcements.getContent());
                builder.setCirCount(announcements.getRounds());
                session.sendClient(Cmd.S2C_Hall_PUSH_ANNOUNCE , builder.build().toByteArray());
            }
        }
        if(addList.size() > 0){
            AnnounceUserDao.getInstance().insert(addList);
        }
    }

}
