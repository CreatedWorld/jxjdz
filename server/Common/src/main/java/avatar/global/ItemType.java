package avatar.global;

/**
 * 道具的类型（可以叠加的）
 */
public enum ItemType {
    CARD(1 , "房卡"),
    GOLD(2 , "金币"),
    DIAMOND(3 , "钻石");

    private int itemType;
    private String desc;

    ItemType(int itemType , String desc){
        this.itemType = itemType;
        this.desc = desc;
    }

    public int getItemType(){
        return itemType;
    }
}
