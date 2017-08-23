using Platform.Model.Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 创建房间View
/// </summary>
public class CreateRoomView : UIView
{
    /// <summary>
    /// 关闭按钮
    /// </summary>
    private Button closeButton;
    /// <summary>
    /// 创建房间按钮
    /// </summary>
    private Button createButton;
    /// <summary>
    /// 选择游戏模式控件
    /// </summary>
    private Transform modeTrans;
    /// <summary>
    /// 游戏模式
    /// </summary>
    private GameModelInfo fourModeInfo;//4局房
    private GameModelInfo eightModeInfo;//8局房
    private GameModelInfo sixteenModeInfo;//16局房
    /// <summary>
    /// 是否封顶
    /// </summary>
    private GameCapScoreInfo capScore;//是否封顶
    /// <summary>
    /// 游戏规则
    /// </summary>
    private GameRuleInfo notWord;//无字模式
    private GameRuleInfo word;//有字模式
    public Button CloseButton
    {
        get
        {
            return closeButton;
        }

        set
        {
            closeButton = value;
        }
    }

    public Button CreateButton
    {
        get
        {
            return createButton;
        }

        set
        {
            createButton = value;
        }
    }

    public Transform ModeTrans
    {
        get
        {
            return modeTrans;
        }

        set
        {
            modeTrans = value;
        }
    }

    public GameModelInfo FourModeInfo
    {
        get
        {
            return fourModeInfo;
        }
        set
        {
            fourModeInfo = value;
        }
    }

    public GameModelInfo EightModeInfo
    {
        get
        {
            return eightModeInfo;
        }

        set
        {
            eightModeInfo = value;
        }
    }

    public GameModelInfo SixteenModeInfo
    {
        get
        {
            return sixteenModeInfo;
        }

        set
        {
            sixteenModeInfo = value;
        }
    }
    public GameCapScoreInfo CapScore
    {
        get
        {
            return capScore;
        }
        set
        {
            capScore = value;
        }
    }
    public GameRuleInfo NotWord
    {
        get
        {
            return notWord;
        }

        set
        {
            notWord = value;
        }
    }
    public GameRuleInfo Word
    {
        get
        {
            return word;
        }

        set
        {
            word = value;
        }
    }



    public override void OnInit()
    {
        this.ViewRoot = this.LaunchUIView("Prefab/UI/CreateRoom/CreateRoomView");
        this.CloseButton = this.ViewRoot.transform.FindChild("CloseButton").GetComponent<Button>();
        this.ModeTrans = this.ViewRoot.transform.FindChild("Mode");
        this.FourModeInfo = new GameModelInfo(this.ModeTrans.transform.FindChild("FourMode").GetComponent<Toggle>(),GameMode.FOUR_ROUND);
        this.EightModeInfo = new GameModelInfo(this.ModeTrans.transform.FindChild("EightMode").GetComponent<Toggle>(), GameMode.EIGHT_ROUND);
        this.SixteenModeInfo = new GameModelInfo(this.ModeTrans.transform.FindChild("SixteenMode").GetComponent<Toggle>(), GameMode.SIXTEEN_ROUND);
        this.CreateButton = this.ViewRoot.transform.FindChild("CreateButton").GetComponent<Button>();
        this.CapScore = new GameCapScoreInfo(this.ViewRoot.transform.FindChild("CapScore").GetComponent<Toggle>(),global::CapScore.CAP_SCORE);
        this.NotWord = new GameRuleInfo(this.ViewRoot.transform.FindChild("Rule").FindChild("NotWord").GetComponent<Toggle>(), GameRule.NOT_WORD);
        this.Word = new GameRuleInfo(this.ViewRoot.transform.FindChild("Rule").FindChild("Word").GetComponent<Toggle>(),GameRule.WORD);
        this.CreateButton = this.ViewRoot.transform.FindChild("CreateButton").GetComponent<Button>();
        this.NotWord.Toggle.interactable = false;
        ApplicationFacade.Instance.RegisterMediator(new CreateRoomMediator(Mediators.HALL_CREATEROOM, this));
    }

    public override void OnShow()
    {
        base.OnShow();
        UIManager.Instance.ShowUIMask(UIViewID.CREATEROOM_VIEW);
        UIManager.Instance.ShowDOTween(this.ViewRoot.GetComponent<RectTransform>());
    }

    public override void OnHide()
    {
        UIManager.Instance.HidenDOTween(this.ViewRoot.GetComponent<RectTransform>(), base.OnHide);

    }
    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/CreateRoom/CreateRoomView");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ApplicationFacade.Instance.RemoveMediator(Mediators.HALL_CREATEROOM);
    }
}
