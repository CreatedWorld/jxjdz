package avatar.entity.item;

import avatar.util.BaseEntity;
import avatar.util.utilDB.annotation.Column;
import avatar.util.utilDB.annotation.Pk;
import avatar.util.utilDB.annotation.Table;
import org.springframework.stereotype.Service;

/**
 * 资源道具的实体类。暂时包括了 房卡，金币
 */
@Service
@Table(name = "Item" , comment = "玩家资源道具表")
public class ItemEntity extends BaseEntity{
    public ItemEntity() {
        super(ItemEntity.class);
    }

    @Pk
    @Column(name = "id" ,comment = "道具表id")
    private int id;

    @Column(name = "userId" , comment = "用户id")
    private int userId;

    @Column(name = "itemType" , comment = "道具类型")
    private int itemType;

    @Column(name = "num" , comment = "道具的数量")
    private int num;


    public int getUserId(){
        return userId;
    }

    public void setUserId(int userId){
        this.userId = userId;
    }

    public int getItemType(){
        return itemType;
    }

    public void setItemType(int itemType){
        this.itemType = itemType;
    }

    public void setNum(int num){
        this.num = num;
    }

    public int getNum(){
        return num;
    }
}
