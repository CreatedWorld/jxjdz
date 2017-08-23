package avatar.module.express.operator;

import com.ql.util.express.Operator;

public class Merge extends Operator {
    public Merge(String aName) {
        this.name = aName;
    }

    @Override
    public Object executeInner(Object[] params) throws Exception {
        Object left = params[0];
        Object right = params[1];

        return new Object[]{left, right};
    }
}