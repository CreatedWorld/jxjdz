package avatar.util;

import avatar.module.mahjong.Combo;
import avatar.module.mahjong.Mahjong;
import avatar.module.mahjong.MahjongConfig;
import avatar.module.mahjong.operate.ActionType;
import avatar.module.mahjong.operate.CanDoOperate;
import avatar.module.mahjong.operate.Operate;
import avatar.protobuf.Battle;
import org.apache.commons.collections.CollectionUtils;

import java.util.ArrayList;
import java.util.List;

/**
 * 后端到前端的数据交换vo类转换工具
 */
public class OperateUtil {

    /**
     * 将一个 canDoOperate 转换为 PushPlayerActTipS2C
     */
    public static Battle.PushPlayerActTipS2C parseCanDoOperateToPushPlayerActTipS2C(CanDoOperate canDoOperate) {
        List<Battle.PlayerActType> playerActTypes = new ArrayList<>(canDoOperate.getOperates().size());
        List<Integer> actCards = new ArrayList<>(canDoOperate.getOperates().size());

        for (Operate operate : canDoOperate.getOperates()) {
            if (CollectionUtils.isEmpty(operate.getCombos())) {
                playerActTypes.add(ActionTypeMapping.parse(operate.getActionType()));
                // 如果操作牌为空，则默认给一只无关的牌
                actCards.add(Mahjong.ONE_WANG.getCode());
                continue;
            }
            // 如果是胡牌，则只传一个combo
            // 暂时不考虑听牌。将胡的操作的combo 传给actCards时，只传一张拍牌
            if (operate.getActionType() == ActionType.ZI_MO
                    || operate.getActionType() == ActionType.QIANG_PENG_GANG_HU
                    || operate.getActionType() == ActionType.QIANG_AN_GANG_HU
                    || operate.getActionType() == ActionType.QIANG_ZHI_GANG_HU
                    || operate.getActionType() == ActionType.CHI_HU) {
                playerActTypes.add(ActionTypeMapping.parse(operate.getActionType()));
                actCards.add(operate.getCombos().get(0).getMahjongs().get(0).getCode());
                continue;
            }

            for (Combo combo : operate.getCombos()) {
                playerActTypes.add(ActionTypeMapping.parse(operate.getActionType()));
                actCards.add(combo.getMahjongs().get(0).getCode());
            }
        }

        return Battle.PushPlayerActTipS2C.newBuilder()
                .setOptUserId(canDoOperate.getUserId())
                .addAllActs(playerActTypes)
                .addAllActCards(actCards)
                .setTipRemainTime(MahjongConfig.TIP_REMAIN_TIME)
                .setTipRemainUT(System.currentTimeMillis())
                .setTargetUserId(canDoOperate.getSpecialUserId())
                .build();
    }

    /**
     * 将玩家的操作封装成PushPlayerActS2C对象
     */
    public static Battle.PushPlayerActS2C parsePlayerActionToPushPlayerActS2C(int userId, ActionType actionType, int actMahjongCode) {
        return parsePlayerActionToPushPlayerActS2C(userId, 0, actionType, actMahjongCode);
    }

    /**
     * 将玩家的操作封装成PushPlayerActS2C对象
     */
    public static Battle.PushPlayerActS2C parsePlayerActionToPushPlayerActS2C(int userId, int targetUserId, ActionType actionType, int actMahjongCode) {
        return Battle.PushPlayerActS2C.newBuilder()
                .setUserId(userId)
                .setAct(ActionTypeMapping.parse(actionType))
                .setActCard(actMahjongCode)
                .setTargetUserId(targetUserId)
                .build();
    }

}
