package avatar.module.express.operator;

import com.ql.util.express.Operator;

import java.util.List;

@SuppressWarnings("unchecked")
public class ListContain extends Operator {
    public ListContain(String aName) {
        this.name = aName;
    }

    @Override
    public Object executeInner(Object[] params) throws Exception {
        List<List<Object>> list = (List<List<Object>>) params[0];
        Object expectObj = params[1];
        for (List<Object> objects : list) {
            for (Object object : objects) {
                if (object.equals(expectObj)) {
                    return true;
                }
            }

        }
        return false;
    }
}