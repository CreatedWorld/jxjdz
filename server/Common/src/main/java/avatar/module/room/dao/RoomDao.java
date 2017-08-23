package avatar.module.room.dao;

import avatar.entity.room.Room;
import avatar.util.GameData;

import java.util.List;

public class RoomDao {

    private static final RoomDao instance = new RoomDao();

    private RoomDao() {
    }

    public static final RoomDao getInstance() {
        return instance;
    }

    private final String key = "room_%d";
    private String getKey(int roomId){
        return String.format(key , roomId);
    }

    public Room loadRoomByRoomId(int roomId) {
        Room room = loadCache(roomId);
        if (room != null) {
            return room;
        }
        room = loadDB(roomId);
        if (room != null) {
            setCache(roomId, room);
        }
        return room;
    }

    public Room loadRoomByRoomCode(String roomCode){
        //cache
        return loadDBByRoomCode(roomCode);
    }

    public boolean update(Room room) {
        if (room == null) {
            return false;
        }
        removeCache(room.getId());
        boolean ret = updateDB(room);
        return ret;
    }

    public boolean deleteBatchRoom(List<Room> deleteList){
        if(deleteList == null || deleteList.size() == 0){
            return false;
        }
        for(Room room : deleteList){
            if(room != null) {
                removeCache(room.getId());
            }
        }
        return deleteRoomDB(deleteList);
    }

    /**
     * 返回刚刚插入的房间的id
     */
    public int addAndReturnId(Room room) {
        boolean ret = insertDB(room);
        if(ret){
            Room room1 = loadLastIdDB();
            return room1.getId();
        }
        return 0;
    }

    public List<String> loadAddRoomCode(){
        return loadAllRoomCodeDB();
    }

    public List<Integer> loadAllRoomId(){
        return loadAllRoomIdDB();
    }

    private Room loadCache(int roomId) {
        //roomId  -- > room
        return (Room)GameData.getCache().get(getKey(roomId));
    }

    private void setCache(int roomId, Room room) {
        GameData.getCache().set(getKey(roomId) , room);
    }

    private void removeCache(int roomId) {
        GameData.getCache().removeCache(getKey(roomId));
    }

    private Room loadDB(int roomId) {
        try {
            return GameData.getDB().get(Room.class, roomId);
        }catch (Exception e){
            e.printStackTrace();
        }
        return null;
    }

    private Room loadLastIdDB(){
        return GameData.getDB().get(Room.class , "select last_insert_id() as id;" , new Object[]{});
    }

    private Room loadDBByRoomCode(String roomCode){
        return GameData.getDB().get(Room.class , "select * from Room where roomCode = ?;" ,
                new Object[]{roomCode});
    }

    private boolean updateDB(Room room) {
        boolean ret = GameData.getDB().update(room);
        if (!ret) {
            //log
        }
        return ret;
    }

    private boolean insertDB(Room room) {
        boolean ret = GameData.getDB().insert(room);
        if (!ret) {
            //log
        }
        return ret;
    }

    private boolean deleteRoomDB(List<Room> rooms){
        return GameData.getDB().delete(rooms);
    }

    private List<String> loadAllRoomCodeDB(){
        List<String> list = GameData.getDB().listString(
                "select roomCode from room" , new Object[]{} );
        return list;
    }

    private List<Integer> loadAllRoomIdDB(){
        try {
            List<Integer> list = GameData.getDB().listInteger("select id from room;" , new Object[]{});
            return list;
        }catch (Exception e){
            e.printStackTrace();
        }
        return null;
    }
}
