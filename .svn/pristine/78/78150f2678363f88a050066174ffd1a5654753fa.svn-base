package avatar.module.express.function;

import avatar.module.express.NameMapping;
import com.ql.util.express.Operator;

import java.util.List;

@SuppressWarnings("unchecked")
public class ContainOneAppointMahjong extends Operator {
    public ContainOneAppointMahjong() {
        this.name = NameMapping.CONTAIN_ONE_APPOINT_MAHJONG.getName();
    }

    /**
     * 含有一个指定牌(手牌,[11,19,21,29,31,39])
     */
    @Override
    public Object executeInner(Object[] params) throws Exception {
        List<Integer> integers = (List<Integer>) params[0];
        Integer[] specials = (Integer[]) params[1];

        for (Integer mahjongCode : specials) {
            if (integers.contains(mahjongCode)) {
                return true;
            }
        }
        return false;
    }
}