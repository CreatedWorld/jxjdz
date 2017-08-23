package avatar.module.express.function;

import avatar.module.express.NameMapping;
import avatar.module.mahjong.Mahjong;
import com.ql.util.express.Operator;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

@SuppressWarnings("unchecked")
public class ContainOrdinal extends Operator {
    public ContainOrdinal() {
        this.name = NameMapping.CONTAIN_ORDINAL.getName();
    }

    /**
     * 含有序数(手牌,(1,2,3,4,5,6,7,8,9))
     */
    @Override
    public Object executeInner(Object[] params) throws Exception {
        List<Integer> integers = (List<Integer>) params[0];
        Integer[] specials = (Integer[]) params[1];

        Map<Integer, List<Mahjong>> mahjongsInType = Mahjong.groupByType(Mahjong.parseFromCodes(integers));
        for (List<Mahjong> mahjongs : mahjongsInType.values()) {
            if (mahjongs.size() < specials.length) {
                continue;
            }

            List<Integer> ordinals = new ArrayList<>();
            for (Mahjong mahjong : mahjongs) {
                ordinals.add(mahjong.getmOrdinal());
            }

            for (Integer special : specials) {
                if (!ordinals.contains(special)) {
                    return false;
                }
            }
            return true;
        }

        return false;
    }
}