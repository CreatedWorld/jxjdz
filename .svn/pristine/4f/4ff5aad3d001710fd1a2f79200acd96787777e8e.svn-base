using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 签到View
/// </summary>
public class SignInView : UIView
{
    /// <summary>
    /// 关闭按钮
    /// </summary>
    private Button closeButton;
    /// <summary>
    /// 签到按钮
    /// </summary>
    private Button signInButton;
    /// <summary>
    /// 领取按钮
    /// </summary>
    private Button getButton;
    /// <summary>
    /// 签到天数组件
    /// </summary>
    private Transform signInState;
    /// <summary>
    /// 签到信息
    /// </summary>
    private List<Transform> dataList;
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
    public Button SignInButton
    {
        get
        {
            return signInButton;
        }

        set
        {
            signInButton = value;
        }
    }
    public List<Transform> DataList
    {
        get
        {
            return dataList;
        }

        set
        {
            dataList = value;
        }
    }

    public Button GetButton
    {
        get
        {
            return getButton;
        }

        set
        {
            getButton = value;
        }
    }

    public override void OnInit()
    {
        this.ViewRoot = this.LaunchUIView("Prefab/UI/SignIn/SignInView");
        this.CloseButton = this.ViewRoot.transform.FindChild("CloseButton").GetComponent<Button>() ;
        this.SignInButton = this.ViewRoot.transform.FindChild("SignInButton").GetComponent<Button>();
        this.GetButton = this.ViewRoot.transform.FindChild("GetButton").GetComponent<Button>();
        this.signInState = this.ViewRoot.transform.FindChild("SignInState");
        this.DataList = new List<Transform>();
        foreach (Transform child in this.signInState)
        {
            this.DataList.Add(child);
        }
        ApplicationFacade.Instance.RegisterMediator(new SignInMediator(Mediators.HALL_SIGNIN, this));
        
    }
    public override void OnShow()
    {
        base.OnShow();
        UIManager.Instance.ShowUIMask(UIViewID.SIGNIN_VIEW);
        UIManager.Instance.ShowDOTween(this.ViewRoot.GetComponent<RectTransform>());
    }

    public override void OnHide()
    {
        UIManager.Instance.HidenDOTween(this.ViewRoot.GetComponent<RectTransform>(), base.OnHide);
    }
    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/SignIn/SignInView");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ApplicationFacade.Instance.RemoveMediator(Mediators.HALL_SIGNIN);
    }
}

