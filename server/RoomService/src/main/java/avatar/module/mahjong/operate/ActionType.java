package avatar.module.mahjong.operate;

import avatar.protobuf.Cmd;

/**
 * 操作类型，具体到真实麻将玩法中客户的请求动作
 * 优先级越高的操作，需要排在前面
 */
public enum ActionType {
    QIANG_AN_GANG_HU(Cmd.C2S_ROOM_QIANG_AN_GANG_HU, "抢暗杠胡"),

    QIANG_ZHI_GANG_HU(Cmd.C2S_ROOM_QIANG_ZHI_GANG_HU, "抢直杠胡"),

    QIANG_PENG_GANG_HU(Cmd.C2S_ROOM_QIANG_PENG_GANG_HU, "抢碰杠胡"),

    ZI_MO(Cmd.C2S_ROOM_ZI_MO_HU, "自摸胡"),

    CHI_HU(Cmd.C2S_ROOM_CHI_HU, "吃胡"),

    COMMON_AN_GANG(Cmd.C2S_ROOM_COMMON_AN_GANG, "普通暗杠"),

    BACK_AN_GANG(Cmd.C2S_ROOM_COMMON_AN_GANG, "回头暗杠"),

    COMMON_PENG_GANG(Cmd.C2S_ROOM_COMMON_PENG_GANG, "普通碰杠（明杠）"),

    BACK_PENG_GANG(Cmd.C2S_ROOM_BACK_PENG_GANG, "回头碰杠（明杠）"),

    ZHI_GANG(Cmd.C2S_ROOM_ZHI_GANG, "直杠（明杠）"),

    PENG(Cmd.C2S_ROOM_PENG, "碰"),

    CHI(Cmd.C2S_ROOM_CHI, "吃"),

    PLAY_A_MAHJONG(Cmd.C2S_ROOM_PLAY_A_MAHJONG, "打出一张牌"),

    GET_A_MAHJONG(0, "摸牌"),

    PASS(Cmd.C2S_ROOM_PASS, "过");

    public static boolean isHu(ActionType actionType) {
        switch (actionType) {
            case ZI_MO:
                return true;
            case QIANG_AN_GANG_HU:
                return true;
            case QIANG_ZHI_GANG_HU:
                return true;
            case QIANG_PENG_GANG_HU:
                return true;
            case CHI_HU:
                return true;
        }
        return false;
    }

    public boolean isGang() {
        switch (this) {
            case COMMON_AN_GANG:
                return true;
            case BACK_AN_GANG:
                return true;
            case ZHI_GANG:
                return true;
            case COMMON_PENG_GANG:
                return true;
            case BACK_PENG_GANG:
                return true;
        }
        return false;
    }

    public boolean isAnGang() {
        switch (this) {
            case COMMON_AN_GANG:
                return true;
            case BACK_AN_GANG:
                return true;
        }
        return false;
    }

    /**
     * 动作对应的cmd
     */
    private int cmd;

    /**
     * 动作名称
     */
    private String name;

    ActionType(int cmd, String name) {
        this.cmd = cmd;
        this.name = name;
    }

    public int getCmd() {
        return cmd;
    }

    public String getName() {
        return name;
    }
}
