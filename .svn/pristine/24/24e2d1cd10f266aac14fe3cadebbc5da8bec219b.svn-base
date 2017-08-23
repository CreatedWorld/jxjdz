package avatar.module.express;


import avatar.util.LogUtil;
import com.ql.util.express.DefaultContext;
import com.ql.util.express.ExpressRunner;
import com.ql.util.express.Operator;
import com.ql.util.express.instruction.op.OperatorBase;

public class QLExpressUtil {


    private static final ExpressRunner runner = new ExpressRunner();

    static {
        try {
            // 自定义Operator
            runner.addOperator(NameMapping.BELONG_GROUP.getName(), (Operator) NameMapping.BELONG_GROUP.getClazz().newInstance());
            runner.addOperator(NameMapping.ONE_OF_THEM_IS.getName(), (Operator) NameMapping.ONE_OF_THEM_IS.getClazz().newInstance());
            runner.addOperator(NameMapping.ONE_OF_THEM_BELONG.getName(), (Operator) NameMapping.ONE_OF_THEM_BELONG.getClazz().newInstance());

            // 自定义Macro
            runner.addMacro(NameMapping.PUT_OUT_MAHJONG.getName(), NameMapping.PUT_OUT_MAHJONG.toString());
            runner.addMacro(NameMapping.TOUCH_MAHJONG.getName(), NameMapping.TOUCH_MAHJONG.toString());
            runner.addMacro(NameMapping.HAND_MAHJONGS.getName(), NameMapping.HAND_MAHJONGS.toString());
            runner.addMacro(NameMapping.PENGED_GROUP.getName(), NameMapping.PENGED_GROUP.toString());
            runner.addMacro(NameMapping.GANGED_GROUP.getName(), NameMapping.GANGED_GROUP.toString());
            runner.addMacro(NameMapping.CHIED_GROUP.getName(), NameMapping.CHIED_GROUP.toString());
            runner.addMacro(NameMapping.PERSONAL_ALL_MAHJONG.getName(), NameMapping.PERSONAL_ALL_MAHJONG.toString());
            runner.addMacro(NameMapping.ZI_MO.getName(), NameMapping.ZI_MO.toString());
            runner.addMacro(NameMapping.CHI_HU.getName(), NameMapping.CHI_HU.toString());

            // 自定义Function
            runner.addFunction(NameMapping.COUNT.getName(), (OperatorBase) NameMapping.COUNT.getClazz().newInstance());
            runner.addFunction(NameMapping.SAME_COLOR.getName(), (OperatorBase) NameMapping.SAME_COLOR.getClazz().newInstance());
            runner.addFunction(NameMapping.SAME_MAHJONG.getName(), (OperatorBase) NameMapping.SAME_MAHJONG.getClazz().newInstance());
            runner.addFunction(NameMapping.CONTAIN_ORDINAL.getName(), (OperatorBase) NameMapping.CONTAIN_ORDINAL.getClazz().newInstance());
            runner.addFunction(NameMapping.COMBO_COUNT.getName(), (OperatorBase) NameMapping.COMBO_COUNT.getClazz().newInstance());
            runner.addFunction(NameMapping.GEN_HU_PAI_COMBO.getName(), (OperatorBase) NameMapping.GEN_HU_PAI_COMBO.getClazz().newInstance());
            runner.addFunction(NameMapping.GEN_AA_HU_PAI_COMBO.getName(), (OperatorBase) NameMapping.GEN_AA_HU_PAI_COMBO.getClazz().newInstance());
            runner.addFunction(NameMapping.CHI_PAI_COMBO.getName(), (OperatorBase) NameMapping.CHI_PAI_COMBO.getClazz().newInstance());
            runner.addFunction(NameMapping.CONTAIN_FENG.getName(), (OperatorBase) NameMapping.CONTAIN_FENG.getClazz().newInstance());
            runner.addFunction(NameMapping.EXCLUDE_SPECIFIED_MAHJONG_OTHER_SAME_COLOR.getName(), (OperatorBase) NameMapping.EXCLUDE_SPECIFIED_MAHJONG_OTHER_SAME_COLOR.getClazz().newInstance());
            runner.addFunction(NameMapping.APPOINT_COLOR.getName(), (OperatorBase) NameMapping.APPOINT_COLOR.getClazz().newInstance());
            runner.addFunction(NameMapping.APPOINT_MAHJONG.getName(), (OperatorBase) NameMapping.APPOINT_MAHJONG.getClazz().newInstance());
            runner.addFunction(NameMapping.CONTAIN_ONE_APPOINT_MAHJONG.getName(), (OperatorBase) NameMapping.CONTAIN_ONE_APPOINT_MAHJONG.getClazz().newInstance());
            runner.addFunction(NameMapping.APPOINT_COLOR_INTERVAL.getName(), (OperatorBase) NameMapping.APPOINT_COLOR_INTERVAL.getClazz().newInstance());
            runner.addFunction(NameMapping.SPECIFIED_COLOR_COMBO_COUNT.getName(), (OperatorBase) NameMapping.SPECIFIED_COLOR_COMBO_COUNT.getClazz().newInstance());
            runner.addFunction(NameMapping.COMBO_CARDS_TYPE.getName(), (OperatorBase) NameMapping.COMBO_CARDS_TYPE.getClazz().newInstance());
            //// 自定义Operator
            //runner.addOperator("包含", new Contain("包含"));
//            runner.addOperator("计算数量", new ContainCount("计算数量"));
            //runner.addOperator("合并", new Merge("合并"));
            //
            //// 自定义Macro
            ////runner.addMacro("手牌", "getTouchMahjong(gc)");
            //runner.addMacro("打出的牌", "putOutMahjong");
            //runner.addMacro("手牌", "handMahjongs");
            //runner.addMacro("碰了的牌", "getPengs(gc)");
            //
            //// 自定义Function
            //runner.addFunction("getTouchMahjong", new TouchMahjong("getTouchMahjong"));
            //runner.addFunction("getPutOutMahjong", new PutOutMahjong("getPutOutMahjong"));
            //runner.addFunction("getHandCard", new HandCard("getHandCard"));
            //runner.addFunction("getPengs", new Pengs("getPengs"));
            //runner.addFunction("包含数量", new Count("包含数量"));


            // qlExpress自带
            runner.addOperatorWithAlias("如果", "if", null);
            runner.addOperatorWithAlias("则", "then", null);
            runner.addOperatorWithAlias("否则", "else", null);

            runner.addOperatorWithAlias("属于", "in", "用户$1不在允许的范围");
            runner.addOperatorWithAlias("小于", "<", "$1 < $2 不符合");
            runner.addOperatorWithAlias("大于", ">", "$1 > $2 不符合");
            runner.addOperatorWithAlias("等于", "==", "$1 == $2 不符合");
            runner.addOperatorWithAlias("不等于", "!=", "$1 == $2 不符合");
            runner.addOperatorWithAlias("大于等于", ">=", "$1 == $2 不符合");
            runner.addOperatorWithAlias("并且", "and", "用户$1不在允许的范围");
            runner.addOperatorWithAlias("或者", "or", "用户$1不在允许的范围");

        } catch (Exception e) {
            e.printStackTrace();
        }

    }

    public static Object execute(String express, DefaultContext<String, Object> context) throws Exception {
        Object r = runner.execute(express, context, null, true, false);
        for (String s : context.keySet()) {
            if (express.contains(s)) {
                express.replaceAll(s, context.get(s).toString());
            }
        }
        LogUtil.getLogger().debug("计算表达式：{}={}", express, r);
        LogUtil.getLogger().debug("DefaultContext：{}", context);
        return r;
    }


}
