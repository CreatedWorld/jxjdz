using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;

public class PlayerInfoMediator : Mediator, IMediator
{
    private PlayerInfoProxy playerInfoProxy;
    public PlayerInfoMediator(string NAME, object viewComponent) : base(NAME, viewComponent)
    {

    }
    public PlayerInfoView View
    {
        get
        {
            return (PlayerInfoView)ViewComponent;
        }
    }

    public override IList<string> ListNotificationInterests()
    {
        IList<string> list = new List<string>();
        list.Add(NotificationConstant.MEDI_PLAYINFO_REFRESHUSERINFO);
        return list;
    }

    public override void OnRegister()
    {
        base.OnRegister();
        this.playerInfoProxy = Facade.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
        this.View.ButtonAddListening(this.View.CloseButton,
        () =>
        {
            UIManager.Instance.HideUI(UIViewID.PLATER_INFO_VIEW);
        });

        //刷新用户信息
        //this.RefreshPlayerInfo();
    }

    public override void OnRemove()
    {
        base.OnRemove();
        UIManager.Instance.DestroyUI(UIViewID.PLATER_INFO_VIEW);
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case (NotificationConstant.MEDI_PLAYINFO_REFRESHUSERINFO):
                this.RefreshPlayerInfo();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 更新玩家信息
    /// </summary>
    public void RefreshPlayerInfo()
    {
        if (View.data.userId == playerInfoProxy.UserInfo.UserID)
        {
            this.View.UserID.text = this.playerInfoProxy.UserInfo.ShowID;
            this.View.UsernameText.text = this.playerInfoProxy.UserInfo.UserName;
            if (playerInfoProxy.UserInfo.UserItems.ContainsKey(ItemType.ROOMCARD))
            {
                this.View.CardText.text = this.playerInfoProxy.UserInfo.UserItems[ItemType.ROOMCARD].amount.ToString();
            }
            this.View.IpText.text = string.Format("IP:{0}", Network.player.ipAddress);
            GameMgr.Instance.StartCoroutine(DownIcon(playerInfoProxy.UserInfo.HeadIconUrl));
        }
        else
        {
            View.UserID.text = View.data.userId.ToString();
            View.UsernameText.text = View.data.userName;
            View.CardText.text = View.data.userItems[0].amount.ToString();
            View.data.ip = View.data.ip.Replace("/","");
            View.IpText.text = string.Format("IP:{0}", View.data.ip);
            GameMgr.Instance.StartCoroutine(DownIcon(View.data.imageUrl));
        }
    }

    System.Collections.IEnumerator DownIcon(string headUrl)
    {
        WWW www = new WWW(headUrl);
        yield return www;
        if (www.error == null)
        {
            View.HeadIcon.texture = www.texture;
        }
    }
}

