package avatar.module.express.function;

import avatar.module.express.NameMapping;
import avatar.module.mahjong.Combo;
import avatar.module.mahjong.Mahjong;
import com.ql.util.express.Operator;

import java.util.ArrayList;
import java.util.List;

/**
 * 吃牌的判断：返回
 */
@SuppressWarnings("unchecked")
public class ChiPaiComBo extends Operator {
    public ChiPaiComBo() {
        this.name = NameMapping.CHI_PAI_COMBO.getName();
    }

    @Override
    public Object executeInner(Object[] list) throws Exception {
        Integer putOutMahjongCode = (Integer) list[0];
        List<Integer> handCards = (List<Integer>) list[1];
        List<Combo> combos = new ArrayList<>(3);
        if (putOutMahjongCode > 40) {
            return combos;
        }
        char[] chars = (putOutMahjongCode + "").toCharArray();
        int ordinal = Integer.parseInt(String.valueOf(chars[chars.length - 1]));
        if (ordinal == 1) {
            handleRight(handCards, putOutMahjongCode, combos);
        } else if (ordinal == 2) {
            handleMiddle(handCards, putOutMahjongCode, combos);
            handleRight(handCards, putOutMahjongCode, combos);
        } else if (ordinal == 8) {
            handleLeft(handCards, putOutMahjongCode, combos);
            handleMiddle(handCards, putOutMahjongCode, combos);

        } else if (ordinal == 9) {
            handleLeft(handCards, putOutMahjongCode, combos);
        } else {
            handleLeft(handCards, putOutMahjongCode, combos);
            handleMiddle(handCards, putOutMahjongCode, combos);
            handleRight(handCards, putOutMahjongCode, combos);
        }
        return combos;
    }

    private static void handleLeft(List<Integer> handCards, Integer putOutMahjongCode, List<Combo> combos) {
        if (handCards.contains(putOutMahjongCode - 1)
                && handCards.contains(putOutMahjongCode - 2)) {
            Combo combo = Combo.newABC();
            List<Mahjong> mahjongs = new ArrayList<>(3);
            mahjongs.add(Mahjong.parseFromCode(putOutMahjongCode - 2));
            mahjongs.add(Mahjong.parseFromCode(putOutMahjongCode - 1));
            mahjongs.add(Mahjong.parseFromCode(putOutMahjongCode));
            combo.setMahjongs(mahjongs);
            combos.add(combo);
        }
    }

    private static void handleRight(List<Integer> handCards, Integer putOutMahjongCode, List<Combo> combos) {
        if (handCards.contains(putOutMahjongCode + 1)
                && handCards.contains(putOutMahjongCode + 2)) {
            Combo combo = Combo.newABC();
            List<Mahjong> mahjongs = new ArrayList<>(3);
            mahjongs.add(Mahjong.parseFromCode(putOutMahjongCode));
            mahjongs.add(Mahjong.parseFromCode(putOutMahjongCode + 1));
            mahjongs.add(Mahjong.parseFromCode(putOutMahjongCode + 2));
            combo.setMahjongs(mahjongs);
            combos.add(combo);
        }
    }

    private static void handleMiddle(List<Integer> handCards, Integer putOutMahjongCode, List<Combo> combos) {
        if (handCards.contains(putOutMahjongCode - 1)
                && handCards.contains(putOutMahjongCode + 1)) {
            Combo combo = Combo.newABC();
            List<Mahjong> mahjongs = new ArrayList<>(3);
            mahjongs.add(Mahjong.parseFromCode(putOutMahjongCode - 1));
            mahjongs.add(Mahjong.parseFromCode(putOutMahjongCode));
            mahjongs.add(Mahjong.parseFromCode(putOutMahjongCode + 1));
            combo.setMahjongs(mahjongs);
            combos.add(combo);
        }
    }

//        Integer integer = (Integer) list[0];
//        List<Integer> mahjongNumbers = (List<Integer>) list[1];
//        ArrayList<Combo> comboList = new ArrayList<>();
//
//        ArrayList<ArrayList<Integer>> all = new ArrayList<>();
//        initComboArr(11, 19, all);
//        initComboArr(21, 29, all);
//        initComboArr(31, 39, all);
//
//        for (ArrayList<Integer> temp : all) {
//            if (temp.indexOf(integer) > -1) {
//                Combo combo = new Combo();
//                ArrayList<Mahjong> tmpList = new ArrayList<>();
//                Mahjong m1 = Mahjong.parseFromCode(integer);
//                tmpList.add(m1);
//
//                for (Integer id : mahjongNumbers) {
//                    if (integer == id) {
//                        continue;
//                    }
//                    if (temp.indexOf(id) > -1) {
//                        m1 = Mahjong.parseFromCode(id);
//                        tmpList.add(m1);
//                        if (tmpList.size() == 3) {
//                            break;
//                        }
//                    }
//                }
//                combo.setMahjongs(tmpList);
//                if (tmpList.size() == 3) {
//                    comboList.add(combo);
//                }
//            }
//        }
//        return comboList;



//        ArrayList<Combo> comboList = new ArrayList<>();
//
//        //===判断能否组成吃牌Combo的逻辑===//
//        Integer[] yaoArr = {11, 19, 21};//幺牌
//        Integer[] jiuArr = {29, 31, 39};//九牌
//        Integer[] erArr = {12, 18, 22};//二牌
//        Integer[] baArr = {28, 32, 38};//八牌
//        Integer[] ziPaiArr = {41, 42, 43, 44, 51, 52, 53};//字牌
//        List<Integer> yaoList = Arrays.asList(yaoArr);
//        List<Integer> jiuList = Arrays.asList(jiuArr);
//        List<Integer> erList = Arrays.asList(erArr);
//        List<Integer> baList = Arrays.asList(baArr);
//        List<Integer> ziPaiList = Arrays.asList(ziPaiArr);
//
//        //如果打出的牌是字牌直接返回空的combolist
//        if (ziPaiList.contains(integer)){
//            return comboList;
//        }
//        //数字牌则按以下逻辑判断
//        if (yaoList.contains(integer)){//如果是幺牌，只要下家手牌有2、3就可以吃
//            if (mahjongNumbers.contains(integer + 1) && mahjongNumbers.contains(integer + 2)){
//                Combo combo = new Combo();
//                ArrayList<Mahjong> tmpList = new ArrayList<>();
//                Mahjong m1 = Mahjong.parseFromCode(integer + 1);
//                Mahjong m2 = Mahjong.parseFromCode(integer + 2);
//                tmpList.add(m1);
//                tmpList.add(m2);
//                combo.setMahjongs(tmpList);
//                comboList.add(combo);
//            }
//        }else if (jiuList.contains(integer)){//如果是九牌，只要下家手牌有8、7九可以吃
//            if (mahjongNumbers.contains(integer - 1) && mahjongNumbers.contains(integer - 2)){
//                Combo combo = new Combo();
//                ArrayList<Mahjong> tmpList = new ArrayList<>();
//                Mahjong m1 = Mahjong.parseFromCode(integer - 1);
//                Mahjong m2 = Mahjong.parseFromCode(integer - 2);
//                tmpList.add(m1);
//                tmpList.add(m2);
//                combo.setMahjongs(tmpList);
//                comboList.add(combo);
//            }
//
//        }else if (erList.contains(integer)){//如果是二牌，只要下家手牌有1、3或者3、4九可以吃
//            if (mahjongNumbers.contains(integer + 1) && mahjongNumbers.contains(integer + 2)){//如果下家手牌有3、4
//                Combo combo = new Combo();
//                ArrayList<Mahjong> tmpList = new ArrayList<>();
//                Mahjong m1 = Mahjong.parseFromCode(integer + 1);
//                Mahjong m2 = Mahjong.parseFromCode(integer + 2);
//                tmpList.add(m1);
//                tmpList.add(m2);
//                combo.setMahjongs(tmpList);
//                comboList.add(combo);
//            }
//            if (mahjongNumbers.contains(integer - 1) && mahjongNumbers.contains(integer + 1)){//如果下家手牌有1、3
//                Combo combo = new Combo();
//                ArrayList<Mahjong> tmpList = new ArrayList<>();
//                Mahjong m1 = Mahjong.parseFromCode(integer - 1);
//                Mahjong m2 = Mahjong.parseFromCode(integer + 1);
//                tmpList.add(m1);
//                tmpList.add(m2);
//                combo.setMahjongs(tmpList);
//                comboList.add(combo);
//            }
//        }else if (baList.contains(integer)){//如果是八牌，只要下家手牌有9、7或者7、6就可以吃
//            if (mahjongNumbers.contains(integer - 1) && mahjongNumbers.contains(integer - 2)){//如果下家手牌有7、6
//                Combo combo = new Combo();
//                ArrayList<Mahjong> tmpList = new ArrayList<>();
//                Mahjong m1 = Mahjong.parseFromCode(integer - 1);
//                Mahjong m2 = Mahjong.parseFromCode(integer - 2);
//                tmpList.add(m1);
//                tmpList.add(m2);
//                combo.setMahjongs(tmpList);
//                comboList.add(combo);
//            }
//            if (mahjongNumbers.contains(integer + 1) && mahjongNumbers.contains(integer - 1)){//如果下家手牌有9、7
//                Combo combo = new Combo();
//                ArrayList<Mahjong> tmpList = new ArrayList<>();
//                Mahjong m1 = Mahjong.parseFromCode(integer - 1);
//                Mahjong m2 = Mahjong.parseFromCode(integer + 1);
//                tmpList.add(m1);
//                tmpList.add(m2);
//                combo.setMahjongs(tmpList);
//                comboList.add(combo);
//            }
//        }else {
//
//        }
//
//        return comboList;


//    private void initComboArr(int start, int end, ArrayList<ArrayList<Integer>> all) {
//        for (int i = start; i <= end; i++) {
//            ArrayList<Integer> l;
//            if (all.size() == 0) {
//                l = new ArrayList<>();
//                all.add(l);
//            } else {
//                l = all.get(all.size() - 1);
//            }
//            if (l.size() == 3) {
//                l = new ArrayList<>();
//                all.add(l);
//                if (i != start) {
//                    i -= 2;
//                }
//            }
//            l.add(i);
//        }
//    }

    public static void main(String[] args) {
        /*Integer integer = 15;
        List<Integer> mahjongNumbers = new ArrayList<>();
        mahjongNumbers.add(11);
        mahjongNumbers.add(12);
        mahjongNumbers.add(13);
        mahjongNumbers.add(14);
        mahjongNumbers.add(16);
        mahjongNumbers.add(19);
        mahjongNumbers.add(22);
        mahjongNumbers.add(23);


        ArrayList<Combo> comboList = new ArrayList<>();

        ArrayList<ArrayList<Integer>> all = new ArrayList<>();
        initComboArr(11, 19, all);
        initComboArr(21, 29, all);
        initComboArr(31, 39, all);

        boolean flag = false;
        for (ArrayList<Integer> temp : all) {
            if (temp.indexOf(integer) > -1) {
                int count = 0;
                Combo combo = new Combo();
                ArrayList<Mahjong> tmpList = new ArrayList<>();
                Mahjong m1 = Mahjong.parseFromCode(integer);
                tmpList.add(m1);

                for (Integer id : mahjongNumbers) {
                    if (integer == id) {
                        continue;
                    }
                    if (temp.indexOf(id) > -1) {
                        m1 = Mahjong.parseFromCode(id);
                        tmpList.add(m1);
                        if (tmpList.size() == 3) {
                            break;
                        }
                    }
                }
                combo.setMahjongs(tmpList);
                if (tmpList.size() == 3) {
                    comboList.add(combo);
                }
            }
        }
        for (Combo c : comboList) {
            System.out.println(c.toString());
        }*/





        /*Integer putOutMahjongCode = 15;
        List<Integer> handCards = new ArrayList<>();
        handCards.add(11);
        handCards.add(12);
        handCards.add(13);
        handCards.add(14);
        handCards.add(16);
        handCards.add(17);
        handCards.add(19);
        handCards.add(22);
        handCards.add(23);
        handCards.add(36);
        handCards.add(37);
        handCards.add(39);
        List<Combo> combos = new ArrayList<>(3);
        if (putOutMahjongCode > 40) {
            System.out.println("*******沒有combo*********");
        }
        char[] chars = (putOutMahjongCode + "").toCharArray();
        int ordinal = Integer.parseInt(String.valueOf(chars[chars.length - 1]));
        if (ordinal == 1) {
            handleRight(handCards, putOutMahjongCode, combos);
        } else if (ordinal == 2) {
            handleMiddle(handCards, putOutMahjongCode, combos);
            handleRight(handCards, putOutMahjongCode, combos);
        } else if (ordinal == 8) {
            handleLeft(handCards, putOutMahjongCode, combos);
            handleMiddle(handCards, putOutMahjongCode, combos);

        } else if (ordinal == 9) {
            handleLeft(handCards, putOutMahjongCode, combos);
        } else {
            handleLeft(handCards, putOutMahjongCode, combos);
            handleMiddle(handCards, putOutMahjongCode, combos);
            handleRight(handCards, putOutMahjongCode, combos);
        }
        for (Combo c : combos){
            System.out.println(c.getMahjongs());
        }*/
    }
}
