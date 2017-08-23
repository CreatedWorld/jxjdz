using Platform.View.Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 聊天输入界面
/// </summary>
public class ChatView : UIView
{
    /// <summary>
    /// 关闭按钮
    /// </summary>
    public Button closeBtn;
    public Button SpeakingBtn;
    public Button EmojiBtn;
    public GameObject SpeakingGrid;
    public GameObject EmojiGrid;
    public Transform faceList;
    /// <summary>
    /// 聊天文本列表
    /// </summary>
    public TableView chatList;
    public override void OnInit()
    {
        ViewRoot = LaunchUIView("Prefab/UI/Battle/ChatView");
        chatList = ViewRoot.transform.Find("ChatList").GetComponent<TableView>();
        closeBtn = ViewRoot.transform.Find("CloseBtn").GetComponent<Button>();
        SpeakingBtn = ViewRoot.transform.Find("SpeakingAndEmojiBG/Speaking").GetComponent< Button>();
        EmojiBtn = ViewRoot.transform.Find("SpeakingAndEmojiBG/Emoji").GetComponent<Button>();

        //EmojiGrid = ViewRoot.transform.Find("EmojiGrid").gameObject;
        //SpeakingGrid = ViewRoot.transform.Find("SpeakingGrid").gameObject;
        SpeakingGrid = ViewRoot.transform.Find("ChatList").gameObject;
        faceList = ViewRoot.transform.Find("FaceSelectView/FaceList");
        EmojiGrid = ViewRoot.transform.Find("FaceSelectView").gameObject;
        chatList.DataProvider = new ArrayList(GlobalData.Chat_Const);
        ApplicationFacade.Instance.RegisterMediator(new ChatViewMediator(Mediators.CHAT_VIEW_MEDIATOR,this));
    }

    public override void OnRegister()
    {
        ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Battle/ChatView");
    }

    public override void OnShow()
    {
        base.OnShow();
        UIManager.Instance.ShowUIMask(UIViewID.CHAT_VIEW);
        UIManager.Instance.ShowDOTween(ViewRoot.GetComponent<RectTransform>());
    }

    public override void OnHide()
    {
        base.OnHide();
        UIManager.Instance.HidenDOTween(ViewRoot.GetComponent<RectTransform>(),null);
    }

    public override void OnDestroy()
    {
        ApplicationFacade.Instance.RemoveMediator(Mediators.CHAT_VIEW_MEDIATOR);
        base.OnDestroy();
    }
}
