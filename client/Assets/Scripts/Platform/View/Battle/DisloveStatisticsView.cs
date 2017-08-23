﻿using Platform.View.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 解散申请统计界面
/// </summary>
class DisloveStatisticsView : UIView
{
    /// <summary>
    /// 玩家名称数组
    /// </summary>
    public List<Text> nameTxtArr;
    /// <summary>
    /// 状态文本数组
    /// </summary>
    public List<Text> statusTxtArr;
    /// <summary>
    /// 剩余时间数组
    /// </summary>
    public Text remainTimeTxt;

    public override void OnInit()
    {
        ViewRoot = LaunchUIView("Prefab/UI/Battle/DisloveStatisticsView");
        nameTxtArr = new List<Text>();
        statusTxtArr = new List<Text>();
        remainTimeTxt = ViewRoot.transform.Find("RemainTimeTxt").GetComponent<Text>();
        //closeBtn = ViewRoot.transform.Find("CloseBtn").GetComponent<Button>();
        //falseBtn = ViewRoot.transform.Find("FlaseButton").GetComponent<Button>();
        //trueBtn = ViewRoot.transform.Find("tureButton").GetComponent<Button>();
        var nameTxt1 = ViewRoot.transform.Find("NameTxt1").GetComponent<Text>();
        var nameTxt2 = ViewRoot.transform.Find("NameTxt2").GetComponent<Text>();
        var nameTxt3 = ViewRoot.transform.Find("NameTxt3").GetComponent<Text>();
        var nameTxt4 = ViewRoot.transform.Find("NameTxt4").GetComponent<Text>();
        nameTxtArr.Add(nameTxt1);
        nameTxtArr.Add(nameTxt2);
        nameTxtArr.Add(nameTxt3);
        nameTxtArr.Add(nameTxt4);

        var statusTxt1 = ViewRoot.transform.Find("StatusTxt1").GetComponent<Text>();
        var statusTxt2 = ViewRoot.transform.Find("StatusTxt2").GetComponent<Text>();
        var statusTxt3 = ViewRoot.transform.Find("StatusTxt3").GetComponent<Text>();
        var statusTxt4 = ViewRoot.transform.Find("StatusTxt4").GetComponent<Text>();
        statusTxtArr.Add(statusTxt1);
        statusTxtArr.Add(statusTxt2);
        statusTxtArr.Add(statusTxt3);
        statusTxtArr.Add(statusTxt4);
        ApplicationFacade.Instance.RegisterMediator(new DisloveStatisticsViewMediator(Mediators.DISLOVESTATISTICS_VIEW_MEDIATOR, this));
    }

    public override void OnRegister()
    {
        ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Battle/DisloveStatisticsView");
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
     //   ApplicationFacade.Instance.RemoveMediator(Mediators.DISLOVESTATISTICS_VIEW_MEDIATOR);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ApplicationFacade.Instance.RemoveMediator(Mediators.DISLOVESTATISTICS_VIEW_MEDIATOR);
    }

}
