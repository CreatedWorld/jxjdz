package avatar.entity.roomType;

import avatar.util.BaseEntity;
import avatar.util.utilDB.annotation.Column;
import avatar.util.utilDB.annotation.Pk;
import avatar.util.utilDB.annotation.Table;
import org.springframework.stereotype.Service;

/**
 * 房间列表
 */
@Service
@Table(name = "config_roomtype" , comment = "房间列表配置")
public class RoomType extends BaseEntity{
    public RoomType() {
        super(RoomType.class);
    }

    @Pk(auto = false)
    @Column(name = "roomType",comment = "主键")
    private int roomType;

    @Column(name = "name",comment = "房间名")
    private String name;

    @Column(name="url",comment = "图片url")
    private String url;

    @Column(name = "gameType",comment = "游戏类型")
    private int gameType;


    @Column(name = "seatNum" ,comment = "座位数")
    private int seatNum;

    @Column(name = "isCost" , comment = "是否需要花费道具")
    private int isCost;


    @Column(name = "itemTypeId" , comment = "花费的道具的id")
    private int itemTypeId;

    @Column(name = "needScore" , comment = "消耗的道具的数量")
    private int needScore;

    public int getRoomType() {
        return roomType;
    }

    public int getGameType(){
        return gameType;
    }

    public String getUrl(){
        return url;
    }

    public int getSeatNum(){
        return seatNum;
    }

    public int getIsCost(){
        return isCost;
    }

    public int getItemTypeId(){
        return itemTypeId;
    }

    public int getNeedScore(){
        return needScore;
    }
}
