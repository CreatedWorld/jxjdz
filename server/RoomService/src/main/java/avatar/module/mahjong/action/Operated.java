package avatar.module.mahjong.action;

import avatar.module.mahjong.Mahjong;
import avatar.module.mahjong.operate.ActionType;

import java.util.ArrayList;
import java.util.List;

/**
 * 记录所有玩家执行过的操作
 */
public class Operated {
    /**
     * 执行动作的玩家
     */
    private int actionUserId;

    /**
     * 被操作的玩家
     */
    private int targetUserId;

    /**
     * 执行麻将时操作的麻将，例如打出2万，被别人吃，则存2万
     */
    private Mahjong actionMahjong;

    /**
     * 执行麻将时操作的麻将，例如打出2万，被别人吃，则存1,2，3万
     */
    private List<Mahjong> actionMahjongs;

    public int getActionUserId() {
        return actionUserId;
    }

    public int getTargetUserId() {
        return targetUserId;
    }

    public Mahjong getActionMahjong() {
        return actionMahjong;
    }

    public List<Mahjong> getActionMahjongs() {
        return actionMahjongs;
    }

    public ActionType getActionType() {
        return actionType;
    }

    private ActionType actionType;

    public static Operated newPass(int actionUserId) {
        Operated operated = new Operated();
        operated.actionUserId = actionUserId;
        operated.actionType = ActionType.PASS;
        return operated;
    }

    public static Operated newPlayAMahjong(int actionUserId, Mahjong mahjong) {
        Operated operated = new Operated();
        operated.actionUserId = actionUserId;
        operated.actionMahjong = mahjong;
        operated.actionType = ActionType.PLAY_A_MAHJONG;
        return operated;
    }

    public static Operated newCommonAnGang(int actionUserId, Mahjong mahjong) {
        Operated operated = new Operated();
        operated.actionUserId = actionUserId;
        operated.actionMahjong = mahjong;
        operated.actionMahjongs = new ArrayList<>(4);
        operated.actionMahjongs.add(mahjong);
        operated.actionMahjongs.add(mahjong);
        operated.actionMahjongs.add(mahjong);
        operated.actionMahjongs.add(mahjong);
        operated.actionType = ActionType.COMMON_AN_GANG;
        return operated;
    }

    public static Operated newBackAnGang(int actionUserId, Mahjong mahjong) {
        Operated operated = new Operated();
        operated.actionUserId = actionUserId;
        operated.actionMahjong = mahjong;
        operated.actionMahjongs = new ArrayList<>(4);
        operated.actionMahjongs.add(mahjong);
        operated.actionMahjongs.add(mahjong);
        operated.actionMahjongs.add(mahjong);
        operated.actionMahjongs.add(mahjong);
        operated.actionType = ActionType.BACK_AN_GANG;
        return operated;
    }

    public static Operated newZhiGang(int actionUserId, int targetUserId, Mahjong mahjong) {
        Operated operated = new Operated();
        operated.actionUserId = actionUserId;
        operated.targetUserId = targetUserId;
        operated.actionMahjong = mahjong;
        operated.actionMahjongs = new ArrayList<>(4);
        operated.actionMahjongs.add(mahjong);
        operated.actionMahjongs.add(mahjong);
        operated.actionMahjongs.add(mahjong);
        operated.actionMahjongs.add(mahjong);
        operated.actionType = ActionType.ZHI_GANG;
        return operated;
    }


    public static Operated newCommonPengGang(int actionUserId, Mahjong mahjong) {
        Operated operated = new Operated();
        operated.actionUserId = actionUserId;
        operated.actionMahjong = mahjong;
        operated.actionMahjongs = new ArrayList<>(4);
        operated.actionMahjongs.add(mahjong);
        operated.actionMahjongs.add(mahjong);
        operated.actionMahjongs.add(mahjong);
        operated.actionMahjongs.add(mahjong);
        operated.actionType = ActionType.COMMON_PENG_GANG;
        return operated;
    }


    public static Operated newBackPengGang(int actionUserId, Mahjong mahjong) {
        Operated operated = new Operated();
        operated.actionUserId = actionUserId;
        operated.actionMahjong = mahjong;
        operated.actionMahjongs = new ArrayList<>(4);
        operated.actionMahjongs.add(mahjong);
        operated.actionMahjongs.add(mahjong);
        operated.actionMahjongs.add(mahjong);
        operated.actionMahjongs.add(mahjong);
        operated.actionType = ActionType.BACK_PENG_GANG;
        return operated;
    }


    public static Operated newPeng(int actionUserId, int targetUserId, Mahjong mahjong) {
        Operated operated = new Operated();
        operated.actionUserId = actionUserId;
        operated.targetUserId = targetUserId;
        operated.actionMahjong = mahjong;
        operated.actionMahjongs = new ArrayList<>(3);
        operated.actionMahjongs.add(mahjong);
        operated.actionMahjongs.add(mahjong);
        operated.actionMahjongs.add(mahjong);
        operated.actionType = ActionType.PENG;
        return operated;
    }

    public static Operated newChi(int actionUserId, int targetUserId, Mahjong mahjong, List<Mahjong> mahjongs) {
        Operated operated = new Operated();
        operated.actionUserId = actionUserId;
        operated.targetUserId = targetUserId;
        operated.actionMahjong = mahjong;
        operated.actionMahjongs = new ArrayList<>(mahjongs);
        operated.actionType = ActionType.CHI;
        return operated;
    }


    public static Operated newZiMo(int actionUserId,  Mahjong mahjong, List<Mahjong> mahjongs) {
        Operated operated = new Operated();
        operated.actionUserId = actionUserId;
        operated.actionMahjong = mahjong;
        operated.actionMahjongs = new ArrayList<>(mahjongs);
        operated.actionType = ActionType.ZI_MO;
        return operated;
    }

    public static Operated newChiHu(int actionUserId, int targetUserId, Mahjong mahjong, List<Mahjong> mahjongs) {
        Operated operated = new Operated();
        operated.actionUserId = actionUserId;
        operated.targetUserId = targetUserId;
        operated.actionMahjong = mahjong;
        operated.actionMahjongs = new ArrayList<>(mahjongs);
        operated.actionType = ActionType.CHI_HU;
        return operated;
    }

    public static Operated newQiangAnGangHu(int actionUserId, int targetUserId, Mahjong mahjong, List<Mahjong> mahjongs) {
        Operated operated = new Operated();
        operated.actionUserId = actionUserId;
        operated.targetUserId = targetUserId;
        operated.actionMahjong = mahjong;
        operated.actionMahjongs = new ArrayList<>(mahjongs);
        operated.actionType = ActionType.QIANG_AN_GANG_HU;
        return operated;
    }

    public static Operated newQiangZhiGangHu(int actionUserId, int targetUserId, Mahjong mahjong, List<Mahjong> mahjongs) {
        Operated operated = new Operated();
        operated.actionUserId = actionUserId;
        operated.targetUserId = targetUserId;
        operated.actionMahjong = mahjong;
        operated.actionMahjongs = new ArrayList<>(mahjongs);
        operated.actionType = ActionType.QIANG_ZHI_GANG_HU;
        return operated;
    }

    public static Operated newQiangPengGangHu(int actionUserId, int targetUserId, Mahjong mahjong, List<Mahjong> mahjongs) {
        Operated operated = new Operated();
        operated.actionUserId = actionUserId;
        operated.targetUserId = targetUserId;
        operated.actionMahjong = mahjong;
        operated.actionMahjongs = new ArrayList<>(mahjongs);
        operated.actionType = ActionType.QIANG_PENG_GANG_HU;
        return operated;
    }

}