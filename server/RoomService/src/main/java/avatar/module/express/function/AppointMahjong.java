package avatar.module.express.function;

import avatar.module.express.NameMapping;
import com.ql.util.express.Operator;

import java.util.List;

@SuppressWarnings("unchecked")
public class AppointMahjong extends Operator {
    public AppointMahjong() {
        this.name = NameMapping.APPOINT_MAHJONG.getName();
    }

    /**
     * 全部是指定牌(手牌,[11,19,21,29,31,39,41,42,43,44,51,52,53])
     */
    @Override
    public Object executeInner(Object[] params) throws Exception {
        List<Integer> integers = (List<Integer>) params[0];
        Integer[] specials = (Integer[]) params[1];

        for (Integer mahjongCode : specials) {
            if (!integers.contains(mahjongCode)) {
                return false;
            }
        }
        return true;
    }
}