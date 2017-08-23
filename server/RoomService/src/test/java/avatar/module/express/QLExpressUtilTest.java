package avatar.module.express;

import com.ql.util.express.DefaultContext;
import org.junit.Assert;
import org.junit.Test;
import org.omg.CORBA.OBJECT_NOT_EXIST;

import java.lang.reflect.Array;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class QLExpressUtilTest {

    // 碰
    @Test
    public void testPeng() throws Exception {
        DefaultContext<String, Object> context = new DefaultContext<>();
        // assertTrue
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 12, 13, 14, 15, 16));
        // 打出的牌
        context.put(NameMapping.PUT_OUT_MAHJONG.toString(), 11);
        Assert.assertTrue((Boolean) QLExpressUtil.execute(ExpressStr.PENG_EXPRESS, context));


        // assertFalse
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 12, 13, 14, 15, 16));
        // 打出的牌
        context.put(NameMapping.PUT_OUT_MAHJONG.toString(), 11);
        Assert.assertFalse((Boolean) QLExpressUtil.execute(ExpressStr.PENG_EXPRESS, context));
    }

    // 直杠（明杠）
    @Test
    public void testZhiGang() throws Exception {
        DefaultContext<String, Object> context = new DefaultContext<>();
        // assertTrue
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 11, 12, 13, 14, 15, 16));
        // 打出的牌
        context.put(NameMapping.PUT_OUT_MAHJONG.toString(), 11);
        Assert.assertTrue((Boolean) QLExpressUtil.execute(ExpressStr.ZHI_GANG_EXPRESS, context));


        // assertFalse
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 12, 13, 14, 15, 16));
        // 打出的牌
        context.put(NameMapping.PUT_OUT_MAHJONG.toString(), 11);
        Assert.assertFalse((Boolean) QLExpressUtil.execute(ExpressStr.ZHI_GANG_EXPRESS, context));
    }

    // 普通碰杠（明杠）
    @Test
    public void testPengGang() throws Exception {
        DefaultContext<String, Object> context = new DefaultContext<>();
        // assertTrue
        // 碰了的牌
        context.put(NameMapping.PENGED_GROUP.toString(),
                Arrays.asList(
                        Arrays.asList(11, 11, 11),
                        Arrays.asList(31, 31, 31)
                )
        );
        // 打出的牌
        context.put(NameMapping.PUT_OUT_MAHJONG.toString(), 11);
        Assert.assertTrue((Boolean) QLExpressUtil.execute(ExpressStr.COMMON_PENG_GANG_EXPRESS, context));


        // assertFalse
        // 碰了的牌
        context.put(NameMapping.PENGED_GROUP.toString(),
                Arrays.asList(
                        Arrays.asList(13, 13, 13),
                        Arrays.asList(12, 12, 12)
                )
        );
        // 打出的牌
        context.put(NameMapping.PUT_OUT_MAHJONG.toString(), 11);
        Assert.assertFalse((Boolean) QLExpressUtil.execute(ExpressStr.COMMON_PENG_GANG_EXPRESS, context));
    }

    // 回头碰杠（明杠）
    @Test
    public void testBackPengGang() throws Exception {
        DefaultContext<String, Object> context = new DefaultContext<>();
        // assertTrue
        // 碰了的牌
        context.put(NameMapping.PENGED_GROUP.toString(),
                Arrays.asList(
                        Arrays.asList(11, 11, 11),
                        Arrays.asList(12, 12, 12)
                )
        );
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 13, 14, 15, 16));
        Assert.assertTrue((Boolean) QLExpressUtil.execute(ExpressStr.BACK_PENG_GANG_EXPRESS, context));


        // assertFalse
        // 碰了的牌
        context.put(NameMapping.PENGED_GROUP.toString(),
                Arrays.asList(
                        Arrays.asList(13, 13, 13),
                        Arrays.asList(12, 12, 12)
                )
        );
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(14, 15, 16));
        Assert.assertFalse((Boolean) QLExpressUtil.execute(ExpressStr.BACK_PENG_GANG_EXPRESS, context));
    }

    // 普通暗杠
    @Test
    public void testCommonAnGang() throws Exception {
        DefaultContext<String, Object> context = new DefaultContext<>();
        // assertTrue
        // 摸到的牌
        context.put(NameMapping.TOUCH_MAHJONG.toString(), 11);
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 11, 13, 14, 15, 16));
        Assert.assertTrue((Boolean) QLExpressUtil.execute(ExpressStr.COMMON_AN_GANG_EXPRESS, context));


        // assertFalse
        // 摸到的牌
        context.put(NameMapping.TOUCH_MAHJONG.toString(), 11);
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 14, 15, 16));
        Assert.assertFalse((Boolean) QLExpressUtil.execute(ExpressStr.COMMON_AN_GANG_EXPRESS, context));
    }

    /**
     * 豪华七对胡
     * 胡牌组合 = 组成胡牌组合(手牌); 胡牌组合.size() 不等于 0 并且 组合个数(胡牌组合,'AA') 等于 7 并且 含有相同麻将(手牌, 4)
     */
    @Test
    public void testLuxuryQiDuiHu() throws Exception {
        DefaultContext<String, Object> context = new DefaultContext<>();
        // assertTrue
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 12, 12, 24, 24, 35, 35, 36, 36, 36, 36, 41, 41));
        Assert.assertTrue((Boolean) QLExpressUtil.execute(ExpressStr.LUXURY_QI_DUI_HU_EXPRESS, context));


        // assertFalse
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 12, 12, 24, 24, 35, 35, 36, 36, 37, 37, 41, 41));
        Assert.assertFalse((Boolean) QLExpressUtil.execute(ExpressStr.LUXURY_QI_DUI_HU_EXPRESS, context));
    }

    /**
     * 清一色七对胡
     * 胡牌组合 = 组成胡牌组合(手牌); 胡牌组合.size() 不等于 0 并且 组合个数(胡牌组合,'AA') 等于 7 并且 同一花色(手牌)
     */
    @Test
    public void testPureColorQiDuiHu() throws Exception {
        DefaultContext<String, Object> context = new DefaultContext<>();
        // assertTrue
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 12, 12, 13, 13, 14, 14, 14, 14, 15, 15, 16, 16));
        Assert.assertTrue((Boolean) QLExpressUtil.execute(ExpressStr.PURE_COLOR_QI_DUI_HU_EXPRESS, context));


        // assertFalse
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 12, 12, 24, 24, 35, 35, 36, 36, 37, 37, 41, 41));
        Assert.assertFalse((Boolean) QLExpressUtil.execute(ExpressStr.PURE_COLOR_QI_DUI_HU_EXPRESS, context));
    }

    /**
     * 清一色一条龙胡
     * 胡牌组合 = 组成胡牌组合(手牌); 组合个数(胡牌组合,'AA') 等于 1 并且 组合个数(胡牌组合,'AAA') + 组合个数(胡牌组合,'ABC') 等于 4 并且 含有序数(手牌,[1,2,3,4,5,6,7,8,9]) 并且 同一花色(手牌)
     */
    @Test
    public void testPureColorDragonHu() throws Exception {
        DefaultContext<String, Object> context = new DefaultContext<>();
        // assertTrue
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 12, 13, 14, 14, 14, 13, 14, 15, 17, 18, 19, 16, 16));
        Assert.assertTrue((Boolean) QLExpressUtil.execute(ExpressStr.PURE_COLOR_DRAGON_EXPRESS, context));


        // assertFalse
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 12, 12, 24, 24, 35, 35, 36, 36, 37, 37, 41, 41));
        Assert.assertFalse((Boolean) QLExpressUtil.execute(ExpressStr.PURE_COLOR_DRAGON_EXPRESS, context));
    }

    /**
     * 普通七对胡
     * 胡牌组合 = 组成胡牌组合(手牌); 胡牌组合.size() 不等于 0 并且 组合个数(胡牌组合,'AA') 等于 7 并且 !含有相同麻将(手牌, 4)
     */
    @Test
    public void testCommonQiDuiHu() throws Exception {
        DefaultContext<String, Object> context = new DefaultContext<>();
        // assertTrue
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 12, 12, 13, 13, 14, 14, 17, 17, 15, 15, 16, 16));
        Assert.assertTrue((Boolean) QLExpressUtil.execute(ExpressStr.COMMON_QI_DUI_HU_EXPRESS, context));


        // assertFalse
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 12, 12, 24, 24, 35, 35, 36, 36, 37, 37, 41));
        Assert.assertFalse((Boolean) QLExpressUtil.execute(ExpressStr.COMMON_QI_DUI_HU_EXPRESS, context));
    }

    /**
     * 一条龙胡
     * 胡牌组合 = 组成胡牌组合(手牌); 组合个数(胡牌组合,'AA') 等于 1 并且 含有序数(手牌,[1,2,3,4,5,6,7,8,9])
     */
    @Test
    public void testDragonHu() throws Exception {
        DefaultContext<String, Object> context = new DefaultContext<>();
        // assertTrue
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 12, 13, 14, 14, 14, 14, 15, 16, 17, 17, 18, 19));
        Assert.assertTrue((Boolean) QLExpressUtil.execute(ExpressStr.DRAGON_EXPRESS, context));


        // assertFalse
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 12, 13, 14, 14, 14, 14, 15, 16, 17, 19, 19, 19));
        Assert.assertFalse((Boolean) QLExpressUtil.execute(ExpressStr.DRAGON_EXPRESS, context));
    }

    /**
     * 清一色胡
     * 胡牌组合 = 组成胡牌组合(手牌); 组合个数(胡牌组合,'AA') 等于 1 并且 含有序数(手牌,[1,2,3,4,5,6,7,8,9])
     */
    @Test
    public void testPureColorHu() throws Exception {
        DefaultContext<String, Object> context = new DefaultContext<>();
        // assertTrue
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 12, 13, 14, 14, 14, 14, 15, 16, 17));
        Assert.assertTrue((Boolean) QLExpressUtil.execute(ExpressStr.PURE_COLOR_HU_EXPRESS, context));


        // assertFalse
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 12, 13, 14, 14, 14, 14, 15, 16, 17, 29, 29, 29));
        Assert.assertFalse((Boolean) QLExpressUtil.execute(ExpressStr.PURE_COLOR_HU_EXPRESS, context));
    }

    /**
     * 普通平胡
     * 胡牌组合 = 组成胡牌组合(手牌); 并且 组合个数(胡牌组合,'AA') 等于 1 并且 组合个数(胡牌组合,'AAA') 小于4
     */
    @Test
    public void testCommonPingHu() throws Exception {
        DefaultContext<String, Object> context = new DefaultContext<>();
        // assertTrue
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 12, 13, 14, 14, 14, 14, 15, 16, 17, 17, 18, 19));
        Assert.assertTrue((Boolean) QLExpressUtil.execute(ExpressStr.COMMON_PING_HU_EXPRESS, context));


        // assertTrue
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 12, 13, 14, 14, 14, 14, 15, 16, 17, 29, 29, 29));
        Assert.assertTrue((Boolean) QLExpressUtil.execute(ExpressStr.COMMON_PING_HU_EXPRESS, context));

        // assertFalse
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 12, 13, 14, 14, 14, 14, 15, 16, 17, 27, 29, 29));
        Assert.assertFalse((Boolean) QLExpressUtil.execute(ExpressStr.COMMON_PING_HU_EXPRESS, context));
    }

    /**
     * 普通碰碰胡
     * 胡牌组合 = 组成胡牌组合(手牌); 组合个数(胡牌组合,'AA') 等于1 并且 组合个数(胡牌组合,'AAA') 等于 4
     */
    @Test
    public void testCommonPengPengHu() throws Exception {
        DefaultContext<String, Object> context = new DefaultContext<>();
        // assertTrue
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 12, 12, 12, 14, 14, 14, 25, 25, 25, 36, 36, 36));
        Assert.assertTrue((Boolean) QLExpressUtil.execute(ExpressStr.COMMON_PENG_PENG_HU_EXPRESS, context));

        // assertFalse
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 12, 13, 14, 14, 14, 14, 15, 16, 17, 27, 29, 29));
        Assert.assertFalse((Boolean) QLExpressUtil.execute(ExpressStr.COMMON_PENG_PENG_HU_EXPRESS, context));
    }

    @Test
    public void testJiHu() throws Exception {
        DefaultContext<String, Object> context = new DefaultContext<String, Object>();
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 11, 12, 12, 12, 13, 13, 13, 16, 17, 18));
        final String express = ExpressStr.JI_HU_EXPRESS;
        Object o = QLExpressUtil.execute(express,context);
    }

    /**
     * 软碰碰
     */
    @Test
    public void testRuanPengPentHu() throws Exception {
        DefaultContext<String, Object> context = new DefaultContext<String, Object>();
        //吃了的牌
        context.put(NameMapping.CHIED_GROUP.toString(), Arrays.asList(Arrays.asList(41, 42, 43)));
        //碰了的牌
        context.put(NameMapping.PENGED_GROUP.toString(), Arrays.asList(Arrays.asList(41,41,41)));
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(11, 11, 11, 51, 51));
        Assert.assertTrue((Boolean) QLExpressUtil.execute(ExpressStr.RUAN_RUAN_PENG_HU_EXPRESS, context));
    }

    /**
     * 断断
     */
    @Test
    public void testDuanDuan() throws Exception {
        DefaultContext<String, Object> context = new DefaultContext<String, Object>();
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList(12, 13, 14, 15, 16, 17, 22, 23, 24, 25, 25, 25, 33, 33));
        context.put(NameMapping.PERSONAL_ALL_MAHJONG.toString(), Arrays.asList(12, 13, 14, 15, 16, 17, 22, 23, 24, 25, 25, 25, 33, 33));
        Assert.assertTrue((Boolean) QLExpressUtil.execute(ExpressStr.DUAN_DUAN, context));
    }

    /**
     * 硬碰碰
     */
    @Test
    public void testPengPengHu() throws Exception {
        DefaultContext<String, Object> context = new DefaultContext<String, Object>();
        context.put(NameMapping.HAND_MAHJONGS.toString(), Arrays.asList( 31, 31, 31, 24, 24, 24 ,51, 51));

        //自定义碰牌
        List<Integer> p = Arrays.asList(11,11,11);
        List<List<Integer>> pa = new ArrayList<>();
        pa.add(p);
        context.put( NameMapping.PENGED_GROUP.toString(),pa);

        //自定义杠牌
        List<Integer> g = Arrays.asList(12,12,12,12);
        List<List<Integer>> ga = new ArrayList<>();
        ga.add(g);
        context.put( NameMapping.GANGED_GROUP.toString(),pa);

        Assert.assertTrue((Boolean) QLExpressUtil.execute(ExpressStr.YING_PENG_PENG, context));
    }
}
