using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Platform.Net;

/// <summary>
/// 比赛场View
/// </summary>
public class AthleticsView : UIView
{
    /// <summary>
    /// 匹配按钮
    /// </summary>
    private Button matchingButton;
    /// <summary>
    /// 关闭按钮
    /// </summary>
    private Button closeButton;
    /// <summary>
    /// 为开赛标题
    /// </summary>
    private Text tint;
    /// <summary>
    /// 开赛标题
    /// </summary>
    private Text title;
    /// <summary>
    /// 时间
    /// </summary>
    private Text time;
    /// <summary>
    /// 比赛状态
    /// </summary>
    private Text state;
    public Button MatchingButton
    {
        get
        {
            return matchingButton;
        }

        set
        {
            matchingButton = value;
        }
    }

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

    public Text Tint
    {
        get
        {
            return tint;
        }

        set
        {
            tint = value;
        }
    }

    public Text Title
    {
        get
        {
            return title;
        }

        set
        {
            title = value;
        }
    }

    public Text Time
    {
        get
        {
            return time;
        }

        set
        {
            time = value;
        }
    }

    public Text State
    {
        get
        {
            return state;
        }

        set
        {
            state = value;
        }
    }

    public override void OnInit()
    {
        this.ViewRoot = this.LaunchUIView("Prefab/UI/Athletics/AthleticsView");
        this.MatchingButton = this.ViewRoot.transform.FindChild("AthleticsButton").GetComponent<Button>();
        this.CloseButton = this.ViewRoot.transform.FindChild("CloseButton").GetComponent<Button>();
        this.Tint = this.ViewRoot.transform.FindChild("Info").FindChild("Tint").GetComponent<Text>();
        this.Title = this.ViewRoot.transform.FindChild("Info").FindChild("Title").GetComponent<Text>();
        this.Time = this.ViewRoot.transform.FindChild("Info").FindChild("Time").GetComponent<Text>();
        this.State = this.ViewRoot.transform.FindChild("Info").FindChild("State").GetComponent<Text>();
        ApplicationFacade.Instance.RegisterMediator(new AthleticsMediator(Mediators.HALL_ATHLETICS, this));
    }
    public override void OnShow()
    {
        base.OnShow();
        UIManager.Instance.ShowUIMask(UIViewID.ATHLETICS_VIEW);
        UIManager.Instance.ShowDOTween(this.ViewRoot.GetComponent<RectTransform>());
        ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_HALL_REFRESHATHLETICSVIEW);
    }
    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Athletics/Athletics");
    }
    public override void OnHide()
    {
        UIManager.Instance.HidenDOTween(this.ViewRoot.GetComponent<RectTransform>(), base.OnHide);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ApplicationFacade.Instance.RemoveMediator(Mediators.HALL_ATHLETICS);
    }
}