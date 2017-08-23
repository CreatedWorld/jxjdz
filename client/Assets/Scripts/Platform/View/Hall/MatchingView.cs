using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MatchingView : UIView
{
    private Button closeButton;
    private MatchingMediator macthingMedi;
    private Text round;
    private Text minuteText;
    private Text secondText;
    public Button CloseButton
    {
        get
        {
            return closeButton;
        }
    }

    public Text Round
    {
        get
        {
            return round;
        }
    }

    public Text MinuteText
    {
        get
        {
            return minuteText;
        }
    }

    public Text SecondText
    {
        get
        {
            return secondText;
        }
    }

    public override void OnInit()
    {
        this.ViewRoot = this.LaunchUIView("Prefab/UI/Matching/MatchingView");
        this.closeButton = this.ViewRoot.transform.FindChild("CloseButton").GetComponent<Button>();
        this.round = this.ViewRoot.transform.FindChild("RoundTitle").FindChild("Round").GetComponent<Text>();
        this.minuteText = this.ViewRoot.transform.FindChild("Timer").FindChild("Minute").GetComponent<Text>();
        this.secondText = this.ViewRoot.transform.FindChild("Timer").FindChild("Second").GetComponent<Text>();
        this.macthingMedi = new MatchingMediator(Mediators.HALL_MATCHING,this);
        ApplicationFacade.Instance.RegisterMediator(this.macthingMedi);
    }

    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Matching/MatchingView");
    }
    public override void OnShow()
    {
        base.OnShow();
        UIManager.Instance.ShowUIMask(UIViewID.MATCHING_VIEW);
        UIManager.Instance.ShowDOTween(this.ViewRoot.GetComponent<RectTransform>());
    }
    public override void OnHide()
    {
        UIManager.Instance.HidenDOTween(this.ViewRoot.GetComponent<RectTransform>(), base.OnHide);
        ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_HALL_INITMATCHINGTIME);
    }
    public override void OnDestroy()
    {
        ApplicationFacade.Instance.RemoveMediator(Mediators.HALL_MATCHING);
        ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_HALL_INITMATCHINGTIME);
        base.OnDestroy();
    }
    public override void Update()
    {
        base.Update();
        this.macthingMedi.TimeCount();
    }
}
