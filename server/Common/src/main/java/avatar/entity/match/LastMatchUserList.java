package avatar.entity.match;

import java.util.ArrayList;
import java.util.List;

/**
 * 记录玩家上一次匹配过的
 */
public class LastMatchUserList {
    private List<Integer> userIds = new ArrayList<Integer>();//匹配过的玩家的队列。

    private void resetList(){
        userIds.clear();
    }

    public LastMatchUserList(int myId , List<UserMatchData> matchDataList){
        resetList();
        List<Integer> newList = new ArrayList<>();
        for(UserMatchData data : matchDataList){
            if(data.getUserId() == myId){
                continue;
            }
            newList.add(data.getUserId());
        }
        this.userIds.addAll(newList);
    }


    public boolean hasMatched(int id){
        if(userIds.size() > 0){
            return userIds.indexOf(id) > -1;
        }
        return false;
    }
}
