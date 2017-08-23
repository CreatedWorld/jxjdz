using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Platform.Utils;

public class RankingView : UIView {

    private Button closeButton;

    /// <summary>
    /// 排行关闭按钮
    /// </summary>
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

    public TableView rankTable;
    /// <summary>
    /// 分享按钮
    /// </summary>
    private Button shareBtn;

    public override void OnInit()
    {
        this.ViewRoot = this.LaunchUIView("Prefab/UI/Ranking/RankingView");
        this.CloseButton = this.ViewRoot.transform.FindChild("CloseButton").GetComponent<Button>();
        rankTable = ViewRoot.transform.Find("RankingScrollView").GetComponent<TableView>();
       // shareBtn = ViewRoot.transform.Find("ShareBtn").GetComponent<Button>();
        ApplicationFacade.Instance.RegisterMediator(new RankingMediator(Mediators.HALL_RANKING,this));
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ApplicationFacade.Instance.RemoveMediator(Mediators.HALL_RANKING);
    }

    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Ranking/RankingView");
    }

    public override void OnShow()
    {
        base.OnShow();
        UIManager.Instance.ShowUIMask(UIViewID.RANKING_VIEW);
        this.ViewRoot.GetComponent<Animator>().Play("RankingShow");
        //shareBtn.onClick.AddListener(ShareHandler);
    }

    public override void OnHide()
    {
        this.ViewRoot.GetComponent<Animator>().Play("RankingHide");
    }
    /// <summary>
    /// 分享
    /// </summary>
    private void ShareHandler()
    {
        if (GlobalData.sdkPlatform == SDKPlatform.ANDROID)
        {
            string desc = "快来全民约牌吧";
            AndroidSdkInterface.WeiXinShareScreen(desc, false);
        }
        else if (GlobalData.sdkPlatform == SDKPlatform.IOS)
        {
            UIManager.Instance.StartSaveScreen((Texture2D screenShot) => {
                byte[] screenJpg = screenShot.EncodeToJPG();
                string jpgBase64 = Convert.ToBase64String(screenJpg);
                IOSSdkInterface.WeiXinShareScreen(jpgBase64, false);
            });
        }
    }
}
