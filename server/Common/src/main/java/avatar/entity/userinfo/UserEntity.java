package avatar.entity.userinfo;

import avatar.util.BaseEntity;
import avatar.util.utilDB.annotation.Column;
import avatar.util.utilDB.annotation.Pk;
import avatar.util.utilDB.annotation.Table;
import org.springframework.stereotype.Service;

import java.util.Date;

@Service
@Table(name="player" , comment = "用户表")
public class UserEntity extends BaseEntity{
    public UserEntity() {
        super(UserEntity.class);
    }

    @Pk
    @Column(name = "id" , comment = "玩家id" )
    private int playerId;
    @Column(name = "nickName" , comment = "玩家昵称")
    private String nickName;
    @Column(name = "mac" , comment = "每个玩家对应一个mac地址")
    private String mac;
    @Column(name = "loginTime" , comment = "玩家登录时间")
    private Date loginTime;
    @Column(name ="openId" , comment = "第三方sdk标志的唯一id")
    private String openId;
    @Column(name = "showId" , comment = "游戏内需要显示的Id")
    private String showId;
    @Column(name = "ip" , comment = "每个玩家的ip地址")
    private String ip;
    @Column(name = "psw" , comment = "密码")
    private String psw;

    @Column(name = "imageUrl" ,comment = "玩家头像地址")
    private String imageUrl;

    @Column(name = "sex" , comment = "性别")
    private int sex;//1男 0女

    @Column(name = "phone" , comment = "电话号码")
    private long phone;

    @Column(name = "hallLocalName" ,comment = "记录每次玩家最近一次登录了哪一个大厅" )
    private String hallLocalName;

    public int getPlayerId(){
        return playerId;
    }

    public String getNickName(){
        return nickName;
    }

    public String getMac(){
        return mac;
    }

    public void setNickName(String nickName){
        this.nickName = nickName;
    }

    public void setMac(String mac){
        this.mac = mac;
    }

    public Date getLoginTime(){
        return loginTime;
    }

    public void setLoginTime(){
        this.loginTime = new Date();
    }

    public String getOpenId(){
        return openId;
    }

    public String getShowId(){
        return showId;
    }

    public String getIp(){
        return ip;
    }

    public void setOpenId(String openId){
        this.openId = openId;
    }

    public void setShowId(String showId){
        this.showId = showId;
    }

    public void setIp(String ip){
        this.ip = ip;
    }

    public String getPsw(){
        return psw;
    }

    public void setPsw(String psw){
        this.psw = psw;
    }

    public String getImageUrl(){
        return imageUrl;
    }

    public void setImageUrl(String url){
        this.imageUrl = url;
    }

    public int getSex(){
        return sex;
    }


    public void setSex(int sex){
        this.sex = sex;
    }

    public long getPhone(){
        return phone;
    }

    public void setPhone(long phone){
        this.phone = phone;
    }

    public String getHallLocalName(){
        return hallLocalName;
    }

    public void setHallLocalName(String hallLocalName){
        this.hallLocalName = hallLocalName;
    }


    public String toString(){
        return "id:"+this.playerId+",mac:"+mac;
    }
}
