package avatar.module.express.function;

import avatar.module.express.NameMapping;
import avatar.module.mahjong.Combo;
import com.ql.util.express.Operator;

import java.util.List;

@SuppressWarnings("unchecked")
public class ComboCount extends Operator {
    public ComboCount() {
        this.name = NameMapping.COMBO_COUNT.getName();
    }

    /**
     * 组合个数(胡牌组合,'AA')
     */
    @Override
    public Object executeInner(Object[] params) throws Exception {
        List<Combo> combos = (List<Combo>) params[0];
        Combo.Type type = Combo.Type.valueOf((String) params[1]);

        int count = 0;
        for (Combo combo : combos) {
            if (combo.getType() == type) {
                count++;
            }
        }
        return count;
    }
}