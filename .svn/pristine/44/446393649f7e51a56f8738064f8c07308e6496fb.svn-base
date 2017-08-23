package avatar.module.mahjong.offline;

import java.util.HashSet;
import java.util.Set;

/**
 * 离线用户管理
 */
public class OfflineManager {

    private static Set<Integer> offlineUserId = new HashSet<>(100);

    public static void addOfflineUser(int userId) {
        offlineUserId.add(userId);
    }

    public static void removeOfflineUser(int userId) {
        offlineUserId.remove(userId);
    }

    public static boolean isOffline(int userId) {
        return offlineUserId.contains(userId);
    }
}
