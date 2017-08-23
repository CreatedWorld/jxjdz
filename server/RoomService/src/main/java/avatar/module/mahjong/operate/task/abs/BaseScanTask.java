package avatar.module.mahjong.operate.task.abs;

import avatar.module.express.NameMapping;
import avatar.module.express.QLExpressUtil;
import avatar.module.express.Rule;
import avatar.module.mahjong.*;
import avatar.module.mahjong.operate.ActionType;
import avatar.module.mahjong.operate.CanDoOperate;
import avatar.module.mahjong.operate.Operate;
import avatar.module.mahjong.operate.picker.PersonalMahjongInfoPicker;
import avatar.module.mahjong.operate.scanner.AbstractScanner;
import avatar.module.mahjong.operate.scanner.GetACardScanner;
import avatar.util.LogUtil;
import avatar.util.TreasureCardUtil;
import com.ql.util.express.DefaultContext;
import org.apache.commons.collections.CollectionUtils;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;

@SuppressWarnings("unchecked")
public abstract class BaseScanTask {

    protected static final Logger log = LoggerFactory.getLogger(BaseScanTask.class);

    /**
     * 用户打出或摸到的牌
     */
    protected Mahjong specifiedMahjong;

    /**
     * 出牌的玩家
     */
    protected int specifiedUserId;

    /**
     * 麻将游戏数据
     */
    protected MahjongGameData mahjongGameData;

    /**
     * 需要对这些玩家的手牌进行扫描
     */
    protected List<PersonalMahjongInfo> toBeScanPersonalMahjongInfos;

    /**
     * 具体的任务扫描器判定到某个用户可以执行某些操作时，向此列表添加元素，同一次的scanner下的任务共用此集合
     */
    private List<CanDoOperate> canOperates;

    /**
     * 提取器，从mahjongGameData中提取出需要扫描的玩家手牌
     */
    private PersonalMahjongInfoPicker personalMahjongInfoPicker;

    /**
     * 任务的规则
     */
    protected List<Rule> rules;

    /**
     * 执行这个扫描任务的请求cmd
     */
    protected ActionType actionType;

    /**
     * 可以操作的扫描器
     */
    private Class<? extends AbstractScanner> scanner;

    protected abstract void setActionType();

    protected abstract void setRules();

    public boolean doScan(PersonalMahjongInfo personalCardInfo) throws Exception {
        List<Mahjong> handCards = new ArrayList<>(personalCardInfo.getHandCards());

        List<List<Mahjong>> circulateResult = null;
        if (ActionType.isHu(this.actionType)) {
            List<Mahjong> myBaoMahjongs;
            if (scanner == GetACardScanner.class) {
                List<Mahjong> temp = new ArrayList<>(handCards);
                temp.add(specifiedMahjong);
                myBaoMahjongs = TreasureCardUtil.getMyBaoMahjongs(
                        temp,
                        mahjongGameData.getTreasureCard()
                );
            } else {
                myBaoMahjongs = TreasureCardUtil.getMyBaoMahjongs(
                        handCards,
                        mahjongGameData.getTreasureCard()
                );
            }
            if (!myBaoMahjongs.isEmpty()) {
                // todo 优化算法，尽量过滤掉没必要生成的麻将
                circulateResult = TreasureCardUtil.circulate(
                        myBaoMahjongs.size(),
                        mahjongGameData.getTreasureCard(),
                        mahjongGameData.getMakeUpMahjongs()
                );
                LogUtil.getLogger().info(circulateResult.toString());
            }

            handCards.add(specifiedMahjong);
        }

        DefaultContext<String, Object> context = new DefaultContext<>();

        /*如果没有宝牌，就正常遍历规则表达式进行扫描
          如果有宝牌，就生成宝牌能变成的牌，再遍历规则表达式进行扫描
         */
        if (circulateResult == null) {
            for (Rule rule : rules) {
                if (excludeRule(rule)) {
                    continue;
                }

                if (scanExpress(personalCardInfo, handCards, rule, context)) {
                    return true;
                }
            }
        } else {
            for (List<Mahjong> madeMahjongs : circulateResult) {
                for (Mahjong madeMahjong : madeMahjongs) {
                    handCards.remove(mahjongGameData.getTreasureCard());
                    handCards.add(madeMahjong);
                }
                Collections.sort(handCards);

                for (Rule rule : rules) {
                    if (excludeRule(rule)) {
                        continue;
                    }

                    if (scanExpress(personalCardInfo, handCards, rule, context)) {
                        return true;
                    }
                }

                for (Mahjong madeMahjong : madeMahjongs) {
                    handCards.remove(madeMahjong);
                    handCards.add(mahjongGameData.getTreasureCard());
                }
            }
        }

        return false;
    }

    /**
     * 是否需要过滤掉此规则，不用扫描
     * 表达式配置了excludeHuActions="自摸胡 吃胡 抢暗杠胡 抢直杠胡 抢碰杠胡"，就需要过滤
     */
    private boolean excludeRule(Rule rule) {
        return !CollectionUtils.isEmpty(rule.getExcludeHuActions())
                && ActionType.isHu(this.actionType)
                && rule.getExcludeHuActions().contains(this.actionType.getName());
    }

    private boolean scanExpress(PersonalMahjongInfo personalCardInfo,
                                List<Mahjong> handCards,
                                Rule rule,
                                DefaultContext<String, Object> context) throws Exception {
        // 手牌
        context.put(NameMapping.HAND_MAHJONGS.toString(), Mahjong.parseToCodes(handCards));
        // 摸到的牌
        context.put(NameMapping.TOUCH_MAHJONG.toString(), specifiedMahjong.getCode());
        // 打出的牌
        context.put(NameMapping.PUT_OUT_MAHJONG.toString(), specifiedMahjong.getCode());
        // 碰了的牌
        context.put(
                NameMapping.PENGED_GROUP.toString(),
                Combo.parseToCodes(personalCardInfo.getPengs())
        );
        // 杠了的牌
        context.put(
                NameMapping.GANGED_GROUP.toString(),
                Combo.parseToCodes(personalCardInfo.getGangs())
        );
        // 吃了的牌
        context.put(
                NameMapping.CHIED_GROUP.toString(),
                Combo.parseToCodes(personalCardInfo.getChi())
        );
        // 个人全部牌
        List<Integer> personalAllMahjongs = new ArrayList<>(MahjongConfig.HAND_CARD_NUMBER + 1);
        personalAllMahjongs.addAll(Mahjong.parseToCodes(handCards));
        for (Combo combo : personalCardInfo.getPengs()) {
            personalAllMahjongs.addAll(Mahjong.parseToCodes(combo.getMahjongs()));
        }
        for (Combo combo : personalCardInfo.getGangs()) {
            personalAllMahjongs.addAll(Mahjong.parseToCodes(combo.getMahjongs()));
        }
        for (Combo combo : personalCardInfo.getChi()) {
            personalAllMahjongs.addAll(Mahjong.parseToCodes(combo.getMahjongs()));
        }
        context.put(NameMapping.PERSONAL_ALL_MAHJONG.toString(), personalAllMahjongs);

        boolean matchExpress = (Boolean) QLExpressUtil.execute(rule.getExpress(), context);
        if (matchExpress) {
            log.trace(
                    "用户id={}可以[{}]",
                    personalCardInfo.getUserId(),
                    rule.getName()
            );

            List<Combo> combos = parseScanResultToCombos(context);

            // 添加可行操作
            List<Operate> myOperates = getMyOperates(personalCardInfo.getUserId());
            Operate operate = new Operate();
            operate.setActionType(this.actionType);
            operate.setRule(rule);
            operate.setCombos(combos);
            myOperates.add(operate);
        }
        return matchExpress;
    }

    /**
     * 解析扫描出来的结果，组装为combos
     */
    private List<Combo> parseScanResultToCombos(DefaultContext<String, Object> context) {
        List<Combo> combos = null;
        switch (this.actionType) {
            case PLAY_A_MAHJONG:
                combos = new ArrayList<>(1);
                Combo playAMahjong = new Combo();
                playAMahjong.setMahjongs(Arrays.asList(this.specifiedMahjong));
                combos.add(playAMahjong);
                break;
            case GET_A_MAHJONG:
                combos = new ArrayList<>(1);
                Combo getAMahjong = new Combo();
                getAMahjong.setMahjongs(Arrays.asList(this.specifiedMahjong));
                combos.add(getAMahjong);
                break;
            case PASS:
                combos = new ArrayList<>(1);
                Combo pass = new Combo();
                pass.setMahjongs(Arrays.asList(this.specifiedMahjong));
                combos.add(pass);
                break;
            case PENG:
                combos = new ArrayList<>(1);
                combos.add(Combo.newAAA(specifiedMahjong));
                break;
            case COMMON_AN_GANG:
                combos = new ArrayList<>(1);
                combos.add(Combo.newAAAA(specifiedMahjong));
                break;
            case BACK_AN_GANG:
                List<Integer> backAnGangMahjongCodes = (List<Integer>) context.get("麻将列表");
                combos = new ArrayList<>(backAnGangMahjongCodes.size());
                for (Integer mahjongCode : backAnGangMahjongCodes) {
                    combos.add(Combo.newAAAA(Mahjong.parseFromCode(mahjongCode)));
                }
                break;
            case ZHI_GANG:
                combos = new ArrayList<>(1);
                combos.add(Combo.newAAAA(specifiedMahjong));
                break;
            case COMMON_PENG_GANG:
                combos = new ArrayList<>(1);
                combos.add(Combo.newAAAA(specifiedMahjong));
                break;
            case BACK_PENG_GANG:
                List<Integer> backPengGangMahjongCodes = (List<Integer>) context.get("麻将列表");
                combos = new ArrayList<>(backPengGangMahjongCodes.size());
                for (Integer mahjongCode : backPengGangMahjongCodes) {
                    combos.add(Combo.newAAAA(Mahjong.parseFromCode(mahjongCode)));
                }
                break;
            case ZI_MO:
                combos = (List<Combo>) context.get("胡牌组合");
                break;
            case QIANG_AN_GANG_HU:
                combos = (List<Combo>) context.get("胡牌组合");
                break;
            case QIANG_ZHI_GANG_HU:
                combos = (List<Combo>) context.get("胡牌组合");
                break;
            case QIANG_PENG_GANG_HU:
                combos = (List<Combo>) context.get("胡牌组合");
                break;
            case CHI_HU:
                combos = (List<Combo>) context.get("胡牌组合");
                break;
            case CHI:
                combos = new ArrayList<>(1);
                combos.add(Combo.newAAA(this.specifiedMahjong));
                break;

        }
        if (combos == null) {
            throw new UnsupportedOperationException(
                    String.format("未实现ActionType=%s 的parseScanResultToCombos转换的功能，请添加该转换功能！", actionType)
            );
        }

        return combos;
    }

    public void scan() throws Exception {
        setActionType();
        setRules();

        toBeScanPersonalMahjongInfos = personalMahjongInfoPicker.pick(mahjongGameData, specifiedUserId);

        for (PersonalMahjongInfo toBeScanPersonalCardInfo : toBeScanPersonalMahjongInfos) {
            log.trace(
                    "对用户id={}进行[{}]扫描，手牌：{}",
                    toBeScanPersonalCardInfo.getUserId(),
                    this.actionType.getName(),
                    toBeScanPersonalCardInfo.getHandCards()
            );
            doScan(toBeScanPersonalCardInfo);
        }
    }

    /**
     * 获取本玩家的可行的操作列表
     */
    protected List<Operate> getMyOperates(int userId) {
        // 找出userId的可行的操作列表
        CanDoOperate canDoOperate = null;
        for (CanDoOperate canOperate : this.canOperates) {
            if (canOperate.getUserId() == userId) {
                canDoOperate = canOperate;
                break;
            }
        }
        if (canDoOperate == null) {
            canDoOperate = new CanDoOperate();
            canDoOperate.setUserId(userId);
            canDoOperate.setOperates(new ArrayList<>());
            canDoOperate.setSpecialMahjong(this.specifiedMahjong);
            canDoOperate.setSpecialUserId(this.specifiedUserId);
            canDoOperate.setScanner(this.scanner);
            canOperates.add(canDoOperate);
        }
        return canDoOperate.getOperates();
    }

    public Mahjong getSpecifiedMahjong() {
        return specifiedMahjong;
    }

    public void setSpecifiedMahjong(Mahjong specifiedMahjong) {
        this.specifiedMahjong = specifiedMahjong;
    }

    public int getSpecifiedUserId() {
        return specifiedUserId;
    }

    public void setSpecifiedUserId(int specifiedUserId) {
        this.specifiedUserId = specifiedUserId;
    }

    public MahjongGameData getMahjongGameData() {
        return mahjongGameData;
    }

    public void setMahjongGameData(MahjongGameData mahjongGameData) {
        this.mahjongGameData = mahjongGameData;
    }

    public List<PersonalMahjongInfo> getToBeScanPersonalMahjongInfos() {
        return toBeScanPersonalMahjongInfos;
    }

    public void setToBeScanPersonalMahjongInfos(List<PersonalMahjongInfo> toBeScanPersonalMahjongInfos) {
        this.toBeScanPersonalMahjongInfos = toBeScanPersonalMahjongInfos;
    }

    public List<CanDoOperate> getCanOperates() {
        return canOperates;
    }

    public void setCanOperates(List<CanDoOperate> canOperates) {
        this.canOperates = canOperates;
    }

    public PersonalMahjongInfoPicker getPersonalMahjongInfoPicker() {
        return personalMahjongInfoPicker;
    }

    public void setPersonalMahjongInfoPicker(PersonalMahjongInfoPicker personalMahjongInfoPicker) {
        this.personalMahjongInfoPicker = personalMahjongInfoPicker;
    }

    public Class<? extends AbstractScanner> getScanner() {
        return scanner;
    }

    public void setScanner(Class<? extends AbstractScanner> scanner) {
        this.scanner = scanner;
    }
}
