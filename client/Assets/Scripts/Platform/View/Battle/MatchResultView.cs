﻿using System.Collections.Generic;
using Platform.View.Battle;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     单局结算界面
/// </summary>
public class MatchResultView : UIView
{
    /// <summary>
    ///     节点间距
    /// </summary>
    public float itemGap;

    /// <summary>
    ///     玩家节点数组
    /// </summary>
    public List<MatchResultPlayerItem> playerItems;

    /// <summary>
    ///     开始下一局按钮
    /// </summary>
    public Button startNextBtn;

    /// <summary>
    /// 离开房间按钮
    /// </summary>
    public Button getoutRoomBtn;
    /// <summary>
    ///     开始下一局按钮文本
    /// </summary>
    //public Text startNextBtnTxt;
    /// <summary>
    ///     分享按钮
    /// </summary>
    public Button shareBtn;
    /// <summary>
    ///     单局胜败图标
    /// </summary>
    public Image titleIcon;
   /// <summary>
   /// 动作特效
   /// </summary>
    public GameObject actEffect;

    public override void OnInit()
    {
        ViewRoot = LaunchUIView("Prefab/UI/Battle/MatchResultView");
        startNextBtn = ViewRoot.transform.Find("WinBg/ContinueBtn").GetComponent<Button>();
        //startNextBtnTxt = ViewRoot.transform.Find("WinBg/ContinueBtn/Text").GetComponent<Text>();
        getoutRoomBtn = ViewRoot.transform.Find("WinBg/GetOutRoomBtn").GetComponent<Button>();

        playerItems = new List<MatchResultPlayerItem>();
        for (var i = 0; i < GlobalData.SIT_NUM; i++)
        {
            var playerItem = ViewRoot.transform.Find("WinBg/PlayerItem" + (i + 1)).GetComponent<MatchResultPlayerItem>();
            playerItems.Add(playerItem);
        }
        itemGap = playerItems[1].gameObject.GetComponent<RectTransform>().localPosition.y -
                  playerItems[0].gameObject.GetComponent<RectTransform>().localPosition.y;
        titleIcon = ViewRoot.transform.Find("TitleIcon").GetComponent<Image>();
        
    }

    public override void OnShow()
    {
        base.OnShow();
        UIManager.Instance.ShowUIMask(UIViewID.MATCH_RESULT_VIEW);
        ApplicationFacade.Instance.RegisterMediator(new MatchResultViewMediator(Mediators.MATCH_RESULT_VIEW_MEDIATOR,
            this));
        //ViewRoot.transform.position = new Vector3(1,1,1);
        ViewRoot.transform.localScale = new Vector3(1,1,1);
    }

    public override void OnRegister()
    {
        ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Battle/MatchResultView");
    }

    public override void OnHide()
    {
        base.OnHide();
        if (actEffect != null)
        {
            GameObject.DestroyImmediate(actEffect);
            actEffect = null;
        }
        ApplicationFacade.Instance.RemoveMediator(Mediators.MATCH_RESULT_VIEW_MEDIATOR);
    }

    public override void OnDestroy()
    {
        ApplicationFacade.Instance.RemoveMediator(Mediators.MATCH_RESULT_VIEW_MEDIATOR);
        base.OnDestroy();
    }
}