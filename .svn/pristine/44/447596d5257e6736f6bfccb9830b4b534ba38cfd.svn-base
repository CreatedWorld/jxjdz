package avatar.module.express.function;

import avatar.module.express.NameMapping;
import avatar.module.mahjong.Mahjong;
import com.ql.util.express.Operator;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

public class ContainSpecifiedType extends Operator{
    public ContainSpecifiedType() {
        this.name = NameMapping.CONTAIN_FENG.getName();
    }

    /**
     * 表达式：含有指定花色牌的数量(手牌,['万'])
     * 找出手牌中的风牌数量  例如含有万牌的数量：含有指定花色牌的数量(手牌,['万']).size()
     */
    @Override
    public Object executeInner(Object[] list) throws Exception {
        //手牌
        List<Integer> handPai = (List<Integer>)list[0];
        Character[] typeNames = (Character[]) list[1];

        //转换成麻将类型id
        List<Integer> types = new ArrayList<>(typeNames.length);
        for (Character typeName : typeNames) {
            types.add(FunctionUtil.parseTypeNameToTypeInt(typeName));
        }

        //返回匹配的麻将集合
        List<Mahjong> result = new ArrayList<>(14);
        //手牌分组
        Map<Integer, List<Mahjong>> mahjongsInType = Mahjong.groupByType(Mahjong.parseFromCodes(handPai));
        for (List<Mahjong> mahjongs : mahjongsInType.values()) {
            for (Mahjong mahjong : mahjongs){
                if (types.contains(mahjong.getType())){
                    result.add(mahjong);
                } else {
                    break;
                }
            }
        }
        return result;
    }
}
