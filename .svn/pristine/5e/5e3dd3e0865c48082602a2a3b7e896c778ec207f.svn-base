package avatar.module.room.dao;

import avatar.entity.room.RoomPlayer;
import avatar.util.GameData;

import java.util.ArrayList;
import java.util.List;

public class RoomPlayerDao {

    private static final RoomPlayerDao instance = new RoomPlayerDao();

    private RoomPlayerDao() {
    }

    private final String key = "roomPlayer_%d";

    private String getKey(int playerId) {
        return String.format(key, playerId);
    }

    public static final RoomPlayerDao getInstance() {
        return instance;
    }

    public boolean deleteRoomPlayer(RoomPlayer roomPlayer) {
        if (roomPlayer == null) {
            return false;
        }

        removeCache(roomPlayer.getId());
        List<RoomPlayer> roomPlayers = new ArrayList<>(1);
        roomPlayers.add(roomPlayer);
        return deleteRoomPlayerDB(roomPlayers);
    }

    public RoomPlayer loadRoomPlayerByPlayerId(int playerId) {
        RoomPlayer roomPlayer = loadCache(playerId);
        if (roomPlayer != null) {
            return roomPlayer;
        }
        roomPlayer = loadDB(playerId);
        if (roomPlayer != null) {
            setCache(playerId, roomPlayer);
        }
        return roomPlayer;
    }

    public List<RoomPlayer> loadRoomPlayersByRoomId(int roomId) {
        //loadCache

        List<RoomPlayer> list = loadListDB(roomId);
        //set cache
        return list;
    }

    public boolean update(RoomPlayer roomPlayer) {
        if (roomPlayer == null) {
            return false;
        }
        removeCache(roomPlayer.getId());
        boolean ret = updateDB(roomPlayer);
        return ret;
    }

    public boolean addBatch(List<RoomPlayer> roomPlayers) {
        return insertBatch(roomPlayers);
    }


    public boolean add(RoomPlayer roomPlayer) {
        boolean ret = insert(roomPlayer);
        return ret;
    }

    //删除所有在同一个房间的roomPlayer
    public boolean deleteAllRoomPlayerInRoom(List<RoomPlayer> roomPlayers) {
        if (roomPlayers == null || roomPlayers.size() == 0) {
            return false;
        }
        for (RoomPlayer roomPlayer : roomPlayers) {
            removeCache(roomPlayer.getPlayerId());
        }
        return deleteRoomPlayerDB(roomPlayers);
    }


    //================================================================

    private RoomPlayer loadCache(int roomPlayerId) {
//        return (RoomPlayer) GameData.getCache().get(getKey(roomPlayerId));
        return null;
    }

    private void setCache(int playerId, RoomPlayer roomPlayer) {
//        GameData.getCache().set(getKey(playerId) , roomPlayer);
    }

    private void removeCache(int roomPlayerId) {
//        GameData.getCache().removeCache(getKey(roomPlayerId));
    }

    private RoomPlayer loadDB(int roomPlayerId) {
        String sql = "select * from roomPlayer where playerId = ?";
        RoomPlayer roomPlayer = GameData.getDB().get(RoomPlayer.class, sql, new Object[]{roomPlayerId});
        return roomPlayer;
    }

    private boolean updateDB(RoomPlayer roomPlayer) {
        boolean ret = GameData.getDB().update(roomPlayer);
        if (!ret) {
            //log
        }
        return ret;
    }

    private boolean insert(RoomPlayer roomPlayer) {
        boolean ret = GameData.getDB().insert(roomPlayer);
        if (!ret) {
            //log
        }
        return ret;
    }

    private boolean insertBatch(List<RoomPlayer> roomPlayers) {
        return GameData.getDB().insert(roomPlayers);
    }

    private List<RoomPlayer> loadListDB(int roomId) {
        String sql = "select * from RoomPlayer where roomId = ?;";
        List<RoomPlayer> roomPlayers = GameData.getDB().list(RoomPlayer.class, sql, new Object[]{roomId});
        return roomPlayers;
    }

    private boolean deleteRoomPlayerDB(List<RoomPlayer> roomPlayers) {
        return GameData.getDB().delete(roomPlayers);
    }
}
