package avatar.entity.room;

/**
 * 房间玩法集合
 */
public enum RoomPlayType {
    NO_WIND(1, "有风(有东南西北中发白)"),
    HAS_WIND(2, "无风(没有东南西北中发白)（默认）"),

    NO_MULTIPLE_CHI_HU(3, "不能一炮多响"),
    CAN_MULTIPLE_CHI_HU(4, "能一炮多响（默认）"),

    NO_CHI_HU(5, "不能吃胡"),
    CAN_CHI_HU(6, "能吃胡（默认）"),

    NO_CHI(7, "不能吃（默认）"),
    CAN_CHI(8, "能吃"),

    NO_QIANG_AN_GANG(9, "不能抢暗杠（默认）"),
    CAN_QIANG_AN_GANG(10, "能抢暗杠"),

    NO_QIANG_ZHI_GANG(11, "不能抢直杠（默认）"),
    CAN_QIANG_ZHI_GANG(12, "能抢直杠"),

    NO_QIANG_PENG_GANG(13, "不能抢碰杠（默认）"),
    CAN_QIANG_PENG_GANG(14, "能抢碰杠"),

    NO_WIND_1(15, "无风(没有东南西北)"),
    HAS_WIND_1(16, "有风(有东南西北)（默认）"),

    NO_FA_BAI(17, "无发白(没有发白)"),
    HAS_FA_BAI(18, "有发白(有发白)（默认）"),

    SETTLE_ROOM_OWNER(19,"房主扣（默认）"),
    SETTLE_AA(20,"AA扣"),

    LIMIT_100(21,"封顶100分"),
    NOT_LIMIT_100(22,"不封顶");

    private int id;
    private String desc;

    RoomPlayType(int id, String desc) {
        this.id = id;
        this.desc = desc;
    }

    public int getId() {
        return id;
    }
}
