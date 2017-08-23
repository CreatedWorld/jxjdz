package avatar.module.express.operator;

import com.ql.util.express.Operator;

import java.util.List;

@SuppressWarnings("unchecked")
public class ContainCount extends Operator {
    public ContainCount(String aName) {
        this.name = aName;
    }

    @Override
    public Object executeInner(Object[] params) throws Exception {
        Object[] objects = (Object[]) params[0];
        Integer expectCount = (Integer) params[1];

        Object expectObj = objects[0];
        List<Object> objList = (List<Object>) objects[1];

        int count = 0;
        for (Object o : objList) {
            if (o.equals(expectObj)) {
                count++;
            }
        }

        return count;
    }
}