package avatar.module.express;


import org.apache.commons.configuration2.XMLConfiguration;
import org.apache.commons.configuration2.builder.fluent.Configurations;
import org.apache.commons.configuration2.ex.ConfigurationException;
import org.apache.commons.lang3.StringUtils;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;

public class XmlExpress {
    /**
     * 胡牌的规则
     */
    private static List<Rule> hus;

    /**
     * 普通暗杠
     */
    private static List<Rule> commonAnGangs;

    /**
     * 回头暗杠
     */
    private static List<Rule> backAnGangs;

    /**
     * 直杠
     */
    private static List<Rule> zhiGangs;

    /**
     * 普通碰杠
     */
    private static List<Rule> commonPengGangs;

    /**
     * 回头碰杠
     */
    private static List<Rule> backPengGangs;

    /**
     * 碰
     */
    private static List<Rule> pengs;

    /**
     * 吃
     */
    private static List<Rule> chis;

    private static XMLConfiguration config;

    static {
        try {
            config = new Configurations().xml("express.xml");

            // 胡牌的规则
            hus = loadRules("hu");

            // 普通暗杠
            commonAnGangs = loadRules("commonAnGang");

            //回头暗杠
            backAnGangs = loadRules("backAnGang");

            // 直杠
            zhiGangs = loadRules("zhiGang");

            // 普通碰杠
            commonPengGangs = loadRules("commonPengGang");

            // 回头碰杠
            backPengGangs = loadRules("backPengGang");

            // 碰
            pengs = loadRules("peng");

            //吃
            chis = loadRules("chi");
        } catch (ConfigurationException e) {
            e.printStackTrace();
        }
    }

    private static List<Rule> loadRules(String ruleName) {
        List<Rule> rules = new ArrayList<>(config.getList(ruleName + ".express").size());
        for (int i = 0; i < config.getList(ruleName + ".express").size(); i++) {
            Rule rule = new Rule();
            rule.setName(config.getString(ruleName + ".express(" + i + ")[@name]"));
            rule.setScore(config.getDouble(ruleName + ".express(" + i + ")[@score]"));
            rule.setExpress(config.getString(ruleName + ".express(" + i + ")"));
            String excludeHuActionsStr = config.getString(ruleName + ".express(" + i + ")[@excludeHuActions]");
            if (StringUtils.isNotEmpty(excludeHuActionsStr)) {
                rule.setExcludeHuActions(Arrays.asList(excludeHuActionsStr.split(" ")));
            } else {
                rule.setExcludeHuActions(Collections.EMPTY_LIST);
            }
            rules.add(rule);
        }
        return rules;
    }

    public static List<Rule> getHus() {
        return hus;
    }

    public static List<Rule> getCommonAnGangs() {
        return commonAnGangs;
    }

    public static List<Rule> getBackAnGangs() {
        return backAnGangs;
    }

    public static List<Rule> getZhiGangs() {
        return zhiGangs;
    }

    public static List<Rule> getCommonPengGangs() {
        return commonPengGangs;
    }

    public static List<Rule> getBackPengGangs() {
        return backPengGangs;
    }

    public static List<Rule> getPengs() {
        return pengs;
    }

    public static List<Rule> getChis() {
        return chis;
    }

    public static void main(String[] args) {
        new XmlExpress();
    }
}
