package avatar.util;


import avatar.module.mahjong.Mahjong;
import avatar.module.mahjong.MahjongConfig;
import org.apache.commons.lang.math.RandomUtils;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

/**
 * 模拟麻将，发牌时，发指定的牌
 */
public class MockMahjongUtil {

    /**
     * 手里有杠不杠，下一轮自摸时，此时提示【杠】【过】，点【过】后没有出【胡】【过】
     */
    public static List<Mahjong> anGangZiMo() {
        List<List<Mahjong>> personalMahjongs = new ArrayList<>(4);


        List<Mahjong> user1 = new ArrayList<>(1);
        user1.add(Mahjong.ONE_WANG);
        user1.add(Mahjong.ONE_WANG);
        user1.add(Mahjong.ONE_WANG);
        user1.add(Mahjong.TWO_WANG);
        user1.add(Mahjong.THREE_WANG);
        user1.add(Mahjong.SIX_TIAO);
        user1.add(Mahjong.SIX_TIAO);
        user1.add(Mahjong.SIX_TIAO);
        user1.add(Mahjong.DONG_FENG);
        user1.add(Mahjong.DONG_FENG);
        user1.add(Mahjong.EIGHT_TIAO);
        user1.add(Mahjong.NINE_TIAO);
        user1.add(Mahjong.SEVEN_TIAO);


        List<Mahjong> user2 = new ArrayList<>(3);


        List<Mahjong> user3 = new ArrayList<>(1);


        List<Mahjong> user4 = new ArrayList<>(1);

        personalMahjongs.add(user1);
        personalMahjongs.add(user2);
        personalMahjongs.add(user3);
        personalMahjongs.add(user4);


        // 排在列表最前的，先摸牌
        List<Mahjong> beTouchMahjongs = new ArrayList<>(1);
        beTouchMahjongs.add(Mahjong.ONE_WANG);
        beTouchMahjongs.add(Mahjong.FOUR_WANG);
        beTouchMahjongs.add(Mahjong.THREE_TONG);
        beTouchMahjongs.add(Mahjong.THREE_TONG);
        beTouchMahjongs.add(Mahjong.THREE_WANG);

        return genMockAllMahjongs(personalMahjongs, beTouchMahjongs);
    }

    /**
     * 打出一张牌，A可碰，B可胡，此时只显示A的【碰】【过】按钮，B没有任何显示
     */
    public static List<Mahjong> pengChiHu() {
        List<List<Mahjong>> personalMahjongs = new ArrayList<>(4);


        List<Mahjong> user1 = new ArrayList<>(1);


        List<Mahjong> user2 = new ArrayList<>(3);
        user2.add(Mahjong.ONE_WANG);
        user2.add(Mahjong.ONE_WANG);
        user2.add(Mahjong.ONE_WANG);
        user2.add(Mahjong.SIX_TIAO);
        user2.add(Mahjong.SIX_TIAO);
        user2.add(Mahjong.SIX_TIAO);
        user2.add(Mahjong.FIVE_TONG);
        user2.add(Mahjong.SIX_TONG);
        user2.add(Mahjong.DONG_FENG);
        user2.add(Mahjong.DONG_FENG);
        user2.add(Mahjong.EIGHT_TIAO);
        user2.add(Mahjong.NINE_TIAO);
        user2.add(Mahjong.SEVEN_TIAO);

        List<Mahjong> user3 = new ArrayList<>(1);
        user3.add(Mahjong.SEVEN_TONG);
        user3.add(Mahjong.SEVEN_TONG);

        List<Mahjong> user4 = new ArrayList<>(1);

        personalMahjongs.add(user1);
        personalMahjongs.add(user2);
        personalMahjongs.add(user3);
        personalMahjongs.add(user4);


        // 排在列表最前的，先摸牌
        List<Mahjong> beTouchMahjongs = new ArrayList<>(1);
        beTouchMahjongs.add(Mahjong.SEVEN_TONG);
        beTouchMahjongs.add(Mahjong.XI_FENG);
        beTouchMahjongs.add(Mahjong.XI_FENG);
        beTouchMahjongs.add(Mahjong.SEVEN_TONG);

        return genMockAllMahjongs(personalMahjongs, beTouchMahjongs);
    }

    /**
     * 直杠、碰
     */
    public static List<Mahjong> zhiGang() {
        List<List<Mahjong>> personalMahjongs = new ArrayList<>(4);


        List<Mahjong> user1 = new ArrayList<>(1);
        user1.add(Mahjong.ONE_WANG);

        List<Mahjong> user2 = new ArrayList<>(3);
        user2.add(Mahjong.ONE_WANG);
        user2.add(Mahjong.ONE_WANG);
        user2.add(Mahjong.ONE_WANG);

        personalMahjongs.add(user1);
        personalMahjongs.add(user2);

        List<Mahjong> beTouchMahjongs = new ArrayList<>(1);

        return genMockAllMahjongs(personalMahjongs, beTouchMahjongs);
    }

    /**
     * 普通碰杠、回头碰杠、普通暗杠、回头暗杠
     */
    public static List<Mahjong> ziMOPingHu() {
        List<List<Mahjong>> personalMahjongs = new ArrayList<>(4);


        List<Mahjong> user1 = new ArrayList<>(1);
        user1.add(Mahjong.ONE_WANG);
        user1.add(Mahjong.ONE_WANG);
        user1.add(Mahjong.ONE_WANG);
        user1.add(Mahjong.SIX_TIAO);
        user1.add(Mahjong.SIX_TIAO);
        user1.add(Mahjong.SIX_TIAO);
        user1.add(Mahjong.FIVE_TONG);
        user1.add(Mahjong.SIX_TONG);
        user1.add(Mahjong.SEVEN_TONG);
        user1.add(Mahjong.DONG_FENG);
        user1.add(Mahjong.DONG_FENG);
        user1.add(Mahjong.EIGHT_TIAO);
        user1.add(Mahjong.NINE_TIAO);


        List<Mahjong> user2 = new ArrayList<>(3);


        List<Mahjong> user3 = new ArrayList<>(1);


        List<Mahjong> user4 = new ArrayList<>(1);

        personalMahjongs.add(user1);
        personalMahjongs.add(user2);
        personalMahjongs.add(user3);
        personalMahjongs.add(user4);


        // 排在列表最前的，先摸牌
        List<Mahjong> beTouchMahjongs = new ArrayList<>(1);
        beTouchMahjongs.add(Mahjong.SEVEN_TIAO);
        beTouchMahjongs.add(Mahjong.DONG_FENG);

        return genMockAllMahjongs(personalMahjongs, beTouchMahjongs);
    }

    /**
     * 一炮多响
     */
    public static List<Mahjong> multipleChiHu() {
        List<List<Mahjong>> personalMahjongs = new ArrayList<>(4);


        List<Mahjong> user1 = new ArrayList<>(1);


        List<Mahjong> user2 = new ArrayList<>(3);
        user2.add(Mahjong.ONE_WANG);
        user2.add(Mahjong.TWO_WANG);
        user2.add(Mahjong.THREE_WANG);
        user2.add(Mahjong.FOUR_TIAO);
        user2.add(Mahjong.FIVE_TIAO);
        user2.add(Mahjong.SIX_TIAO);
        user2.add(Mahjong.FIVE_TONG);
        user2.add(Mahjong.SIX_TONG);
        user2.add(Mahjong.SEVEN_TONG);
        user2.add(Mahjong.NINE_WANG);
        user2.add(Mahjong.NINE_WANG);
        user2.add(Mahjong.EIGHT_TIAO);
        user2.add(Mahjong.NINE_TIAO);

        List<Mahjong> user3 = new ArrayList<>(1);
        user3.add(Mahjong.ONE_WANG);
        user3.add(Mahjong.TWO_WANG);
        user3.add(Mahjong.THREE_WANG);
        user3.add(Mahjong.FOUR_TIAO);
        user3.add(Mahjong.FIVE_TIAO);
        user3.add(Mahjong.SIX_TIAO);
        user3.add(Mahjong.FIVE_TONG);
        user3.add(Mahjong.SIX_TONG);
        user3.add(Mahjong.SEVEN_TONG);
        user3.add(Mahjong.EIGHT_WANG);
        user3.add(Mahjong.EIGHT_WANG);
        user3.add(Mahjong.EIGHT_TIAO);
        user3.add(Mahjong.NINE_TIAO);

        List<Mahjong> user4 = new ArrayList<>(1);
        user4.add(Mahjong.ONE_WANG);
        user4.add(Mahjong.TWO_WANG);
        user4.add(Mahjong.THREE_WANG);
        user4.add(Mahjong.FOUR_TIAO);
        user4.add(Mahjong.FIVE_TIAO);
        user4.add(Mahjong.SIX_TIAO);
        user4.add(Mahjong.FIVE_TONG);
        user4.add(Mahjong.SIX_TONG);
        user4.add(Mahjong.SEVEN_TONG);
        user4.add(Mahjong.XI_FENG);
        user4.add(Mahjong.XI_FENG);
        user4.add(Mahjong.EIGHT_TIAO);
        user4.add(Mahjong.NINE_TIAO);

        personalMahjongs.add(user1);
        personalMahjongs.add(user2);
        personalMahjongs.add(user3);
        personalMahjongs.add(user4);


        // 排在列表最前的，先摸牌
        List<Mahjong> beTouchMahjongs = new ArrayList<>(1);
        beTouchMahjongs.add(Mahjong.SEVEN_TIAO);
        beTouchMahjongs.add(Mahjong.DONG_FENG);

        return genMockAllMahjongs(personalMahjongs, beTouchMahjongs);
    }

    /**
     * 普通碰杠、回头碰杠、普通暗杠、回头暗杠
     */
    public static List<Mahjong> pengGang() {
        List<List<Mahjong>> personalMahjongs = new ArrayList<>(4);


        List<Mahjong> user1 = new ArrayList<>(1);
        user1.add(Mahjong.ONE_WANG);
        user1.add(Mahjong.DONG_FENG);
        user1.add(Mahjong.DONG_FENG);
        user1.add(Mahjong.DONG_FENG);
        user1.add(Mahjong.XI_FENG);
        user1.add(Mahjong.XI_FENG);
        user1.add(Mahjong.XI_FENG);
        user1.add(Mahjong.ONE_TONG);
        user1.add(Mahjong.TWO_TONG);
        user1.add(Mahjong.THREE_TONG);


        List<Mahjong> user2 = new ArrayList<>(3);
        user2.add(Mahjong.ONE_WANG);
        user2.add(Mahjong.ONE_WANG);
        user2.add(Mahjong.FOUR_WANG);
        user2.add(Mahjong.FIVE_WANG);
        user2.add(Mahjong.FA_CAI);
        user2.add(Mahjong.FA_CAI);
        user2.add(Mahjong.FA_CAI);
        user2.add(Mahjong.EIGHT_TONG);
        user2.add(Mahjong.NINE_TONG);
        user2.add(Mahjong.NINE_TIAO);
        user2.add(Mahjong.EIGHT_TIAO);
        user2.add(Mahjong.SEVEN_TIAO);

        List<Mahjong> user3 = new ArrayList<>(1);
        user3.add(Mahjong.SIX_TIAO);
        user3.add(Mahjong.SIX_TIAO);
        user3.add(Mahjong.SIX_TIAO);
        user3.add(Mahjong.SEVEN_TIAO);
        user3.add(Mahjong.SEVEN_TIAO);
        user3.add(Mahjong.SEVEN_TIAO);

        List<Mahjong> user4 = new ArrayList<>(1);

        personalMahjongs.add(user1);
        personalMahjongs.add(user2);
        personalMahjongs.add(user3);
        personalMahjongs.add(user4);


        // 排在列表最前的，先摸牌
        List<Mahjong> beTouchMahjongs = new ArrayList<>(1);
        beTouchMahjongs.add(Mahjong.THREE_TIAO);
        beTouchMahjongs.add(Mahjong.THREE_WANG);
        beTouchMahjongs.add(Mahjong.TWO_WANG);
        beTouchMahjongs.add(Mahjong.THREE_WANG);
        beTouchMahjongs.add(Mahjong.TWO_WANG);
        beTouchMahjongs.add(Mahjong.ONE_WANG);
        beTouchMahjongs.add(Mahjong.SIX_TIAO);
        beTouchMahjongs.add(Mahjong.SIX_TONG);
        beTouchMahjongs.add(Mahjong.SIX_TONG);
        beTouchMahjongs.add(Mahjong.SIX_TONG);

        return genMockAllMahjongs(personalMahjongs, beTouchMahjongs);
    }

    /**
     * 回头碰杠
     */
    public static List<Mahjong> backPengGang() {
        List<List<Mahjong>> personalMahjongs = new ArrayList<>(4);


        List<Mahjong> user1 = new ArrayList<>(1);
        user1.add(Mahjong.ONE_WANG);
        user1.add(Mahjong.SIX_TIAO);


        List<Mahjong> user2 = new ArrayList<>(3);
        user2.add(Mahjong.ONE_WANG);
        user2.add(Mahjong.ONE_WANG);
        user2.add(Mahjong.ONE_WANG);
        user2.add(Mahjong.SIX_TIAO);
        user2.add(Mahjong.SIX_TIAO);


        List<Mahjong> user3 = new ArrayList<>(1);

        List<Mahjong> user4 = new ArrayList<>(1);

        personalMahjongs.add(user1);
        personalMahjongs.add(user2);
        personalMahjongs.add(user3);
        personalMahjongs.add(user4);


        // 排在列表最前的，先摸牌
        List<Mahjong> beTouchMahjongs = new ArrayList<>(1);
        beTouchMahjongs.add(Mahjong.THREE_TIAO);
        beTouchMahjongs.add(Mahjong.THREE_TONG);
        beTouchMahjongs.add(Mahjong.THREE_TONG);
        beTouchMahjongs.add(Mahjong.THREE_TONG);
        beTouchMahjongs.add(Mahjong.SIX_TIAO);

        return genMockAllMahjongs(personalMahjongs, beTouchMahjongs);
    }

    /**
     * 生成所有人的模拟手牌
     */
    private static List<Mahjong> genMockAllMahjongs(List<List<Mahjong>> personalMahjongs, List<Mahjong> beTouchMahjongs) {
        List<Mahjong> allMahjongs = Mahjong.getAllMahjongs();
        Mahjong[] m = new Mahjong[allMahjongs.size()];

        int index = allMahjongs.size() - 1;

        // 找到剩下的手牌
        for (List<Mahjong> personalMahjong : personalMahjongs) {
            for (Mahjong mahjong : personalMahjong) {
                allMahjongs.remove(mahjong);
            }
        }
        for (Mahjong beTouchMahjong : beTouchMahjongs) {
            allMahjongs.remove(beTouchMahjong);
        }

        // 生成每个人的手牌
        for (List<Mahjong> personalMahjong : personalMahjongs) {
            for (int i = 0; i < MahjongConfig.HAND_CARD_NUMBER; i++) {
                if (i < personalMahjong.size()) {
                    m[index--] = personalMahjong.get(i);
                } else {
                    m[index--] = getOneForm(allMahjongs);
                }

            }
        }

        // 添加设计好要摸的牌
        for (Mahjong beTouchMahjong : beTouchMahjongs) {
            m[index--] = beTouchMahjong;
        }

        // 添加剩余随机要摸的牌
        for (int i = 0, c = allMahjongs.size(); i < c; i++) {
            m[index--] = getOneForm(allMahjongs);
        }

        List<Mahjong> rst = new ArrayList<>(m.length);
        rst.addAll(Arrays.asList(m));
        return rst;
    }


    /**
     * 从mahjongs中抽一个麻将出来，并在mahjongs中移除此麻将
     */
    private static Mahjong getOneForm(List<Mahjong> mahjongs) {
        int i = RandomUtils.nextInt(mahjongs.size());
        return mahjongs.remove(i);
    }
}
