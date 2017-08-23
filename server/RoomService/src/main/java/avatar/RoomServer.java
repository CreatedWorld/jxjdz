package avatar;

/**
 * Created by Administrator on 2017/4/25.
 */
public class RoomServer {
    public static void main(String[] args){
        String name = "roomServer";
        try {
            Class.forName("avatar.module.mahjong.Mahjong");
            Class.forName("avatar.module.mahjong.service.BaseGameService");
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        }
        new GameConfig(name , args).init(new RoomInit() , new InitRoomDBConfig()).start();
    }
}
