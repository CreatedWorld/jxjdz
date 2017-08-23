package avatar.module.room.service;

import avatar.entity.room.Room;
import avatar.entity.room.RoomPlayer;
import avatar.global.ClientCode;
import avatar.module.room.dao.RoomDao;
import avatar.module.room.dao.RoomPlayerDao;

import java.util.ArrayList;
import java.util.List;

/**
 * 房间数据接口
 */
public class RoomDataService {

    private static final RoomDataService instance = new RoomDataService();

    public static final RoomDataService getInstance() {
        return instance;
    }

    /**
     * 通过玩家id查询玩家所在的房间（玩家状态是非退出状态）
     */
    public Room getInRoomByUserId(int userId) {
        RoomPlayer roomPlayer = RoomPlayerDao.getInstance().loadRoomPlayerByPlayerId(userId);
        if (roomPlayer == null) {
            return null;
        }
        int roomId = roomPlayer.getRoomId();
        Room room = RoomDao.getInstance().loadRoomByRoomId(roomId);
        return room;
    }

    public Room getRoomByRoomId(int roomId) {
        return RoomDao.getInstance().loadRoomByRoomId(roomId);
    }

    /**
     * 获取还在房间的用户id列表
     *
     * @param roomId
     * @return
     */
    public List<Integer> getRoomUserIds(int roomId) {
        Room room = RoomDao.getInstance().loadRoomByRoomId(roomId);
        if (room != null) {
            List<RoomPlayer> roomPlayers = RoomPlayerDao.getInstance().loadRoomPlayersByRoomId(roomId);
            if (roomPlayers == null || roomPlayers.size() == 0) {
                return null;
            }
            List<Integer> ret = new ArrayList<>();
            for (RoomPlayer roomPlayer : roomPlayers) {
                ret.add(roomPlayer.getPlayerId());
            }
            return ret;
        }
        return null;
    }

    /**
     * 获取房间内所有的roomPlayer
     *
     * @param roomId
     * @return
     */
    public List<RoomPlayer> loadAllInRoom(int roomId) {
        List<RoomPlayer> roomPlayers = RoomPlayerDao.getInstance().loadRoomPlayersByRoomId(roomId);
        return roomPlayers;
    }

    /**
     * 根据状态获取房间内所有相同状态的roomPlayer
     *
     * @param roomId 房间id
     * @param state  状态
     */
    public List<RoomPlayer> loadRoomPlayerListByState(int roomId, int state) {
        List<RoomPlayer> roomPlayers = RoomPlayerDao.getInstance().loadRoomPlayersByRoomId(roomId);
        if (roomPlayers == null || roomPlayers.size() == 0) {
            return null;
        }
        List<RoomPlayer> ret = new ArrayList<>();
        for (RoomPlayer roomPlayer : roomPlayers) {
            if (roomPlayer.getState() == state) {
                ret.add(roomPlayer);
            }
        }
        return ret;
    }


    /**
     * 开始新的一局游戏，更新房间信息
     *
     * @param roomId   房间id
     * @param newTimes 局数
     */
    public int setStartNewRound(int roomId, int newTimes) {
        Room room = RoomDao.getInstance().loadRoomByRoomId(roomId);
        if (room == null) {
            return ClientCode.FAILED;
        }
        if (room.getCurRounds() >= room.getRounds()) {
            return ClientCode.OVERFLOW_ROOM_ROUNDS;
        }
        room.setTime();
        room.setCurRounds(newTimes);
        boolean ret = RoomDao.getInstance().update(room);
        if (!ret) {
            return ClientCode.FAILED;
        }
        return ClientCode.SUCCESS;
    }


    /**
     * 统计房间内未退出房间的人数
     */
    public int count(int roomId) {
        List<Integer> list = getRoomUserIds(roomId);
        if (list == null) {
            return 0;
        }
        return list.size();
    }

    /**
     * 统计房间内状态为已准备的玩家人数
     */
    public int readyCount(int roomId) {
        List<RoomPlayer> roomPlayers = loadRoomPlayerListByState(roomId, RoomPlayer.State.READY.getCode());
        if (roomPlayers == null) {
            return 0;
        }
        return roomPlayers.size();
    }

    /**
     * 根据房间id，删除多个房间。并且删除roomplayer对象
     *
     * @param roomIds     房间id列表
     * @param deleteCount 删除个数（小于等于roomIds数量）
     * @param limitTime   如果有时间限制，那么删除范围为：当前时间 - 创建时间差 > limitTime
     */
    public void deleteBatchRoom(List<Integer> roomIds, int deleteCount, long limitTime) {
        if (roomIds == null || roomIds.size() == 0) {
            return;
        }
        //每次只做前面n个房间的清理操作
        int count = deleteCount;
        int size = roomIds.size();
        Room room;
        List<RoomPlayer> players;
        List<Room> deletes = new ArrayList<>();
        List<RoomPlayer> delRoomPlayers = new ArrayList<>();
        for (int i = 0; i < size; i++) {
            int roomId = roomIds.get(i);
            room = RoomDao.getInstance().loadRoomByRoomId(roomId);
            if (room != null) {
                if (System.currentTimeMillis() - room.getTime().getTime() > limitTime) {
                    deletes.add(room);
                    players = RoomDataService.getInstance().loadAllInRoom(roomId);
                    if (players != null && players.size() > 0) {
                        delRoomPlayers.addAll(players);
                    }
                    count--;
                }
            }
            if (count <= 0) {
                break;
            }
        }
        if (deletes.size() > 0) {
            RoomDao.getInstance().deleteBatchRoom(deletes);
        }
        if (delRoomPlayers.size() > 0) {
            RoomPlayerDao.getInstance().deleteAllRoomPlayerInRoom(delRoomPlayers);
        }
    }
}
