package avatar.module.mahjong.replay;

import avatar.module.mahjong.MahjongGameData;
import avatar.module.room.service.RoomDataService;

import java.util.HashMap;
import java.util.Map;

/**
 * 一局游戏的回放
 */
public class Replay {

    private JoinInfo joinInfo;

    private String startTime;

    private Map<String, Action> actions;

    public Replay(MahjongGameData mahjongGameData) {
        this.joinInfo = new JoinInfo(mahjongGameData);
        this.startTime = RoomDataService.getInstance().getRoomByRoomId(mahjongGameData.getRoomId()).getTime().getTime() + "";
        this.actions = new HashMap<>();
    }

    public JoinInfo getJoinInfo() {
        return joinInfo;
    }

    public void setJoinInfo(JoinInfo joinInfo) {
        this.joinInfo = joinInfo;
    }

    public String getStartTime() {
        return startTime;
    }

    public void setStartTime(String startTime) {
        this.startTime = startTime;
    }

    public Map<String, Action> getActions() {
        return actions;
    }

    public void setActions(Map<String, Action> actions) {
        this.actions = actions;
    }

}
