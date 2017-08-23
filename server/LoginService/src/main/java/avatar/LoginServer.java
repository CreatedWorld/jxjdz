package avatar;

public class LoginServer {
    public static void main(String[] args){
        String gameName = "loginServer";
        new GameConfig(gameName , args).init(new LoginInit() , new InitLoginDBConfig()).start();
      //  avatar.LoginServer.test();
    }


    private static void test(){

    }

}
