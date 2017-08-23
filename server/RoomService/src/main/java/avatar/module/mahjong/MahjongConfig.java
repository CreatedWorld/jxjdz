package avatar.module.mahjong;


import org.apache.commons.lang3.StringUtils;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class MahjongConfig {

    private static final Logger logger = LoggerFactory.getLogger(MahjongConfig.class);

    private static final Properties p;

    //正常情况下手牌的数量
    public static int HAND_CARD_NUMBER;

    // 游戏过程中，用户的倒计时时间
    public static int TIP_REMAIN_TIME;

    // 申请解散房间倒计时
    public static int DISSOLVE_REMAIN_TIME;

    // 模拟发牌的方法名
    public static String mockMahjongMethod;

    // 算分器
    public static String calculator;

    // 总结算算分器
    public static String totalCalculator;

    // 流局判断器
    public static String drawClass;

    static {
        logger.info("正在加载麻将游戏设置。。。。。。");
        InputStream in = null;
        p = new Properties();
        try {
            in = MahjongConfig.class.getClassLoader().getResourceAsStream("mahjongGameSetting.properties");
            p.load(in);
            HAND_CARD_NUMBER = Integer.parseInt(p.getProperty("hand_card_number"));
            TIP_REMAIN_TIME = Integer.parseInt(p.getProperty("tip_remain_time"));
            DISSOLVE_REMAIN_TIME = Integer.parseInt(p.getProperty("dissolveRemainTime"));
            mockMahjongMethod = StringUtils.isEmpty(p.getProperty("mockMahjong")) ? null : p.getProperty("mockMahjong");
            calculator = p.getProperty("calculator");
            totalCalculator = p.getProperty("totalCalculator");
            drawClass = p.getProperty("drawClass");
            logger.info("加载麻将游戏设置成功。。。。。");
            logger.info(p.toString());
        } catch (IOException e) {
            logger.info(String.format("加载麻将游戏设置失败。%s", e.getMessage()), e);
        } finally {
            if (in != null) {
                try {
                    in.close();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        }
    }

}
