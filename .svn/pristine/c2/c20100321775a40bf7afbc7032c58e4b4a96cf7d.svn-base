package avatar.module.user;

import avatar.event.InternalEventDispatcher;
import avatar.event.ListenInternalEvent;
import avatar.event.stopserver.StopServerEvent;

/**
 * 处理停止服务器命令接口
 */
public class StopServerService {
    private static volatile StopServerService instance = null;

    public StopServerService(){
        InternalEventDispatcher.getInstance().addEventListener(StopServerEvent.TYPE , this.getClass());
    }

    public static final StopServerService getInstance(){
        if(instance == null){
            instance = new StopServerService();
        }
        return instance;
    }

    @ListenInternalEvent(StopServerEvent.TYPE)
    public void handler(StopServerEvent stopServerEvent){

        System.out.println("==========停止服务器之前，回收资源=============");
    }
}
