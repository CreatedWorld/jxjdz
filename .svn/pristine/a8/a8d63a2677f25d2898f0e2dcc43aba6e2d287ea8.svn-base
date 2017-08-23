package avatar.module.express.operator;

import avatar.module.express.NameMapping;
import com.ql.util.express.Operator;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

@SuppressWarnings("unchecked")
public class OneOfThemBelong extends Operator {
    public OneOfThemBelong() {
        this.name = NameMapping.ONE_OF_THEM_BELONG.getName();
    }

    /**
     * 一个列表中其中一只牌
     * 是不是在第二个参数中的其中一个列表中的列表中出现
     * 例如：手牌 其中一只属于 碰了的牌
     */
    @Override
    public Object executeInner(Object[] params) throws Exception {
        List<Integer> specialList = (List<Integer>) params[0];
        List<List<Integer>> baseList = (List<List<Integer>>) params[1];
        Set<Integer> result = new HashSet<>(2);

        for (Integer integer : specialList) {
            for (List<Integer> integers : baseList) {
                if (integers.contains(integer)) {
                    result.add(integer);
                }
            }
        }

        return new ArrayList<>(result);
    }
}