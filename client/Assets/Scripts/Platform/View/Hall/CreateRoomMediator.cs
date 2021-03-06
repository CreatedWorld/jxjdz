﻿using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using Platform.Net;
using Platform.Model;
using UnityEngine;

public class CreateRoomMediator : Mediator, IMediator
{
    private HallProxy hallProxy;
    private PlayerInfoProxy playerInfoProxy;
    public CreateRoomMediator(string NAME, object viewComponent) : base(NAME, viewComponent)
    {

    }

    public CreateRoomView View
    {
        get
        {
            return (CreateRoomView)ViewComponent;
        }
    }
    public override void OnRegister()
    {
        base.OnRegister();
        this.hallProxy = Facade.RetrieveProxy(Proxys.HALL_PROXY) as HallProxy;
        this.playerInfoProxy = Facade.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
        this.View.ButtonAddListening(this.View.CloseButton,
            () =>
            {
                UIManager.Instance.HideUI(UIViewID.CREATEROOM_VIEW);
            });
        this.View.FourModeInfo.Toggle.onValueChanged.AddListener(
            (bool isOn) =>
            {
                this.ChangeGameModel(isOn,this.View.FourModeInfo.Model);
                this.View.NotWord.Toggle.interactable = false;
            });
        this.View.EightModeInfo.Toggle.onValueChanged.AddListener(
            (bool isOn) =>
            {
                this.ChangeGameModel(isOn, this.View.EightModeInfo.Model);
                this.View.NotWord.Toggle.interactable = false;
            });
        this.View.SixteenModeInfo.Toggle.onValueChanged.AddListener(
            (bool isOn) =>
            {
                this.ChangeGameModel(isOn, this.View.SixteenModeInfo.Model);
                this.View.NotWord.Toggle.interactable = true;
            });
        this.View.CapScore.Toggle.onValueChanged.AddListener(
            (bool isOn)=>
            {
                this.ChangeGameCap(isOn,this.View.CapScore.Cap);
            }
            );
        this.View.NotWord.Toggle.onValueChanged.AddListener(
            (bool isOn)=> 
            {
                this.View.FourModeInfo.Toggle.interactable = false;
                this.View.EightModeInfo.Toggle.interactable = false;
                this.ChangeGameRule(isOn, this.View.NotWord.Rule);
            });
        this.View.Word.Toggle.onValueChanged.AddListener(
            (bool isOn)=>
            {
                this.View.FourModeInfo.Toggle.interactable = true;
                this.View.EightModeInfo.Toggle.interactable = true;
                this.ChangeGameRule(isOn, this.View.Word.Rule);
            }
            );
        this.View.ButtonAddListening(this.View.CreateButton, SendCreateRoom);
        this.hallProxy.HallInfo.Innings = (int)(GameMode.FOUR_ROUND);
    }

    public override void OnRemove()
    {
        base.OnRemove();
        UIManager.Instance.DestroyUI(UIViewID.CREATEROOM_VIEW);
    }
    public override IList<string> ListNotificationInterests()
    {
        IList<string> list = new List<string>();
        return list;
    }
    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            default:
                break;
        }
    }
    private void SendCreateRoom()
    {
        if (this.CheckRoomCard ())
        {
            this.hallProxy.HallInfo.CapScore = (this.View.CapScore.Toggle.isOn) ? 21 : 22;
            this.hallProxy.HallInfo.PayType = (this.View.NotWord.Toggle.isOn) ? 20 : 19;

            CheckCreateRoomC2S package = new CheckCreateRoomC2S();
            package.roomRounds = this.hallProxy.HallInfo.Innings;
            package.playType.Add((this.View.NotWord.Toggle.isOn) ? 20 : 19);
            package.playType.Add(this.hallProxy.HallInfo.CapScore);
            package.roomRule = roomTypeInt();
            NetMgr.Instance.SendBuff<CheckCreateRoomC2S>(SocketType.HALL, MsgNoC2S.REQUEST_CREATEROOM_C2S.GetHashCode(), 0, package);

            Debug.Log(package.roomRule);
        }
        else
        {
            DialogMsgVO dialogVO = new DialogMsgVO();
            dialogVO.dialogType = DialogType.ALERT;
            dialogVO.title = "创建房间失败";
            dialogVO.content = "您的房卡不足,请充值";
            DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
            dialogView.data = dialogVO;
        }
    }

    private int roomTypeInt()
    {
        switch (hallProxy.HallInfo.Innings)
        {
            case 4:
                return 1;
            case 8:
                return 2;
            default:
                return 3;
        }
    }



    private bool CheckRoomCard()
    {
        int cost;
        switch (this.hallProxy.HallInfo.Innings)
        {
            case (int)GameMode.FOUR_ROUND:
                cost = 1;
                break;
            case (int)GameMode.EIGHT_ROUND:
                cost = 2;
                break;
            case (int)GameMode.SIXTEEN_ROUND:
                cost = 4;
                break;
            default:
                cost = 0;
                break;
        }
        if (!this.playerInfoProxy.UserInfo.UserItems.ContainsKey(ItemType.ROOMCARD))
        {
            return false;
        }
        int roomCard = this.playerInfoProxy.UserInfo.UserItems[ItemType.ROOMCARD].amount;
        return roomCard >= cost;
    }
    private void ChangeGameModel(bool isOn, GameMode model)
    {
        if (isOn)
        {
            this.hallProxy.HallInfo.Innings = (int)((GameMode)model);
        }
    }
    private void ChangeGameCap(bool isOn,CapScore cap)
    {
        if (isOn)
        {
            this.hallProxy.HallInfo.CapScore = (int)((CapScore)cap);
        }
    }
    private void ChangeGameRule(bool isOn,GameRule rule)
    {   
        if (isOn)
        {
            this.hallProxy.HallInfo.GameRule = rule;
        }
    }
}