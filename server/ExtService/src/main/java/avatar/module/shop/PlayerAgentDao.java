package avatar.module.shop;

import avatar.entity.shop.PlayerAgent;
import avatar.util.GameData;

import java.util.List;
import java.util.Optional;

/**
 * 大厅配置接口（包括公告、喇叭、客服）
 */
public class PlayerAgentDao {
    private PlayerAgentDao() {
    }

    private static final PlayerAgentDao instance = new PlayerAgentDao();

    public static final PlayerAgentDao getInstance() {
        return instance;
    }

    //=========接口=========//

    /**
     * 根据用户id获取用户的代理
     */
    public PlayerAgent loadPlayerAgentByPlayerId(int playerId) {
        return Optional
                .ofNullable(getPlayerAgentByPlayerIdCache(playerId))
                .orElseGet(() -> {
                    PlayerAgent playerAgent = getPlayerAgentByPlayerIdDB(playerId);
                    if (playerAgent != null) {
                        setPlayerAgentCacheByPlayerId(playerId, playerAgent);
                    }
                    return playerAgent;
                });
    }


    public boolean insert(PlayerAgent playerAgent) {
        removeCacheByPlayerId(playerAgent.getPlayerId());
        return insertDB(playerAgent);
    }


    //=========cache=========//
    private PlayerAgent getPlayerAgentByPlayerIdCache(int playerId) {
        return null;
    }

    private void removeCacheByPlayerId(int playerId) {

    }

    private void setPlayerAgentCacheByPlayerId(int playerId, PlayerAgent playerAgent) {

    }

    //=========DB=========//
    //查询

    /**
     * 根据用户id获取用户的代理，如果数据库中player存在多个代理，则默认取第一个
     */
    private PlayerAgent getPlayerAgentByPlayerIdDB(int playerId) {
        List<PlayerAgent> list = GameData
                .getDB()
                .list(PlayerAgent.class, "select * from playeragent where playerId = ?", new Object[]{playerId});
        if (list == null || list.isEmpty()) {
            return null;
        }

        return list.get(0);
    }


    private boolean insertDB(PlayerAgent playerAgent) {
        boolean ret = GameData.getDB().insert(playerAgent);
        if (!ret) {
            //log
        }
        return ret;
    }

}
