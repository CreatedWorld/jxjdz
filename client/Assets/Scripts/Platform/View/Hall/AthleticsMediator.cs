﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;
using UnityEngine.UI;
using Platform.Net;
using Platform.Model;
/// <summary>
/// 比赛场模块
/// </summary>
public class AthleticsMediator : Mediator, IMediator
{
    public AthleticsMediator(string mediatorName, object viewComponent) : base(mediatorName, viewComponent)
    {
    }
    public AthleticsView View
    {
        get
        {
            return (AthleticsView)ViewComponent;
        }
    }

    public override IList<string> ListNotificationInterests()
    {
        IList<string> list = new List<string>();
        list.Add(NotificationConstant.MEDI_HALL_REFRESHATHLETICSVIEW);
        return list;
    }
    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case (NotificationConstant.MEDI_HALL_REFRESHATHLETICSVIEW):
                this.RefreshAthleticsView();
                break;
            default:
                break;
        }
    }

    public override void OnRegister()
    {
        base.OnRegister();
        this.View.ButtonAddListening(this.View.CloseButton,
            () =>
            {
                UIManager.Instance.HideUI(UIViewID.ATHLETICS_VIEW);
            });
        this.View.ButtonAddListening(this.View.MatchingButton,
            () =>
            {
                this.SendMatchingRequest();
            });
    }
    public override void OnRemove()
    {
        base.OnRemove();
        UIManager.Instance.DestroyUI(UIViewID.ATHLETICS_VIEW);
    }
    /// <summary>
    /// 刷新比赛场信息View
    /// </summary>
    private void RefreshAthleticsView()
    {
        HallProxy hallProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.HALL_PROXY) as HallProxy;
        DateTime currentDateTime = TimeHandle.Instance.GetDateTimeByTimestamp(hallProxy.HallInfo.CurrentTime);
        DateTime startDateTime = TimeHandle.Instance.GetDateTimeByTimestamp(hallProxy.HallInfo.StartTime);
        DateTime endDateTime = TimeHandle.Instance.GetDateTimeByTimestamp(hallProxy.HallInfo.EndTime);
        TimeSpan startTime = startDateTime - currentDateTime;
        if (startTime.TotalMinutes > 5)
        {
            this.View.Title.text = "未开启";
            this.View.Tint.text = "查看上一轮排名";
            this.View.Time.text = "";
            this.View.State.text = "";
        }
        else if (startTime.TotalMinutes > 0 && startTime.TotalMinutes <= 5)
        {
            this.View.Title.text = "即将开启";
            this.View.Tint.text = "";
            this.View.Time.text = startDateTime.ToString("HH:mm");
            this.View.State.text = "开始";
        }
        else if (startTime.TotalMinutes <= 0)
        {
            if (currentDateTime.TimeOfDay.TotalMilliseconds <= endDateTime.TimeOfDay.TotalMilliseconds)
            {
                this.View.Title.text = "活动开启";
                this.View.Tint.text = "";
                this.View.Time.text = endDateTime.ToString("HH:mm");
                this.View.State.text = "结束";
            }
            else if (currentDateTime.TimeOfDay.TotalMilliseconds > endDateTime.TimeOfDay.TotalMilliseconds)
            {
                this.View.Title.text = "未开启";
                this.View.Tint.text = "查看上一轮排名";
                this.View.Time.text = "";
                this.View.State.text = "";
            }
        }
    }
    /// <summary>
    /// 发送匹配请求
    /// </summary>
    private void SendMatchingRequest()
    {
        HallProxy hallProxy = Facade.RetrieveProxy(Proxys.HALL_PROXY) as HallProxy;
        ApplyMatchingC2S package = new ApplyMatchingC2S();
        package.roomType = (int)hallProxy.HallInfo.CompetitionRule;
        package.roomRounds = (int)hallProxy.HallInfo.CompetitionRound;
        Debug.Log(package.roomType);
        Debug.Log(package.roomRounds);
        NetMgr.Instance.SendBuff<ApplyMatchingC2S>(SocketType.HALL, MsgNoC2S.REQUEST_APPLYMATCHING_C2S.GetHashCode(),0,package);
    }
}