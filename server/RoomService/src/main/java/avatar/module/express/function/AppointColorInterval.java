package avatar.module.express.function;

import avatar.module.express.NameMapping;
import com.ql.util.express.Operator;

import java.util.*;

/**
 * Created by Administrator on 2017/6/16.
 */
public class AppointColorInterval extends Operator {

    public AppointColorInterval() {
        this.name = NameMapping.APPOINT_COLOR_INTERVAL.getName();
    }

    /**
     * 指定花色不靠
     * @param :参数一：牌，参数二：需要不靠的牌型，参数三：相隔几个牌
     */
    @Override
    public Object executeInner(Object[] params) throws Exception {
        List<Integer> handCards = (List<Integer>) params[0];
        Character[] typeNames = (Character[]) params[1];
        Integer interval = (Integer)params[2];
        if(typeNames.length == 0){
            return false;
        }

        Map<Integer, List<Integer>> mahjongsInType = FunctionUtil.groupByType(handCards);// {1:[11,12,13],2:[21,22],3:[32],5:[51]}
        List<Integer> types = new ArrayList<>(typeNames.length);
        for (Character typeName : typeNames) {
            types.add(FunctionUtil.parseTypeNameToTypeInt(typeName));
        }

        for (Integer integer : mahjongsInType.keySet()) {
            if (types.contains(integer)){
                List<Integer> integers = mahjongsInType.get(integer);
                Integer[] cards = integers.toArray(new Integer[0]);
                Arrays.sort(cards);
                for (int i= 0;i < cards.length - 1; i++){
                    if ((cards[i] >= (cards[i+1] - interval))){
                        return false;
                    }
                }
            }
        }
        return true;
    }
}
