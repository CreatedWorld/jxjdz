package avatar.module.mahjong;


import avatar.entity.room.Room;
import avatar.entity.room.RoomPlayType;
import avatar.module.mahjong.operate.ActionType;
import avatar.module.mahjong.operate.CanDoOperate;
import avatar.module.mahjong.operate.Operate;
import avatar.module.room.service.RoomDataService;
import org.apache.commons.collections.CollectionUtils;

import java.util.List;

/**
 * 出现一炮多响时，记录每个玩家选择过或胡的情况
 */
public class MultipleChiHu {
    private CanDoOperate canDoOperate;

    private Select select;

    /**
     * 是否只有一个人选择吃胡
     */
    public static boolean onlyOneChiHu(List<MultipleChiHu> multipleChiHus) {
        if (CollectionUtils.isEmpty(multipleChiHus)) {
            return false;
        }

        int count = 0;
        for (MultipleChiHu chiHus : multipleChiHus) {
            if (chiHus.getSelect() == Select.CHI_HU) {
                count++;
            }
        }
        return count == 1;
    }

    /**
     * 是否含有一炮多响
     *
     * @param canDoOperates scan扫描出来的canDoOperates，不要做remove，直接传进来
     */
    public static boolean hasMultipleChiHu(List<CanDoOperate> canDoOperates) {
        if (CollectionUtils.isEmpty(canDoOperates)) {
            return false;
        }

        Room room = RoomDataService.getInstance().getInRoomByUserId(canDoOperates.get(0).getUserId());
        if (room == null) {
            return false;
        }

        if (room.getPlayTypeList().contains(RoomPlayType.NO_MULTIPLE_CHI_HU.getId())) {
            return false;
        }

        int count = 0;
        for (CanDoOperate canDoOperate : canDoOperates) {
            List<Operate> operates = canDoOperate.getOperates();
            for (Operate operate : operates) {
                if (operate.getActionType() == ActionType.CHI_HU) {
                    count++;
                }
            }
        }
        return count > 1;
    }

    // 记录用户的选择
    public static void select(List<MultipleChiHu> multipleChiHus, int userId, Select select) {
        for (MultipleChiHu multipleChiHu : multipleChiHus) {
            if (multipleChiHu.getCanDoOperate().getUserId() == userId) {
                multipleChiHu.setSelect(select);
                break;
            }
        }
    }

    /**
     * 判断是否全部人都进行了选择了吃胡或过
     */
    public static boolean isAllSelected(List<MultipleChiHu> multipleChiHus) {
        if (multipleChiHus == null) {
            return false;
        }
        return multipleChiHus.stream().noneMatch(multipleChiHu -> multipleChiHu.getSelect() == null);
    }

    /**
     * 判断是否全部人都进行了选择了吃胡或过
     */
    public static boolean hasSelectChiHu(List<MultipleChiHu> multipleChiHus) {
        return multipleChiHus.stream().anyMatch(multipleChiHu -> multipleChiHu.getSelect() == Select.CHI_HU);
    }

    public MultipleChiHu(CanDoOperate canDoOperate) {
        this.canDoOperate = canDoOperate;
    }

    public enum Select {
        PASS(),
        CHI_HU();

        Select() {
        }
    }

    public CanDoOperate getCanDoOperate() {
        return canDoOperate;
    }

    public void setCanDoOperate(CanDoOperate canDoOperate) {
        this.canDoOperate = canDoOperate;
    }

    public Select getSelect() {
        return select;
    }

    public void setSelect(Select select) {
        this.select = select;
    }
}
