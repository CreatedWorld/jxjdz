package avatar.entity.shop;

import avatar.util.BaseEntity;
import avatar.util.utilDB.annotation.Column;
import avatar.util.utilDB.annotation.Pk;
import avatar.util.utilDB.annotation.Table;
import org.springframework.stereotype.Service;

/**
 * 代理（玩家可以成为代理）
 */
@Service
@Table(name = "admin", comment = "玩家与代理的绑定关系")
public class Admin extends BaseEntity {
    public Admin() {
        super(Admin.class);
    }

    @Pk
    @Column(name = "admin_id", comment = "admin_id、即代理id，也是玩家id")
    private int adminId;

    @Column(name = "admin_name", comment = "管理员名称")
    private String adminName;

    @Column(name = "admin_password", comment = "登录密码；md5加密")
    private String adminPassword;


    @Column(name = "status", comment = "用户状态 0：禁用； 1：正常 ；2：未验证")
    private int state;

    @Column(name = "last_login_ip", comment = "最后登录ip")
    private String lastLoginIp;

    @Column(name = "last_login_time", comment = "最后登录时间")
    private long lastLoginTime;

    @Column(name = "avatar")
    private String avatar;

    @Column(name = "email", comment = "管理员邮箱")
    private String email;

    @Column(name = "email_code")
    private String emailCode;

    @Column(name = "phone", comment = "管理员联系方式")
    private String phone;

    @Column(name = "register_time", comment = "注册时间")
    private long registerTime;

    @Column(name = "level", comment = "管理员等级 0 总代理 1 一级代理 2 二级代理")
    private int level;

    @Column(name = "pid", comment = "上级id")
    private int pid;

    @Column(name = "sup_amount", comment = "直属充值总数")
    private double supAmount;

    @Column(name = "amount", comment = "下级充值总数")
    private double amount;

    @Column(name = "rebate", comment = "已提现抽成")
    private double rebate;

    public enum State {
        DISABLE(1, "禁用"),
        ENABLE(2, "正常"),
        NOT_VALIDATED(3, "未验证");

        private int id;

        private String desc;

        State(int id, String desc) {
            this.id = id;
            this.desc = desc;
        }

        public int getId() {
            return id;
        }

        public String getDesc() {
            return desc;
        }
    }

    public int getState() {
        return state;
    }

    public void setState(int state) {
        this.state = state;
    }

    public int getAdminId() {
        return adminId;
    }

    public void setAdminId(int adminId) {
        this.adminId = adminId;
    }

    public String getAdminName() {
        return adminName;
    }

    public void setAdminName(String adminName) {
        this.adminName = adminName;
    }

    public String getAdminPassword() {
        return adminPassword;
    }

    public void setAdminPassword(String adminPassword) {
        this.adminPassword = adminPassword;
    }


    public String getLastLoginIp() {
        return lastLoginIp;
    }

    public void setLastLoginIp(String lastLoginIp) {
        this.lastLoginIp = lastLoginIp;
    }

    public long getLastLoginTime() {
        return lastLoginTime;
    }

    public void setLastLoginTime(long lastLoginTime) {
        this.lastLoginTime = lastLoginTime;
    }

    public String getAvatar() {
        return avatar;
    }

    public void setAvatar(String avatar) {
        this.avatar = avatar;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getEmailCode() {
        return emailCode;
    }

    public void setEmailCode(String emailCode) {
        this.emailCode = emailCode;
    }

    public String getPhone() {
        return phone;
    }

    public void setPhone(String phone) {
        this.phone = phone;
    }

    public long getRegisterTime() {
        return registerTime;
    }

    public void setRegisterTime(long registerTime) {
        this.registerTime = registerTime;
    }

    public int getLevel() {
        return level;
    }

    public void setLevel(int level) {
        this.level = level;
    }

    public int getPid() {
        return pid;
    }

    public void setPid(int pid) {
        this.pid = pid;
    }

    public double getSupAmount() {
        return supAmount;
    }

    public void setSupAmount(double supAmount) {
        this.supAmount = supAmount;
    }

    public double getAmount() {
        return amount;
    }

    public void setAmount(double amount) {
        this.amount = amount;
    }

    public double getRebate() {
        return rebate;
    }

    public void setRebate(double rebate) {
        this.rebate = rebate;
    }
}
