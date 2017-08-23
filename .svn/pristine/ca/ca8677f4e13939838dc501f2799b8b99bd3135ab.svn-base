using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 分享View
/// </summary>
public class ShareView : UIView
{
    private Button closeButton;
    private Button friendButton;
    private Button communityButton;

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
    public Button FriendButton
    {
        get
        {
            return friendButton;
        }

        set
        {
            friendButton = value;
        }
    }

    public Button CommunityButton
    {
        get
        {
            return communityButton;
        }

        set
        {
            communityButton = value;
        }
    }



    public override void OnInit()
    {
        this.ViewRoot = this.LaunchUIView("Prefab/UI/Share/ShareView");
        this.CloseButton = this.ViewRoot.transform.FindChild("CloseButton").GetComponent<Button>();
        this.FriendButton = this.ViewRoot.transform.FindChild("FriendButton").GetComponent<Button>();
        this.CommunityButton = this.ViewRoot.transform.FindChild("CommunityButton").GetComponent<Button>();
        ApplicationFacade.Instance.RegisterMediator(new ShareMediator(Mediators.HALL_SHARE, this));
    }
    public override void OnShow()
    {
        base.OnShow();
        UIManager.Instance.ShowUIMask(UIViewID.SHARE_VIEW);
        UIManager.Instance.ShowDOTween(this.ViewRoot.GetComponent<RectTransform>());
    }

    public override void OnHide()
    {
        UIManager.Instance.HidenDOTween(this.ViewRoot.GetComponent<RectTransform>(), base.OnHide);

    }
    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Share/ShareView");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ApplicationFacade.Instance.RemoveMediator(Mediators.HALL_SHARE);
    }
}
