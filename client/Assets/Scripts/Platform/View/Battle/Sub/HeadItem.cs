﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Platform.Model;
using UnityEngine;
using UnityEngine.UI;
using Platform.Model.Battle;
using Platform.Net;
using Utils;
using Platform.Utils;
using System;

/// <summary>
/// 战斗场景内单个头像框
/// </summary>
public class HeadItem : MonoBehaviour
{
    /// <summary>
    /// 节点玩家数据
    /// </summary>
    private PlayerInfoVOS2C _data;
    /// <summary>
    /// 战斗模块数据
    /// </summary>
    private BattleProxy battleProxy;
    /// <summary>
    /// 玩家模块数据
    /// </summary>
    private PlayerInfoProxy playerInfoProxy;
    /// <summary>
    /// 头像按钮
    /// </summary>
    private Button headBtn;
    /// <summary>
    /// 玩家名称
    /// </summary>
    private Text nameTxt;
    /// <summary>
    /// 积分值
    /// </summary>
    private Text coinTxt;
    /// <summary>
    /// 头像图标
    /// </summary>
    private RawImage headIcon;
    /// <summary>
    /// 准备图标
    /// </summary>
    private GameObject readyIcon;
    /// <summary>
    /// 庄家图标
    /// </summary>
    private GameObject bankerIcon;
    /// <summary>
    /// 操作提示按钮1
    /// </summary>
    private List<Button> actionBtns;
    /// <summary>
    /// 正在播放声音图标
    /// </summary>
    private GameObject voicePlayIcon;
    /// <summary>
    /// 聊天显示区域
    /// </summary>
    private GameObject chatView;
    /// <summary>
    /// 聊天文本框
    /// </summary>
    private Text chatTxt;
    /// <summary>
    /// 动作特效
    /// </summary>
    private GameObject actEffect;
    /// <summary>
    /// 表情图标
    /// </summary>
    private Image faceIcon;
    /// <summary>
    /// 等待特效
    /// </summary>
    private GameObject waitingIcon;
    /// <summary>
    /// 听牌的图标
    /// </summary>
    private GameObject tingIcon;

    /// <summary>
    /// 胡牌特效容器
    /// </summary>
    private Transform huView;
    /// <summary>
    /// 胡牌特效
    /// </summary>
    private GameObject huEffect;
    /// <summary>
    /// 操作提示按钮容器
    /// </summary>
    private List<RectTransform> actionBtnContainers;
    /// <summary>
    ///  发语言是否结束
    /// </summary>
    private bool isover = true;
    void Awake()
    {
        battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
        playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
        headBtn = transform.Find("Head").GetComponent<Button>();
        nameTxt = transform.Find("NameTxt").GetComponent<Text>();
        coinTxt = transform.Find("CoinTxt").GetComponent<Text>();
        headIcon = transform.Find("Head/HeadIcon").GetComponent<RawImage>();
        readyIcon = transform.Find("ReadyIcon").gameObject;
        bankerIcon = transform.Find("BankerIcon").gameObject;
        voicePlayIcon = transform.Find("VoicePlayIcon").gameObject;
        chatView = transform.Find("ChatView").gameObject;
        chatTxt = transform.Find("ChatView/ChatTxt").GetComponent<Text>();
        faceIcon = transform.Find("FaceIcon").GetComponent<Image>();
        waitingIcon = transform.Find("WaitingIcon").gameObject;
        huView = transform.Find("Huview");
        headBtn.onClick.AddListener(OpenPlayerInfo);

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 头像框对应的玩家数据
    /// </summary>
    public PlayerInfoVOS2C data
    {
        get { return _data; }
        set
        {
            if (value != null)
            {
                waitingIcon.SetActive(false);
                nameTxt.text = value.name;
                coinTxt.text = value.score.ToString();
                if (battleProxy.isStart)
                {
                    readyIcon.SetActive(false);
                    bankerIcon.SetActive(value.isBanker);
                }
                else
                {
                    readyIcon.SetActive(value.isReady);
                    readyIcon.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    bankerIcon.SetActive(false);
                }
                GameMgr.Instance.StartCoroutine(DownIcon(value.headIcon));
                if (_data == null)
                {//头像由没有->有
                    var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
                    var sitOffset = (value.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
                    if (sitOffset != 0)
                    {
                        headBtn.gameObject.GetComponent<Animator>().Play(HeadItemAnimationName.ShowNameArr[sitOffset], 0, 0);
                    }
                    GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio("Voices/Effect/PlayerEnter"));
                }
                if (value.userId == playerInfoProxy.UserInfo.UserID)
                {
                    var actionBtn1 = transform.Find("ActionBtnContainer1/ActionBtn1").GetComponent<Button>();
                    var actionBtn2 = transform.Find("ActionBtnContainer2/ActionBtn2").GetComponent<Button>();
                    var actionBtn3 = transform.Find("ActionBtnContainer3/ActionBtn3").GetComponent<Button>();
                    var actionBtn4 = transform.Find("ActionBtnContainer4/ActionBtn4").GetComponent<Button>();
                    var actionBtn5 = transform.Find("ActionBtnContainer5/ActionBtn5").GetComponent<Button>();
                    actionBtns = new List<Button>();
                    actionBtnContainers = new List<RectTransform>();
                    actionBtns.Add(actionBtn1);
                    actionBtns.Add(actionBtn2);
                    actionBtns.Add(actionBtn3);
                    actionBtns.Add(actionBtn4);
                    actionBtns.Add(actionBtn5);

                    actionBtns[0].onClick.AddListener(() =>
                    {
                        ActHandler(0);
                    });
                    actionBtns[1].onClick.AddListener(() =>
                    {
                        ActHandler(1);
                    });
                    actionBtns[2].onClick.AddListener(() =>
                    {
                        ActHandler(2);
                    });
                    actionBtns[3].onClick.AddListener(() =>
                    {
                        ActHandler(3);
                    });
                    actionBtns[4].onClick.AddListener(() =>
                    {
                        ActHandler(4);
                    });

                    actionBtnContainers.Add(transform.Find("ActionBtnContainer1").GetComponent<RectTransform>());
                    actionBtnContainers.Add(transform.Find("ActionBtnContainer2").GetComponent<RectTransform>());
                    actionBtnContainers.Add(transform.Find("ActionBtnContainer3").GetComponent<RectTransform>());
                    actionBtnContainers.Add(transform.Find("ActionBtnContainer4").GetComponent<RectTransform>());
                    actionBtnContainers.Add(transform.Find("ActionBtnContainer5").GetComponent<RectTransform>());
                    tingIcon = transform.Find("TingIcon").gameObject;
                    //UpdateTingIcon();
                }
            }
            else
            {
                waitingIcon.SetActive(true);
                readyIcon.SetActive(false);
                bankerIcon.SetActive(false);
                if (_data != null)
                {//头像由有->没有
                    var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
                    var sitOffset = (_data.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
                    headBtn.gameObject.GetComponent<Animator>().Play(HeadItemAnimationName.HidenNameArr[sitOffset], 0, 0);
                    GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio("Voices/Effect/PlayerEnter"));
                }
            }
            _data = value;
        }
    }

    IEnumerator DownIcon(string headUrl)
    {
        WWW www = new WWW(headUrl);
        yield return www;
        if (www.error == null)
        {
            headIcon.texture = www.texture;
        }
    }

    /// <summary>
    /// 隐藏准备标志
    /// </summary>
    public void HidenReady()
    {
        readyIcon.GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 0.5f);
    }

    /// <summary>
    /// 隐藏庄家标志
    /// </summary>
    public void HidemBanker()
    {
        bankerIcon.SetActive(false);

    }

    /// <summary>
    /// 设置庄家图标显示
    /// </summary>
    /// <param name="bankerLocalId">庄家id</param>
    public void ShowBankerIcon(int bankerLocalId)
    {
        if (_data == null)
        {
            return;
        }
        if (_data.userId == bankerLocalId)
        {
            bankerIcon.SetActive(true);
            var rectTransform = bankerIcon.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(3, 3);
            Tweener tweener = rectTransform.DOScale(new Vector3(1, 1, 1), 0.5f);
            tweener.SetAutoKill(true);

            bankerIcon.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            Tweener tweener2 = bankerIcon.GetComponent<Image>().DOColor(Color.white, 0.5f);
            tweener2.SetAutoKill(true);
        }
    }

    /// <summary>
    /// 动作提示数组
    /// </summary>
    private List<OperateType> operates;
    /// <summary>
    /// 动作提示的牌数组
    /// </summary>
    private Dictionary<OperateType, List<int>> operateCards;
    /// <summary>
    /// 动作提示对应的后端操作数组
    /// </summary>
    private Dictionary<OperateType, List<PlayerActType>> operateActs;
    /// <summary>
    /// 提示点击的牌数组
    /// </summary>
    private List<GameObject> operateCardBtns;
    /// <summary>
    /// 可以吃的牌数组
    /// </summary>
    private List<List<int>> chiSelectArr;

    /// <summary>
    /// 显示玩家操作提示
    /// </summary>
    public void ShowPlayActTip() 
    {
        //ClearOperateCardBtns();
        operates = new List<OperateType>();
        operateCards = new Dictionary<OperateType, List<int>>();
        operateActs = new Dictionary<OperateType, List<PlayerActType>>();
        operateCardBtns = new List<GameObject>();
        for (int i = 0; i < battleProxy.playerActTipS2C.acts.Count; i++)
        {
            PlayerActType actType = battleProxy.playerActTipS2C.acts[i];
            OperateType operateType = OperateType.PASS;
            if (actType == PlayerActType.PUT_CARD)
            {
                continue;
            }
            if (actType == PlayerActType.PUT_FLOWER_CARD)
            {
                //PutFlowerCard();
                continue;
            }
            switch (actType)
            {
                case PlayerActType.ZHI_GANG:
                case PlayerActType.BACK_AN_GANG:
                case PlayerActType.COMMON_AN_GANG:
                case PlayerActType.BACK_PENG_GANG:
                case PlayerActType.COMMON_PENG_GANG:
                    operateType = OperateType.GANG;
                    break;
                case PlayerActType.SELF_HU:
                case PlayerActType.QIANG_ZHI_GANG_HU:
                case PlayerActType.QIANG_PENG_GANG_HU:
                case PlayerActType.QIANG_AN_GANG_HU:
                case PlayerActType.CHI_HU:
                    operateType = OperateType.HU;
                    break;
                case PlayerActType.PASS:
                    operateType = OperateType.PASS;
                    break;
                case PlayerActType.PENG:
                    operateType = OperateType.PENG;
                    break;
                case PlayerActType.CHI:
                    operateType = OperateType.CHI;
                    chiSelectArr = BattleAreaUtil.GetCanChiArr(battleProxy.playerActTipS2C.actCards[i]);
                    break;
                    //case PlayerActType.PUT_FLOWER_CARD:
                    //    //发送打花牌消息
                    //    operateType = OperateType.FLOWER;
                    //    break;

            }
            if (operates.IndexOf(operateType) == -1)
            {
                operates.Add(operateType);
                operateCards.Add(operateType, new List<int>());
                operateActs.Add(operateType, new List<PlayerActType>());
            }
            operateCards[operateType].Add(battleProxy.playerActTipS2C.actCards[i]);
            operateActs[operateType].Add(actType);
        }
        for (int i = 0; i < actionBtns.Count; i++)
        {

            if (i >= operateActs.Count)
            {
                actionBtnContainers[i].gameObject.SetActive(false);
                continue;
            }
            actionBtnContainers[i].gameObject.SetActive(true);
            switch (operates[i])
            {
                case OperateType.GANG:
                    actionBtns[i].gameObject.SetActive(true);

                    actionBtns[i].gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/ActionGang");
                    break;
                case OperateType.HU:
                    actionBtns[i].gameObject.SetActive(true);

                    actionBtns[i].gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/ActionHu");
                    break;
                case OperateType.PASS:
                    actionBtns[i].gameObject.SetActive(true);

                    actionBtns[i].gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/ActionGuo");
                    break;
                case OperateType.PENG:
                    actionBtns[i].gameObject.SetActive(true);

                    actionBtns[i].gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/ActionPeng");
                    break;
                case OperateType.CHI:
                    actionBtns[i].gameObject.SetActive(true);

                    actionBtns[i].gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/ActionChi");
                    break;
            }
            Animator animator = actionBtns[i].gameObject.GetComponent<Animator>();
            actionBtns[i].gameObject.GetComponent<Animator>().Play("ShowActBtn" + (i + 1), 0, 0);
        }
    }

    /// <summary>
    /// 打花牌
    /// </summary>
    private void PutFlowerCard()
    {
        if (battleProxy.playerActTipS2C != null && battleProxy.isSelfAction)
        {
            List<int> flowers = new List<int>();
            for (int i = 0; i < data.handCards.Count; i++)
            {
                if (battleProxy.flowerCardList.Contains(data.handCards[i]))
                {
                    flowers.Add(data.handCards[i]);
                }
            }
            if (flowers.Count> 0)
            {
                PlayAFlowerMahjongC2S actC2S = new PlayAFlowerMahjongC2S();
                actC2S.mahjongCode = flowers[0];
                NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.PlayAFlowerMahjongC2S.GetHashCode(), 0, actC2S);
                flowers.Clear();
                Debug.Log("这是刚发完牌打的花牌："+ actC2S.mahjongCode);
            }
            if (battleProxy.playerActTipS2C.actCards != null)
            {
                if (battleProxy.flowerCardList.Contains(battleProxy.playerActTipS2C.actCards[0]))
                {
                    PlayAFlowerMahjongC2S actC2S = new PlayAFlowerMahjongC2S();
                    flowers.Add(battleProxy.playerActTipS2C.actCards[0]);
                    actC2S.mahjongCode = flowers[0];
                    NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.PlayAFlowerMahjongC2S.GetHashCode(), 0, actC2S);
                    Debug.Log("这是正常打牌打的花牌：" + actC2S.mahjongCode);
                }
            }
        }

    }

    /// <summary>
    /// 上次点击准备的时间
    /// </summary>
    private float perClickTime = 0;

    /// <summary>
    /// 点击动作按钮
    /// </summary>
    /// <param name="btnIndex"></param>
    private void ActHandler(int btnIndex)
    {
        if (Time.time - perClickTime < 1)
        {
            return;
        }
        perClickTime = Time.time;
        ClearOperateCardBtns();
        var operateType = operates[btnIndex];
        if (operateType == OperateType.CHI)
        {//吃牌按钮特殊操作
            if (chiSelectArr.Count > 1)
            { //有多种吃牌选择
                ShowChiSelectView(btnIndex);
            }
            else
            {
                Debug.Log("chiSelectArr[0],btnIndex" + chiSelectArr[0] + "  " + btnIndex);
                onChiClick(PlayerActType.CHI, chiSelectArr[0], btnIndex);
            }
        }
        else
        {
            if (operateCards[operateType].Count > 1)
            { //操作的牌有多张
                ShowOperateSelectView(operateType, btnIndex);
            }
            else
            {
                OperateSingle(operateActs[operateType][0], operateCards[operateType][0], btnIndex);
            }
        }
    }

    /// <summary>
    /// 显示单个操作的候选按钮
    /// </summary>
    /// <param name="operate"></param>
    private void ShowOperateSelectView(OperateType operate, int btnIndex)
    {
        var cardNum = operateCards[operate].Count;
        var clickBtn = actionBtns[btnIndex];
        float startX = -(cardNum * 76 + (cardNum - 1) * 5) / 2;
        GameObject cardPerfab = Resources.Load<GameObject>("Prefab/UI/Battle/CardSelectBtn");
        for (int i = 0; i < cardNum; i++)
        {
            var cardGameObject = Instantiate(cardPerfab);
            var card = operateCards[operate][i];
            var act = operateActs[operate][i];
            cardGameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                OperateSingle(act, card, btnIndex);
            });
            cardGameObject.transform.Find("CardFront").GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("Textures/Card/{0}", card));
            cardGameObject.transform.SetParent(clickBtn.transform);
            cardGameObject.transform.localScale = Vector3.one;
            cardGameObject.transform.localPosition = new Vector3(startX + 38, 121, 0);
            startX += 78;
            if (startX < cardNum - 1)
            {
                startX += 5;
            }
            operateCardBtns.Add(cardGameObject);
        }
    }

    /// <summary>
    /// 显示吃牌操作的候选按钮
    /// </summary>
    /// <param name="operate"></param>
    private void ShowChiSelectView(int btnIndex)
    {
        var chiSelectNum = chiSelectArr.Count;
        var clickBtn = actionBtns[btnIndex];
        float startX = -(chiSelectNum * 250 + (chiSelectNum - 1) * 20) / 2;
        GameObject cardPerfab = Resources.Load<GameObject>("Prefab/UI/Battle/CardSelectBtn");
        for (int i = 0; i < chiSelectNum; i++)
        {
            for (int j = 0; j < chiSelectArr[i].Count; j++)
            {
                var cardGameObject = Instantiate(cardPerfab);
                var canChiArr = chiSelectArr[i];
                cardGameObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    onChiClick(PlayerActType.CHI, canChiArr, btnIndex);
                });
                cardGameObject.transform.Find("CardFront").GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("Textures/Card/{0}", chiSelectArr[i][j]));
                cardGameObject.transform.SetParent(clickBtn.transform);
                cardGameObject.transform.localScale = Vector3.one;
                cardGameObject.transform.localPosition = new Vector3(startX + 60, 121, 0);
                startX += 72;
                if (j <= chiSelectArr[i].Count - 1)
                {
                    startX += 5;
                }
                operateCardBtns.Add(cardGameObject);
            }

            startX += 15;
        }

    }

    /// <summary>
    /// 操作单张牌
    /// </summary>
    /// <param name="act"></param>
    /// <param name="card"></param>
    private void OperateSingle(PlayerActType act, int card, int btnIndex)
    {
        switch (act)
        {
            case PlayerActType.ZHI_GANG:
                onZhiGangClick(act, card);
                break;
            case PlayerActType.BACK_AN_GANG:
            case PlayerActType.COMMON_AN_GANG:
                onAnGangClick(act, card);
                break;
            case PlayerActType.BACK_PENG_GANG:
            case PlayerActType.COMMON_PENG_GANG:
                onPengGangClick(act, card);
                break;
            case PlayerActType.SELF_HU:
            case PlayerActType.QIANG_ZHI_GANG_HU:
            case PlayerActType.QIANG_PENG_GANG_HU:
            case PlayerActType.QIANG_AN_GANG_HU:
            case PlayerActType.CHI_HU:
                onHuClick(act, card);
                break;
            case PlayerActType.PASS:
                onPassClick();
                break;
            case PlayerActType.PENG:
                onPengClick(act, card);
                break;
        }
        ClearOperateCardBtns();
    }

    /// <summary>
    /// 清除显示的候选牌
    /// </summary>
    private void ClearOperateCardBtns()
    {
        if (operateCardBtns == null)
        {
            return;
        }
        foreach (GameObject btn in operateCardBtns)
        {
            GameObject.Destroy(btn);
        }
        operateCardBtns.Clear();
    }


    /// <summary>
    /// 隐藏玩家操作提示
    /// </summary>
    public void HidenPlayActTip()
    {
        foreach (Button actionBtn in actionBtns)
        {
            actionBtn.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 播放玩家动作
    /// </summary>
    /// <param name="act"></param>
    public void PlayAct(PlayerActType act)
    {
        GameObject effectPerfab = null;
        switch (act)
        {
            case PlayerActType.COMMON_AN_GANG:
            case PlayerActType.BACK_AN_GANG:
            case PlayerActType.COMMON_PENG_GANG:
            case PlayerActType.BACK_PENG_GANG:
            case PlayerActType.ZHI_GANG:
                effectPerfab = Resources.Load<GameObject>("Effect/GangEffect/GangEffect");
                break;
            case PlayerActType.PENG:
                effectPerfab = Resources.Load<GameObject>("Effect/PengEffect/PengEffect");
                break;
            case PlayerActType.CHI:
                effectPerfab = Resources.Load<GameObject>("Effect/ChiEffect/ChiEffect");
                break;

        }
        if (effectPerfab != null)
        {
            actEffect = Instantiate(effectPerfab);
            var perPosition = actEffect.GetComponent<RectTransform>().localPosition;
            actEffect.GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>());
            actEffect.GetComponent<RectTransform>().localPosition = Vector3.zero;
            if (actEffect.name == "ChiEffect(Clone)")
            {
                actEffect.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);
            }
            else
            {
                actEffect.GetComponent<RectTransform>().localScale = Vector3.one;
            }

            actEffect.GetComponent<Animator>().enabled = true;
            Timer.Instance.AddTimer(0.5f, 1, 2.5f, RemoveActEffect);
        }
    }

    /// <summary>
    /// 移除动作提示特效
    /// </summary>
    private void RemoveActEffect()
    {
        DestroyImmediate(actEffect);
        actEffect = null;
    }

    /// <summary>
    /// 点击直杠按钮
    /// </summary>
    private void onZhiGangClick(PlayerActType act, int card)
    {
        var actC2S = new ZhiGangC2S();
        actC2S.mahjongCode = card;
        NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.ZHIGANG_C2S.GetHashCode(), 0, actC2S);
    }

    /// <summary>
    /// 点击暗杠按钮
    /// </summary>
    private void onAnGangClick(PlayerActType act, int card)
    {
        if (act == PlayerActType.COMMON_AN_GANG)
        {
            var actC2S = new CommonAnGangC2S();
            actC2S.mahjongCode = card;
            NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.COMMONANGANG_C2S.GetHashCode(), 0, actC2S);
        }
        else if (act == PlayerActType.BACK_AN_GANG)
        {
            var actC2S = new BackAnGangC2S();
            actC2S.mahjongCode = card;
            NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.BACKANGANG_C2S.GetHashCode(), 0, actC2S);
        }
    }

    /// <summary>
    /// 播放胡牌特效
    /// </summary>
	public void PlayHu()
    {
        var huEffectPerfab = Resources.Load<GameObject>("Effect/HuEffect/HuEffect");
        huEffect = GameObject.Instantiate(huEffectPerfab);
        // var defaultPosition = huEffect.GetComponent<RectTransform>().localPosition;
        huEffect.GetComponent<RectTransform>().SetParent(huView);
        huEffect.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        huEffect.GetComponent<RectTransform>().localPosition = Vector3.zero;
        huEffect.GetComponent<Animator>().enabled = true;
        Timer.Instance.AddDeltaTimer(0.5f, 1, 1, RemoveHuEffect);


    }

    /// <summary>
    /// 移除胡牌动作
    /// </summary>
    private void RemoveHuEffect()
    {
        GameObject.DestroyImmediate(huEffect);
        huEffect = null;
        battleProxy.isPlayHu = false;
        if (battleProxy.matchResultS2C != null && !UIManager.Instance.GetUIView(UIViewID.MATCH_RESULT_VIEW).IsShow)
        {
            UIManager.Instance.ShowUI(UIViewID.MATCH_RESULT_VIEW);
        }
    }

    /// <summary>
    /// 点击碰杠按钮
    /// </summary>
    private void onPengGangClick(PlayerActType act, int card)
    {
        if (act == PlayerActType.COMMON_PENG_GANG)
        {
            var actC2S = new CommonPengGangC2S();
            actC2S.mahjongCode = card;
            NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.COMMONPENGGANG_C2S.GetHashCode(), 0, actC2S);
        }
        else if (act == PlayerActType.BACK_PENG_GANG)
        {
            var actC2S = new BackPengGangC2S();
            actC2S.mahjongCode = card;
            NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.BACKPENGGANG_C2S.GetHashCode(), 0, actC2S);
        }
        for (int i = 0; i < actionBtns.Count; i++)
        {
            for (int j = 0; j < actionBtns[i].gameObject.transform.childCount; j++)
            {
                Destroy(actionBtns[i].gameObject.transform.GetChild(j).gameObject);
            }
        }
    }

    /// <summary>
    /// 点击胡按钮
    /// </summary>
    private void onHuClick(PlayerActType act, int card)
    {
        //Debug.Log("onHuClick ********************************");
        if (act == PlayerActType.SELF_HU)
        {
            var actC2S = new ZiMoHuC2S();
            NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.ZIMOHU_C2S.GetHashCode(), 0, actC2S);
        }
        else if (act == PlayerActType.QIANG_AN_GANG_HU)
        {
            var actC2S = new QiangAnGangHuC2S();
            NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.QIANGANGANGHU_C2S.GetHashCode(), 0, actC2S);
        }
        else if (act == PlayerActType.QIANG_PENG_GANG_HU)
        {
            var actC2S = new QiangPengGangHuC2S();
            NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.QIANGPENGGANGHU_C2S.GetHashCode(), 0, actC2S);
        }
        else if (act == PlayerActType.QIANG_ZHI_GANG_HU)
        {
            var actC2S = new QiangZhiGangHuC2S();
            NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.QIANGZHIGANGHU_C2S.GetHashCode(), 0, actC2S);
        }
        else if (act == PlayerActType.CHI_HU)
        {
            var actC2S = new ChiHuC2S();
            NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.CHIHU_C2S.GetHashCode(), 0, actC2S);
        }
        for (int i = 0; i < actionBtns.Count; i++)
        {
            actionBtns[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < actionBtns.Count; i++)
        {
            Debug.Log("childCount = " + actionBtns[i].gameObject.transform.childCount);
            for (int j = 0; j < actionBtns[i].gameObject.transform.childCount; j++)
            {
                Destroy(actionBtns[i].gameObject.transform.GetChild(j).gameObject);
            }
        }
    }

    /// <summary>
    /// 点击过按钮
    /// </summary>
    private void onPassClick()
    {
        var actC2S = new GuoC2S();
        NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.GUO_C2S.GetHashCode(), 0, actC2S);
        for (int i = 0; i < actionBtns.Count; i++)
        {
            Debug.Log("childCount = " + actionBtns[i].gameObject.transform.childCount);
            for (int j = 0; j < actionBtns[i].gameObject.transform.childCount; j++)
            {
                Destroy(actionBtns[i].gameObject.transform.GetChild(j).gameObject);
            }
        }
    }

    /// <summary>
    /// 点击碰按钮
    /// </summary>
    private void onPengClick(PlayerActType act, int card)
    {
        var actC2S = new PengC2S();
        actC2S.mahjongCode = card;
        NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.PENG_C2S.GetHashCode(), 0, actC2S);
        for (int i = 0; i < actionBtns.Count; i++)
        {
            Debug.Log("childCount = " + actionBtns[i].gameObject.transform.childCount);
            for (int j = 0; j < actionBtns[i].gameObject.transform.childCount; j++)
            {
                Destroy(actionBtns[i].gameObject.transform.GetChild(j).gameObject);
            }
        }
    }

    /// <summary>
    /// 点击吃按钮
    /// </summary>
    /// <param name="act"></param>
    /// <param name="cards"></param>
    private void onChiClick(PlayerActType act, List<int> cards, int btnIndex)
    {
        var actC2S = new ChiC2S();
        for (int i = 0; i < cards.Count; i++)
        {
            actC2S.mahjongCodes.Add(cards[i]);
        }
        var card = battleProxy.playerActTipS2C.actCards[btnIndex];
        actC2S.mahjongCodes.Add(card);//将自己吃的牌放进数组
        actC2S.forbitCards.Add(card);
        if (cards[0] == card + 1)
        {
            if (Array.IndexOf(GlobalData.CardValues, card + 3) != -1)
            {
                actC2S.forbitCards.Add(card + 3);
            }
        }
        else if (cards[0] == card - 2)
        {
            if (Array.IndexOf(GlobalData.CardValues, card - 3) != -1)
            {
                actC2S.forbitCards.Add(card - 3);
            }
        }
        NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.CHI_C2S.GetHashCode(), 0, actC2S);
        cards.Clear();
        for (int i = 0; i < actionBtns.Count; i++)
        {
            Debug.Log("childCount = " + actionBtns[i].gameObject.transform.childCount);
            for (int j = 0; j < actionBtns[i].gameObject.transform.childCount; j++)
            {
                Destroy(actionBtns[i].gameObject.transform.GetChild(j).gameObject);
            }
        }
    }

    /// <summary>
    /// 显示播放语言标志
    /// </summary>
    public void ShowVoicePlayIcon()
    {
        voicePlayIcon.SetActive(true);
    }

    /// <summary>
    /// 隐藏播放语言标志
    /// </summary>
    public void HidenVoicePlayIcon()
    {
        voicePlayIcon.SetActive(false);
    }

    /// <summary>
    /// 显示聊天消息
    /// </summary>
    public void ShowChatInfo(string chatStr)
    {
        if (isover)
        {
            chatView.SetActive(true);
            chatTxt.text = chatStr;
            isover = false;
            Timer.Instance.AddTimer(1, 1, 3, HidenChatInfo);
        }
        else
        {
            PopMsg.Instance.ShowMsg("请不要频繁发送");
        }

    }

    /// <summary>
    /// 隐藏聊天消息
    /// </summary>
    private void HidenChatInfo()
    {
        chatView.SetActive(false);
        isover = true;
    }

    /// <summary>
    /// 打开玩家信息
    /// </summary>
    private void OpenPlayerInfo()
    {
        if (_data == null)
        {
            return;
        }
        var getPlayerInfoC2S = new GetUserInfoByIdC2S();
        getPlayerInfoC2S.userId = _data.userId;
        NetMgr.Instance.SendBuff(SocketType.HALL, MsgNoC2S.GET_PLAYERINFO_C2S.GetHashCode(), 0, getPlayerInfoC2S);
    }

    /// <summary>
    /// 表情协程
    /// </summary>
    private Coroutine faceCoroutine;

    /// <summary>
    /// 播放表情包
    /// </summary>
    /// <param name="faceIndex"></param>
    public void ShowFace(int faceIndex)
    {
        if (faceCoroutine != null)
        {
            StopCoroutine(faceCoroutine);
        }
        faceCoroutine = StartCoroutine(PlayFace(faceIndex));
    }

    private IEnumerator PlayFace(int faceIndex)
    {
        faceIcon.gameObject.SetActive(true);

        float start = Time.time + GlobalData.STICKER_LENGTH;
        Sprite[] stickers = ResourcesMgr.Instance.stickerLib[faceIndex];
        while (start > Time.time)
        {
            for (int i = 0; i < stickers.Length; ++i)
            {
                faceIcon.sprite = stickers[i];
                yield return new WaitForSeconds(GlobalData.STICKER_SPEED);
            }
        }
        //关闭表情
        faceIcon.gameObject.SetActive(false);
        faceCoroutine = null;
    }

    /// <summary>
    /// 听牌的卡牌数组
    /// </summary>
    private List<GameObject> tingCards = new List<GameObject>();

    /// <summary>
    /// 更新听牌数组
    /// </summary>
    public void UpdateTingIcon()
    {
        if (battleProxy.tingCards.Count == 0)
        {
            tingIcon.SetActive(false);
            ClearTingCardBtns();
            return;
        }

        tingIcon.SetActive(true);

        GameObject cardPerfab = Resources.Load<GameObject>("Prefab/UI/Battle/CardSelectBtn");
        int startX = 91;
        int startY = 0;
        int spaceX = 91;
        if (tingIcon.transform.childCount > 0)
        {
            for (int j = 0; j < tingIcon.transform.childCount; j++)
            {
                Destroy(tingIcon.transform.GetChild(j).gameObject);
            }
        }
        for (int i = 0; i < battleProxy.tingCards.Count; i++)
        {

             
            GameObject cardGameObject = null;

            if (battleProxy.tingCards.Count < 20)
            {
                cardGameObject = Instantiate(cardPerfab);
                cardGameObject.transform.Find("CardFront").GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("Textures/Card/{0}", battleProxy.tingCards[i]));
                cardGameObject.transform.SetParent(tingIcon.transform);
                cardGameObject.transform.localScale = Vector3.one;
                cardGameObject.transform.localPosition = new Vector3(startX, 0, 0);
                startX += 78;
                if (i < battleProxy.tingCards.Count - 1)
                {
                    startX += 5;
                }
                tingCards.Add(cardGameObject);
            }
            else 
            {
                cardGameObject = Instantiate(cardPerfab);
                cardGameObject.transform.Find("CardFront").GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("Textures/Card/{0}", battleProxy.tingCards[i]));

                if (i<19)
                {
                    cardGameObject.transform.SetParent(tingIcon.transform);
                    cardGameObject.transform.localScale = Vector3.one;
                    cardGameObject.transform.localPosition = new Vector3(startX, startY + 35, 0);
                    startX += 78;
                    if (i < battleProxy.tingCards.Count - 1)
                    {
                        startX += 5;
                    }
                }
                else
                {
                    cardGameObject.transform.SetParent(tingIcon.transform);
                    cardGameObject.transform.localScale = Vector3.one;
                    cardGameObject.transform.localPosition = new Vector3((i-19)*78+ spaceX, startY -80, 0);
                    //spaceX += 78;
                    if (i < battleProxy.tingCards.Count - 1)
                    {
                        spaceX += 5;
                    }
                }
                
                tingCards.Add(cardGameObject);

            }
        }
    }

    /// <summary>
    /// 清除显示的听牌
    /// </summary>
    private void ClearTingCardBtns()
    {
        foreach (GameObject btn in tingCards)
        {
            GameObject.Destroy(btn);
        }
        tingCards.Clear();
    }
}

/// <summary>
///     动作名称
/// </summary>
internal class HeadItemAnimationName
{
    /// <summary>
    /// 头像隐藏动画名称
    /// </summary>
    public static string[] HidenNameArr = {
        "",
        "RightHeadHiden",
        "UpHeadHiden",
        "LeftHeadHiden"
    };
    /// <summary>
    /// 头像显示动画名称
    /// </summary>
    public static string[] ShowNameArr = {
        "",
        "RightHeadShow",
        "UpHeadShow",
        "LeftHeadShow"
    };
}

/// <summary>
/// 操作类型
/// </summary>
internal enum OperateType
{
    FLOWER,
    PENG,
    GANG,
    HU,
    PASS,
    CHI,
}