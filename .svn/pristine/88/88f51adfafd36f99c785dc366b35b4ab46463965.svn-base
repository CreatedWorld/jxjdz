package avatar.module.mahjong.replay;

import avatar.entity.battlerecord.UserBattleScoreEntity;
import avatar.entity.room.Room;
import avatar.entity.room.RoomPlayer;
import avatar.entity.userinfo.UserEntity;
import avatar.module.battleRecord.dao.UserBattleScoreDao;
import avatar.module.mahjong.Combo;
import avatar.module.mahjong.Mahjong;
import avatar.module.mahjong.MahjongGameData;
import avatar.module.mahjong.PersonalMahjongInfo;
import avatar.module.room.service.RoomDataService;
import avatar.module.user.UserDao;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class JoinInfo {
    private String roomCode;

    private String innings;

    private String curInnings;

    private String isStart;

    private String createId;

    private String leftCardCount;

    private Map<String, PlayInfo> playInfoArr;

    private Map<String, String> playerTipAct;

    public JoinInfo(MahjongGameData mahjongGameData) {
        Room room = RoomDataService.getInstance().getRoomByRoomId(mahjongGameData.getRoomId());
        this.roomCode = room.getRoomCode();
        this.innings = mahjongGameData.getTotalTimes() + "";
        this.curInnings = mahjongGameData.getCurrentTimes() + "";
        this.isStart = "true";
        this.leftCardCount = mahjongGameData.getLeftCards().size() + "";
        this.createId = room.getCreateUserId() + "";
        this.playInfoArr = new HashMap<>();

        List<RoomPlayer> roomPlayers = RoomDataService.getInstance().loadRoomPlayerListByState(
                mahjongGameData.getRoomId(),
                RoomPlayer.State.READY.getCode()
        );

        for (RoomPlayer roomPlayer : roomPlayers) {
            UserEntity user = UserDao.getInstance().loadUserEntityByUserId(roomPlayer.getPlayerId());

            // 上一局的分数
            UserBattleScoreEntity userScore = UserBattleScoreDao
                    .getInstance()
                    .getUserBattleScoreByRoomIdAndPlayerId(mahjongGameData.getRoomId(), roomPlayer.getPlayerId());

            PlayInfo playInfo = new PlayInfo();
            playInfo.setUserId(roomPlayer.getPlayerId() + "");
            playInfo.setSit(roomPlayer.getSeat() + "");
            playInfo.setName(user.getNickName());
            playInfo.setIsMaster((roomPlayer.getSeat() == 1) + "");
            playInfo.setHeadIcon(user.getImageUrl());
            playInfo.setSex(user.getSex() + "");
            playInfo.setIsReady((roomPlayer.getState() == RoomPlayer.State.READY.getCode()) + "");
            playInfo.setScore(userScore == null ? 0 + "" : userScore.getAllScore() + "");
            playInfo.setIsBanker((mahjongGameData.getBankerSite() == roomPlayer.getSeat()) + "");

            PersonalMahjongInfo myInfo = PersonalMahjongInfo.getMyInfo(
                    mahjongGameData.getPersonalMahjongInfos(),
                    user.getPlayerId()
            );
            playInfo.setHandCards(new HashMap<>());
            for (Mahjong mahjong : myInfo.getHandCards()) {
                playInfo.getHandCards().put(playInfo.getHandCards().size() + "", mahjong.getCode() + "");
            }
            if (myInfo.getTouchMahjong() != null) {
                playInfo.setGetCard(myInfo.getTouchMahjong().getCode() + "");
            }
            playInfo.setPengGangCards(new HashMap<>());
            for (Combo combo : myInfo.getPengs()) {
                playInfo.getPengGangCards().put(
                        playInfo.getPengGangCards().size() + "",
                        combo.getMahjongs().get(0).getCode() + ""
                );
            }
            for (Combo combo : myInfo.getGangs()) {
                playInfo.getPengGangCards().put(
                        playInfo.getPengGangCards().size() + "",
                        combo.getMahjongs().get(0).getCode() + ""
                );
            }
            playInfo.setPutCards(new HashMap<>());

            this.playInfoArr.put(roomPlayer.getSeat() - 1 + "", playInfo);

        }


        this.playerTipAct = new HashMap<>();
    }

    public String getRoomCode() {
        return roomCode;
    }

    public String getInnings() {
        return innings;
    }

    public String getCurInnings() {
        return curInnings;
    }

    public String getIsStart() {
        return isStart;
    }

    public String getCreateId() {
        return createId;
    }

    public Map<String, PlayInfo> getPlayInfoArr() {
        return playInfoArr;
    }

    public Map<String, String> getPlayerTipAct() {
        return playerTipAct;
    }

    public String getLeftCardCount() {
        return leftCardCount;
    }

}
