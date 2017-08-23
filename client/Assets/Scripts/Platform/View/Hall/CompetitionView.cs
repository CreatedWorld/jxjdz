using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// 申请比赛
/// </summary>
public class CompetitionView : UIView
{
    /// <summary>
    /// 申请比赛主面板
    /// </summary>
    private GameObject apply;
    /// <summary>
    /// 规则面板
    /// </summary>
    private GameObject rule;
    /// <summary>
    /// 报名成功面板
    /// </summary>
    private GameObject applyPass;
    /// <summary>
    /// 关闭按钮
    /// </summary>
    private Button applyCloseButton;
    /// <summary>
    /// 申请匹配按钮
    /// </summary>
    private Button applyButton;
    /// <summary>
    /// 规则按钮
    /// </summary>
    private Button ruleButton;
    /// <summary>
    /// 规则关闭按钮
    /// </summary>
    private Button ruleCloseButton;
    /// <summary>
    /// 关闭申请匹配面板成功按钮
    /// </summary>
    private Button applyPassCloseButton;
    public GameObject Apply
    {
        get
        {
            return apply;
        }

        set
        {
            apply = value;
        }
    }
    public GameObject Rule
    {
        get
        {
            return rule;
        }

        set
        {
            rule = value;
        }
    }
    public GameObject ApplyPass
    {
        get
        {
            return applyPass;
        }

        set
        {
            applyPass = value;
        }
    }
    public Button ApplyCloseButton
    {
        get
        {
            return applyCloseButton;
        }

        set
        {
            applyCloseButton = value;
        }
    }
    public Button ApplyButton
    {
        get
        {
            return applyButton;
        }

        set
        {
            applyButton = value;
        }
    }
    public Button RuleButton
    {
        get
        {
            return ruleButton;
        }

        set
        {
            ruleButton = value;
        }
    }
    public Button RuleCloseButton
    {
        get
        {
            return ruleCloseButton;
        }

        set
        {
            ruleCloseButton = value;
        }
    }
    public Button ApplyPassCloseButton
    {
        get
        {
            return applyPassCloseButton;
        }

        set
        {
            applyPassCloseButton = value;
        }
    }
    public override void OnInit()
    {
        this.ViewRoot = this.LaunchUIView("Prefab/UI/Competition/CompetitionView");
        this.Apply = this.ViewRoot.transform.FindChild("Apply").gameObject;
        this.Rule = this.ViewRoot.transform.FindChild("Rule").gameObject;
        this.ApplyPass = this.ViewRoot.transform.FindChild("ApplyPass").gameObject;
        this.ApplyCloseButton = Apply.transform.FindChild("ApplyCloseButton").GetComponent<Button>();
        this.ApplyButton = Apply.transform.FindChild("ApplyButton").GetComponent<Button>();
        this.RuleButton = Apply.transform.FindChild("RuleButton").GetComponent<Button>();
        this.RuleCloseButton = Rule.transform.FindChild("RuleCloseButton").GetComponent<Button>();
        this.ApplyPassCloseButton = ApplyPass.transform.FindChild("ApplyPassCloseButton").GetComponent<Button>();
        ApplicationFacade.Instance.RegisterMediator(new CompetitionMediator(Mediators.HALL_COMPETITION, this));
    }
    public override void OnShow()
    {
        base.OnShow();
        UIManager.Instance.ShowUIMask(UIViewID.COMPETITION_VIEW);
        UIManager.Instance.ShowDOTween(this.ViewRoot.GetComponent<RectTransform>());
    }
    public override void OnHide()
    {
        UIManager.Instance.HidenDOTween(this.ViewRoot.GetComponent<RectTransform>(), 
            ()=> 
            {
                base.OnHide();
                this.Apply.transform.localScale = Vector3.one;
                this.Rule.transform.localScale = Vector3.one;
                this.ApplyPass.transform.localScale = Vector3.one;
                this.Apply.SetActive(true);
                this.Rule.SetActive(false);
                this.ApplyPass.SetActive(false);
            });
    }
    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Competition/CompetitionView");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ApplicationFacade.Instance.RemoveMediator(Mediators.HALL_COMPETITION);
    }
}