﻿using System.Collections.Generic;
using Platform.View.Battle;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     房间结算界面
/// </summary>
public class RoomResultView : UIView
{
    /// <summary>
    ///     关闭按钮
    /// </summary>
    public Button closeBtn;

    /// <summary>
    ///     头像间距
    /// </summary>
    public float itemGap;

    /// <summary>
    ///     玩家节点数组
    /// </summary>
    public List<RoomHeadItem> playerItems;

    /// <summary>
    ///     房间id
    /// </summary>
    public Text roomIdTxt;

    /// <summary>
    ///     分享按钮
    /// </summary>
    public Button shareBtn;

    /// <summary>
    /// 继续按钮
    /// </summary>
    public Button goonBtn;

    /// <summary>
    ///     当前服务器时间
    /// </summary>
    public Text timeTxt;
    /// <summary>
    ///     游戏进行局数
    /// </summary>
    public Text numberTxt;

    /// <summary>
    ///     输赢标志
    /// </summary>
    public RectTransform winnerIcon;

    public override void OnInit()
    {
        ViewRoot = LaunchUIView("Prefab/UI/Battle/RoomResultView");
        playerItems = new List<RoomHeadItem>();
        for (var i = 0; i < GlobalData.SIT_NUM; i++)
        {
            var playerItem = ViewRoot.transform.Find("HeadItem" + (i + 1)).GetComponent<RoomHeadItem>();
            playerItems.Add(playerItem);
        }
        itemGap = playerItems[1].gameObject.GetComponent<RectTransform>().localPosition.x -
                  playerItems[0].gameObject.GetComponent<RectTransform>().localPosition.x;
        winnerIcon = ViewRoot.transform.Find("WinnerIcon").GetComponent<RectTransform>();
        timeTxt = ViewRoot.transform.Find("TimeTxt").GetComponent<Text>();
        roomIdTxt = ViewRoot.transform.Find("RoomIdTxt").GetComponent<Text>();
        shareBtn = ViewRoot.transform.Find("ShareBtn").GetComponent<Button>();
        goonBtn = ViewRoot.transform.Find("GoonBtn").GetComponent<Button>();
        //closeBtn = ViewRoot.transform.Find("CloseBtn").GetComponent<Button>();
        numberTxt = ViewRoot.transform.Find("NumberTxt").GetComponent<Text>();

        ApplicationFacade.Instance.RegisterMediator(new RoomResultViewMediator(Mediators.ROOM_RESULT_VIEW_MEDIATOR,
            this));
    }

    public override void OnRegister()
    {
        ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Battle/RoomResultView");
    }

    public override void OnShow()
    {
        base.OnShow();
        UIManager.Instance.ShowUIMask(UIViewID.ROOM_RESULT_VIEW);
    }

    public override void OnDestroy()
    {
        ApplicationFacade.Instance.RemoveMediator(Mediators.ROOM_RESULT_VIEW_MEDIATOR);
        base.OnDestroy();
    }
}