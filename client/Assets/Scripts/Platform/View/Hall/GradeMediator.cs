﻿using Platform.Model;
using Platform.Net;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class GradeMediator : Mediator, IMediator
{
    //private ArrayList gradeList;
    public GradeMediator(string NAME, object viewComponent) : base(NAME, viewComponent)
    {
        //this.gradeList = new ArrayList();
    }
    public GradeView View
    {
        get
        {
            return (GradeView)ViewComponent;
        }
        
    }
    public override IList<string> ListNotificationInterests()
    {
        IList<string> list = new List<string>();
        list.Add(NotificationConstant.MEDI_HALL_INITGRADEINFO);
        return list;
    }
    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case NotificationConstant.MEDI_HALL_INITGRADEINFO:
                this.InitGradeInfo(notification.Body);
                break;
            default:
                break;
        }
    }
    public override void OnRegister()
    {
        base.OnRegister();
        this.View.CloseButton = this.View.ViewRoot.transform.FindChild("CloseButton").GetComponent<Button>();
        this.View.ButtonAddListening(this.View.CloseButton,
            () =>
            {
                UIManager.Instance.HideUI(UIViewID.GRADE_VIEW);
            });
        SendGetGrade();
    }
    public override void OnRemove()
    {
        base.OnRemove();
        UIManager.Instance.DestroyUI(UIViewID.GRADE_VIEW);
    }


    //模拟获取战绩数据
    private void SendGetGrade()
    {
        GetGradeInfoC2S package = new GetGradeInfoC2S();
       
        NetMgr.Instance.SendBuff<GetGradeInfoC2S>(SocketType.HALL, MsgNoC2S.REQUEST_GETGRADEINFO_C2S.GetHashCode(),0,package);
    }


    /// <summary>
    /// 初始化战绩数据
    /// </summary>
    /// <param name="body"></param>
    private void InitGradeInfo(object body)
    {
        ArrayList gradeList = new ArrayList();
        GetGradeInfoS2C package = (GetGradeInfoS2C)body;
        
        for (int i = 0; i < package.gradeDataS2C.Count; i++)
        {
            GradeDataS2C gradeInfo = package.gradeDataS2C[i];
            
            gradeList.Add(gradeInfo);
        }
        this.View.ParticularsTableView.DataProvider = gradeList;
    }
}