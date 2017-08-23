package avatar.entity.noticeconfig;

import avatar.util.BaseEntity;
import avatar.util.utilDB.annotation.Column;
import avatar.util.utilDB.annotation.Pk;
import avatar.util.utilDB.annotation.Table;
import org.springframework.stereotype.Service;

/**
 * 大厅公告、喇叭、以及客服的配置类
 */
@Service
@Table(name = "config_notice", comment = "大厅公告、喇叭、以及客服的配置表")
public class NoticeConfig extends BaseEntity{
    public NoticeConfig() {
        super(NoticeConfig.class);
    }

    @Pk
    @Column(name = "id", comment = "id")
    private int id;

    @Column(name = "type", comment = "类型：1为公告；2为喇叭；3为客服")
    private int type;

    @Column(name = "content", comment = "内容")
    private String content;

    @Column(name = "title", comment = "标题")
    private String title;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public int getType() {
        return type;
    }

    public void setType(int type) {
        this.type = type;
    }

    public String getContent() {
        return content;
    }

    public void setContent(String content) {
        this.content = content;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }
}
