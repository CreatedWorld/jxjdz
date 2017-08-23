package avatar.module.express.function;

import avatar.module.express.NameMapping;
import com.ql.util.express.Operator;

import java.util.List;

@SuppressWarnings("unchecked")
public class Count extends Operator {
    public Count() {
        this.name = NameMapping.COUNT.getName();
    }

    @Override
    public Object executeInner(Object[] list) throws Exception {
        Integer integer = (Integer) list[0];
        List<Integer> mahjongNumbers = (List<Integer>) list[1];

        int count = 0;
        for (Integer mahjongNumber : mahjongNumbers) {
            if (mahjongNumber.equals(integer)) {
                count++;
            }
        }

        return count;
    }
}