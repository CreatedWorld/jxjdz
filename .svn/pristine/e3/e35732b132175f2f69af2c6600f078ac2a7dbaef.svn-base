package avatar.module.room;

import avatar.entity.room.Room;
import avatar.entity.room.RoomPlayer;
import avatar.global.ClientCode;
import avatar.module.room.dao.RoomDao;
import avatar.module.room.dao.RoomPlayerDao;
import avatar.module.room.service.RoomDataService;
import avatar.protobuf.SysInner;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Iterator;
import java.util.List;

/**
 * 处理退出房间业务
 */
public class ExitRoomService {
    private static final Logger logger = LoggerFactory.getLogger(ExitRoomService.class);
    private static final ExitRoomService instance = new ExitRoomService();

    public static final ExitRoomService getInstance() {
        return instance;
    }

    /**
     * 退出房间<br></br>
     * 每个房间服务器的退出房间业务应该是独立线程中处理。所以这里是线程安全的、
     */
    public int exitRoom(int userId, SysInner.InnerExitCenter2Room.Builder builder) {
        Room room = RoomDataService.getInstance().getInRoomByUserId(userId);
        if (room == null) {
            return ClientCode.NOT_IN_ROOM;
        }
        RoomPlayer roomPlayer = null;
        List<RoomPlayer> roomPlayers = RoomDataService.getInstance().loadAllInRoom(room.getId());
        if (roomPlayers == null || roomPlayers.size() == 0) {
            return ClientCode.FAILED;
        }
        List<Integer> ids = new ArrayList<>();
        Iterator<RoomPlayer> it = roomPlayers.iterator();
        while (it.hasNext()) {
            RoomPlayer player = it.next();
            ids.add(player.getPlayerId());
            if (player.getPlayerId() == userId) {
                roomPlayer = player;
                it.remove();
            }
        }

        if (roomPlayer == null) {
            return ClientCode.FAILED;
        }

        boolean ret = RoomPlayerDao.getInstance().deleteRoomPlayer(roomPlayer);
        if (!ret) {
            return ClientCode.FAILED;
        }

        builder.addAllMemberIds(ids);
        //是否全部玩家都退出了房间
        if (roomPlayers.isEmpty()) {
            builder.setAllExit(true);
            builder.setRoomId(room.getId());
        } else {
            builder.setAllExit(false);
        }
        logger.info("[ExitRoomService]exitRoom , userId = " + userId + ", roomId = " + room.getId());
        return ClientCode.SUCCESS;
    }

    /**
     * 房主解散房间
     *
     * @param userId
     * @return
     */
    public int dissolveRoom(int userId, SysInner.InnerDissolveCenter2Room.Builder builder) {
        Room room = RoomDataService.getInstance().getInRoomByUserId(userId);
        if (room == null) {
            return ClientCode.NO_ROOM;
        }
        int roomId = room.getId();
        //不是创建房间者不能解散房间
        if (room.getCreateUserId() != userId) {
            return ClientCode.NOT_LIMIT_DISSOLVE_ROOM;
        }
        //退出了房间的玩家，不能解散
        List<RoomPlayer> list = RoomDataService.getInstance().loadAllInRoom(roomId);
        if (list == null || list.size() == 0) {
            return ClientCode.FAILED;
        }
        List<Integer> playerIds = new ArrayList<>();
        for (RoomPlayer player : list) {
            playerIds.add(player.getPlayerId());
            if (player.getPlayerId() != userId) {
                continue;
            }
        }
        boolean ret = RoomPlayerDao.getInstance().deleteAllRoomPlayerInRoom(list);
        if (ret) {
            List<Room> deleteRooms = Arrays.asList(room);
            ret = RoomDao.getInstance().deleteBatchRoom(deleteRooms);
        }
        if (!ret) {
            return ClientCode.FAILED;
        }
        builder.setRoomId(roomId);
        builder.addAllMemberIds(playerIds);
        builder.setIsStart(room.getState() == Room.State.STARTED.getCode());
        return ClientCode.SUCCESS;
    }
}
