package avatar.module.express.operator;

import avatar.module.express.NameMapping;
import com.ql.util.express.Operator;

import java.util.List;

@SuppressWarnings("unchecked")
public class BelongGroup extends Operator {
    public BelongGroup() {
        this.name = NameMapping.BELONG_GROUP.getName();
    }

    @Override
    public Object executeInner(Object[] params) throws Exception {
        Integer single = (Integer) params[0];
        List<List<Integer>> groups = (List<List<Integer>>) params[1];
        for (List<Integer> group : groups) {
            if (single.equals(group.get(0))) {
                return true;
            }
        }
        return false;
    }
}