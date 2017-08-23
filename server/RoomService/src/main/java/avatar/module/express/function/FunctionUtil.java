package avatar.module.express.function;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * Created by Administrator on 2017/6/16.
 */
public class FunctionUtil {
    /**
     * 例如手牌是[11,12,13,21,22,32,51]，经过此该当后变成{1:[11,12,13],2:[21,22],3:[32],5:[51]}
     * @return
     */
    public static Map<Integer, List<Integer>> groupByType(List<Integer>  handCards){
        Map<Integer, List<Integer>>  map = new HashMap();
        for (Integer handCard : handCards) {
            Integer ten = handCard/10;
            List<Integer> values = map.get(ten);
            if(values == null){
                values = new ArrayList<>();
                map.put(ten,values);
            }
            values.add(handCard);
        }
        return map;
    }

    /**
     * 根据牌型名称转换成牌类型数字
     * @return
     */
    public static Integer parseTypeNameToTypeInt(Character typeName){
        Integer type = 0;
        switch (typeName){
            case  '万':type = 1;break;
            case  '筒':type = 2;break;
            case  '索':type = 3;break;
            case  '风':type = 4;break;
            case  '箭':type = 5;break;
            case  '花':type = 6;break;
            default: break;
        }
        return type;
    }
}
