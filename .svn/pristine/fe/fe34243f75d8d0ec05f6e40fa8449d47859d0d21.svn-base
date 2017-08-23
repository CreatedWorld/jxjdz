package avatar.module.express.function;

import avatar.module.express.NameMapping;
import avatar.module.mahjong.Combo;
import avatar.module.mahjong.Mahjong;
import com.ql.util.express.Operator;

import java.util.ArrayList;
import java.util.List;

@SuppressWarnings("unchecked")
public class SpecifiedColorComboCount extends Operator {
    public SpecifiedColorComboCount() {
        this.name = NameMapping.COMBO_COUNT.getName();
    }

    /**
     * 指定花色的组合数量(胡牌组合,'AA/AAA/ABC',['万'])
     * 胡牌组合AA/AAA/ABC花色万、索花色数量
     */
    @Override
    public Object executeInner(Object[] params) throws Exception {
        List<Combo> combos = (List<Combo>) params[0];
        Combo.Type type = Combo.Type.valueOf((String) params[1]);
        Character[] typeNames = (Character[]) params[2];

        //指定组合的花色集合
        List<Integer> comboTypes = new ArrayList<>(Mahjong.Type.values().length);
        for (Combo combo : combos) {
           if(combo.getType() == type){
               comboTypes.add(combo.getMahjongs().get(0).getType());
            }
        }

        //指定花色转换成麻将花色id
        List<Integer> types = new ArrayList<>(typeNames.length);
        for (Character typeName : typeNames) {
            types.add(FunctionUtil.parseTypeNameToTypeInt(typeName));
        }

        //指定组合的花色集合中含有指定花色数量
        int count = 0;
        for (Integer comboType : comboTypes){
            if (types.contains(comboType)){
                count++;
            }
        }
        return count;
    }
}