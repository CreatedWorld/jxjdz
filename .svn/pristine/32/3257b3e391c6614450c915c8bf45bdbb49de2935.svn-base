package avatar.util;

import avatar.module.mahjong.Mahjong;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.List;

/**
 * 宝牌工具
 */
public class TreasureCardUtil {

    /**
     * 从手牌中获取宝牌
     *
     * @param mahjongs 手牌
     * @return
     */
    public static List<Mahjong> getMyBaoMahjongs(List<Mahjong> mahjongs, Mahjong treasureCard) {
        if (treasureCard == null) {
            return Collections.EMPTY_LIST;
        }

        List<Mahjong> treasureMahjongs = new ArrayList<>();
        mahjongs.forEach(m -> {
            if (m == treasureCard) {
                treasureMahjongs.add(m);
            }
        });
        return treasureMahjongs;
    }

    /**
     * 生成宝牌能变成的牌的所有组合
     *
     * @param treasureCardNum 宝牌数量
     * @param makeUpMahjongs  宝牌能变成的麻将
     */
    public static List<List<Mahjong>> circulate(int treasureCardNum,
                                                Mahjong treasureCard,
                                                Collection<Mahjong> makeUpMahjongs) {
        List<List<Mahjong>> baoMahjongs = new ArrayList<>(treasureCardNum);

        for (int i = 0; i < treasureCardNum; i++) {
            List<Mahjong> m = new ArrayList<>(makeUpMahjongs);
            m.add(treasureCard);
            baoMahjongs.add(m);
        }

        return circulate(baoMahjongs);
    }

    /**
     * 循环实现dimValue中的笛卡尔积
     *
     * @param dimValue 原始数据
     */
    private static List<List<Mahjong>> circulate(List<List<Mahjong>> dimValue) {
        int total = 1;
        for (List<Mahjong> list : dimValue) {
            total *= list.size();
        }
        List<List<Mahjong>> myResult = new ArrayList<>(total);

        int itemLoopNum;
        int loopPerItem;
        int now = 1;
        for (List<Mahjong> list : dimValue) {
            now *= list.size();

            int index = 0;
            int currentSize = list.size();

            itemLoopNum = total / now;
            loopPerItem = total / (itemLoopNum * currentSize);
            int myIndex = 0;

            for (Mahjong string : list) {
                for (int i = 0; i < loopPerItem; i++) {
                    if (myIndex == list.size()) {
                        myIndex = 0;
                    }

                    for (int j = 0; j < itemLoopNum; j++) {
                        if (myResult.size() == index) {
                            List<Mahjong> temp = new ArrayList<>(dimValue
                                    .size());
                            temp.add(list.get(myIndex));
                            myResult.add(temp);
                        } else {
                            myResult.get(index).add(list.get(myIndex));
                        }
                        index++;
                    }
                    myIndex++;
                }

            }
        }

        return myResult;
    }
}
