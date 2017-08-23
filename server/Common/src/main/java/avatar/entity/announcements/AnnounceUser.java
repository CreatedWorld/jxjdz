package avatar.entity.announcements;

import avatar.util.BaseEntity;
import avatar.util.utilDB.annotation.Column;
import avatar.util.utilDB.annotation.Pk;
import avatar.util.utilDB.annotation.Table;
import org.springframework.stereotype.Service;

/**
 * 记录用户已经发送过的公告
 */
@Service
@Table(name = "AnnounceUser" , comment = "记录用户已经发送过的公告")
public class AnnounceUser extends BaseEntity{
    public AnnounceUser() {
        super(AnnounceUser.class);
    }

    @Pk(auto = true)
    @Column(name = "id" , comment = "主键id")
    private int id;

    @Column(name = "userId" , comment = "用户id")
    private int userId;

    @Column(name = "announceId" , comment = "对应已经发送过的公告表id")
    private int announceId;

    public int getUserId(){
        return userId;
    }

    public int getAnnounceId(){
        return announceId;
    }

    public void setUserId(int userId){
        this.userId = userId;
    }

    public void setAnnounceId(int announceId){
        this.announceId = announceId;
    }
}
