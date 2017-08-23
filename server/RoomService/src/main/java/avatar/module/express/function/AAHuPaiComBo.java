package avatar.module.express.function;

import avatar.module.express.NameMapping;
import avatar.module.mahjong.Combo;
import avatar.module.mahjong.HuPaiAlgorithm;
import avatar.module.mahjong.Mahjong;
import com.ql.util.express.Operator;

import java.util.List;

@SuppressWarnings("unchecked")
public class AAHuPaiComBo extends Operator {
    public AAHuPaiComBo() {
        this.name = NameMapping.COMBO_COUNT.getName();
    }

    /**
     * 组成AA胡牌组合(手牌)(主要用于七对胡)
     */
    @Override
    public Object executeInner(Object[] params) throws Exception {
        List<Integer> integers = (List<Integer>) params[0];
        List<Mahjong> mahjongs = Mahjong.parseFromCodes(integers);
        List<Combo> combos = HuPaiAlgorithm.genAACombos(mahjongs);
        return combos;
    }
}