﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 战斗UI界面
/// </summary>
public class BattleView : UIView
{   
    /// <summary>
    /// 当前日期
    /// </summary>
    public Text dateTxt;
    /// <summary>
    /// 当前时间
    /// </summary>
    public Text timeTxt;
    /// <summary>
    /// 房间id
    /// </summary>
    public Text roomIdTxt;
    /// <summary>
    /// 扣房卡方式
    /// </summary>
    public Text roundTxt;
    /// <summary>
    /// 
    /// </summary>
    public Text leftCardNumTxt;
    /// <summary>
    /// 当前回合、局数信息
    /// </summary>
    public Text roundNumberTxt;
    /// <summary>
    /// 剩余牌数
    /// </summary>
    public Text CardNumberTxt;
    /// <summary>
    /// 手机信号图标
    /// </summary>
    public Image netIcon;
    /// <summary>
    /// 手机电量图标
    /// </summary>
    public Slider Battery;

    /// <summary>
    /// 房间操作区域
    /// </summary>
    public GameObject roomOperateView;
    /// <summary>
    /// 操作栏展开、缩回按钮
    /// </summary>
    //public Button expandBtn;
    /// <summary>
    /// 解散按钮
    /// </summary>
    public Button dissolutionBtn;
    /// <summary>
    /// 退出房间按钮
    /// </summary>
    public Button exitBtn;

    /// <summary>
    /// 发送语音
    /// </summary>
    public Button voiceBtn;
    /// <summary>
    /// 正在录音标志
    /// </summary>
    public GameObject recordingIcon;
    /// <summary>
    /// 发送快速聊天
    /// </summary>
    public Button chatBtn;
    /// <summary>
    /// 设置按钮
    /// </summary>
    public Button settingBtn;

    /// <summary>
    /// 底部玩家头像
    /// </summary>
    public GameObject downHead;
    /// <summary>
    /// 左边玩家头像
    /// </summary>
    public GameObject leftHead;
    /// <summary>
    /// 右边玩家头像
    /// </summary>
    public GameObject rightHead;
    /// <summary>
    /// 顶部头像
    /// </summary>
    public GameObject upHead;

    /// <summary>
    /// 头像列表
    /// </summary>
    public List<GameObject> headItemList;

    /// <summary>
    /// 中央转圈标志
    /// </summary>
    //public MasterView masterView;

    /// <summary>
    /// 准备按钮
    /// </summary>
    public Button readyBtn;
    /// <summary>
    /// 开始按钮
    /// </summary>
    public Button startBtn;
    /// <summary>
    /// 邀请按钮
    /// </summary>
    public Button inviteBtn;
    /// <summary>
    /// 战报播放界面
    /// </summary>
    public GameObject reportView;
    /// <summary>
    /// 胡牌特效
    /// </summary>
    public GameObject huEffect;
    /// <summary>
    /// alpha渐变容器
    /// </summary>
    public CanvasGroup canvasGroup;
    /// <summary>
    /// 精牌的值
    /// </summary>
    public Image AlterableCardNumImg;
    /// <summary>
    /// 精牌的精图片
    /// </summary>
    public Image AlterableCardJingImg;
    /// <summary>
    /// 精牌的Root
    /// </summary>
    public GameObject AlterableCardRoot;
    // Use this for initialization
    public override void OnInit()  
    {
        ViewRoot = this.LaunchUIView("Prefab/UI/Battle/BattleView");
        AlterableCardRoot = ViewRoot.transform.Find("AlterableCard").gameObject;
        AlterableCardJingImg = ViewRoot.transform.Find("AlterableCard/jing").gameObject.GetComponent<Image>();
        AlterableCardNumImg = ViewRoot.transform.Find("AlterableCard/bg/Num").gameObject.GetComponent<Image>();
        timeTxt = ViewRoot.transform.Find("RoomInfoBg/TimeTxt").gameObject.GetComponent<Text>();
        roomIdTxt = ViewRoot.transform.Find("RoomInfoBg/RoomIdTxt").gameObject.GetComponent<Text>();
        roundTxt = ViewRoot.transform.Find("RoomInfoBg/RoundTxt").gameObject.GetComponent<Text>();
        netIcon = ViewRoot.transform.Find("RoomInfoBg/NetIcon").gameObject.GetComponent<Image>();
        Battery = ViewRoot.transform.Find("RoomInfoBg/BatterySlider").gameObject.GetComponent<Slider>();
        roundNumberTxt = ViewRoot.transform.Find("RoomInfoBg/RoundNumberText").gameObject.GetComponent<Text>();
        CardNumberTxt = ViewRoot.transform.Find("RoomInfoBg/CardNumberText").gameObject.GetComponent<Text>();

        roomOperateView = ViewRoot.transform.Find("RoomOperateView").gameObject;
        //expandBtn = ViewRoot.transform.Find("RoomOperateView/ExpandBtn").gameObject.GetComponent<Button>();
        dissolutionBtn = ViewRoot.transform.Find("RoomOperateView/DissolutionBtn").gameObject.GetComponent<Button>();
        exitBtn = ViewRoot.transform.Find("RoomOperateView/ExitBtn").gameObject.GetComponent<Button>();

        voiceBtn = ViewRoot.transform.Find("ChatView/VoiceBtn").gameObject.GetComponent<Button>();
        chatBtn = ViewRoot.transform.Find("ChatView/ChatBtn").gameObject.GetComponent<Button>();
        settingBtn = ViewRoot.transform.Find("ChatView/SettingBtn").gameObject.GetComponent<Button>();
        recordingIcon = ViewRoot.transform.Find("ChatView/RecordingIcon").gameObject;

        downHead = ViewRoot.transform.Find("DownHead").gameObject;
        leftHead = ViewRoot.transform.Find("LeftHead").gameObject;
        rightHead = ViewRoot.transform.Find("RightHead").gameObject;
        upHead = ViewRoot.transform.Find("UpHead").gameObject;
        canvasGroup = ViewRoot.transform.GetComponent<CanvasGroup>();
        headItemList = new List<GameObject>();
        headItemList.Add(downHead);
        headItemList.Add(rightHead);
        headItemList.Add(upHead);
        headItemList.Add(leftHead);

        //masterView = ViewRoot.transform.Find("MasterView").GetComponent<MasterView>();

        readyBtn = ViewRoot.transform.Find("ReadyBtn").gameObject.GetComponent<Button>();
        startBtn = ViewRoot.transform.Find("StartBtn").gameObject.GetComponent<Button>();
        inviteBtn = ViewRoot.transform.Find("InviteBtn").gameObject.GetComponent<Button>();
        reportView = ViewRoot.transform.Find("ReportView").gameObject;

        ApplicationFacade.Instance.RegisterMediator(new BattleViewMediator(Mediators.BATTLE_VIEW_MEDIATOR, this));
    }
	
    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Battle/BattleView");
    }

    public override void OnDestroy()
    {
        ApplicationFacade.Instance.RemoveMediator(Mediators.BATTLE_VIEW_MEDIATOR);
        base.OnDestroy();
    }

}