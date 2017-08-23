package avatar.util;

import avatar.module.mahjong.operate.ActionType;
import avatar.protobuf.Battle;

/**
 * ActionType 映射客户端对应的 PlayerActType
 */
public class ActionTypeMapping {

    public static Battle.PlayerActType parse(ActionType actionType) {
        switch (actionType) {
            case PLAY_A_MAHJONG:
                return Battle.PlayerActType.PUT_CARD;
            case GET_A_MAHJONG:
                return Battle.PlayerActType.GET_CARD;
            case PASS:
                return Battle.PlayerActType.PASS;
            case PENG:
                return Battle.PlayerActType.PENG;
            case COMMON_AN_GANG:
                return Battle.PlayerActType.COMMON_AN_GANG;
            case BACK_AN_GANG:
                return Battle.PlayerActType.BACK_AN_GANG;
            case ZHI_GANG:
                return Battle.PlayerActType.ZHI_GANG;
            case COMMON_PENG_GANG:
                return Battle.PlayerActType.COMMON_PENG_GANG;
            case BACK_PENG_GANG:
                return Battle.PlayerActType.BACK_PENG_GANG;
            case ZI_MO:
                return Battle.PlayerActType.SELF_HU;
            case QIANG_AN_GANG_HU:
                return Battle.PlayerActType.QIANG_AN_GANG_HU;
            case QIANG_ZHI_GANG_HU:
                return Battle.PlayerActType.QIANG_ZHI_GANG_HU;
            case QIANG_PENG_GANG_HU:
                return Battle.PlayerActType.QIANG_PENG_GANG_HU;
            case CHI_HU:
                return Battle.PlayerActType.CHI_HU;
            case CHI:
                return Battle.PlayerActType.CHI;
        }
        throw new UnsupportedOperationException(
                String.format("未实现ActionType=%s转换为Battle.PlayerActType的功能，请添加该转换功能！", actionType)
        );
    }

}
