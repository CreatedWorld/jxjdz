using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 商场View
/// </summary>
public class ShoppingView : UIView
{
    /// <summary>
    /// 关闭按钮
    /// </summary>
    private Button closeButton;
    /// <summary>
    /// 客服按钮
    /// </summary>
    private Button serviceButton;
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
    public Button ServiceButton
    {
        get
        {
            return serviceButton;
        }
        set
        {
            serviceButton = value;
        }
    }


    public override void OnInit()
    {
        this.ViewRoot = this.LaunchUIView("Prefab/UI/Shopping/ShoppingView");
        this.CloseButton = this.ViewRoot.transform.FindChild("CloseButton").GetComponent<Button>();
        this.ServiceButton = this.ViewRoot.transform.FindChild("ServiceButton").GetComponent<Button>();
        ApplicationFacade.Instance.RegisterMediator(new ShoppingMediator(Mediators.HALL_SHOPPING, this));
    }
    public override void OnShow()
    {
        base.OnShow();
        UIManager.Instance.ShowUIMask(UIViewID.SHOPPING_VIEW);
        UIManager.Instance.ShowDOTween(this.ViewRoot.GetComponent<RectTransform>());
    }

    public override void OnHide()
    {
        UIManager.Instance.HidenDOTween(this.ViewRoot.GetComponent<RectTransform>(), base.OnHide);
    }

    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Shopping/ShoppingView");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ApplicationFacade.Instance.RemoveMediator(Mediators.HALL_SHOPPING);
    }
}
