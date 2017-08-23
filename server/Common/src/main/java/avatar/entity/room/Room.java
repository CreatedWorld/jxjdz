package avatar.entity.room;

import avatar.util.BaseEntity;
import avatar.util.StringUtil;
import avatar.util.utilDB.annotation.Column;
import avatar.util.utilDB.annotation.Pk;
import avatar.util.utilDB.annotation.Table;
import org.springframework.stereotype.Service;

import java.util.Date;
import java.util.List;

@Service
@Table(name = "Room", comment = "房间")
public class Room extends BaseEntity {
    public Room() {
        super(Room.class);
    }

    @Pk(auto = true)
    @Column(name = "id", comment = "房间id")
    protected int id;

    @Column(name = "roomCode", comment = "房间号")
    protected String roomCode;

    @Column(name = "state", comment = "房间状态")
    protected int state;

    @Column(name = "needPlayerNum", comment = "游戏开始需要达到的玩家人数")
    protected int needPlayerNum;

    @Column(name = "roomIp" , comment = "房间服务器的ip")
    private String roomIp;

    @Column(name = "roomPort" , comment = "房间服务器端口")
    private int port;

    @Column(name =  "roomLocalName" , comment = "房间服务器别名" )
    private String roomLocalName;

    @Column(name = "createUserId" , comment = "房间创建者id")
    private int createUserId;

    @Column(name = "rounds" , comment = "总局数")
    private int rounds;

    @Column(name = "roomType" , comment = "房间类型")
    private int roomType;

    @Column(name = "time" , comment = "记录玩家创建房间、开始游戏的时间")
    private Date time;

    @Column(name = "playType" , comment = "玩法。这里是一个列表例如： 1,2,3", notNull = false)
    private String playType;

    @Column(name = "curRounds" , comment = "当前局数" , notNull = false)
    private int curRounds;


    public String getRoomIp(){
        return roomIp;
    }

    public int getPort(){
        return port;
    }

    public void setRoomIp(String ip){
        this.roomIp = ip;
    }

    public void setPort(int port){
        this.port = port;
    }

    public int getNeedPlayerNum() {
        return needPlayerNum;
    }

    public void setNeedPlayerNum(int playerNum) {
        this.needPlayerNum = playerNum;
    }

    public int getId() {
        return id;
    }

    public String getRoomCode() {
        return roomCode;
    }

    public void setRoomCode(String roomCode) {
        this.roomCode = roomCode;
    }

    public int getState() {
        return state;
    }

    public void setState(State state) {
        this.state = state.code;
    }

    public void setCreateUserId(int userId){
        this.createUserId = userId;
    }

    public int getCreateUserId(){
        return createUserId;
    }

    public int getRounds(){
        return rounds;
    }

    public void setRounds(int rounds){
        this.rounds = rounds;
    }

    public void setRoomType(int roomType){
        this.roomType = roomType;
    }

    public int getRoomType(){
        return roomType;
    }

    public void setTime(){
        this.time = new Date();
    }

    public Date getTime(){
        return time;
    }

    public String getRoomLocalName(){
        return roomLocalName;
    }

    public void setRoomLocalName(String roomLocalName){
        this.roomLocalName = roomLocalName;
    }

    public List<Integer> getPlayTypeList(){
        List<Integer> list = StringUtil.parseString2IntList(this.playType , ",");
        return list;
    }

    public void setPlayType(String str){
        this.playType = str;
    }

    public int getCurRounds(){
        return curRounds;
    }

    public void setCurRounds(int curRounds){
        this.curRounds = curRounds;
    }

    public enum State {
        CREATED(1, "创建"),
        STARTED(2, "游戏中"),
        DISSOLVED(3, "解散"),;

        private int code;

        private String name;

        State(int code, String name) {
            this.code = code;
            this.name = name;
        }

        public int getCode(){
            return code;
        }
    }

}
