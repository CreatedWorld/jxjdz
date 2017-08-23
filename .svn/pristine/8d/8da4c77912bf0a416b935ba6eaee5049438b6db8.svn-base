package avatar.module.express.function;

import avatar.module.express.NameMapping;
import com.ql.util.express.Operator;

import java.util.*;

public class AppointColor extends Operator {
    public AppointColor() {
        this.name = NameMapping.APPOINT_COLOR.getName();
    }


    /**
     * 表达式：全部是指定花色（手牌，牌的类型）
     */
    @Override
    public Object executeInner(Object[] params) throws Exception {
        //手牌
        List<Integer> handCards = (List<Integer>) params[0];
        Character[] typeNames = (Character[]) params[1];
        if(typeNames.length == 0){
            return false;
        }

        Map<Integer, List<Integer>> mahjongsInType = FunctionUtil.groupByType(handCards);// {1:[11,12,13],2:[21,22],3:[32],5:[51]}
        if (mahjongsInType.size() > typeNames.length) {
            return false;
        }

        List<Integer> types = new ArrayList<>(typeNames.length);
        for (Character typeName : typeNames) {
            types.add(FunctionUtil.parseTypeNameToTypeInt(typeName));
        }

        for (Integer typeId : mahjongsInType.keySet()) {
            if(!types.contains(typeId)){
                return  false;
            }
        }
        return true;
    }
}
