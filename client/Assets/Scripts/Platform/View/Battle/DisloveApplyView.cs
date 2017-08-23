﻿using Platform.Model;
using Platform.Model.Battle;
using Platform.Net;
using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

/// <summary>
/// 解散申请界面
/// </summary>
class DisloveApplyView : UIView
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
    /// 取消按钮
    /// </summary>
    public Button cancelBtn;
    /// <summary>
    /// 确定按钮
    /// </summary>
    public Button confirmBtn;
    /// <summary>
    /// 关闭按钮
    /// </summary>
    public Button closeBtn;


    public override void OnInit()
    {
        battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
        gameMgrProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.GAMEMGR_PROXY) as GameMgrProxy;

        ViewRoot = LaunchUIView("Prefab/UI/Battle/DisloveApplyView");

        cancelBtn = ViewRoot.transform.Find("FlaseButton").GetComponent<Button>();
        confirmBtn = ViewRoot.transform.Find("tureButton").GetComponent<Button>();
        closeBtn = ViewRoot.transform.Find("CloseBtn").GetComponent<Button>();

        confirmBtn.onClick.AddListener(ConfirmDisloveHandler);
        cancelBtn.onClick.AddListener(CancelDisloveHandler);
        closeBtn.onClick.AddListener(CancelDisloveHandler);
    }

    public override void OnRegister()
    {
        ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Battle/DisloveApplyView");
    }

    public override void OnShow()
    {
        base.OnShow();
        UIManager.Instance.ShowUIMask(UIViewID.DISLOVE_STATISTICS_VIEW);
        UIManager.Instance.ShowDOTween(ViewRoot.GetComponent<RectTransform>());
    }

    public override void OnHide()
    {
        base.OnHide();
        UIManager.Instance.HidenDOTween(ViewRoot.GetComponent<RectTransform>(), null);
  
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

    }


    /// <summary>
    /// 同意解散请求
    /// </summary>
    private void ConfirmDisloveHandler()
    {
        var disloveC2S = new DissloveRoomConfirmC2S();
        NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.DISSLOVEROOM_CONFIRM_C2S.GetHashCode(), 0, disloveC2S,false);
        //UIManager.Instance.HideUI(UIViewID.DISLOVE_APPLY_VIEW);
        ViewRoot.SetActive(false);
    }

    /// <summary>
    /// 取消解散请求
    /// </summary>
    private void CancelDisloveHandler()
    {
        var disloveC2S = new CancelDissolveRoomC2S();
        NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.CANCEL_DISSLOVEAPPLY_C2S.GetHashCode(), 0, disloveC2S,false);
        UIManager.Instance.HideUI(UIViewID.DISLOVE_APPLY_VIEW);
        UIManager.Instance.HideUI(UIViewID.DISLOVE_STATISTICS_VIEW);
    }
}