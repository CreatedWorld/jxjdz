using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// 顶部按钮
/// </summary>
public class TopMenuView : UIView
{
    /// <summary>
    /// 头像
    /// </summary>
    private RawImage photo;
    /// <summary>
    /// 用户名
    /// </summary>
    private Text username;
    /// <summary>
    /// 房卡
    /// </summary>
    private Text roomCardText;
    /// <summary>
    /// 头像按钮
    /// </summary>
    private Button photoButton;
    /// <summary>
    /// 购买按钮
    /// </summary>
    private Button roomCardButton;
    /// <summary>
    /// 分享按钮
    /// </summary>
    private Button shareButton;
    /// <summary>
    /// 帮助按钮
    /// </summary>
    private Button helpButton;
    /// <summary>
    /// 战绩按钮
    /// </summary>
    private Button gradeButton;
    /// <summary>
    /// 设置按钮
    /// </summary>
    private Button settingButton;

    /// <summary>
    /// 头像
    /// </summary>
    public Button PhotoButton
    {
        get
        {
            return photoButton;
        }

        set
        {
            photoButton = value;
        }
    }

    /// <summary>
    /// 房卡
    /// </summary>
    public Button RoomCardButton
    {
        get
        {
            return roomCardButton;
        }

        set
        {
            roomCardButton = value;
        }
    }

    /// <summary>
    /// 分享按钮
    /// </summary>
    public Button ShareButton
    {
        get
        {
            return shareButton;
        }

        set
        {
            shareButton = value;
        }
    }

    /// <summary>
    /// 帮助按钮
    /// </summary>
    public Button HelpButton
    {
        get
        {
            return helpButton;
        }

        set
        {
            helpButton = value;
        }
    }

    /// <summary>
    /// 战绩按钮
    /// </summary>
    public Button GradeButton
    {
        get
        {
            return gradeButton;
        }
        set
        {
            gradeButton = value;
        }
    }

    /// <summary>
    /// 设置按钮
    /// </summary>
    public Button SettingButton
    {
        get
        {
            return settingButton;
        }
        set
        {
            settingButton = value;
        }
    }

    /// <summary>
    /// 头像图片
    /// </summary>
    public RawImage Photo
    {
        get
        {
            return photo;
        }

        set
        {
            photo = value;
        }
    }

    /// <summary>
    /// 用户名
    /// </summary>
    public Text Username
    {
        get
        {
            return username;
        }

        set
        {
            username = value;
        }
    }

    /// <summary>
    /// 房卡
    /// </summary>
    public Text RoomCardText
    {
        get
        {
            return roomCardText;
        }

        set
        {
            roomCardText = value;
        }
    }



    public override void OnInit()
    {
        this.ViewRoot = this.LaunchUIView("Prefab/UI/Hall/TopMenuView", UIManager.Instance.GetUIView(UIViewID.HALL_VIEW).ViewRoot.transform);
        this.Photo = this.ViewRoot.transform.FindChild("UserInfo").FindChild("PhotoMask").FindChild("Photo").GetComponent<RawImage>();
        this.PhotoButton = this.ViewRoot.transform.FindChild("UserInfo").FindChild("PhotoMask").FindChild("Photo").GetComponent<Button>();
        this.Username = this.ViewRoot.transform.FindChild("UserInfo").FindChild("NameText").GetComponent<Text>();
        this.RoomCardText = this.ViewRoot.transform.FindChild("UserInfo").FindChild("RoomCardText").GetComponent<Text>();
        this.RoomCardButton = this.ViewRoot.transform.FindChild("UserInfo").FindChild("RoomCardButton").GetComponent<Button>();
        this.ShareButton = this.ViewRoot.transform.FindChild("Buttons").FindChild("ShareButton").GetComponent<Button>();
        this.GradeButton = this.ViewRoot.transform.FindChild("Buttons").FindChild("GradeButton").GetComponent<Button>();
        this.SettingButton = this.ViewRoot.transform.FindChild("Buttons").FindChild("SettingButton").GetComponent<Button>();
        this.HelpButton = this.ViewRoot.transform.FindChild("Buttons").FindChild("HelpButton").GetComponent<Button>();
    }
    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Hall/TopMenuView");
    }
}
