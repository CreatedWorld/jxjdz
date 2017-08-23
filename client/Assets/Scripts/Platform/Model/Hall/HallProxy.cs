using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using Platform.Net;
using Platform.Model;
using UnityEngine.SceneManagement;
using System.Net;
using Platform.Utils;
using Utils;
using Platform.Global;
using System;
using LZR.Data.NetWork.Client;
/// <summary>
/// 大厅数据代理
/// </summary>
public class HallProxy : Proxy, IProxy
{
    /// <summary>
    /// 进入战斗的方式
    /// </summary>
    private string battleMsgNo;

    public HallInfoVO HallInfo
    {
        get
        {
            return (HallInfoVO)this.Data;
        }
    }

    public HallProxy(string NAME) : base(NAME, new HallInfoVO())
    {
        this.HallInfo.CompetitionRule = GameRule.WORD;
        this.HallInfo.CompetitionRound = GameMode.EIGHT_ROUND;
    }

    public override void OnRegister()
    {
        GameMgr.Instance.AddMsgHandler(MsgNoS2C.RESPONSE_GETSIGNININFO_S2C, GetSignInInfoResponse);
        GameMgr.Instance.AddMsgHandler(MsgNoS2C.RESPONSE_USERSIGNIN_S2C, RefreshSignInInfoResponse);
        GameMgr.Instance.AddMsgHandler(MsgNoS2C.RESPONSE_CREATEROOM_S2C, CreateRoomResponse);
        GameMgr.Instance.AddMsgHandler(MsgNoS2C.RESPONSE_JOINROOM_S2C, JoinRoomResponse);
        GameMgr.Instance.AddMsgHandler(MsgNoS2C.RESPONSE_APPLYCOMPETITION_S2C, ApplySucceed);
        GameMgr.Instance.AddMsgHandler(MsgNoS2C.RESPONSE_CHECKAPPLYSTATE_S2C, CheckApplyState);
        GameMgr.Instance.AddMsgHandler(MsgNoS2C.RESPONSE_GETGRADEINFO_S2C, GetGradeInfo);
        GameMgr.Instance.AddMsgHandler(MsgNoS2C.RESPONSE_GETROUNDINFO_S2C, GetRoundInfo);
        GameMgr.Instance.AddMsgHandler(MsgNoS2C.RESPONSE_APPLYMATCHING_S2C, StartMatching);
        GameMgr.Instance.AddMsgHandler(MsgNoS2C.RESPONSE_GETCHECKINREWARD_S2C, GetCheckInreward);
        GameMgr.Instance.AddMsgHandler(MsgNoS2C.RESPONSE_CHECKINVITATIONCODE_S2C, InviteCodeSucceed);
        GameMgr.Instance.AddMsgHandler(MsgNoS2C.RESPONSE_PUSHANNOUNCEMENT_S2C, UpdateAnnouncementContent);
        GameMgr.Instance.AddMsgHandler(MsgNoS2C.RESPONSE_NOTICECONFIG_S2C, GetNoticeInfo);
        GameMgr.Instance.AddMsgHandler(MsgNoS2C.RESPONSE_MATCHINGSUCCEED_S2C, MatchingSucceed);
        GameMgr.Instance.AddMsgHandler(MsgNoS2C.RECONNECT_S2C, ReConnectHandler);
        GameMgr.Instance.AddMsgHandler(MsgNoS2C.RESPONSE_CANCELMATCHING_S2C, CancelMatching);
        GameMgr.Instance.AddMsgHandler(MsgNoS2C.RESPONSE_GetRankingListC2S, GetRankingListHandler);

    }
    /// <summary>
    /// 接收排行榜信息
    /// </summary>
    /// <param name="bytes"></param>
    private void GetRankingListHandler(byte[] bytes)
    {
        GetRankingListS2C package = NetMgr.Instance.DeSerializes<GetRankingListS2C>(bytes);
        HallInfo.userInfo = package.userInfo;

        //for (int i =0;i<HallInfo.userInfo.Count;i++)
        //{
        //     Debug.Log("win:"+ HallInfo.userInfo[i].win+" id:" + HallInfo.userInfo[i].userId +
        //          "  name:" + HallInfo.userInfo[i].userName + "  ImgURL:" + HallInfo.userInfo[i].imageUrl);
        //}
        SendNotification(NotificationConstant.MEDI_HALL_RANKINGIFON, package);
    }

    /// <summary>
    /// 登陆大厅获取签到信息
    /// </summary>
    /// <param name="bytes">签到信息</param>
    private void GetSignInInfoResponse(byte[] bytes)
    {
        GetCheckInInfoS2C package = NetMgr.Instance.DeSerializes<GetCheckInInfoS2C>(bytes);
        this.HallInfo.SignInDay = package.days;
        this.HallInfo.SignInState = package.status;
    }

    /// <summary>
    /// 刷新签到数据
    /// </summary>
    /// <param name="bytes">签到信息</param>
    private void RefreshSignInInfoResponse(byte[] bytes)
    {
        CheckInS2C package = NetMgr.Instance.DeSerializes<CheckInS2C>(bytes);
        this.HallInfo.SignInDay = package.days;
        this.HallInfo.SignInState = package.status;
        Facade.SendNotification(NotificationConstant.MEDI_SIGNIN_REFRESHSIGN);//发送刷新签到视图消息
    }
    /// <summary>
    /// 创建房间响应
    /// </summary>
    /// <param name="bytes"></param>
    private void CreateRoomResponse(byte[] bytes)
    {
        CheckCreateRoomS2C package = NetMgr.Instance.DeSerializes<CheckCreateRoomS2C>(bytes);
        if (package.clientCode == (int)ErrorCode.SUCCESS)
        {
            this.HallInfo.BattleSeverIP = package.roomServerIp;
            this.HallInfo.BattleSeverPort = package.roomServerPort;
            this.HallInfo.RoomCode = package.roomCode;
            this.HallInfo.Seat = package.seat;
            NSocket.RoomId = HallInfo.RoomCode == "" ? 0 : int.Parse(HallInfo.RoomCode);
            this.BattleServerConnect(NotificationConstant.MEDI_HALL_CUTCREATESCENE);
        }
        else
        {
            DialogMsgVO dialogVO = new DialogMsgVO();
            dialogVO.dialogType = DialogType.ALERT;
            dialogVO.title = "创建房间失败";
            dialogVO.content = "创建房间失败,请检查网络连接";
            DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
            dialogView.data = dialogVO;
        }
    }
    /// <summary>
    /// 加入房间响应
    /// </summary>
    /// <param name="bytes"></param>
    private void JoinRoomResponse(byte[] bytes)
    {
        JoinInRoomS2C package = NetMgr.Instance.DeSerializes<JoinInRoomS2C>(bytes);
        if (package.clientCode == (int)ErrorCode.SUCCESS)
        {
            this.HallInfo.Innings = package.roomRounds;
            this.HallInfo.GameRule = (GameRule)package.roomRule;
            this.HallInfo.BattleSeverIP = package.roomServerIp;
            this.HallInfo.BattleSeverPort = package.roomServerPort;
            this.HallInfo.Seat = package.seat;
            NSocket.RoomId = HallInfo.RoomCode == "" ? 0 : int.Parse(HallInfo.RoomCode);
            this.BattleServerConnect(NotificationConstant.MEDI_HALL_CUTJOINSCENE);
        }
        else if (package.clientCode == (int)ErrorCode.NO_ROOM)
        {
            DialogMsgVO dialogVO = new DialogMsgVO();
            dialogVO.dialogType = DialogType.ALERT;
            dialogVO.title = "加入提示";
            dialogVO.content = "房间不存在";
            DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
            dialogView.data = dialogVO;
        }
        else if (package.clientCode == (int)ErrorCode.OVERFLOW_ROOM_PLAYERS)
        {
            DialogMsgVO dialogVO = new DialogMsgVO();
            dialogVO.dialogType = DialogType.ALERT;
            dialogVO.title = "加入提示";
            dialogVO.content = "房间人数已满！";
            DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
            dialogView.data = dialogVO;
        }
        else
        {
            DialogMsgVO dialogVO = new DialogMsgVO();
            dialogVO.dialogType = DialogType.ALERT;
            dialogVO.title = "加入房间失败";
            dialogVO.content = "加入房间失败,请检查网络连接";
            DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
            dialogView.data = dialogVO;
        }
    }
    /// <summary>
    /// 报名成功消息
    /// </summary>
    /// <param name="bytes"></param>
    private void ApplySucceed(byte[] bytes)
    {
        ApplyCompetitionS2C package = NetMgr.Instance.DeSerializes<ApplyCompetitionS2C>(bytes);
        if (package.status == 1)
        {
            Facade.SendNotification(NotificationConstant.MEDI_HALL_APPLYSUCCEED);
        }
        else
        {
            DialogMsgVO dialogVO = new DialogMsgVO();
            dialogVO.dialogType = DialogType.ALERT;
            dialogVO.title = "报名失败";
            dialogVO.content = "报名失败,请检查网络连接";
            DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
            dialogView.data = dialogVO;
        }
    }
    /// <summary>
    /// 检查报名时间
    /// </summary>
    private void CheckApplyState(byte[] bytes)
    {
        CheckApplyStatusS2C package = NetMgr.Instance.DeSerializes<CheckApplyStatusS2C>(bytes);
        int state = package.status;

        if (state == 0)
        {
            UIManager.Instance.ShowUI(UIViewID.COMPETITION_VIEW);//报名界面
        }
        else if (state == 1)
        {
            this.HallInfo.CurrentTime = package.currentTime;
            this.HallInfo.StartTime = package.startTime;
            this.HallInfo.EndTime = package.endTime;
            UIManager.Instance.ShowUI(UIViewID.ATHLETICS_VIEW);//比赛界面
        }
    }

    /// <summary>
    /// 连接游戏服务器
    /// </summary>
    /// <param name="msgNo"></param>
    private void BattleServerConnect(string msgNo)
    {
        battleMsgNo = msgNo;
        NetMgr.Instance.CreateConnect(SocketType.BATTLE, HallInfo.BattleSeverIP, HallInfo.BattleSeverPort, BattleConnectHandler);
    }

    /// <summary>
    /// 战斗服务器连接回调
    /// </summary>
    private void BattleConnectHandler()
    {
        Debug.Log("连接游戏服务器成功");
        NetMgr.Instance.ConnentionDic[SocketType.BATTLE].OnConnectSuccessful = null;
        var battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as Platform.Model.Battle.BattleProxy;
        battleProxy.isReport = false;
        if (battleMsgNo == NotificationConstant.MEDI_HALL_CUTCREATESCENE)
        {
            NetMgr.Instance.SendBuff<EnterRoomServerC2S>(SocketType.BATTLE, MsgNoC2S.ENTER_ROOMSERVER_C2S.GetHashCode(), 0, new EnterRoomServerC2S());
        }
        else if (battleMsgNo == NotificationConstant.MEDI_HALL_CUTJOINSCENE)
        {
            JoinRoomC2S joinC2S = new JoinRoomC2S();
            joinC2S.seat = this.HallInfo.Seat;
            joinC2S.roomCode = this.HallInfo.RoomCode;
            NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.JOIN_ROOM_C2S.GetHashCode(), 0, joinC2S);
        }
    }

    /// <summary>
    /// 获取战绩信息
    /// </summary>
    /// <param name="bytes"></param>
    private void GetGradeInfo(byte[] bytes)
    {
        GetGradeInfoS2C package = NetMgr.Instance.DeSerializes<GetGradeInfoS2C>(bytes);
        if (package != null)
        {
            SendNotification(NotificationConstant.MEDI_HALL_INITGRADEINFO, package);
        }
    }

    /// <summary>
    /// 获取房间具体对战信息
    /// </summary>
    /// <param name="bytes"></param>
    private void GetRoundInfo(byte[] bytes)
    {
        GetRoundInfoS2C package = NetMgr.Instance.DeSerializes<GetRoundInfoS2C>(bytes);
        if (package != null)
        {
            SendNotification(NotificationConstant.MEDI_HALL_GETROUNDINFO, package);
        }
    }
    /// <summary>
    /// 匹配
    /// </summary>
    /// <param name="bytes"></param>
    private void StartMatching(byte[] bytes)
    {
        JoinCompetitionS2C package = NetMgr.Instance.DeSerializes<JoinCompetitionS2C>(bytes);
        if (package.status == 1)
        {
            Debug.Log("开始匹配");
            UIManager.Instance.HideUI(UIViewID.ATHLETICS_VIEW,
                () =>
                {
                    UIManager.Instance.ShowUI(UIViewID.MATCHING_VIEW);
                });
        }
        else if (package.status == 0)
        {
            DialogMsgVO dialogVO = new DialogMsgVO();
            dialogVO.dialogType = DialogType.ALERT;
            dialogVO.title = "匹配失败";
            dialogVO.content = "匹配失败,请检查网络连接";
            DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
            dialogView.data = dialogVO;
        }
    }
    /// <summary>
    /// 领取房卡
    /// </summary>
    /// <param name="bytes"></param>
    private void GetCheckInreward(byte[] bytes)
    {
        GetCardInfoS2C package = NetMgr.Instance.DeSerializes<GetCardInfoS2C>(bytes);
        if (package.status == 1)
        {
            Debug.Log("领取奖励成功");
            UIManager.Instance.HideUI(UIViewID.SIGNIN_VIEW);
            PlayerInfoProxy playerProxy = Facade.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            foreach (UserItem item in package.userItems)
            {
                if (playerProxy.UserInfo.UserItems.ContainsKey(item.type))
                {
                    playerProxy.UserInfo.UserItems[item.type] = item;
                }
                else
                {
                    playerProxy.UserInfo.UserItems.Add(item.type, item);
                }
            }
            SendNotification(NotificationConstant.MEDI_HALL_REFRESHITEM);
        }
        else
        {
            DialogMsgVO dialogVO = new DialogMsgVO();
            dialogVO.dialogType = DialogType.ALERT;
            dialogVO.title = "领取奖励失败";
            dialogVO.content = "领取奖励失败,请检查网络连接";
            DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
            dialogView.data = dialogVO;
        }
    }
    /// <summary>
    /// 验证码检验成功
    /// </summary>
    /// <param name="bytes"></param>
    private void InviteCodeSucceed(byte[] bytes)
    {
        CheckInvitationCodeS2C package = NetMgr.Instance.DeSerializes<CheckInvitationCodeS2C>(bytes);
        if (package.status == ErrorCode.SUCCESS)
        {
            UIManager.Instance.HideUI(UIViewID.INVITE_VIEW,
                () =>
                {
                    UIManager.Instance.ShowUI(UIViewID.SHOPPING_VIEW);
                });
            PlayerInfoProxy pip = Facade.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            if (pip.UserInfo.BoundAgency == ErrorCode.FAILT)
            {
                pip.UserInfo.BoundAgency = ErrorCode.SUCCESS;
            }
        }
        else
        {
            DialogMsgVO dialogVO = new DialogMsgVO();
            dialogVO.dialogType = DialogType.ALERT;
            dialogVO.title = "输入验证码错误";
            dialogVO.content = "输入验证码错误,请重新输入";
            DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
            dialogView.data = dialogVO;
        }
        ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_HALL_CLEARINPUTTEXT);
    }
    /// <summary>
    /// 更新公告内容
    /// </summary>
    /// <param name="bytes"></param>
    private void UpdateAnnouncementContent(byte[] bytes)
    {
        PushAnnouncementS2C package = NetMgr.Instance.DeSerializes<PushAnnouncementS2C>(bytes);
        this.HallInfo.AnnouncementQueue.Enqueue(new AnnouncementData(package.content, package.cirCount));
        SendNotification(NotificationConstant.MEDI_HALL_REFRESHANNOUNCEMENT);
    }
    /// <summary>
    /// 更新大厅公告
    /// </summary>
    private void GetNoticeInfo(byte[] bytes)
    {
        NoticeConfigS2C package = NetMgr.Instance.DeSerializes<NoticeConfigS2C>(bytes);
        foreach (NoticeConfigDataS2C data in package.noticeConfigData)
        {
            if (this.HallInfo.NoticeList.ContainsKey((HallNoticeType)data.type))
            {
                this.HallInfo.NoticeList[(HallNoticeType)data.type] = data;
            }
            else
            {
                this.HallInfo.NoticeList.Add((HallNoticeType)data.type, data);
            }
        }
    }
    /// <summary>
    /// 匹配成功
    /// </summary>
    /// <param name="bytes"></param>
    private void MatchingSucceed(byte[] bytes)
    {
        Debug.Log("匹配成功");
        MatchingSucceedS2C package = NetMgr.Instance.DeSerializes<MatchingSucceedS2C>(bytes);
        this.HallInfo.RoomCode = package.roomCode.ToString();
        this.HallInfo.Innings = package.roomRounds;
        this.HallInfo.GameRule = (GameRule)package.roomRule;
        this.HallInfo.BattleSeverIP = package.roomServerIp;
        this.HallInfo.BattleSeverPort = package.roomServerPort;
        this.HallInfo.Seat = package.seat;
        this.BattleServerConnect(NotificationConstant.MEDI_HALL_CUTJOINSCENE);
    }
    private void CancelMatching(byte[] bytes)
    {
        CancelMatchingS2C package = NetMgr.Instance.DeSerializes<CancelMatchingS2C>(bytes);
        if (package.status == (int)ErrorCode.SUCCESS)
        {
            UIManager.Instance.HideUI(UIViewID.MATCHING_VIEW);
        }
        else
        {
            DialogMsgVO dialogVO = new DialogMsgVO();
            dialogVO.dialogType = DialogType.ALERT;
            dialogVO.title = "取消匹配失败";
            dialogVO.content = "取消匹配失败,请检查网络连接";
            DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
            dialogView.data = dialogVO;
        }
    }

    /// <summary>
    /// 断线重连
    /// </summary>
    /// <param name="bytes"></param>
    private void ReConnectHandler(byte[] bytes)
    {
        ReConnectS2C reconnectS2C = NetMgr.Instance.DeSerializes<ReConnectS2C>(bytes);
        this.HallInfo.RoomCode = reconnectS2C.roomCode.ToString();
        this.HallInfo.BattleSeverIP = reconnectS2C.roomIp;
        this.HallInfo.BattleSeverPort = reconnectS2C.roomPort;
        NSocket.RoomId = HallInfo.RoomCode == "" ? 0 : int.Parse(HallInfo.RoomCode);
        if (reconnectS2C.roomId > 0)
        {
            this.BattleServerConnect(NotificationConstant.MEDI_HALL_CUTJOINSCENE);
        }
        else
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                SendNotification(NotificationConstant.MEDI_LOGIN_SWITCHHALLSCENE);
            }
            else
            {
                SendNotification(NotificationConstant.MEDI_LOGIN_SWITCHHALLSCENE);
            }
        }
    }
}
