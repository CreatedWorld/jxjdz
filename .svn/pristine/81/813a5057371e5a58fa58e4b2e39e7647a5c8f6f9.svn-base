package avatar.module.mahjong.dao;

import avatar.module.mahjong.MahjongGameData;

import java.util.HashMap;
import java.util.Map;

public class MahjongGameDataDao {
    private static final Map<String, MahjongGameData> MAHJONG_GAME_DATA_MAP = new HashMap<>();

    private static final MahjongGameDataDao instance = new MahjongGameDataDao();

    public static final MahjongGameDataDao getInstance() {
        return instance;
    }

    public Map<String, MahjongGameData> getMahjongGameDataMap(){
        return MAHJONG_GAME_DATA_MAP;
    }

    public void save(MahjongGameData mahjongGameData) {
        MAHJONG_GAME_DATA_MAP.put(String.valueOf(mahjongGameData.getRoomId()), mahjongGameData);
    }

    public MahjongGameData get(int roomId) {
        return MAHJONG_GAME_DATA_MAP.get(String.valueOf(roomId));
    }


    public void remove(int roomId){
        String key = String.valueOf(roomId);
        if(MAHJONG_GAME_DATA_MAP.containsKey(key)){
            MAHJONG_GAME_DATA_MAP.remove(key);
        }
    }

}
