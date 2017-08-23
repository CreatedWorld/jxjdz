package avatar.module.express.function;

import avatar.module.express.NameMapping;
import com.ql.util.express.Operator;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@SuppressWarnings("unchecked")
public class SameMahjong extends Operator {
    public SameMahjong() {
        this.name = NameMapping.SAME_MAHJONG.getName();
    }

    /**
     * 含有相同麻将(手牌, 4)
     */
    @Override
    public Object executeInner(Object[] params) throws Exception {
        List<Integer> mahjongCodes = (List<Integer>) params[0];
        Integer count = (Integer) params[1];
        List<Integer> sameMahjongs = new ArrayList<>(2);

        Map<Integer, Integer> counts = new HashMap<>(mahjongCodes.size());
        for (Integer integer : mahjongCodes) {
            int c = counts.getOrDefault(integer, 0);
            c++;
            counts.put(integer, c);
        }

        for (Map.Entry<Integer, Integer> entry : counts.entrySet()) {
            if (entry.getValue() >= count) {
                sameMahjongs.add(entry.getKey());
            }
        }

        return sameMahjongs;
    }
}