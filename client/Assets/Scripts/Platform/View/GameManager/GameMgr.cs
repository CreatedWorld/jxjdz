﻿using System.Collections.Generic;
using UnityEngine;
using Platform.Net;
using Platform.Utils;
using Utils;
using Platform.Model.Battle;
using System.Collections;
using UnityEngine.UI;
using System.Threading;

public class GameMgr : MonoBehaviour
{
    public GameObject SplashImage;
    private static GameMgr instance;
    public static GameMgr Instance
    {
        get
        {
            return instance;
         }
    }
    /// <summary>
    /// 接收消息池
    /// </summary>
    public ConcurrentLinkedQueue<ReciveMsgVO> ReciveMsgPool = new ConcurrentLinkedQueue<ReciveMsgVO>();
    /// <summary>
    /// 发送消息池
    /// </summary>
    public ConcurrentLinkedQueue<SendMsgVO> SendMsgPool = new ConcurrentLinkedQueue<SendMsgVO>();
    /// <summary>
    /// 消息响应方法代理
    /// </summary>
    /// <param PlayerName="bytes"></param>
    public delegate void MsgHandlerFun(byte[] bytes);
    /// <summary>
    /// 消息号映射回调方法字典
    /// </summary>
    private Dictionary<MsgNoS2C, List<MsgHandlerFun>> msgHandleDic;
    private GUIStyle style;

    public GameMgr()
    {
    }
    void Awake()
    {
        SplashImage.SetActive(true);
        style = new GUIStyle();
        style.fontSize = 30;
        style.normal.textColor = Color.yellow;
        GameObject.Find("SplashImage").GetComponent<SpriteRenderer>().color = Color.white;
        this.GameMgrInit();
        Utils.Timer.Instance.Init();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    void Start()
    {
        Application.targetFrameRate = 30;
        ApplicationFacade.Instance.SendNotification(NotificationConstant.COMM_GAMEMGR_INIT);
        gameObject.AddComponent<ClientAIMgr>();
    }
    void OnGUI()
    {
        GUI.Label(new Rect(0, Screen.height - 30, 200, 50), GlobalData.VERSIONS, style);
        if (UIManager.Instance.needSaveScreen)
        {
            UIManager.Instance.SaveScreenTexture();
        }
    }
    /// <summary>
    /// 点击的起始坐标
    /// </summary>
    Vector2 moveStart = Vector2.zero;
    /// <summary>
    /// 解锁步骤
    /// </summary>
    uint moveIndex = 0;
    void Update()
    {
        SendMsgHandler();
        ReciveMsgHandler();
        Utils.Timer.Instance.DoUpdate();
        UIManager.Instance.Update();
        if (Input.GetMouseButtonDown(0))
        {
            moveStart = Input.mousePosition;
            moveIndex = 0;
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                var curPos = Input.mousePosition;
                if (moveIndex == 0 && moveStart.y - curPos.y > 100)
                {
                    moveIndex++;
                    moveStart = curPos;
                }
                if (moveIndex == 1 && curPos.x - moveStart.x > 100)
                {
                    moveIndex++;
                    moveStart = curPos;
                }
                if (moveIndex == 2 && curPos.y - moveStart.y > 100)
                {
                    moveIndex++;
                    var logView = GameObject.Find("UIRoot").transform.Find("LogView").gameObject;
                    logView.SetActive(!logView.activeSelf);
                }
            }
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            DialogMsgVO dialogMsgVO = new DialogMsgVO();
            dialogMsgVO.title = "退出提示";
            dialogMsgVO.content = "是否退出游戏";
            dialogMsgVO.dialogType = DialogType.CONFIRM;
            dialogMsgVO.confirmCallBack = delegate { Application.Quit(); };
            DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
            dialogView.data = dialogMsgVO;
        }

    }

    void FixedUpdate()
    {
        Utils.Timer.Instance.DoFixUpdate();
        UIManager.Instance.FixedUpdate();
    }

    void OnDestroy()
    {
        ApplicationFacade.Instance.RemoveMediator(Mediators.GAMEMGR_MEDIATOR);
        NetMgr.Instance.OnDisable();
    }

    /// <summary>
    /// 发送消息池处理
    /// </summary>
    private void SendMsgHandler()
    {
        while (!SendMsgPool.IsEmpty)
        {
            SendMsgVO sendMsgVO = null;
            SendMsgPool.TryDequeue(out sendMsgVO);
            if (sendMsgVO == null)
            {
                continue;
            }
            if (GlobalData.LoginServer == "127.0.0.1")
            {
                ClientAIMgr.Instance.SendBuff(sendMsgVO.channel, sendMsgVO.type, sendMsgVO.tbuff);
             }
            else
            {
                if (NetMgr.Instance.ConnentionDic.ContainsKey(sendMsgVO.socketType))
                {
                    NetMgr.Instance.ConnentionDic[sendMsgVO.socketType].SendBuff(sendMsgVO.channel, sendMsgVO.type, sendMsgVO.tbuff, sendMsgVO.offCheckTimeOut);
                }
            }
            ClientAIMgr.Instance.ShowSendMsgLog((MsgNoC2S)sendMsgVO.channel, sendMsgVO.tbuff);
        }
    }

    /// <summary>
    /// 接收消息池处理方法,每帧调用一次
    /// </summary>
    private void ReciveMsgHandler()
    {
        while (!ReciveMsgPool.IsEmpty)
        {
            ReciveMsgVO msg = null;
            ReciveMsgPool.TryDequeue(out msg);
            if (msg == null)
            {
                continue;
            }
            MsgNoS2C msgNo = (MsgNoS2C)msg.channel;
            ClientAIMgr.Instance.ShowMsgLog(string.Format("消息:{0} 消息号:{1}", msgNo.ToString(), msg.channel));
            if (msgHandleDic.ContainsKey(msgNo))
            {
                for (int i = 0; i < msgHandleDic[msgNo].Count; i++)
                {
                    
                    try
                    {
                        msgHandleDic[msgNo][i](msg.bytes);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError(string.Format("{0} {1}", e.Message, e.StackTrace));
                    }
                }
            }
        }
    }

    private void GameMgrInit()
    {
        this.RegisterUIView();
        instance = this;
        var audioSystem = GameObject.Find("AudioSystem").gameObject;
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(Camera.main);
        DontDestroyOnLoad(UIManager.Instance.UiRoot);
        DontDestroyOnLoad(audioSystem);
        DontDestroyOnLoad(GetComponent<Camera>());
        this.msgHandleDic = new Dictionary<MsgNoS2C, List<MsgHandlerFun>>();
        ApplicationFacade.Instance.RegisterMediator(new GameMgrMediator(Mediators.GAMEMGR_MEDIATOR, this));
        ResourcesMgr resMgr = ResourcesMgr.Instance; 
        this.InitPrefsKey();

    }
    /// <summary>
    /// 添加消息映射响应
    /// </summary>
    /// <param PlayerName="msgNo">消息号</param>
    /// <param PlayerName="msgHandler">消息回调方法</param>
    public void AddMsgHandler(MsgNoS2C msgNo, MsgHandlerFun msgHandler)
    {
        if (!msgHandleDic.ContainsKey(msgNo))
        {
            msgHandleDic.Add(msgNo, new List<MsgHandlerFun>());
        }
        msgHandleDic[msgNo].Add(msgHandler);
    }


    /// <summary>
    /// 移除消息映射
    /// </summary>
    /// <param PlayerName="msgNo">消息号</param>
    /// <param PlayerName="msgHandler">消息回调方法</param>
    public void RemoveMsgHandler(MsgNoS2C msgNo, MsgHandlerFun msgHandler)
    {
        if (!msgHandleDic.ContainsKey(msgNo))
        {
            return;
        }
        for (int i = 0; i < msgHandleDic[msgNo].Count; i++)
        {
            if (msgHandleDic[msgNo][i] == msgHandler)
            {
                msgHandleDic[msgNo].Remove(msgHandler);
                return;
            }
        }
    }

    private void RegisterUIView()
    {
        UIManager.Instance.RegisterUI(UIViewID.CUSTOMERSERVICE_VIEW,new CustomerServiceView());
        UIManager.Instance.RegisterUI(UIViewID.LOGIN_VIEW, new LoginView());
        UIManager.Instance.RegisterUI(UIViewID.HALL_VIEW,new HallView());
        UIManager.Instance.RegisterUI(UIViewID.TOPMENU_VIEW, new TopMenuView());
        UIManager.Instance.RegisterUI(UIViewID.BATTLE_VIEW, new BattleView());
        UIManager.Instance.RegisterUI(UIViewID.DIALOG_VIEW, new DialogView());
        UIManager.Instance.RegisterUI(UIViewID.MATCH_RESULT_VIEW, new MatchResultView());
        UIManager.Instance.RegisterUI(UIViewID.ROOM_RESULT_VIEW, new RoomResultView());
        //UIManager.Instance.RegisterUI(UIViewID.MIDDLEMENU_VIEW, new MiddleMenuView());
        UIManager.Instance.RegisterUI(UIViewID.PLATER_INFO_VIEW,new PlayerInfoView());
        UIManager.Instance.RegisterUI(UIViewID.SHOPPING_VIEW,new ShoppingView());
        UIManager.Instance.RegisterUI(UIViewID.SIGNIN_VIEW,new SignInView());
        UIManager.Instance.RegisterUI(UIViewID.HELP_VIEW,new HelpView());
        UIManager.Instance.RegisterUI(UIViewID.INVITE_VIEW,new InviteView());
        UIManager.Instance.RegisterUI(UIViewID.SHARE_VIEW, new ShareView());
        UIManager.Instance.RegisterUI(UIViewID.GRADE_VIEW,new GradeView());
        UIManager.Instance.RegisterUI(UIViewID.RANKING_VIEW,new RankingView());
        UIManager.Instance.RegisterUI(UIViewID.ACTIVITY_VIEW,new ActivityView());
        UIManager.Instance.RegisterUI(UIViewID.SETTING_VIEW,new SettingView());
        UIManager.Instance.RegisterUI(UIViewID.COMPETITION_VIEW,new CompetitionView());
        UIManager.Instance.RegisterUI(UIViewID.CREATEROOM_VIEW, new CreateRoomView());
        UIManager.Instance.RegisterUI(UIViewID.JOINROOM_VIEW,new JoinRoomView());
        UIManager.Instance.RegisterUI(UIViewID.CHAT_VIEW, new ChatView());
        UIManager.Instance.RegisterUI(UIViewID.DISLOVE_APPLY_VIEW, new DisloveApplyView());
        UIManager.Instance.RegisterUI(UIViewID.DISLOVE_STATISTICS_VIEW, new DisloveStatisticsView());
        UIManager.Instance.RegisterUI(UIViewID.MATCHING_VIEW,new MatchingView());
        UIManager.Instance.RegisterUI(UIViewID.GRADEINFORMATION_VIEW, new GradeInformationView());

    }

    private void InitPrefsKey()
    {
        if (PlayerPrefs.HasKey(PrefsKey.SERVERIP))
        {
            GlobalData.LoginServer = PlayerPrefs.GetString(PrefsKey.SERVERIP);
        }
        if (PlayerPrefs.HasKey(PrefsKey.SERVERPORT))
        {
            GlobalData.LoginPort = PlayerPrefs.GetInt(PrefsKey.SERVERPORT);
        }
        if (PlayerPrefs.HasKey(PrefsKey.USERNAME))
        {
            GlobalData.UserName = PlayerPrefs.GetString(PrefsKey.USERNAME);
        }
        if (PlayerPrefs.HasKey(PrefsKey.USERMAC))
        {
            GlobalData.UserMac = PlayerPrefs.GetString(PrefsKey.USERMAC);
        }
        if (PlayerPrefs.HasKey(PrefsKey.SOUNDSET))
        {
            GlobalData.AudioVolume = PlayerPrefs.GetFloat(PrefsKey.SOUNDSET);
        }
        if (PlayerPrefs.HasKey(PrefsKey.MUSICSET))
        {
            GlobalData.BGMVolume = PlayerPrefs.GetFloat(PrefsKey.MUSICSET);
        }
    }

    public IEnumerator LoadRegionImage(string titleUrl, RawImage title)
    {
        WWW loadImage = new WWW(titleUrl);
        yield return loadImage;
        if (title != null)
        {
            title.texture = loadImage.texture;
        }
    }
}

public class ConcurrentLinkedQueue<T>
{
    private class Node
    {
        internal T Item;
        internal Node Next;

        public Node(T item, Node next)
        {
            this.Item = item;
            this.Next = next;
        }
    }

    private Node _head;
    private Node _tail;

    public ConcurrentLinkedQueue()
    {
        _head = new Node(default(T), null);
        _tail = _head;
    }

    public bool IsEmpty
    {
        get { return (_head.Next == null); }
    }

    public void Enqueue(T item)
    {
        Node newNode = new Node(item, null);
        while (true)
        {
            Node curTail = _tail;
            Node residue = curTail.Next;

            //判断_tail是否被其他process改变
            if (curTail == _tail)
            {
                //A 有其他rocess执行C成功，_tail应该指向新的节点
                if (residue == null)
                {
                    //C 其他process改变了tail节点，需要重新取tail节点
                    if (Interlocked.CompareExchange(
          ref curTail.Next, newNode, residue) == residue)
                    {
                        //D 尝试修改tail
                        Interlocked.CompareExchange(ref _tail, newNode, curTail);
                        return;
                    }
                }
                else
                {
                    //B 帮助其他线程完成D操作
                    Interlocked.CompareExchange(ref _tail, residue, curTail);
                }
            }
        }
    }

    public bool TryDequeue(out T result)
    {
        result = default(T);
        Node curHead;
        Node curTail;
        Node next;
        do
        {
            curHead = _head;
            curTail = _tail;
            next = curHead.Next;
            if (curHead == _head)
            {
                if (next == null) //Queue为空
                {
                    result = default(T);
                    return false;
                }
                if (curHead == curTail) //Queue处于Enqueue第一个node的过程中
                {
                    //尝试帮助其他Process完成操作
                    Interlocked.CompareExchange(ref _tail, next, curTail);
                }
                else
                {
                    //取next.Item必须放到CAS之前
                    result = next.Item;
                    //如果_head没有发生改变，则将_head指向next并退出
                    if (Interlocked.CompareExchange(ref _head,
          next, curHead) == curHead)
                        break;
                }
            }
        }
        while (true);
        return true;
    }
}