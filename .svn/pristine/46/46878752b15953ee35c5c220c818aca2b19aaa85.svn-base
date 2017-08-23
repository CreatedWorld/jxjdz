package avatar.module.offline;

import avatar.event.InternalEventDispatcher;
import avatar.event.ListenInternalEvent;
import avatar.net.event.UserOfflineEvent;
import org.springframework.stereotype.Service;

/**
 * 处理玩家下线后的事件操作
 */
@Service
public class UserOfflineService {
    private static final UserOfflineService instance = new UserOfflineService();
    public UserOfflineService(){
        InternalEventDispatcher.getInstance().addEventListener(UserOfflineEvent.type , this.getClass());
    }

    public static UserOfflineService getInstance(){
        return instance;
    }

    @ListenInternalEvent(UserOfflineEvent.type)
    public void handlerUserOffline(UserOfflineEvent userOfflineEvent){
        int userId = userOfflineEvent.getUserId();



    }
}
