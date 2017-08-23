package avatar.module.mahjong;

import org.apache.log4j.Logger;

import java.io.IOException;
import java.io.InputStream;
import java.util.*;

/**
 * 序数牌:
 * 1:万子牌：从一万至九万，各4张，共36张
 * 2:筒子牌：从一筒至九筒，各4张，共36张。也有的地方称为饼，从一饼到九饼
 * 3:索子牌：从一索至九索，各4张，共36张。也有的地方称为条，从一条到九条
 * 字牌:
 * 4:风牌：东、南、西、北，各4张，共16张
 * 5:箭牌：中、发、白，各4张，共12张
 * 花牌:
 * 6:春、夏、秋、冬，梅、兰、竹、菊，各一张，共8张
 */
public enum Mahjong {
    ONE_WANG(11, 1, Type.WANG.getId(), "一万"),
    TWO_WANG(12, 2, Type.WANG.getId(), "二万"),
    THREE_WANG(13, 3, Type.WANG.getId(), "三万"),
    FOUR_WANG(14, 4, Type.WANG.getId(), "四万"),
    FIVE_WANG(15, 5, Type.WANG.getId(), "五万"),
    SIX_WANG(16, 6, Type.WANG.getId(), "六万"),
    SEVEN_WANG(17, 7, Type.WANG.getId(), "七万"),
    EIGHT_WANG(18, 8, Type.WANG.getId(), "八万"),
    NINE_WANG(19, 9, Type.WANG.getId(), "九万"),

    ONE_TONG(21, 1, Type.TONG.getId(), "一筒"),
    TWO_TONG(22, 2, Type.TONG.getId(), "二筒"),
    THREE_TONG(23, 3, Type.TONG.getId(), "三筒"),
    FOUR_TONG(24, 4, Type.TONG.getId(), "四筒"),
    FIVE_TONG(25, 5, Type.TONG.getId(), "五筒"),
    SIX_TONG(26, 6, Type.TONG.getId(), "六筒"),
    SEVEN_TONG(27, 7, Type.TONG.getId(), "七筒"),
    EIGHT_TONG(28, 8, Type.TONG.getId(), "八筒"),
    NINE_TONG(29, 9, Type.TONG.getId(), "九筒"),

    ONE_TIAO(31, 1, Type.TIAO.getId(), "一条"),
    TWO_TIAO(32, 2, Type.TIAO.getId(), "二条"),
    THREE_TIAO(33, 3, Type.TIAO.getId(), "三条"),
    FOUR_TIAO(34, 4, Type.TIAO.getId(), "四条"),
    FIVE_TIAO(35, 5, Type.TIAO.getId(), "五条"),
    SIX_TIAO(36, 6, Type.TIAO.getId(), "六条"),
    SEVEN_TIAO(37, 7, Type.TIAO.getId(), "七条"),
    EIGHT_TIAO(38, 8, Type.TIAO.getId(), "八条"),
    NINE_TIAO(39, 9, Type.TIAO.getId(), "九条"),

    DONG_FENG(41, 1, Type.FENG.getId(), "东"),
    NAN_FENG(42, 2, Type.FENG.getId(), "南"),
    XI_FENG(43, 3, Type.FENG.getId(), "西"),
    BEI_FENG(44, 4, Type.FENG.getId(), "北"),

    HONG_ZHONG(51, 1, Type.JIAN.getId(), "红中"),
    FA_CAI(52, 2, Type.JIAN.getId(), "发财"),
    BAI_BAN(53, 3, Type.JIAN.getId(), "白板"),

    CHUN(61, 1, Type.HUA.getId(), "春"),
    XIA(62, 2, Type.HUA.getId(), "夏"),
    QIU(63, 3, Type.HUA.getId(), "秋"),
    DONG(64, 4, Type.HUA.getId(), "东"),
    MEI(65, 5, Type.HUA.getId(), "梅"),
    LAN(66, 6, Type.HUA.getId(), "兰"),
    ZHU(67, 7, Type.HUA.getId(), "竹"),
    JU(68, 8, Type.HUA.getId(), "菊"),;

    /**
     * 所有麻将的集合
     */
    private static List<Mahjong> allMahjongs;

    /**
     * 按麻将类型分类的麻将的集合
     */
    private static Map<Integer, List<Mahjong>> groupByTypeMahjongs;

    private static final Logger logger = Logger.getLogger(Mahjong.class);

    static {
        logger.info("正在加载麻将牌。。。。。。");
        InputStream in = null;
        try {
            Properties p = new Properties();
            in = Mahjong.class.getClassLoader().getResourceAsStream("mahjong.properties");
            p.load(in);
            allMahjongs = new ArrayList<>(p.size());
            for (Map.Entry<Object, Object> entry : p.entrySet()) {
                String key = (String) entry.getKey();
                String value = (String) entry.getValue();
                Mahjong mahjong = Mahjong.valueOf(key);
                String[] values = value.split("\\*");
                int count = Integer.parseInt(values[1]);
                for (int i = 0; i < count; i++) {
                    allMahjongs.add(mahjong);
                }
            }
            Collections.sort(allMahjongs);
            logger.info(String.format("加载麻将牌成功。一共加载到%s张麻将牌。。。。。", allMahjongs.size()));
            logger.info(allMahjongs);


            // 初始化按麻将类型分类的麻将的集合
            Map<Integer, List<Mahjong>> tempMap = groupByType(allMahjongs);
            groupByTypeMahjongs = new HashMap<>((int) ((tempMap.size() / 0.75) + 1));
            for (Map.Entry<Integer, List<Mahjong>> entry : tempMap.entrySet()) {
                List<Mahjong> tempMahjongs = new ArrayList<>();

                List<Mahjong> value = entry.getValue();
                for (Mahjong mahjong : value) {
                    if (!tempMahjongs.contains(mahjong)) {
                        tempMahjongs.add(mahjong);
                    }
                }

                groupByTypeMahjongs.put(entry.getKey(), tempMahjongs);
            }

        } catch (IOException e) {
            logger.info(String.format("加载麻将牌失败。%s", e.getMessage()), e);
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

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public int getType() {
        return type;
    }

    public void setType(int type) {
        this.type = type;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public int getmOrdinal() {
        return mOrdinal;
    }

    public void setmOrdinal(int mOrdinal) {
        this.mOrdinal = mOrdinal;
    }

    /**
     * 麻将的编码
     */
    private int code;


    /**
     * 麻将的序数
     */
    private int mOrdinal;

    /**
     * 麻将的类型
     * 1:万子牌
     * 2:筒子牌
     * 3:索子牌
     * 4:风牌
     * 5:箭牌
     * 6:花牌
     */
    private int type;

    /**
     * 麻将的名称
     */
    private String name;

    Mahjong(int code, int mOrdinal, int type, String name) {
        this.code = code;
        this.mOrdinal = mOrdinal;
        this.type = type;
        this.name = name;
    }

    /**
     * 麻将的类型
     */
    public enum Type {
        WANG(1, "万"),
        TONG(2, "筒"),
        TIAO(3, "索"),
        FENG(4, "风"),
        JIAN(5, "箭"),
        HUA(6, "花");

        private int id;

        private String name;

        Type(int id, String name) {
            this.id = id;
            this.name = name;
        }

        public int getId() {
            return id;
        }

        public String getName() {
            return name;
        }
    }

    /**
     * 根据id，解析为麻将牌对象
     */
    public static Mahjong parseFromCode(int code) {
        for (Mahjong mahjong : Mahjong.values()) {
            if (mahjong.code == code) {
                return mahjong;
            }
        }
        throw new RuntimeException("麻将code参数错误，不能解析到对应的麻将牌");
    }

    /**
     * 根据Type的名称，解析为麻将类型
     */
    public static Type parseFromTypeName(String typeName ) {
        for (Type type : Mahjong.Type.values()) {
            if(type.getName().equals(typeName)){
                return type;
            }
        }
        throw new RuntimeException("麻将类型参数错误，不能解析到对应的麻将类型");
    }

    /**
     * 获取全部麻将牌
     */
    public static List<Mahjong> getAllMahjongs() {
        return new ArrayList<>(allMahjongs);
    }

    /**
     * 把麻将枚举对象列表转换为麻将code列表
     */
    public static List<Integer> parseToCodes(Collection<Mahjong> mahjongs) {
        if (mahjongs.size() == 0) {
            return Collections.emptyList();
        }

        List<Integer> mahjongCodes = new ArrayList<>(mahjongs.size());
        for (Mahjong mahjong : mahjongs) {
            mahjongCodes.add(mahjong.code);
        }
        return mahjongCodes;
    }

    /**
     * 把麻将code列表转为枚举对象列表
     */
    public static List<Mahjong> parseFromCodes(Collection<Integer> mahjongCodes) {
        if (mahjongCodes.size() == 0) {
            return Collections.emptyList();
        }

        List<Mahjong> mahjongs = new ArrayList<>(mahjongCodes.size());
        for (Integer mahjongId : mahjongCodes) {
            mahjongs.add(parseFromCode(mahjongId));
        }
        return mahjongs;
    }

    /**
     * 把麻将枚举类型的碰列表转为code的碰列表
     */
    public static List<List<Integer>> parseCombosToMahjongCodes(Collection<Combo> Combos) {
        if (Combos.size() == 0) {
            return Collections.emptyList();
        }

        List<List<Integer>> comboCodes = new ArrayList<>(Combos.size());
        for (Combo combo : Combos) {
            List<Integer> temp = parseToCodes(combo.getMahjongs());
            comboCodes.add(temp);
        }
        return comboCodes;
    }

    /**
     * 判断集合中的麻将是否都是一样，例如都是1万
     */
    public static boolean isSame(Collection<Mahjong> mahjongs) {
        if (mahjongs.isEmpty()) {
            return false;
        }
        Mahjong first = null;
        for (Mahjong mahjong : mahjongs) {
            if (first == null) {
                first = mahjong;
            } else {
                if (first != mahjong) {
                    return false;
                }
            }
        }
        return true;
    }

    /**
     * 获取下一张麻将，例如输入1万，输出2万，输入9万，输出1万
     */
    public static Mahjong next(Mahjong mahjong) {
        int type = mahjong.getType();
        List<Mahjong> mahjongs = groupByTypeMahjongs.get(type);

        int preMahjongCode = mahjong.code + 1;
        for (Mahjong m : mahjongs) {
            if (m.getCode() == preMahjongCode) {
                return m;
            }
        }
        return mahjongs.get(0);
    }

    /**
     * 获取上一张麻将，例如输入1万，输出9万，输入9万，输出8万
     */
    public static Mahjong pre(Mahjong mahjong) {
        int type = mahjong.getType();
        List<Mahjong> mahjongs = groupByTypeMahjongs.get(type);

        int preMahjongCode = mahjong.code - 1;
        for (Mahjong m : mahjongs) {
            if (m.getCode() == preMahjongCode) {
                return m;
            }
        }
        return mahjongs.get(mahjongs.size() - 1);
    }

    /**
     * 按麻将类型分组
     */
    public static Map<Integer, List<Mahjong>> groupByType(List<Mahjong> mahjongs) {
        Map<Integer, List<Mahjong>> Mahjongs = new HashMap<>(6);
        for (Mahjong mahjong : mahjongs) {
            Integer type = mahjong.type;
            List<Mahjong> tempMahjongs = Mahjongs.get(type);
            if (tempMahjongs == null) {
                tempMahjongs = new ArrayList<>(8);
                Mahjongs.put(type, tempMahjongs);
            }
            tempMahjongs.add(mahjong);
        }
        return Mahjongs;
    }

    /**
     * 判断是否花牌
     */
    public static boolean isFlower(Mahjong mahjong) {
        if (mahjong.getType() == Type.HUA.getId()) {
            return true;
        }
        return false;
    }

    /**
     * 清除花牌
     * 不改变原来的集合
     */
    public static List<Mahjong> removeFlower(List<Mahjong> mahjongs) {
        List<Mahjong> mahjongList = new ArrayList<>(mahjongs);
        mahjongList.removeIf(Mahjong::isFlower);
        return mahjongList;
    }

    @Override
    public String toString() {
        //return "{\"id\":\"" + id + "\""
        //        + ", \"number\":\"" + number + "\""
        //        + ", \"name\":\"" + name + "\""
        //        + "}";
        return toSimpleString();
    }

    public String toSimpleString() {
        return "\"" + name + "\"";
    }
}
