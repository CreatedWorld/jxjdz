using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using Platform.Net;
using Platform.Model;
using System;

public class RankingMediator : Mediator, IMediator
{

    public RankingMediator(string NAME, object viewComponent):base(NAME,viewComponent)
    {

    }

    public RankingView View
    {
        get
        {
            return (RankingView)ViewComponent;
        }
    }

    public override void OnRegister()
    {
        base.OnRegister();
        this.View.ButtonAddListening(this.View.CloseButton,
        () =>
        {
            UIManager.Instance.HideUI(UIViewID.RANKING_VIEW);
            
        });
        GetRankingListC2S RankingList = new GetRankingListC2S();
        RankingList.num = 20;
        RankingList.rankingType = 1;
        NetMgr.Instance.SendBuff<GetRankingListC2S>(SocketType.HALL, MsgNoC2S.REQUEST_GetRankingListC2S.GetHashCode(), 0, RankingList);
    }

    public override IList<string> ListNotificationInterests()
    {
        IList<string> list = new List<string>();
        list.Add(NotificationConstant.MEDI_HALL_RANKINGIFON);
        return list;
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case NotificationConstant.MEDI_HALL_RANKINGIFON:
                UpdateRankList();
                break;
        }
    }

    public override void OnRemove()
    {
        base.OnRemove();
        UIManager.Instance.DestroyUI(UIViewID.RANKING_VIEW);
    }

    private void UpdateRankList()
    {
        HallProxy hallProxy = Facade.RetrieveProxy(Proxys.HALL_PROXY) as HallProxy;
        ArrayList dataProvider = new ArrayList();
        dataProvider.AddRange(hallProxy.HallInfo.userInfo);
        View.rankTable.DataProvider = dataProvider;
    }
}
