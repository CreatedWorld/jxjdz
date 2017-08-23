﻿using Platform.Model;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GradeInfoViewMediator : Mediator, IMediator
{
    public static Dictionary<RoundData, int> gamesNumberDic;

    private string roomIdStr;
    /// <summary>
    /// 房间号
    /// </summary>
    private Text roomIdText;
    //private ArrayList gradeList;
    public GradeInfoViewMediator(string NAME, object viewComponent) : base(NAME, viewComponent)
    {
        //this.gradeList = new ArrayList();
    }
    public GradeInformationView View
    {
        get
        {
            return (GradeInformationView)ViewComponent;
        }
        
    }
    public override IList<string> ListNotificationInterests()
    {
        IList<string> list = new List<string>();
        list.Add(NotificationConstant.MEDI_HALL_GETROUNDINFO);
        return list;
    }
    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case NotificationConstant.MEDI_HALL_GETROUNDINFO:
                this.InifRoundInfo(notification.Body);
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
                UIManager.Instance.HideUI(UIViewID.GRADEINFORMATION_VIEW);
            });
    }
    public override void OnRemove()
    {
        base.OnRemove();
        UIManager.Instance.DestroyUI(UIViewID.GRADEINFORMATION_VIEW);
    }

    /// <summary>
    /// 初始化战绩房间数据
    /// </summary>
    /// <param name="body"></param>
    private void InifRoundInfo(object body)
    {
        gamesNumberDic = new Dictionary<RoundData, int>();
        ArrayList roundList = new ArrayList();
        GetRoundInfoS2C package = (GetRoundInfoS2C)body;
        foreach (var item in package.roundData)
        {
            roomIdText = this.View.ViewRoot.transform.FindChild("RoomId/Text").GetComponent<Text>();
            roomIdStr = System.Convert.ToString( item.roomID);
            
            roomIdText.text = roomIdStr;
         
        }
        for (int i = 0; i < package.roundData.Count; i++)
        {
            RoundData roundInfo = package.roundData[i];
            gamesNumberDic[roundInfo] = i;
            if (roundList.Contains(roundInfo))
            {
            roundList.Add(roundInfo);

            }

        }
        roundList.AddRange(package.roundData);
        this.View.ParticularsTableView.DataProvider = roundList;
    }
}