﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;
using UnityEngine.SceneManagement;
using Platform.Net;
using Platform.Model;
using Platform.Model.Battle;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Net;
using Platform.Global;
using Platform.Utils;
/// <summary>
/// 大厅中介
/// </summary>
public class HallMediator : Mediator, IMediator
{
    /// <summary>
    /// 大厅数据
    /// </summary>
    private HallProxy hallProxy;
    /// <summary>
    /// 登录中介
    /// </summary>
    private LoginProxy loginProxy;
    public HallMediator(string NAME,object viewComponent):base(NAME,viewComponent)
    {
    }
    public HallMgr View
    {
        get
        {
            return (HallMgr)ViewComponent;
        }
    }
    public override void OnRegister()
    {
        base.OnRegister();
        hallProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.HALL_PROXY) as HallProxy;
        loginProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.LOGIN_PROXY) as LoginProxy;
        View.HallView = (HallView)UIManager.Instance.ShowUI(UIViewID.HALL_VIEW);
        View.TopView = (TopMenuView)UIManager.Instance.ShowUI(UIViewID.TOPMENU_VIEW);
        View.MiddleView = (MiddleMenuView)UIManager.Instance.ShowUI(UIViewID.MIDDLEMENU_VIEW);
        TopMenuAddEvent();
        MiddleMenuAddEvent();
        View.TopView.ViewRoot.SetActive(false);
        View.MiddleView.ViewRoot.SetActive(false);
        AudioSystem.Instance.PlayBgm(Resources.Load<AudioClip>("Voices/Bgm/HallBgm"));
        if (GlobalData.LoginServer != "127.0.0.1")
        {
            NetMgr.Instance.StopTcpConnection(SocketType.BATTLE);
            if (!NetMgr.Instance.ConnentionDic.ContainsKey(SocketType.HALL))
            {
                NetMgr.Instance.CreateConnect(SocketType.HALL, loginProxy.hallServerIP, loginProxy.hallServerPort);
            }
        }
        var startUpParam = AndroidSdkInterface.GetStartParam();
        if (startUpParam != null)
        {
            Dictionary<string, string> paramDic = StringUtil.ParseParam(startUpParam);
            if (paramDic.ContainsKey(StartUpParam.TYPE) && paramDic[StartUpParam.TYPE] == StartUpType.JOINROOM)
            {
                hallProxy.HallInfo.RoomCode = paramDic[StartUpParam.ROOMID];
                JoinInRoomC2S package = new JoinInRoomC2S();
                package.roomCode = hallProxy.HallInfo.RoomCode;
                package.seat = 0;
                NetMgr.Instance.SendBuff<JoinInRoomC2S>(SocketType.HALL, MsgNoC2S.REQUEST_JOINROOM_C2S.GetHashCode(), 0, package);
            }
        }
    }
    public override void OnRemove()
    {
        base.OnRemove();
    }
    public override IList<string> ListNotificationInterests()
    {
        IList<string> list = new List<string>();
        list.Add(NotificationConstant.MEDI_HALL_REFRESHUSERINFO);
        list.Add(NotificationConstant.MEDI_HALL_REFRESHITEM);
        list.Add(NotificationConstant.MEDI_HALL_REFRESHANNOUNCEMENT);
        list.Add(NotificationConstant.MEDI_HALL_ANNOUNCEMENTFINISH);
        list.Add(NotificationConstant.MEDI_GAMEMGR_SCENE_CHANGED);
        list.Add(NotificationConstant.MEDI_HALL_REFRESHHALLNOTICE);
        return list;
    }
    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case (NotificationConstant.MEDI_HALL_REFRESHUSERINFO):
                RefreshUserInfo();
                break;
            case (NotificationConstant.MEDI_HALL_REFRESHITEM):
                RefreshItem();
                break;
            case (NotificationConstant.MEDI_HALL_REFRESHANNOUNCEMENT):
                RefreshAnnouncement();
                break;
            case (NotificationConstant.MEDI_HALL_ANNOUNCEMENTFINISH):
                AnnouncementFinish();
                break;
            case (NotificationConstant.MEDI_GAMEMGR_SCENE_CHANGED):
                View.TopView.ViewRoot.SetActive(true);
                View.MiddleView.ViewRoot.SetActive(true);
                break;
            case NotificationConstant.MEDI_HALL_REFRESHHALLNOTICE:
                RefreshNotice();
                break;
            default:
                break;
        }
    }

 
    /// <summary>
    /// 绑定顶部UI按钮事件
    /// </summary>
    private void TopMenuAddEvent()
    {
        View.TopView.ButtonAddListening(View.TopView.PhotoButton,
            () =>
            {
                PlayerInfoProxy playerInfoProxy = Facade.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
                var playerInfoView = UIManager.Instance.ShowUI(UIViewID.PLATER_INFO_VIEW) as PlayerInfoView;
                var playerInfoVO = new GetUserInfoByIdS2C();
                playerInfoVO.userId = playerInfoProxy.UserInfo.UserID;
                playerInfoView.data = playerInfoVO;
            });
        View.TopView.ButtonAddListening(View.TopView.RoomCardButton,
            () =>
            {
                UIManager.Instance.ShowUI(UIViewID.SHOPPING_VIEW);
            });
        View.TopView.ButtonAddListening(View.TopView.ShareButton,
            () =>
            {
                UIManager.Instance.ShowUI(UIViewID.SHARE_VIEW);
            });
        View.TopView.ButtonAddListening(View.TopView.GradeButton,
            ()=>
            {
                UIManager.Instance.ShowUI(UIViewID.GRADE_VIEW);
            }
            );
        View.TopView.ButtonAddListening(View.TopView.SettingButton,
            () =>
            {
                UIManager.Instance.ShowUI(UIViewID.SETTING_VIEW);
            }
            );
        View.TopView.ButtonAddListening(View.TopView.HelpButton,
            () =>
            {
                UIManager.Instance.ShowUI(UIViewID.HELP_VIEW);
            });
    }
    /// <summary>
    /// 绑定中部UI按钮事件
    /// </summary>
    private void MiddleMenuAddEvent()
    {
        View.MiddleView.ButtonAddListening(View.MiddleView.RankingButton,
            () =>
            {
                UIManager.Instance.ShowUI(UIViewID.RANKING_VIEW);
            },true
            );
        View.MiddleView.ButtonAddListening(View.MiddleView.CreateRoomButton, () =>
        {
            UIManager.Instance.ShowUI(UIViewID.CREATEROOM_VIEW);
        },true);
        View.MiddleView.ButtonAddListening(View.MiddleView.JoinRoomButton, () =>
        {
            UIManager.Instance.ShowUI(UIViewID.JOINROOM_VIEW);
        });
    }
    /// <summary>
    /// 刷新用户大厅界面
    /// </summary>
    private void RefreshUserInfo()
    {
        PlayerInfoProxy playerInfoProxy = Facade.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
        View.TopView.Username.text = playerInfoProxy.UserInfo.ShowID;
        GameMgr.Instance.StartCoroutine(DownIcon(playerInfoProxy.UserInfo.HeadIconUrl));
        if (playerInfoProxy.UserInfo.UserItems.ContainsKey(ItemType.ROOMCARD))
        {
            View.TopView.RoomCardText.text = playerInfoProxy.UserInfo.UserItems[ItemType.ROOMCARD].amount.ToString();
        }
        else
        {
            View.TopView.RoomCardText.text = "0";
        }
    }
    /// <summary>
    /// 下载回调
    /// </summary>
    /// <param name="headUrl"></param>
    /// <returns></returns>
    IEnumerator DownIcon(string headUrl)
    {
        WWW www = new WWW(headUrl);
        yield return www;
        if (www.error == null)
        {
            PlayerInfoProxy playerProxy = Facade.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            playerProxy.UserInfo.HeadIcon = www.texture;
            View.TopView.Photo.texture = www.texture;
        }
    }
    /// <summary>
    /// 刷新玩家货币
    /// </summary>
    private void RefreshItem()
    {
        PlayerInfoProxy playerInfoProxy = Facade.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
        if (playerInfoProxy.UserInfo.UserItems.ContainsKey(ItemType.ROOMCARD))
        {
            View.TopView.RoomCardText.text = playerInfoProxy.UserInfo.UserItems[ItemType.ROOMCARD].amount.ToString();
        }
        else
        {
            View.TopView.RoomCardText.text = "0";
        }
    }
    /// <summary>
    /// 刷新公告
    /// </summary>
    private void RefreshAnnouncement()
    {
        if (View.MiddleView.AnnouncementText == null || View.MiddleView.AnnouncementText.text == "")
        {
            if (hallProxy.HallInfo.AnnouncementQueue.Count > 0)
            {
                AnnouncementData data = hallProxy.HallInfo.AnnouncementQueue.Peek();
                View.MiddleView.AnnouncementText.text = data.Content;
                View.MiddleView.AnnouncementText.rectTransform.localPosition = new Vector3(520, View.MiddleView.AnnouncementText.rectTransform.localPosition.y, 0);
            }
            else
            {
                if (hallProxy.HallInfo.NoticeList.ContainsKey(HallNoticeType.HALL_NOTICE))
                {
                    View.MiddleView.AnnouncementText.text = hallProxy.HallInfo.NoticeList[HallNoticeType.HALL_NOTICE].content;
                }
            }
        }
    }
    /// <summary>
    /// 公告播放完毕回调
    /// </summary>
    private void AnnouncementFinish()
    {
        View.MiddleView.AnnouncementText.text = "";
        View.MiddleView.AnnouncementText.rectTransform.localPosition = new Vector3(450, 0, 0);
        if (hallProxy.HallInfo.AnnouncementQueue.Count > 0)//hallProxy.HallInfo.AnnouncementQueue.Count > 0
        {
            AnnouncementData data = hallProxy.HallInfo.AnnouncementQueue.Dequeue();//
            if (data.ReduceCirCount() > 0)
            {
                hallProxy.HallInfo.AnnouncementQueue.Enqueue(data);
            }
            SendNotification(NotificationConstant.MEDI_HALL_REFRESHANNOUNCEMENT);
        }
        SendNotification(NotificationConstant.MEDI_HALL_REFRESHANNOUNCEMENT);//原来这里没有这行的
    }

    /// <summary>
    /// 下载大厅图片
    /// </summary>
    /// <param name="noticeUrl"></param>
    /// <returns></returns>
    IEnumerator DownNoticeIcon(string noticeUrl)
    {
        WWW www = new WWW(noticeUrl);
        yield return www;
        if (www.error == null)
        {
            View.MiddleView.NoticContent.gameObject.SetActive(true);
            View.MiddleView.NoticContent.texture = www.texture;
            if (View.MiddleView.Notice.gameObject.activeSelf)
            {
                View.MiddleView.Notice.gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 刷新大厅公告
    /// </summary>
    private void RefreshNotice()
    {
        if (hallProxy.HallInfo.NoticeList.ContainsKey(HallNoticeType.HALL_CONTENT))
        {
            GameMgr.Instance.StartCoroutine(DownNoticeIcon(hallProxy.HallInfo.NoticeList[HallNoticeType.HALL_CONTENT].content));
        }
        if (hallProxy.HallInfo.NoticeList.ContainsKey(HallNoticeType.HALL_NOTICE))
        {
            //View.MiddleView.NoticeTitle.text = hallProxy.HallInfo.NoticeList[HallNoticeType.HALL_NOTICE].title;
            View.MiddleView.AnnouncementText.text = hallProxy.HallInfo.NoticeList[HallNoticeType.HALL_NOTICE].content;
            View.MiddleView.Notice.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 获取排行榜的数据 
    /// </summary>
    private void ShowRankingInfo()
    {
        if (hallProxy.HallInfo.userInfo != null)
        {

        }
    }

}