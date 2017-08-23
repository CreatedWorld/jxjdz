package avatar.module.mahjong.operate;

import avatar.module.mahjong.Combo;
import avatar.module.mahjong.Mahjong;
import avatar.module.mahjong.MultipleChiHu;
import avatar.module.mahjong.operate.scanner.AbstractScanner;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

/**
 * 储存别人出牌后某个玩家可以的操作
 * 如胡、杠、碰
 */
public class CanDoOperate implements Comparable {

    /**
     * 可以操作的用户id
     */
    private int userId;

    private List<Operate> operates;

    /**
     * 打出牌的或摸到牌的玩家的userId 例如A碰了B，则specialUserId为A
     */
    private int specialUserId;

    /**
     * 打出牌的或摸到牌的麻将
     */
    private Mahjong specialMahjong;

    /**
     * 可以操作的扫描器
     */
    private Class<? extends AbstractScanner> scanner;

    public Class<? extends AbstractScanner> getScanner() {
        return scanner;
    }

    public void setScanner(Class<? extends AbstractScanner> scanner) {
        this.scanner = scanner;
    }

    public int getUserId() {
        return userId;
    }

    public void setUserId(int userId) {
        this.userId = userId;
    }

    public List<Operate> getOperates() {
        return operates;
    }

    public void setOperates(List<Operate> operates) {
        this.operates = operates;
    }

    public int getSpecialUserId() {
        return specialUserId;
    }

    public void setSpecialUserId(int specialUserId) {
        this.specialUserId = specialUserId;
    }

    public Mahjong getSpecialMahjong() {
        return specialMahjong;
    }

    public void setSpecialMahjong(Mahjong specialMahjong) {
        this.specialMahjong = specialMahjong;
    }

    public static CanDoOperate newPlayAMahjongOperate(int userId, Mahjong mahjong) {
        CanDoOperate canDoOperate = new CanDoOperate();
        canDoOperate.setUserId(userId);
        canDoOperate.setSpecialUserId(userId);
        canDoOperate.setSpecialMahjong(mahjong);

        List<Operate> operates = new ArrayList<>(1);
        canDoOperate.setOperates(operates);

        Operate operate = new Operate();
        operate.setActionType(ActionType.PLAY_A_MAHJONG);
        List<Combo> combos = new ArrayList<>(1);
        Combo combo = new Combo();
        combo.setMahjongs(Arrays.asList(mahjong));
        combos.add(combo);
        operate.setCombos(combos);
        operates.add(operate);

        return canDoOperate;
    }

    public static CanDoOperate newPassOperate(int userId, Mahjong mahjong) {
        CanDoOperate canDoOperate = new CanDoOperate();
        canDoOperate.setUserId(userId);
        canDoOperate.setSpecialUserId(userId);
        canDoOperate.setSpecialMahjong(mahjong);

        List<Operate> operates = new ArrayList<>(1);
        canDoOperate.setOperates(operates);

        Operate operate = new Operate();

        operates.add(operate);

        return canDoOperate;
    }

    @Override
    public String toString() {
        String o = "";
        for (Operate operate : operates) {
            o += operate.getRule().getName() + "、";
        }
        return String.format("{用户id=%s,可以%s}", userId, o);
    }

    public String toOperateNameString() {
        String o = "";
        for (Operate operate : operates) {
            o += operate.getRule().getName() + "、";
        }
        return String.format("{、}", o);
    }

    @Override
    public int compareTo(Object o) {
        if (!(o instanceof CanDoOperate)) {
            throw new IllegalArgumentException("需要比较的对象不是CanDoOperate");
        }

        if (this.operates.isEmpty()) {
            return -1;
        }

        if (((CanDoOperate) o).getOperates().isEmpty()) {
            return 1;
        }

        ActionType firstActionType = null;
        for (Operate operate : this.operates) {
            firstActionType = operate.getActionType();
            break;
        }

        ActionType secondActionType = null;
        for (Operate operate : ((CanDoOperate) o).getOperates()) {
            secondActionType = operate.getActionType();
            break;
        }

        return firstActionType.ordinal() - secondActionType.ordinal();

    }


    public static CanDoOperate findMyCanDoOperate(List<CanDoOperate> canDoOperates, int userId) {
        for (CanDoOperate canDoOperate : canDoOperates) {
            if (canDoOperate.getUserId() == userId) {
                return canDoOperate;
            }
        }
        return null;
    }
    /**
     * 获取吃胡的玩家的CanDoOperate
     */
    public static List<CanDoOperate> findChiHuCanDoOperates(List<MultipleChiHu> multipleChiHus)
            throws IllegalAccessException, ClassNotFoundException, InstantiationException {
        List<CanDoOperate> winners = new ArrayList<>(multipleChiHus.size());
        for (MultipleChiHu multipleChiHu : multipleChiHus) {
            if (multipleChiHu.getSelect() == MultipleChiHu.Select.CHI_HU) {
                winners.add(multipleChiHu.getCanDoOperate());
            }
        }
        return winners;
    }

}