package avatar.module.shop;

import avatar.entity.shop.Admin;
import avatar.util.GameData;

import java.util.Optional;

/**
 * 大厅配置接口（包括公告、喇叭、客服）
 */
public class AdminDao {
    private AdminDao() {
    }

    private static final AdminDao instance = new AdminDao();

    public static final AdminDao getInstance() {
        return instance;
    }

    //=========接口=========//

    /**
     * 根据用户id获取用户的代理
     */
    public Admin loadAdmin(int adminId) {
        return Optional
                .ofNullable(getAdminCache(adminId))
                .orElseGet(() -> {
                    Admin admin = getAdminDB(adminId);
                    if (admin != null) {
                        setAdminCache(adminId, admin);
                    }
                    return admin;
                });
    }


    //=========cache=========//
    private Admin getAdminCache(int adminId) {
        return null;
    }

    private void setAdminCache(int adminId, Admin admin) {

    }

    //=========DB=========//
    //查询

    /**
     * 根据用户id获取用户的代理，如果数据库中player存在多个代理，则默认取第一个
     */
    private Admin getAdminDB(int adminId) {
        return GameData.getDB().get(Admin.class, adminId);
    }

}
