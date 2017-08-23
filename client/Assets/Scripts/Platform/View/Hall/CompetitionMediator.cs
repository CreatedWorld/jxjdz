using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;
using Platform.Net;
using Platform.Model;
public class CompetitionMediator : Mediator, IMediator
{
    public CompetitionMediator(string NAME, object viewComponent):base(NAME,viewComponent)
    {

    }
    public CompetitionView View
    {
        get
        {
            return (CompetitionView)ViewComponent;
        }
    }
    public override void OnRegister()
    {
        base.OnRegister();
        this.View.ButtonAddListening(this.View.ApplyCloseButton,
            () =>
            {
                UIManager.Instance.HideUI(UIViewID.COMPETITION_VIEW);
            });

        this.View.ButtonAddListening(this.View.ApplyButton,
            () =>
            {
                ApplyCompetitionC2S package = new ApplyCompetitionC2S();
                NetMgr.Instance.SendBuff<ApplyCompetitionC2S>(SocketType.HALL, MsgNoC2S.REQUEST_APPLYCOMPETITION_C2S.GetHashCode(),0,package);
            });
        this.View.ButtonAddListening(this.View.RuleButton,
            () =>
            {
                this.View.ApplyPass.gameObject.SetActive(false);
                UIManager.Instance.HidenDOTween(this.View.Apply.GetComponent<RectTransform>(),
                    ()=> 
                    {
                        this.View.Apply.gameObject.SetActive(false);
                        UIManager.Instance.ShowDOTween(this.View.Rule.GetComponent<RectTransform>());
                        this.View.Rule.gameObject.SetActive(true);
                    });
            });
        this.View.ButtonAddListening(this.View.RuleCloseButton,
            () =>
            {
                this.View.ApplyPass.gameObject.SetActive(false);
                UIManager.Instance.HidenDOTween(this.View.Rule.GetComponent<RectTransform>(), 
                    () => 
                    {
                        this.View.Apply.gameObject.SetActive(true);
                        UIManager.Instance.ShowDOTween(this.View.Apply.GetComponent<RectTransform>());
                        this.View.Rule.gameObject.SetActive(false);
                    });
            });
        this.View.ButtonAddListening(this.View.ApplyPassCloseButton,
            () =>
            {
                UIManager.Instance.HideUI(UIViewID.COMPETITION_VIEW);
            });
    }
    public override void OnRemove()
    {
        base.OnRemove();
        UIManager.Instance.DestroyUI(UIViewID.COMPETITION_VIEW);
    }

    public override IList<string> ListNotificationInterests()
    {
        IList<string> list = new List<string>();
        list.Add(NotificationConstant.MEDI_HALL_APPLYSUCCEED);
        return list;
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case NotificationConstant.MEDI_HALL_APPLYSUCCEED:
                this.ShowApplySucceed();
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 显示报名成功
    /// </summary>
    private void ShowApplySucceed()
    {
        this.View.Rule.gameObject.SetActive(false);
        UIManager.Instance.HidenDOTween(this.View.Apply.GetComponent<RectTransform>(),
            () =>
            {
                this.View.Apply.gameObject.SetActive(false);
                UIManager.Instance.ShowDOTween(this.View.ApplyPass.GetComponent<RectTransform>());
                this.View.ApplyPass.gameObject.SetActive(true);
            });
    }
}

