using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;
using Platform.Utils;

public class ShareMediator : Mediator, IMediator
{
    public ShareMediator(string NAME, object viewComponent):base(NAME,viewComponent)
    {

    }

    public ShareView View
    {
        get
        {
            return (ShareView)ViewComponent;
        }
    }

    public override void OnRegister()
    {
        base.OnRegister();
        this.View.ButtonAddListening(this.View.CloseButton,
        () =>
        {
            UIManager.Instance.HideUI(UIViewID.SHARE_VIEW);
        });
        this.View.ButtonAddListening(this.View.FriendButton,
          () =>
          {
              if (GlobalData.sdkPlatform == SDKPlatform.ANDROID)
              {
                  string title = "江西景德镇翻精软四粒";
                  string desc = "快来翻精软四粒吧";
                  AndroidSdkInterface.WeiXinShare(GlobalData.ShareUrl, title, desc, false);
              }
              else if (GlobalData.sdkPlatform == SDKPlatform.IOS)
              {
                  string title = "江西景德镇翻精软四粒";
                  string desc = "快来翻精软四粒吧";
                  IOSSdkInterface.WeiXinShare(GlobalData.ShareUrl, title, desc, false);
              }
          });
        this.View.ButtonAddListening(this.View.CommunityButton,
            () =>
            {
                if (GlobalData.sdkPlatform == SDKPlatform.ANDROID)
                {
                    string title = "江西景德镇翻精软四粒";
                    string desc = "快来翻精软四粒吧";
                    AndroidSdkInterface.WeiXinShare(GlobalData.ShareUrl, title, desc, true);
                }
                else if (GlobalData.sdkPlatform == SDKPlatform.IOS)
                {
                    string title = "江西景德镇翻精软四粒";
                    string desc = "快来翻精软四粒吧";
                    IOSSdkInterface.WeiXinShare(GlobalData.ShareUrl, title, desc, true);
                }
            });
    }
    public override void OnRemove()
    {
        base.OnRemove();
        UIManager.Instance.DestroyUI(UIViewID.SHARE_VIEW);
    }
}

