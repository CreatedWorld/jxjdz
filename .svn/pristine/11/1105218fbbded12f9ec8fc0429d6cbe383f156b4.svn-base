using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 输入邀请码View
/// </summary>
public class InviteView : UIView
{
    /// <summary>
    /// 确定按钮
    /// </summary>
    private Button confirmButton;
    /// <summary>
    /// 关闭按钮
    /// </summary>
    private Button closeButton;
    /// <summary>
    /// 输入框
    /// </summary>
    private InputField inputField;

    public Button ConfirmButton
    {
        get
        {
            return confirmButton;
        }

        set
        {
            confirmButton = value;
        }
    }

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

    public InputField InputField
    {
        get
        {
            return inputField;
        }

        set
        {
            inputField = value;
        }
    }

    public override void OnInit()
    {
        this.ViewRoot = this.LaunchUIView("Prefab/UI/Invite/InviteView");
        this.ConfirmButton = this.ViewRoot.transform.FindChild("ConfirmButton").GetComponent<Button>();
        this.CloseButton = this.ViewRoot.transform.FindChild("CloseButton").GetComponent<Button>();
        this.InputField = this.ViewRoot.transform.FindChild("Input").FindChild("InputField").GetComponent<InputField>();
        ApplicationFacade.Instance.RegisterMediator(new InviteMediator(Mediators.HALL_INVITE, this));
    }
    public override void OnShow()
    {
        base.OnShow();
        UIManager.Instance.ShowUIMask(UIViewID.INVITE_VIEW);
        UIManager.Instance.ShowDOTween(this.ViewRoot.GetComponent<RectTransform>());
    }

    public override void OnHide()
    {
        UIManager.Instance.HidenDOTween(this.ViewRoot.GetComponent<RectTransform>(),base.OnHide);
    }
    public override void OnHide(Action callBack)
    {
        UIManager.Instance.HidenDOTween(this.ViewRoot.GetComponent<RectTransform>(), 
            ()=> 
            {
                base.OnHide();
                callBack();
            });
    }
    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Invite/InviteView");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ApplicationFacade.Instance.RemoveMediator(Mediators.HALL_INVITE);
    }
}