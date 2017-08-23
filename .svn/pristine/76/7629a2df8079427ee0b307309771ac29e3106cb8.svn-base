package avatar.entity.match;

import java.util.Date;

/**
 * 玩家匹配信息
 */
public class UserMatchData {
    private int userId;//玩家userId
    private int roomType; //匹配的房间类型
    private int roomRounds;//
    private Date startMatch;
    private String fromHallServerName;

    public UserMatchData(int userId , int roomType , int roomRounds , String fromHallServerName , Date startMatch){
        this.userId = userId;
        this.roomType = roomType;
        this.roomRounds = roomRounds;
        this.fromHallServerName = fromHallServerName;
        this.startMatch = startMatch;
    }

    public int getUserId(){
        return userId;
    }

    public int getRoomType(){
        return roomType;
    }

    public int getRoomRounds(){
        return roomRounds;
    }

    public Date getStartMatch(){
        return startMatch;
    }

    public String getFromHallServerName(){
        return fromHallServerName;
    }
}
