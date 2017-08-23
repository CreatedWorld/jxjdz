package avatar.module.express.function;

import avatar.module.express.NameMapping;
import avatar.module.mahjong.Combo;
import avatar.module.mahjong.Mahjong;
import com.ql.util.express.Operator;

import java.util.ArrayList;
import java.util.List;

@SuppressWarnings("unchecked")
public class AppointColorComboTypeCombo extends Operator {
    public AppointColorComboTypeCombo() {
        this.name = NameMapping.APPOINT_COLOR_COMBOTYPE_COMBO.getName();
    }

    /**
     * 指定花色和组合类型的组合(胡牌组合,'AA')
     */
    @Override
    public Object executeInner(Object[] params) throws Exception {
        List<Combo> combos = (List<Combo>) params[0];
        List<Mahjong.Type> types = (( List<Mahjong.Type>) params[1]);
        List<Combo.Type> comboTypes = (( List<Combo.Type>) params[2]);

        List<Combo> temp = new ArrayList<>();
        for (Combo.Type comboType : comboTypes) {
            for (Combo combo : combos) {
                if (combo.getType() == comboType) {
                    temp.add(combo);
                }
            }
        }

        List<Combo> reult = new ArrayList<>();
        for (Mahjong.Type type : types) {
            for (Combo combo : temp) {
                if (combo.getMahjongs().get(0).getType() == type.getId() ){
                    reult.add(combo);
                }
            }
        }

        return reult;
    }
}