package avatar.module.mahjong;

import java.util.*;

public class HuPaiAlgorithm {

    /**
     * 不能组成胡牌的麻将个数
     * <p>
     * 手牌完全可以组成combo组合的麻将个数：
     * （x为不能完全可以组成combo）
     * 二万 1只 x
     * 二万 二万 2只
     * 二万 二万 二万 3只
     * 二万 二万 三万 三万 4只
     * 二万 二万 二万 三万 三万 5只
     * 二万 二万 二万 三万 三万 三万 6只
     * 二万 二万 三万 三万 四万 四万 6只
     * 二万 二万 三万 三万 四万 四万 五万 7只 x
     * 二万 二万 二万 三万 三万 三万 五万 7只 x
     * 二万 二万 二万 三万 三万 三万 四万 四万 8只
     * 二万 二万 二万 三万 三万 三万 四万 四万 四万 9只
     * 二万 二万 二万 三万 三万 三万 四万 四万 四万 五万 10只 x
     * 二万 二万 三万 三万 四万 四万 五万 五万 六万 六万 10只
     * 二万 二万 二万 三万 三万 三万 四万 四万 四万 五万 五万 11只
     * 二万 二万 三万 三万 四万 四万 五万 五万 六万 六万 七万 11只 x
     * 二万 二万 二万 三万 三万 三万 四万 四万 四万 五万 五万 五万 12只
     * 二万 二万 三万 三万 四万 四万 五万 五万 六万 六万 七万 七万 12只
     * 二万 二万 二万 三万 三万 三万 四万 四万 四万 五万 五万 五万 六万 13只 x
     * 二万 二万 三万 三万 四万 四万 五万 五万 六万 六万 七万 七万 八万 13只 x
     * 二万 二万 二万 三万 三万 三万 四万 四万 四万 五万 五万 五万 六万 六万 14只
     * 二万 二万 三万 三万 四万 四万 五万 五万 六万 六万 七万 七万 八万 八万 14只
     */

    /**
     * 不能组成AAA、ABC、AA混合组合的麻将个数
     */
    private static List<Integer> NOT_HU_SIZE = Arrays.asList(1, 4, 7, 10, 13, 16);

    /**
     * 不能组成全部都是AA组合的麻将个数
     */
    private static List<Integer> NOT_AA_HU_SIZE = Arrays.asList(1, 3, 5, 7, 9, 11, 13, 15, 17);

    /**
     * 根据传入的mahjongs，完全组成AAA、ABC、AA组合
     * 组合成功，返回的List<Combo>的大小大于0
     * 组合不成功，返回的List<Combo>的等于0
     */
    public static List<Combo> genCombo(List<Mahjong> mahjongs) {
        //麻将类型(type)为键 , 值为麻将集合
        Map<Integer, List<Mahjong>> mahjongsByType = Mahjong.groupByType(mahjongs);
        List<Combo> combos = new ArrayList<>();
        for (Map.Entry<Integer, List<Mahjong>> mahjongEntry : mahjongsByType.entrySet()) {
            if (!genComboInOneMahjongType(combos, mahjongEntry.getValue())) {
                combos.clear();
                return combos;
            }
        }
        return combos;
    }

    /**
     * 根据传入的mahjongs，完全组成AA组合(主要用于七对胡)
     * 组合成功，返回的List<Combo>的大小大于0
     * 组合不成功，返回的List<Combo>的等于0
     */
    public static List<Combo> genAACombos(List<Mahjong> mahjongs) {
        Map<Integer, List<Mahjong>> mahjongsByType = Mahjong.groupByType(mahjongs);
        List<Combo> combos = new ArrayList<>();
        for (Map.Entry<Integer, List<Mahjong>> mahjongEntry : mahjongsByType.entrySet()) {
            if (!genAAComboInOneMahjongType(combos, mahjongEntry.getValue())) {
                combos.clear();
                return combos;
            }
        }
        return combos;
    }


    /**
     * 根据传入的mahjongs，完全组成AA组合
     * 组合成功，返回true，combos为组合成功后的组合
     * 组合不成功返回false
     */
    private static boolean genAAComboInOneMahjongType(List<Combo> combos, List<Mahjong> mahjongs) {
        if (NOT_AA_HU_SIZE.contains(mahjongs.size())) {
            return false;
        }

        if (mahjongs.size() == 0) {
            return true;
        }

        Collections.sort(mahjongs);

        Combo combo = AA(mahjongs);
        if (combo != null) {
            combos.add(combo);
            return genAAComboInOneMahjongType(combos, mahjongs);
        } else {
            return false;
        }
    }

    /**
     * 根据传入的mahjongs，完全组成AAA、ABC、AA组合
     * 组合成功，返回true，combos为组合成功后的组合
     * 组合不成功返回false
     */
    private static boolean genComboInOneMahjongType(List<Combo> combos, List<Mahjong> mahjongs) {
        if (NOT_HU_SIZE.contains(mahjongs.size())) {
            return false;
        }

        if (mahjongs.size() == 0) {
            return true;
        }

        Collections.sort(mahjongs);

        Combo combo = AAA(mahjongs);
        if (combo != null) {
            combos.add(combo);
            if (genComboInOneMahjongType(combos, mahjongs)) {
                return true;
            } else {
                putBackMahjongToList(combo, mahjongs);
                combos.remove(combo);
                combo = ABC(mahjongs);
                return checkABC(combos, mahjongs, combo);
            }
        } else {
            combo = ABC(mahjongs);
            return checkABC(combos, mahjongs, combo);
        }
    }

    private static boolean checkAA(List<Combo> combos, List<Mahjong> mahjongs, Combo combo) {
        if (combo != null) {
            combos.add(combo);
            if (genComboInOneMahjongType(combos, mahjongs)) {
                return true;
            } else {
                putBackMahjongToList(combo, mahjongs);
                combos.remove(combo);
                return false;
            }
        } else {
            return false;
        }
    }

    private static boolean checkABC(List<Combo> combos, List<Mahjong> mahjongs, Combo combo) {
        if (combo != null) {
            combos.add(combo);
            if (genComboInOneMahjongType(combos, mahjongs)) {
                return true;
            } else {
                putBackMahjongToList(combo, mahjongs);
                combos.remove(combo);
                combo = AA(mahjongs);
                return checkAA(combos, mahjongs, combo);
            }
        } else {
            combo = AA(mahjongs);
            return checkAA(combos, mahjongs, combo);
        }
    }

    /**
     * 从列表第一只为指定开始牌，找出一个AAA的组合
     */
    private static Combo AAA(List<Mahjong> mahjongs) {
        if (mahjongs.size() < 3) {
            return null;
        }

        Combo combo = Combo.newAAA();
        combo.getMahjongs().add(mahjongs.remove(0));

        for (int i = 0; i < mahjongs.size(); i++) {
            if (combo.getMahjongs().get(0).equals(mahjongs.get(i))) {
                combo.getMahjongs().add(mahjongs.remove(i));
                break;
            }
        }

        if (combo.getMahjongs().size() != 2) {
            putBackMahjongToList(combo, mahjongs);
            return null;
        }

        for (int i = 0; i < mahjongs.size(); i++) {
            if (combo.getMahjongs().get(0).equals(mahjongs.get(i))) {
                combo.getMahjongs().add(mahjongs.remove(i));
                break;
            }
        }

        if (combo.getMahjongs().size() != 3) {
            putBackMahjongToList(combo, mahjongs);
            return null;
        }

        return combo;
    }

    /**
     * 从列表第一只为指定开始牌，找出一个ABC的组合
     */
    private static Combo ABC(List<Mahjong> mahjongs) {
        if (mahjongs.size() < 3) {
            return null;
        }

        Combo combo = Combo.newABC();
        combo.getMahjongs().add(mahjongs.remove(0));

        for (int i = 0; i < mahjongs.size(); i++) {
            if (combo.getMahjongs().get(0).getCode() + 1 == mahjongs.get(i).getCode()) {
                combo.getMahjongs().add(mahjongs.remove(i));
                break;
            }
        }

        if (combo.getMahjongs().size() != 2) {
            putBackMahjongToList(combo, mahjongs);
            return null;
        }

        for (int i = 0; i < mahjongs.size(); i++) {
            if (combo.getMahjongs().get(0).getCode() + 2 == mahjongs.get(i).getCode()) {
                combo.getMahjongs().add(mahjongs.remove(i));
                break;
            }
        }

        if (combo.getMahjongs().size() != 3) {
            putBackMahjongToList(combo, mahjongs);
            return null;
        }

        return combo;
    }

    /**
     * 从列表第一只为指定开始牌，找出一个AA的组合
     */
    private static Combo AA(List<Mahjong> mahjongs) {
        Combo combo = Combo.newAA();
        combo.getMahjongs().add(mahjongs.remove(0));

        for (int i = 0; i < mahjongs.size(); i++) {
            if (combo.getMahjongs().get(0).equals(mahjongs.get(i))) {
                combo.getMahjongs().add(mahjongs.remove(i));
                break;
            }
        }

        if (combo.getMahjongs().size() != 2) {
            putBackMahjongToList(combo, mahjongs);
            return null;
        }

        return combo;
    }

    private static void putBackMahjongToList(Combo combo, List<Mahjong> mahjongs) {
        for (Mahjong mahjong : combo.getMahjongs()) {
            mahjongs.add(mahjong);
        }
        Collections.sort(mahjongs);
    }
}