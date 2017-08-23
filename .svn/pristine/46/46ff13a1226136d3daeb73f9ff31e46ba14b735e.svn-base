package avatar.module.express.operator;

import avatar.module.express.NameMapping;
import com.ql.util.express.Operator;

import java.util.List;

@SuppressWarnings("unchecked")
public class OneOfThemIs extends Operator {
    public OneOfThemIs() {
        this.name = NameMapping.ONE_OF_THEM_IS.getName();
    }

    /**
     * 一个列表中其中一只牌
     * 是不是在第二个列表中出现
     */
    @Override
    public Object executeInner(Object[] params) throws Exception {
        List<Integer> specialList = (List<Integer>) params[0];
        List<Integer> baseList = (List<Integer>) params[1];
        for (Integer integer : specialList) {
            if (baseList.contains(integer)) {
                return true;
            }
        }
        return false;
    }
}