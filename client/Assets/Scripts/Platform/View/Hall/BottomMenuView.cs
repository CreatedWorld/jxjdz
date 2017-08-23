using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 大厅底部界面
/// </summary>
public class BottomMenuView : UIView
{
    /// <summary>
    /// 商城按钮
    /// </summary>
    private Button shopButton;
    /// <summary>
    /// 分享按钮
    /// </summary>
    private Button shareButton;
    /// <summary>
    /// 战绩按钮
    /// </summary>
    private Button militaryExploitsButton;
    /// <summary>
    /// 公告按钮
    /// </summary>
    private Button activityButton;
    /// <summary>
    /// 设置按钮
    /// </summary>
    private Button settingButton;

    public Button ShopButton
    {
        get
        {
            return shopButton;
        }

        set
        {
            shopButton = value;
        }
    }

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

    public Button MilitaryExploitsButton
    {
        get
        {
            return militaryExploitsButton;
        }

        set
        {
            militaryExploitsButton = value;
        }
    }

    public Button ActivityButton
    {
        get
        {
            return activityButton;
        }

        set
        {
            activityButton = value;
        }
    }

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

    public override void OnInit()
    {
        this.ViewRoot = this.LaunchUIView("Prefab/UI/Hall/BottomMenuView", UIManager.Instance.GetUIView(UIViewID.HALL_VIEW).ViewRoot.transform);
        this.ShopButton = this.ViewRoot.transform.FindChild("Buttons").FindChild("ShopButton").GetComponent<Button>();
        this.ShareButton = this.ViewRoot.transform.FindChild("Buttons").FindChild("ShareButton").GetComponent<Button>();
        this.MilitaryExploitsButton = this.ViewRoot.transform.FindChild("Buttons").FindChild("MilitaryExploitsButton").GetComponent<Button>();
        this.ActivityButton = this.ViewRoot.transform.FindChild("Buttons").FindChild("ActivityButton").GetComponent<Button>();
        this.SettingButton = this.ViewRoot.transform.FindChild("Buttons").FindChild("SettingButton").GetComponent<Button>();
    }
    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Hall/BottomMenuView");
    }
}
