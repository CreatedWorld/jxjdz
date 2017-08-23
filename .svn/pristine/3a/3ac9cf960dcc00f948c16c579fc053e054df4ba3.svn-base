package avatar.module.express.operator;

import com.ql.util.express.Operator;

import java.util.List;

@SuppressWarnings("unchecked")
public class Contain extends Operator {
    public Contain(String aName) {
        this.name = aName;
    }

    @Override
    public Object executeInner(Object[] params) throws Exception {
        List<Object> list = (List<Object>) params[0];
        Object obj = params[1];
        for (Object o : list) {
            if (o.equals(obj)) {
                return true;
            }
        }
        return false;
    }
}