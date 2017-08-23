package avatar.module.express.function;

import avatar.module.express.NameMapping;
import avatar.module.mahjong.Mahjong;
import com.ql.util.express.Operator;

import java.util.List;

@SuppressWarnings("unchecked")
public class SameColor extends Operator {
    public SameColor() {
        this.name = NameMapping.SAME_COLOR.getName();
    }

    @Override
    public Object executeInner(Object[] params) throws Exception {
        List<Integer> integers = (List<Integer>) params[0];
        List<Mahjong> mahjongs = Mahjong.parseFromCodes(integers);
        int firstType = -1;
        for (int i = 0; i < mahjongs.size(); i++) {
            Mahjong mahjong = mahjongs.get(i);
            if (i == 0) {
                firstType = mahjong.getType();
            } else {
                if (firstType != mahjong.getType()) {
                    return false;
                }
            }
        }

        return true;
    }
}