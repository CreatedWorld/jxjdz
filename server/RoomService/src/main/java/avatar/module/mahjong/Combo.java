package avatar.module.mahjong;

import avatar.protobuf.Cmd;

import java.util.ArrayList;
import java.util.List;

public class Combo {

    private Type type;

    private List<Mahjong> mahjongs;

    /**
     * 组成组合的来源，即是什么请求创建这个combo的
     */
    private int cmd;

    /**
     * 被碰杠的玩家id
     */
    private int targetUserId;

    public Combo() {

    }

    private Combo(Type type, List<Mahjong> mahjongs) {
        this.type = type;
        this.mahjongs = mahjongs;
    }

    public static Combo newAA() {
        return new Combo(Type.AA, new ArrayList<>(2));
    }

    public static Combo newAAA() {
        return new Combo(Type.AAA, new ArrayList<>(3));
    }

    public static Combo newABC() {
        return new Combo(Type.ABC, new ArrayList<>(3));
    }

    public static Combo newAAA(Mahjong mahjong) {
        Combo combo = new Combo(Type.AAA, new ArrayList<>(3));
        combo.getMahjongs().add(mahjong);
        combo.getMahjongs().add(mahjong);
        combo.getMahjongs().add(mahjong);
        return combo;
    }

    public static Combo newPeng(Mahjong mahjong, int targetUserId) {
        Combo combo = newAAA(mahjong);
        combo.cmd = Cmd.C2S_ROOM_PENG;
        combo.targetUserId = targetUserId;
        return combo;
    }

    public static Combo newZhiGang(Mahjong mahjong, int targetUserId) {
        Combo combo = newAAAA(mahjong);
        combo.cmd = Cmd.C2S_ROOM_ZHI_GANG;
        combo.targetUserId = targetUserId;
        return combo;
    }

    public static Combo newCommonAnGang(Mahjong mahjong, int targetUserId) {
        Combo combo = newAAAA(mahjong);
        combo.cmd = Cmd.C2S_ROOM_COMMON_AN_GANG;
        combo.targetUserId = targetUserId;
        return combo;
    }

    public static Combo newBackAnGang(Mahjong mahjong, int targetUserId) {
        Combo combo = newAAAA(mahjong);
        combo.cmd = Cmd.C2S_ROOM_COMMON_AN_GANG;
        combo.targetUserId = targetUserId;
        return combo;
    }

    public static Combo newCommonPengGang(Mahjong mahjong, int targetUserId) {
        Combo combo = newAAAA(mahjong);
        combo.cmd = Cmd.C2S_ROOM_COMMON_PENG_GANG;
        combo.targetUserId = targetUserId;
        return combo;
    }

    public static Combo newBackPengGang(Mahjong mahjong, int targetUserId) {
        Combo combo = newAAAA(mahjong);
        combo.cmd = Cmd.C2S_ROOM_BACK_PENG_GANG;
        combo.targetUserId = targetUserId;
        return combo;
    }

    public static Combo newAAAA(Mahjong mahjong) {
        Combo combo = new Combo(Type.AAAA, new ArrayList<>(4));
        combo.getMahjongs().add(mahjong);
        combo.getMahjongs().add(mahjong);
        combo.getMahjongs().add(mahjong);
        combo.getMahjongs().add(mahjong);
        return combo;
    }

    public int getCmd() {
        return cmd;
    }

    public Type getType() {
        return type;
    }

    public List<Mahjong> getMahjongs() {
        return mahjongs;
    }

    public void setMahjongs(List<Mahjong> mahjongs) {
        this.mahjongs = mahjongs;
    }

    public enum Type {
        // 杠
        AAAA(),

        // 碰
        AAA(),

        // 顺子
        ABC(),

        // 眼
        AA();

        Type() {
        }
    }

    @Override
    public String toString() {
        return "Combo{" +
                "type=" + type +
                ", mahjongs=" + mahjongs +
                '}';
    }

    public static List<List<Integer>> parseToCodes(List<Combo> combos) {
        List<List<Integer>> rst = new ArrayList<>(combos.size());

        if (combos.isEmpty()) {
            return rst;
        }

        for (Combo combo : combos) {
            List<Integer> codes = Mahjong.parseToCodes(combo.getMahjongs());
            rst.add(codes);
        }
        return rst;
    }

    public int getTargetUserId() {
        return targetUserId;
    }
}
