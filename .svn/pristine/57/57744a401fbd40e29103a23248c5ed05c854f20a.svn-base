package avatar.module.express;

import avatar.module.express.function.*;
import avatar.module.express.operator.BelongGroup;
import avatar.module.express.operator.OneOfThemBelong;
import avatar.module.express.operator.OneOfThemIs;

public enum NameMapping {
    // Operator
    BELONG_GROUP("属于其中一组", BelongGroup.class),
    ONE_OF_THEM_IS("其中一只是", OneOfThemIs.class),
    ONE_OF_THEM_BELONG("其中一只属于", OneOfThemBelong.class),

    // Macro
    PUT_OUT_MAHJONG("打出的牌", null),
    TOUCH_MAHJONG("摸到的牌", null),
    HAND_MAHJONGS("手牌", null),
    PERSONAL_ALL_MAHJONG("个人全部牌", null),//包括手牌、碰牌、杠牌、摸到的牌或别人打出的牌
    PENGED_GROUP("碰了的牌", null),
    GANGED_GROUP("杠了的牌", null),
    CHIED_GROUP("吃了的牌", null),
    ZI_MO("自摸", null),
    CHI_HU("吃胡", null),


    // Function
    COUNT("包含数量", Count.class),
    SAME_COLOR("同一花色", SameColor.class),
    SAME_MAHJONG("含有相同麻将", SameMahjong.class),
    CONTAIN_ORDINAL("含有序数", ContainOrdinal.class),
    COMBO_COUNT("组合个数", ComboCount.class),
    SPECIFIED_COLOR_COMBO_COUNT("指定花色的组合数量", SpecifiedColorComboCount.class),
    APPOINT_COLOR_COMBOTYPE_COMBO("指定花色和组合类型的组合", AppointColorComboTypeCombo.class),
    GEN_HU_PAI_COMBO("组成胡牌组合", HuPaiComBo.class),
    GEN_AA_HU_PAI_COMBO("组成AA胡牌组合", AAHuPaiComBo.class),
    EXCLUDE_SPECIFIED_MAHJONG_OTHER_SAME_COLOR("除指定花色外的其它牌属同一花色", ExcludeSpecifiedSameColor.class),
    COMBO_CARDS_TYPE("吃碰杠了的牌中含有花色", ComboCardsType.class),//例：吃碰杠了的牌中含有(碰了的牌,'万')   碰了的牌中含有万字牌
    CHI_PAI_COMBO("吃牌组合", ChiPaiComBo.class),
    CONTAIN_FENG("含有指定花色牌的数量", ContainSpecifiedType.class),
    APPOINT_COLOR("全部是指定花色",AppointColor.class),
    APPOINT_MAHJONG("全部是指定牌",AppointMahjong.class),
    CONTAIN_ONE_APPOINT_MAHJONG("含有一个指定牌",ContainOneAppointMahjong.class),
    APPOINT_COLOR_INTERVAL("指定花色不靠",AppointColorInterval.class);
    // todo 包括（全部牌，字牌，14）

    private String name;

    private Class clazz;

    NameMapping(String name, Class clazz) {
        this.name = name;
        this.clazz = clazz;

    }

    public Class getClazz() {
        return clazz;
    }

    public void setClazz(Class clazz) {
        this.clazz = clazz;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }
}
