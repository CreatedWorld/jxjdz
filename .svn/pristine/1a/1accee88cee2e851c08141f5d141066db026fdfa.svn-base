package avatar.module.express;

public class ExpressStr {
    // 碰
    public static final String PENG_EXPRESS = "包含数量(打出的牌, 手牌) 大于等于 2";

    // 直杠（明杠）
    public static final String ZHI_GANG_EXPRESS = "包含数量(打出的牌, 手牌) 大于等于 3";

    // 普通碰杠（明杠）
    public static final String COMMON_PENG_GANG_EXPRESS = "打出的牌 属于其中一组 碰了的牌";

    // 回头碰杠（明杠）
    public static final String BACK_PENG_GANG_EXPRESS = "手牌 其中一只属于 碰了的牌";

    // 普通暗杠
    public static final String COMMON_AN_GANG_EXPRESS = "包含数量(摸到的牌, 手牌) 大于等于 3";

    // 回头暗杠
    public static final String BACK_AN_GANG_EXPRESS = "含有相同麻将(手牌, 4)";

    /**
     * 全民麻将，暂时全部是自摸
     * 屁胡：即基本胡，任何一对牌都可作将。
     * 大胡：七对，清一色，杠上开
     */
    /*************************大胡*************************/
    // 豪华七对胡
    public static final String LUXURY_QI_DUI_HU_EXPRESS = "胡牌组合 = 组成胡牌组合(手牌); 胡牌组合.size() 不等于 0 并且 组合个数(胡牌组合,'AA') 等于 7 并且 含有相同麻将(手牌, 4)";

    // 清一色七对胡
    public static final String PURE_COLOR_QI_DUI_HU_EXPRESS = "胡牌组合 = 组成胡牌组合(手牌); 胡牌组合.size() 不等于 0 并且 组合个数(胡牌组合,'AA') 等于 7 并且 同一花色(手牌)";

    // 清一色一条龙胡
    public static final String PURE_COLOR_DRAGON_EXPRESS = "胡牌组合 = 组成胡牌组合(手牌); 组合个数(胡牌组合,'AA') 等于 1 并且 组合个数(胡牌组合,'AAA') + 组合个数(胡牌组合,'ABC') 等于 4 并且 含有序数(手牌,[1,2,3,4,5,6,7,8,9]) 并且 同一花色(手牌)";

    /*************************中胡*************************/
    // 普通七对胡
    public static final String COMMON_QI_DUI_HU_EXPRESS = "胡牌组合 = 组成胡牌组合(手牌); 胡牌组合.size() 不等于 0 并且 组合个数(胡牌组合,'AA') 等于 7 并且 !含有相同麻将(手牌, 4)";

    // 一条龙胡
    public static final String DRAGON_EXPRESS = "胡牌组合 = 组成胡牌组合(手牌); 组合个数(胡牌组合,'AA') 等于 1 并且 含有序数(手牌,[1,2,3,4,5,6,7,8,9])";

    // 清一色胡
    public static final String PURE_COLOR_HU_EXPRESS = "胡牌组合 = 组成胡牌组合(手牌); 组合个数(胡牌组合,'AA') 大于 0 并且 同一花色(手牌)";

    /*************************小胡*************************/
    // 普通平胡
    public static final String COMMON_PING_HU_EXPRESS = "胡牌组合 = 组成胡牌组合(手牌); 组合个数(胡牌组合,'AA') 等于 1 并且 组合个数(胡牌组合,'AAA') 小于 4";

    /**
     * 其他麻将
     */
    // 普通碰碰胡
    public static final String COMMON_PENG_PENG_HU_EXPRESS = "胡牌组合 = 组成胡牌组合(手牌); 组合个数(胡牌组合,'AA') 等于 1 并且 组合个数(胡牌组合,'AAA') 等于 4";


    /*************************翻精软麻将大胡****************************/
    /**
     * 2、鸡胡
     */
    public static final String JI_HU_EXPRESS = "胡牌组合 = 组成胡牌组合(手牌); 胡牌组合.size() 不等于 0 并且 组合个数(胡牌组合,'AAA') + 组合个数(胡牌组合,'ABC') 等于 4";
    /**
     * 3、混色
     */
    public static final String HUN_SE_HU_EXPRESS = "胡牌组合 = 含有序数 并且 序数相同花色(手牌,[1,2,3,4,5,6,7,8,9]) 并且 含有指定花色牌的数量(手牌,[4]).size() 等于 5";

    /**
     * 全求人
     */
    public static final String QUAN_QIU_REN = "胡牌组合 = 组成胡牌组合(手牌); 手牌.size() 等于 2 并且 吃胡";

    /**
     * 5、断断
     */
    public static final String DUAN_DUAN = "胡牌组合 = 组成胡牌组合(手牌); 组合个数(胡牌组合,'AA') 等于 1 并且 全部是指定花色(个人全部牌,['万','筒','索']) 并且 !含有一个指定牌(个人全部牌,[11,19,21,29,31,39])";

    /**
     * 5、软碰碰
     */
    public static final String RUAN_RUAN_PENG_HU_EXPRESS = "胡牌组合 = 组成胡牌组合(手牌); (指定花色的组合数量(胡牌组合,'AAA',['万','筒','索']) 大于等于 1 或者 吃碰杠了的牌中含有花色(碰了的牌,['万','筒','条'])) 并且 吃碰杠了的牌中含有花色(吃了的牌,['风','箭']) 并且 指定花色的组合数量(胡牌组合,'AA',['风','箭']) 大于等于 1";

    /**
     * 硬碰碰
     */
    public static final String YING_PENG_PENG = "胡牌组合 = 组成胡牌组合(手牌); 组合个数(胡牌组合,'AA') 等于 1 并且 (碰了的牌.size() 大于 0 或者 杠了的牌.size() 大于 0) 并且 组合个数(胡牌组合,'AAA') + 碰了的牌.size() + 杠了的牌.size() 等于 4";

    /**
     * 普乱
     */
    public static final String PU_LUAN = "含有相同麻将(个人全部牌, 2).size() 等于 0 并且 指定花色不靠(个人全部牌,['万','筒','索'],2)";

}
