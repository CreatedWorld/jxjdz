package avatar.module.express.function;

import avatar.module.express.NameMapping;
import avatar.module.mahjong.Mahjong;
import com.ql.util.express.Operator;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

/**
 * desc:除指定花色外其它牌属同种花色(手牌,'风')
 * 例如除了风牌外，其它牌属于同种花色的牌
 * author:wangzhiwen
 * date:2017/6/13
 */

public class ExcludeSpecifiedSameColor extends Operator {
    public ExcludeSpecifiedSameColor() {
        this.name = NameMapping.EXCLUDE_SPECIFIED_MAHJONG_OTHER_SAME_COLOR.getName();
    }

    @Override
    public Object executeInner(Object[] params) throws Exception {
        //手牌
        List<Integer> integers = (List<Integer>) params[0];
        //给定类型
        Character[] specifiedMahjongTypes = (Character[]) params[1];
        //手牌分组
        Map<Integer, List<Mahjong>> mahjongsInType = Mahjong.groupByType(Mahjong.parseFromCodes(integers));
        //移除指定花色的麻将
        for(Mahjong.Type mahjong : Mahjong.Type.values()){
            for (Character specifiedMahjongType : specifiedMahjongTypes){
                if (mahjong.getName().equals(String.valueOf(specifiedMahjongType))){
                    mahjongsInType.remove(mahjong.getId());
                }
            }
        }
        //除指定类型花色外，其它牌属于同一种花色
        if(mahjongsInType.keySet().size() == 1){
            return true;
        }
        return false;
    }
}
