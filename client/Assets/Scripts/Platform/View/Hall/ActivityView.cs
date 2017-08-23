using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// 活动模块View
/// </summary>
public class ActivityView : UIView
{
    /// <summary>
    /// 关闭按钮
    /// </summary>
    private Button closeButton;
    /// <summary>
    /// 公告信息
    /// </summary>
    private ActivContnet information;
    private ActivContnet contact;
    private ActivContnet announcement;
    private ActivContnet generalize;

    public Button CloseButton
    {
        get
        {
            return closeButton;
        }

        set
        {
            closeButton = value;
        }
    }

    public ActivContnet Information
    {
        get
        {
            return information;
        }

        set
        {
            information = value;
        }
    }

    public ActivContnet Contact
    {
        get
        {
            return contact;
        }

        set
        {
            contact = value;
        }
    }

    public ActivContnet Announcement
    {
        get
        {
            return announcement;
        }

        set
        {
            announcement = value;
        }
    }

    public ActivContnet Generalize
    {
        get
        {
            return generalize;
        }

        set
        {
            generalize = value;
        }
    }

    public override void OnInit()
    {
        this.ViewRoot = this.LaunchUIView("Prefab/UI/Activity/ActivityView");
        this.CloseButton = this.ViewRoot.transform.FindChild("CloseButton").GetComponent<Button>();
        this.Information = new ActivContnet(this.ViewRoot.transform.FindChild("Information").gameObject, HallNoticeType.MENU_INFORMATION);
        this.Contact = new ActivContnet(this.ViewRoot.transform.FindChild("Contact").gameObject, HallNoticeType.MENU_CONTACT);
        this.Announcement = new ActivContnet(this.ViewRoot.transform.FindChild("Announcement").gameObject, HallNoticeType.MENU_ANNOUNCEMENT);
        this.Generalize = new ActivContnet(this.ViewRoot.transform.FindChild("Generalize").gameObject, HallNoticeType.MENU_GENERALIZE);
        ApplicationFacade.Instance.RegisterMediator(new ActivityMediator(Mediators.HALL_ACTIVITY, this));
    }
    public override void OnShow()
    {
        base.OnShow();
        UIManager.Instance.ShowUIMask(UIViewID.ACTIVITY_VIEW);
        UIManager.Instance.ShowDOTween(this.ViewRoot.GetComponent<RectTransform>());
    }

    public override void OnHide()
    {
        UIManager.Instance.HidenDOTween(this.ViewRoot.GetComponent<RectTransform>(),base.OnHide);
        
    }

    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Activity/ActivityView");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ApplicationFacade.Instance.RemoveMediator(Mediators.HALL_ACTIVITY);
    }
}