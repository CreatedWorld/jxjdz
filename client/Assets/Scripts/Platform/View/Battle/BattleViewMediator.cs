﻿using System.Collections.Generic;
using Platform.Model;
using Platform.Model.Battle;
using Platform.Net;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;
using Utils;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using Platform.Utils;
using UnityEngine.UI;

/// <summary>
/// 战斗场景UI中介
/// </summary>
public class BattleViewMediator : Mediator, IMediator
{
    /// <summary>
    /// 战斗模块数据中介
    /// </summary>
    private BattleProxy battleProxy;

    /// <summary>
    /// 游戏数据中介
    /// </summary>
    private GameMgrProxy gameMgrProxy;
    /// <summary>
    /// 玩家信息数据
    /// </summary>
    private PlayerInfoProxy playerInfoProxy;
    /// <summary>
    /// 同步时间定时器id
    /// </summary>
    private int sysTimeId;
    /// <summary>
    /// 点击后是否展开
    /// </summary>
    private bool isExpand = false;

    public BattleViewMediator(string mediatorName, object viewComponent) : base(mediatorName, viewComponent)
    {
    }

    public BattleView View
    {
        get { return (BattleView)ViewComponent; }
    }

    public override IList<string> ListNotificationInterests()
    {
        IList<string> list = new List<string>();
        list.Add(NotificationConstant.MEDI_BATTLEVIEW_UPDATEALLHEAD);
        list.Add(NotificationConstant.MEDI_BATTLEVIEW_UPDATESINGLEHEAD);
        list.Add(NotificationConstant.MEDI_READY_COMPLETE);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAYROTATE);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAYACTTIP);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAY_COMMONANGANG);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAY_BACKANGANG);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAYGETCARD);
        list.Add(NotificationConstant.MEDI_BATTLE_UPDATEOOMMESSAGES);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAYPASS);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAYPENG);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAYCHI);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAY_COMMONPENGGANG);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAY_BACKPENGGANG);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAYPUTCARD);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAYPUTFLOWERCARD);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAYZHIGANG);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAYHU);
        list.Add(NotificationConstant.MEDI_BATTLEVIEW_SHOWBANKERICON);
        list.Add(NotificationConstant.MEDI_BATTLEVIEW_HIDENRECORDING);
        list.Add(NotificationConstant.MEDI_BATTLEVIEW_SHOWPLAYINGVOICE);
        list.Add(NotificationConstant.MEDI_BATTLEVIEW_HIDENPLAYINGVOICE);
        list.Add(NotificationConstant.MEDI_BATTLEVIEW_SHOWCHAT);
        list.Add(NotificationConstant.MEDI_BATTLEVIEW_SHOWFACE);
        list.Add(NotificationConstant.MEDI_BATTLEVIEW_SHOW_REPORTVIEW);
        list.Add(NotificationConstant.MEDI_BATTLEVIEW_UPDATEONLINE);
        list.Add(NotificationConstant.MEDI_BATTLEVIEW_INITPLAYERCARDS);
        list.Add(NotificationConstant.TING_UPDATE);
        list.Add(NotificationConstant.MEDI_BATTLEVIEW_UPDATEVOLUME);
        list.Add(NotificationConstant.MEDI_BATTLE_SHOWJING);

        return list;
    }


    public override void OnRegister()
    {
        base.OnRegister();
        battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
        gameMgrProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.GAMEMGR_PROXY) as GameMgrProxy;
        playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
        View.readyBtn.onClick.AddListener(onReadyClick);
        View.exitBtn.onClick.AddListener(OnDissloutionClick);
        View.dissolutionBtn.onClick.AddListener(OnDissloutionClick);
        //View.expandBtn.onClick.AddListener(ExpendOperateView);
        View.chatBtn.onClick.AddListener(OnChatClick);
        View.settingBtn.onClick.AddListener(OnSettingClick);
        View.inviteBtn.onClick.AddListener(OnInviteClick);

        var voideTrigger = View.voiceBtn.GetComponent<EventTrigger>();
        voideTrigger.triggers = new List<EventTrigger.Entry>();
        EventTrigger.Entry onUp = new EventTrigger.Entry();
        onUp.eventID = EventTriggerType.PointerUp;
        onUp.callback.AddListener(OnVoiceUp);
        EventTrigger.Entry onDown = new EventTrigger.Entry();
        onDown.eventID = EventTriggerType.PointerDown;
        onDown.callback.AddListener(OnVoiceDown);
        voideTrigger.triggers.Add(onDown);
        voideTrigger.triggers.Add(onUp);

        Timer.Instance.AddTimer(0, 1, 0, InitView);
    }

    public override void OnRemove()
    {
        base.OnRemove();
        View.readyBtn.onClick.RemoveListener(onReadyClick);
        View.exitBtn.onClick.RemoveListener(OnDissloutionClick);
        View.dissolutionBtn.onClick.RemoveListener(OnDissloutionClick);
        //View.expandBtn.onClick.RemoveListener(ExpendOperateView);
        View.chatBtn.onClick.RemoveListener(OnChatClick);
        View.settingBtn.onClick.RemoveListener(OnSettingClick);
        View.inviteBtn.onClick.RemoveListener(OnInviteClick);
        Timer.Instance.CancelTimer(sysTimeId);
    }

    public override void HandleNotification(INotification notification)
    {
        //Debug.Log("HandleNotification Name:" + notification.Name);
        switch (notification.Name)
        {
            case NotificationConstant.MEDI_BATTLEVIEW_UPDATEALLHEAD:
                UpdateAllHeadItem((bool)notification.Body);
                UpdateInviteBtn();
                break;
            case NotificationConstant.MEDI_BATTLEVIEW_UPDATESINGLEHEAD:
                UpdateSingleHeadItem(notification.Body as PlayerInfoVOS2C);
                UpdateInviteBtn();
                break;
            case NotificationConstant.MEDI_READY_COMPLETE:
                UpdateReadyBtn();
                break;
            case NotificationConstant.MEDI_BATTLE_PLAYROTATE:
                PlayRotate((bool)notification.Body);
                UpdateRoomInfo();
                UpdateDisloveBtn();
                View.readyBtn.gameObject.SetActive(false);
                break;
            case NotificationConstant.MEDI_BATTLEVIEW_SHOWBANKERICON:
                UpdateBankerIcon();
                break;
            case NotificationConstant.MEDI_BATTLE_PLAYACTTIP:
                ShowPlayActTip();

                //View.masterView.ShowPlayActTip();
                break;
            case NotificationConstant.MEDI_BATTLE_PLAY_COMMONANGANG:
                PlayAct(PlayerActType.COMMON_AN_GANG);
                break;
            case NotificationConstant.MEDI_BATTLE_PLAY_BACKANGANG:
                PlayAct(PlayerActType.BACK_AN_GANG);
                break;
            case NotificationConstant.MEDI_BATTLE_PLAYGETCARD:
                //View.masterView.ShowPlayActTip();
                UpdateRoomInfo();
                break;
            case NotificationConstant.MEDI_BATTLE_UPDATEOOMMESSAGES:
                DontUpdateRoomInfo();
                break;
            //case NotificationConstant.MEDI_BATTLEVIEW_INITPLAYERCARDS:
            //    UpdateRoomInfo();
            //    break;
            case NotificationConstant.MEDI_BATTLE_PLAYPASS:
                PlayAct(PlayerActType.PASS);
                break;
            case NotificationConstant.MEDI_BATTLE_PLAYPENG:
                PlayAct(PlayerActType.PENG);
                break;
            case NotificationConstant.MEDI_BATTLE_PLAYCHI:
                PlayAct(PlayerActType.CHI);
                break;
            case NotificationConstant.MEDI_BATTLE_PLAY_COMMONPENGGANG:
                PlayAct(PlayerActType.COMMON_PENG_GANG);
                break;
            case NotificationConstant.MEDI_BATTLE_PLAY_BACKPENGGANG:
                PlayAct(PlayerActType.BACK_PENG_GANG);
                break;
            case NotificationConstant.MEDI_BATTLE_PLAYPUTCARD:
                PlayAct(PlayerActType.PUT_CARD);
                break;
            case NotificationConstant.MEDI_BATTLE_PLAYPUTFLOWERCARD:
                PlayAct(PlayerActType.PUT_FLOWER_CARD);
                break;

            case NotificationConstant.MEDI_BATTLE_PLAYZHIGANG:
                PlayAct(PlayerActType.ZHI_GANG);
                break;
            case NotificationConstant.MEDI_BATTLE_PLAYHU:
                PlayHuAction();
                break;
            case NotificationConstant.MEDI_BATTLEVIEW_HIDENRECORDING:
                View.recordingIcon.SetActive(false);
                // AudioSystem.Instance.ResumeBgm();
                break;
            //case NotificationConstant.MEDI_BATTLEVIEW_UPDATEVOLUME:
            //    UpdateRecorcdVolume((float)notification.Body);
            //    break;
            case NotificationConstant.MEDI_BATTLEVIEW_SHOWPLAYINGVOICE:
                ShowVoicePlayIcon((int)notification.Body);
                break;
            case NotificationConstant.MEDI_BATTLEVIEW_HIDENPLAYINGVOICE:
                HidenVoicePlayIcon((int)notification.Body);
                break;
            case NotificationConstant.MEDI_BATTLEVIEW_SHOWCHAT:
                ShowChatInfo(notification.Body as PushSendChatS2C);
                break;
            case NotificationConstant.MEDI_BATTLEVIEW_SHOWFACE:
                ShowFace(notification.Body as PushSendChatS2C);
                break;
            case NotificationConstant.MEDI_BATTLEVIEW_SHOW_REPORTVIEW:
                ShowReportView();
                break;
            //case NotificationConstant.MEDI_BATTLEVIEW_UPDATEONLINE:
            //    SetOnline((int)notification.Body);
            //    break;
            case NotificationConstant.TING_UPDATE:
                UpdateTingIcon();
                break;
            case NotificationConstant.MEDI_BATTLE_SHOWJING:
                ShowJing();
                break;
        }
    }

    /// <summary>
    /// 初始化界面显示
    /// </summary>
    private void InitView()
    {
        UpdateAllHeadItem(false);
        if (battleProxy.isStart)
        {
            if (battleProxy.playerActTipS2C != null)
            {
                ShowPlayActTip();
                ShowJing();
            }
            UpdateTingIcon();
            //View.masterView.UpdateMasterInfo();
            //UpdateBankerIcon();
        }
        View.readyBtn.gameObject.SetActive(!battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID].isReady);
        //View.masterView.gameObject.SetActive(battleProxy.isStart);
        DontUpdateRoomInfo();
        if (battleProxy.isReport)
        {
            ShowReportView();
        }
        UpdateInviteBtn();
        if (battleProxy.hasDisloveApply)
        {
            UIManager.Instance.ShowUI(UIViewID.DISLOVE_STATISTICS_VIEW);
            if (battleProxy.agreeIds.IndexOf(playerInfoProxy.UserInfo.UserID) == -1 && battleProxy.refuseIds.IndexOf(playerInfoProxy.UserInfo.UserID) == -1)
            {
                UIManager.Instance.ShowUI(UIViewID.DISLOVE_APPLY_VIEW);
            }
           
        }
    }

    /// <summary>
    /// 上次点击准备的时间
    /// </summary>
    private float perClickTime = 0;

    /// <summary>
    /// 点击准备按钮
    /// </summary>
    private void onReadyClick()
    {
        if (Time.time - perClickTime < 1)
        {
            return;
        }
        perClickTime = Time.time;
        var readyC2S = new ReadyC2S();
        NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.READY_C2S.GetHashCode(), 0, readyC2S);
    }

    /// <summary>
    /// 更新所有头像
    /// </summary>
    private void UpdateAllHeadItem(bool isFirstMatch)
    {
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        for (var i = 0; i < View.headItemList.Count; i++)
        {
            var nextSit = GlobalData.GetNextSit(selfInfoVO.sit, i);
            if (!battleProxy.playerSitInfoDic.ContainsKey(nextSit))
            {
                View.headItemList[i].GetComponent<HeadItem>().data = null;
            }
            else
            {
                var nextPlayerInfoVOS2C = battleProxy.playerSitInfoDic[nextSit];
                View.headItemList[i].GetComponent<HeadItem>().data = nextPlayerInfoVOS2C;
                if (isFirstMatch)
                {
                    View.headItemList[i].GetComponent<HeadItem>().HidemBanker();
                }
            }
        }
    }

    /// <summary>
    /// 更新单个头像
    /// </summary>
    /// <param name="updatePlayInfoVOS2C"></param>
    private void UpdateSingleHeadItem(PlayerInfoVOS2C updatePlayInfoVOS2C)
    {
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var updateHeadIndex = (updatePlayInfoVOS2C.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        if (battleProxy.playerIdInfoDic.ContainsKey(updatePlayInfoVOS2C.userId))
        {
            View.headItemList[updateHeadIndex].GetComponent<HeadItem>().data = updatePlayInfoVOS2C;
        }
        else
        {
            View.headItemList[updateHeadIndex].GetComponent<HeadItem>().data = null;
        }
    }

    private void ShowJing()
    {
        View.AlterableCardRoot.SetActive(true);
        View.AlterableCardJingImg.gameObject.SetActive(true);
        View.AlterableCardNumImg.gameObject.SetActive(true);
        int cardValue = battleProxy.treasureCardCode;
        Sprite sCard = Resources.Load<Sprite>("Textures/Card/" + cardValue);
        View.AlterableCardNumImg.sprite = sCard;
    }

    /// <summary>
    /// 更新房间信息
    /// </summary>
    private void UpdateRoomInfo()
    {
        if (sysTimeId > 0)
        {
            Timer.Instance.CancelTimer(sysTimeId);
        }
        sysTimeId = Timer.Instance.AddDeltaTimer(1, 0, 1, UpdateSystemTime);
        UpdateSystemTime();
        var hallProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.HALL_PROXY) as HallProxy;
        View.roomIdTxt.text = hallProxy.HallInfo.RoomCode;
        View.roundTxt.text = string.Format("{0}/{1} 局", battleProxy.curInnings, hallProxy.HallInfo.Innings);
        View.roundNumberTxt.text = string.Format("剩余{0}局", hallProxy.HallInfo.Innings - battleProxy.curInnings);
        View.CardNumberTxt.text = string.Format("剩 {0} 张", battleProxy.leftCard);
        GameMgr.Instance.StartCoroutine(PingIP());
        //TODO...
        //剩余牌数，
    }
    private void DontUpdateRoomInfo()
    {
        if (sysTimeId > 0)
        {
            Timer.Instance.CancelTimer(sysTimeId);
        }
        sysTimeId = Timer.Instance.AddDeltaTimer(1, 0, 1, UpdateSystemTime);
        UpdateSystemTime();
        var hallProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.HALL_PROXY) as HallProxy;
        View.roomIdTxt.text = hallProxy.HallInfo.RoomCode;
        View.roundTxt.text = string.Format("{0}/{1} 局", battleProxy.curInnings, hallProxy.HallInfo.Innings);
        View.roundNumberTxt.text = string.Format("剩余{0}局", hallProxy.HallInfo.Innings - battleProxy.curInnings);
       
        if (battleProxy.isReport)
        {
            View.CardNumberTxt.text = string.Format("剩 {0} 张", 92);
        }
        else if (!battleProxy.isStart)
        {
            View.CardNumberTxt.text = string.Format("剩 {0} 张", 144);
        }
        else
        {
            View.CardNumberTxt.text = string.Format("剩 {0} 张", battleProxy.leftCard);
        }

        GameMgr.Instance.StartCoroutine(PingIP());
        //TODO...
        //剩余牌数，
    }

    /// <summary>
    /// 更新服务器时间
    /// </summary>
    private void UpdateSystemTime()
    {
        if (battleProxy.isReport)
        {//战报显示战报发生时间
            var reportDate = TimeHandle.Instance.GetDateTimeByTimestamp(battleProxy.report.startTime + (long)(Time.time - battleProxy.reportLocalTime) * 1000);
            // View.dateTxt.text = reportDate.ToString("yyyy-MM-dd");
            View.timeTxt.text = reportDate.ToString("HH:mm");
        }
        else
        {
            // View.dateTxt.text = gameMgrProxy.systemDateTime.ToString("yyyy-MM-dd");
            View.timeTxt.text = gameMgrProxy.systemDateTime.ToString("HH:mm");
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            View.Battery.value = (float)AndroidSdkInterface.GetElectricity() / 100f;
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            View.Battery.value = (float)IOSSdkInterface.GetElectricity() / 100f;
        }
        if (Application.internetReachability == UnityEngine.NetworkReachability.NotReachable)
        {
            View.netIcon.sprite = Resources.Load<Sprite>("Textures/NetPoor");
        }
        else if (gameMgrProxy.pingBackMS < 100)
        {
            View.netIcon.sprite = Resources.Load<Sprite>("Textures/NetPerfect");
        }
        else if (gameMgrProxy.pingBackMS < 200)
        {

            View.netIcon.sprite = Resources.Load<Sprite>("Textures/NetGood");
        }
        else
        {
            View.netIcon.sprite = Resources.Load<Sprite>("Textures/NetOK");
        }
    }

    private IEnumerator PingIP()
    {
        var ping = new Ping(GlobalData.LoginServer);
        while (!ping.isDone)
        {
            yield return null;
        }
        gameMgrProxy.pingBackMS = ping.time;
        yield return new WaitForSeconds(10);
        GameMgr.Instance.StartCoroutine(PingIP());
    }

    /// <summary>
    /// 显示庄家旋转标志
    /// </summary>
    private void PlayRotate(bool isFirstMatch)
    {
        //View.masterView.gameObject.SetActive(true);
        //if (isFirstMatch)//第一局需要转庄家
        //{
        //    View.masterView.PlayRotate();
        //}
        //else
        //{
        //    View.masterView.UpdateMasterInfo(true);
        //    View.masterView.ShowPlayActTip();  
        //   SendNotification(NotificationConstant.MEDI_BATTLE_SENDCARD);
        //} 
        //for (var i = 0; i < View.headItemList.Count; i++)
        //{
        //    View.headItemList[i].GetComponent<HeadItem>().HidenReady();
        //}
    }

    /// <summary>
    /// 头像框显示庄家标志
    /// </summary>
    private void UpdateBankerIcon()
    {
        var bankerInfoVO = battleProxy.BankerPlayerInfoVOS2C;
        if (bankerInfoVO == null) Debug.Log("bankerInfo is null...");
        for (var i = 0; i < View.headItemList.Count; i++)
        {
            View.headItemList[i].GetComponent<HeadItem>().ShowBankerIcon(bankerInfoVO.userId);
        }
    }

    int tip = 1;
    /// <summary>
    /// 显示玩家操作提示,其他人提示动作时隐藏自己的动作提示
    /// </summary>
    private void ShowPlayActTip()
    {
        //View.masterView.ShowPlayActTip();
        var tipPlayVO = battleProxy.playerActTipS2C;
        if (tipPlayVO == null)
        {
            return;
        }
        //Debug.Log(string.Format("tipPlayVO.optUserId = {0},playerInfoProxy.UserInfo.UserID = {1}", tipPlayVO.optUserId, playerInfoProxy.UserInfo.UserID));
        if (tipPlayVO.optUserId == playerInfoProxy.UserInfo.UserID)
        {
            View.headItemList[0].GetComponent<HeadItem>().ShowPlayActTip();
        }

    }

    /// <summary>
    /// 播放玩家操作提示
    /// </summary>
    /// <param name="playerAct"></param>
    private void PlayAct(PlayerActType playerAct)
    {
        //View.masterView.ShowPlayActTip();
        //var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        //var actPlayerInfoVO = battleProxy.playerIdInfoDic[battleProxy.playerActS2C.userId];
        //var actIndex = (actPlayerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        //if (actIndex != 0)
        //{
        //    View.headItemList[actIndex].GetComponent<HeadItem>().PlayAct(playerAct);
        //}
        //View.headItemList[0].GetComponent<HeadItem>().HidenPlayActTip();

        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var actPlayerInfoVO = battleProxy.playerIdInfoDic[battleProxy.playerActS2C.userId];
        var actIndex = (actPlayerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        if (actIndex != 0)
        {
            View.headItemList[actIndex].GetComponent<HeadItem>().PlayAct(playerAct);
        }
        if (actIndex == 0)
        {
            View.headItemList[0].GetComponent<HeadItem>().HidenPlayActTip();
        }
        else if (playerAct != PlayerActType.PASS && !battleProxy.huTypes.Contains(playerAct))
        {
            View.headItemList[0].GetComponent<HeadItem>().HidenPlayActTip();
        }
    }

    /// <summary>
    /// 播放胡牌动画
    /// </summary>
    private void PlayHuAction()
    {
        //View.masterView.ShowPlayActTip();

        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var actPlayerInfoVO = battleProxy.playerIdInfoDic[battleProxy.playerActS2C.userId];
        var actIndex = (actPlayerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        View.headItemList[actIndex].GetComponent<HeadItem>().PlayHu();
        if (actIndex == 0)
        {
            View.headItemList[0].GetComponent<HeadItem>().HidenPlayActTip();
        }

        battleProxy.isPlayHu = true;

    }

    /// <summary>
    /// 解散房间
    /// </summary>
    private void OnDissloutionClick()
    {
        if (playerInfoProxy.UserInfo.UserID != battleProxy.creatorId && battleProxy.isFirstMatch)
        {
            DialogMsgVO dialogMsgVO = new DialogMsgVO();
            dialogMsgVO.title = "退出确认";
            dialogMsgVO.content = "是否退出房间";
            dialogMsgVO.dialogType = DialogType.CONFIRM;
            dialogMsgVO.confirmCallBack = delegate
            {
                ConfirmExit();
            };
            DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
            dialogView.data = dialogMsgVO;
        }
        else
        {
            DialogMsgVO dialogMsgVO = new DialogMsgVO();
            dialogMsgVO.dialogType = DialogType.CONFIRM;
            dialogMsgVO.title = "解散确认";
            dialogMsgVO.content = "是否解散房间";
            dialogMsgVO.confirmCallBack = delegate
            {
                ConfirmDissloution();
            };
            DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
            dialogView.data = dialogMsgVO;
        }
    }

    /// <summary>
    /// 退出房间确认回调
    /// </summary>
    private void ConfirmExit()
    {
        var exitC2S = new ExitRoomC2S();
        NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.EXIT_C2S.GetHashCode(), 0, exitC2S);
    }

    /// <summary>
    /// 解散房间确认回调
    /// </summary>
    private void ConfirmDissloution()
    {
        if (battleProxy.isStart)
        {
            var disloveC2S = new ApplyDissolveRoomC2S();
            NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.DISLOVEAPPLY_C2S.GetHashCode(), 0, disloveC2S, false);
        }
        else
        {
            if (battleProxy.creatorId == playerInfoProxy.UserInfo.UserID)
            {
                var disloveC2S = new DissolveRoomC2S();
                NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.DISSOLUTION_C2S.GetHashCode(), 0, disloveC2S);
            }
            else
            {
                var disloveC2S = new ApplyDissolveRoomC2S();
                NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.DISLOVEAPPLY_C2S.GetHashCode(), 0, disloveC2S);
            }
        }
    }

    /// <summary>
    /// 伸缩操作区域
    /// </summary>
    private void ExpendOperateView()
    {
        isExpand = !isExpand;
        if (isExpand)
        {
            UpdateDisloveBtn();
        }
        else
        {
            //View.expandBtn.GetComponent<RectTransform>().localEulerAngles = new Vector3(0,0,180);
            View.dissolutionBtn.gameObject.SetActive(false);
            View.exitBtn.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 更新解散房间界面
    /// </summary>
    private void UpdateDisloveBtn()
    {
        if (!isExpand)
        {
            return;
        }
        if (playerInfoProxy.UserInfo.UserID == battleProxy.creatorId)
        {
            View.dissolutionBtn.gameObject.SetActive(true);
            View.exitBtn.gameObject.SetActive(false);
        }
        else
        {
            View.dissolutionBtn.gameObject.SetActive(false);
            View.exitBtn.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 按下录音按钮
    /// </summary>
    /// <param name="arg0"></param>
    private void OnVoiceDown(BaseEventData arg0)
    {
        View.recordingIcon.SetActive(true);
        // AudioSystem.Instance.PauseBgm();
        SendNotification(NotificationConstant.MEDI_BATTLEREA_STARTRECORD);
    }

    /// <summary>
    /// 松开录音按钮
    /// </summary>
    /// <param name="arg0"></param>
    private void OnVoiceUp(BaseEventData arg0)
    {
        View.recordingIcon.SetActive(false);
        // AudioSystem.Instance.ResumeBgm();
        SendNotification(NotificationConstant.MEDI_BATTLEREA_STOPRECORD);
    }

    /// <summary>
    /// 更新录音音量
    /// </summary>
    private void UpdateRecorcdVolume(float volume)
    {
        int value = Mathf.RoundToInt(volume);
        Sprite targetSprite = Resources.Load<Sprite>(string.Format("Textures/RecordIcon/RecordIcon{0}", value));
        View.recordingIcon.GetComponent<Image>().sprite = targetSprite;
    }

    /// <summary>
    /// 显示语言播放标志
    /// </summary>
    private void ShowVoicePlayIcon(int userId)
    {
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var voicePlayerInfo = battleProxy.playerIdInfoDic[userId];
        var updateHeadIndex = (voicePlayerInfo.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        View.headItemList[updateHeadIndex].GetComponent<HeadItem>().ShowVoicePlayIcon();
    }

    /// <summary>
    /// 隐藏语言播放标志
    /// </summary>
    private void HidenVoicePlayIcon(int userId)
    {
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var voicePlayerInfo = battleProxy.playerIdInfoDic[userId];
        var updateHeadIndex = (voicePlayerInfo.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        View.headItemList[updateHeadIndex].GetComponent<HeadItem>().HidenVoicePlayIcon();
    }

    /// <summary>
    /// 显示聊天信息
    /// </summary>
    /// <param name="chatS2C"></param>
    private void ShowChatInfo(PushSendChatS2C chatS2C)
    {
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var chatPlayerInfo = battleProxy.playerIdInfoDic[chatS2C.senderUserId];
        var updateHeadIndex = (chatPlayerInfo.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        View.headItemList[updateHeadIndex].GetComponent<HeadItem>().ShowChatInfo(chatS2C.content);
        var chatIndex = Array.IndexOf(GlobalData.Chat_Const, chatS2C.content);
        if (chatIndex != -1)
        {
            chatIndex += 1;
            string voiceUrl = string.Empty;
            if (chatPlayerInfo.sex == 0)
            {
                voiceUrl = string.Format("Voices/Woman/{0}", chatIndex);
            }
            else
            {
                voiceUrl = string.Format("Voices/Man/{0}", chatIndex);
            }
            GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio(voiceUrl));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="chatS2C"></param>
    private void ShowFace(PushSendChatS2C chatS2C)
    {
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var chatPlayerInfo = battleProxy.playerIdInfoDic[chatS2C.senderUserId];
        var updateHeadIndex = (chatPlayerInfo.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        int faceIndex = int.Parse(chatS2C.content.Replace(GlobalData.FACE_PREFIX, ""));
        View.headItemList[updateHeadIndex].GetComponent<HeadItem>().ShowFace(faceIndex);
    }

    /// <summary>
    /// 显示播放界面
    /// </summary>
    private void ShowReportView()
    {
        View.reportView.SetActive(true);
        View.reportView.GetComponent<ReportView>().PlayReport();
    }

    /// <summary>
    /// 点击聊天按钮
    /// </summary>
    private void OnChatClick()
    {
        UIManager.Instance.ShowUI(UIViewID.CHAT_VIEW);
    }

    /// <summary>
    /// 打开设置界面
    /// </summary>
    private void OnSettingClick()
    {
        UIManager.Instance.ShowUI(UIViewID.SETTING_VIEW);
    }

    /// <summary>
    /// 更新准备按钮
    /// </summary>
    private void UpdateReadyBtn()
    {
        View.readyBtn.gameObject.SetActive(!battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID].isReady);
    }

    /// <summary>
    /// 更新邀请按钮
    /// </summary>
    private void UpdateInviteBtn()
    {
        if (battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID].isReady && battleProxy.playerIdInfoDic.Count < GlobalData.SIT_NUM)
        {
            View.inviteBtn.gameObject.SetActive(true);
        }
        else
        {
            View.inviteBtn.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 更新听牌图标
    /// </summary>
    private void UpdateTingIcon()
    {
        View.headItemList[0].GetComponent<HeadItem>().UpdateTingIcon();
    }

    /// <summary>
    /// 邀请好友
    /// </summary>
    private void OnInviteClick()
    {
        var hallProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.HALL_PROXY) as HallProxy;
        string inviteUrl = string.Format("{0}?{1}={2}", GlobalData.ShareUrl, StartUpParam.ROOMID, hallProxy.HallInfo.RoomCode);
        string title = string.Format("房间号：{0} 全民麻将", hallProxy.HallInfo.RoomCode);
        string desc = string.Format("我在(全民约牌吧)开了{0}局，{1}风的4人房间，快来一起玩吧！", hallProxy.HallInfo.CompetitionRound.GetHashCode(), hallProxy.HallInfo.CompetitionRule == GameRule.NOT_WORD ? "无" : "有");
        if (GlobalData.sdkPlatform == SDKPlatform.ANDROID)
        {
            AndroidSdkInterface.WeiXinShare(inviteUrl, title, desc, false);
        }
        else if (GlobalData.sdkPlatform == SDKPlatform.IOS)
        {
            IOSSdkInterface.WeiXinShare(inviteUrl, title, desc, false);
        }
    }

    /// <summary>
    /// 设置离线标志
    /// </summary>
    //private void SetOnline(int userId)
    //{
    //    //var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
    //    //var offlinePlayerVO = battleProxy.playerIdInfoDic[userId];
    //    //var actIndex = (offlinePlayerVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
    //    //if (actIndex != 0)
    //    //{
    //    //   // View.headItemList[actIndex].GetComponent<HeadItem>().UpdateOnline(offlinePlayerVO.isOnline);
    //    //}
    //}
}
