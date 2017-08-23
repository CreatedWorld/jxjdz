﻿using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;
using Platform.Model;
using Platform.Net;

public class MatchingMediator : Mediator, IMediator {
    private int minute;
    private int second;
    private float count;
    public MatchingMediator(string NAME, object viewComponent):base(NAME,viewComponent)
    {

    }
    public override IList<string> ListNotificationInterests()
    {
        IList<string> list = new List<string>();
        list.Add(NotificationConstant.MEDI_HALL_INITMATCHINGTIME);
        return list;
    }
    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case (NotificationConstant.MEDI_HALL_INITMATCHINGTIME):
                this.InitTimer();
                break;
            default:
                break;
        }
    }


    public MatchingView View
    {
        get
        {
            return (MatchingView)ViewComponent;
        }
    }
    public override void OnRegister()
    {
        base.OnRegister();
        this.View.ButtonAddListening(this.View.CloseButton, 
            () => 
            {
                HallProxy hallProxy = Facade.RetrieveProxy(Proxys.HALL_PROXY) as HallProxy;
                CancelMatchingC2S package = new CancelMatchingC2S();
                package.roomType = (int)hallProxy.HallInfo.CompetitionRule;
                package.roomRounds = (int)hallProxy.HallInfo.CompetitionRound;
                NetMgr.Instance.SendBuff<CancelMatchingC2S>(SocketType.HALL, MsgNoC2S.REQUEST_CANCELMATCHING_C2S.GetHashCode(),0,package);
            });
        this.InitTimer();
    }
    public override void OnRemove()
    {
        base.OnRemove();
        UIManager.Instance.DestroyUI(UIViewID.MATCHING_VIEW);
        this.InitTimer();
    }
    /// <summary>
    /// 重置计时器
    /// </summary>
    private void InitTimer()
    {
        this.minute = 0;
        this.second = 0;
        this.count = 0;
    }
    /// <summary>
    /// 匹配计时器
    /// </summary>
    public void TimeCount()
    {
        this.count += Time.deltaTime;
        if (this.count >= 1.0f)
        {
            this.second += 1;
            if (this.second >= 60)
            {
                this.second = 0;
                this.minute += 1;
            }
            this.count = 0;
        }
        this.View.MinuteText.text = this.minute.ToString();
        this.View.SecondText.text = this.second.ToString();
    }
}