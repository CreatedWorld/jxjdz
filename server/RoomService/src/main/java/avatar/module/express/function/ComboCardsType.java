package avatar.module.express.function;

import avatar.module.express.NameMapping;
import avatar.module.mahjong.Mahjong;
import com.ql.util.express.Operator;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

/**
 * author: wangzhiwen
 * date: 2017/6/15 11:56
 * desc:吃碰杠了的牌中含有花色(吃了的牌,['风','箭'])
 */
public class ComboCardsType extends Operator {
    public ComboCardsType() {
        this.name = NameMapping.COMBO_CARDS_TYPE.getName();
    }

    @Override
    public Object executeInner(Object[] params) throws Exception {
        List<List<Integer>> combos = (List<List<Integer>>)params[0];
        Character[] typeNames = (Character[]) params[1];

        //转换成麻将类型id
        List<Integer> types = new ArrayList<>(typeNames.length);
        for (Character typeName : typeNames) {
            types.add(FunctionUtil.parseTypeNameToTypeInt(typeName));
        }

        //未找到指定类型的麻将
        if(types.size() == 0){
            return false;
        }

        //combos:[[11,11,11],[12,12,12],[13,13,13]]，计算每一个组合的type
        Set<Integer> comboTypes = new HashSet<>(Mahjong.Type.values().length);
        List<Mahjong> mahjongs = null;
        for (List<Integer> combo : combos){
            mahjongs = Mahjong.parseFromCodes(combo);
            comboTypes.add(mahjongs.get(0).getType());
        }

        //如果组合中含有指定花色，返回
        for (Integer type : comboTypes){
            for (Integer specifiedType : types){
                if (type == specifiedType){
                    return true;
                }
            }
        }

        return false;
    }
}