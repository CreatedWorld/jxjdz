﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 大厅View,背景
/// </summary>
public class HallView : UIView
{
    private CanvasGroup canvasGroup;

    public CanvasGroup CanvasGroup
    {
        get
        {
            return canvasGroup;
        }
    }
    public Button RankingButton;
    public override void OnInit()
    {
        this.ViewRoot = this.LaunchUIView("Prefab/UI/Hall/HallView");
        this.canvasGroup = this.ViewRoot.GetComponent<CanvasGroup>();
        RankingButton = ViewRoot.transform.FindChild("BottomShow/MiddleMenuView/Ranking/RankingButton").GetComponent<Button>();
    }
    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Hall/HallView");
    }
}
