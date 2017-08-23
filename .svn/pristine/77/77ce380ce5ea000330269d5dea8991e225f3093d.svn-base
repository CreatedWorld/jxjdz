package avatar.module.express.function;

import avatar.module.express.NameMapping;
import avatar.module.mahjong.Combo;
import avatar.module.mahjong.HuPaiAlgorithm;
import avatar.module.mahjong.Mahjong;
import com.ql.util.express.Operator;

import java.util.List;

@SuppressWarnings("unchecked")
public class HuPaiComBo extends Operator {
    public HuPaiComBo() {
        this.name = NameMapping.COMBO_COUNT.getName();
    }

    /**
     * 组成胡牌组合(手牌)
     */
    @Override
    public Object executeInner(Object[] params) throws Exception {
        List<Integer> integers = (List<Integer>) params[0];
        List<Mahjong> mahjongs = Mahjong.parseFromCodes(integers);
        List<Combo> combos = HuPaiAlgorithm.genCombo(mahjongs);
        return combos;
    }
}