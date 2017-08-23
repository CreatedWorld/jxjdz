﻿using Platform.Model.Battle;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Platform.Utils;
using Platform.Model;
using UnityEngine;
using Platform.Net;
using Utils;

/// <summary>
/// 单个战区位置
/// </summary>
public class BattleAreaItem : MonoBehaviour
{
    /// <summary>
    /// 牌堆内的所有牌
    /// </summary>
    [HideInInspector]
    public List<GameObject> heapCards;
    /// <summary>
    /// 自己所属方位
    /// </summary>  
    public AreaDir dir;
    /// <summary>
    /// 节点玩家数据
    /// </summary>
    private PlayerInfoVOS2C _data = null;
    /// <summary>
    /// 战斗模块数据中介
    /// </summary>
    private BattleProxy battleProxy;
    /// <summary>
    /// 玩家信息数据中介
    /// </summary>
    private PlayerInfoProxy playerInfoProxy;
    /// <summary>
    /// 出牌区域,水平位置能摆的牌数量
    /// </summary>
    public int putCardHNum;
    /// <summary>
    /// 当前选择的牌
    /// </summary>
    [HideInInspector]
    public GameObject selectCard;
    /// <summary>
    /// 发牌序号
    /// </summary>
    private int sendIndex;
    /// <summary>
    /// 已经发牌的数量
    /// </summary>
    private int sendedCardNum;
    /// <summary>
    /// 位置序号
    /// </summary>
    private int sitOffset;
    /// <summary>
    /// 手上牌之间的间距
    /// </summary>
    [HideInInspector]
    public Vector3 handCardGap;
    /// <summary>
    /// 牌堆的第一张牌位置
    /// </summary>
    [HideInInspector]
    public Vector3 heapHGap;
    /// <summary>
    /// 牌堆的第一张牌位置
    /// </summary>
    [HideInInspector]
    public Vector3 heapVGap;
    /// <summary>
    /// 摸到的牌和手中牌之间的间距
    /// </summary>
    [HideInInspector]
    public Vector3 getHandCardGap;
    /// <summary>
    /// 碰杠牌之间的间距
    /// </summary>
    [HideInInspector]
    public Vector3 pengGangGap;
    /// <summary>
    /// 已出的牌水平间距
    /// </summary>
    [HideInInspector]
    public Vector3 putCardHGap;
    /// <summary>
    /// 已出的牌垂直间距
    /// </summary>
    [HideInInspector]
    public Vector3 putCardVGap;
    /// <summary>
    /// 花牌水平间距
    /// </summary>
    [HideInInspector]
    public Vector3 flowerCardHGap;
    /// <summary>
    /// 手上的牌数组
    /// </summary>
    [HideInInspector]
    public List<GameObject> handCards;
    /// <summary>
    /// 手中摸到的牌
    /// </summary>
    [HideInInspector]
    public GameObject getCard;
    /// <summary>
    /// 手中摸到的牌容器
    /// </summary>
    [HideInInspector]
    public Transform getCardContainer;
    /// <summary>
    /// 已出的牌数组
    /// </summary>
    [HideInInspector]
    public List<GameObject> putCards;
    /// <summary>
    /// 已出的花牌的数组
    /// </summary>
    [HideInInspector]
    public List<GameObject> flowerCards;
    /// <summary>
    /// 碰杠的牌数组
    /// </summary>
    [HideInInspector]
    public List<List<GameObject>> pengGangCards;

    /// <summary>
    /// 手中的牌容器
    /// </summary>
    [HideInInspector]
    public Transform handCardContainer;
    /// <summary>
    /// 手中的第一张牌
    /// </summary>
    [HideInInspector]
    public Transform firstCard;
    /// <summary>
    /// 发牌的位置
    /// </summary>
    [HideInInspector]
    public Transform sendCard;
    /// <summary>
    /// 碰杠牌的位置
    /// </summary>
    [HideInInspector]
    public Transform pengGangCardContainer;
    /// <summary>
    /// 已出的牌位置
    /// </summary>
    [HideInInspector]
    public Transform putCardContainer;
    /// <summary>
    /// 打出花牌的位置
    /// </summary>
    public Transform flowersCardContainer;
    /// <summary>
    /// 自己牌的摄像机
    /// </summary>
    [HideInInspector]
    public Camera myselfCamera;

    /// <summary>
    /// 自己的牌堆起始序号
    /// </summary>
    [HideInInspector]
    public int heapStartIndex;
    /// <summary>
    /// 自己的牌堆结束序号
    /// </summary>
    [HideInInspector]
    public int heapEndIndex;
    /// <summary>
    /// 默认的牌id数组
    /// </summary>
    private List<int> defaultCardIdList;

    [HideInInspector]
    public Transform heapCardContainer;
    /// <summary>
    /// 牌堆的第一张牌位置
    /// </summary>
    [HideInInspector]
    public Transform heapFirstCard;
    private bool initPutFlowerCard = true;
    void Awake()
    {
        battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
        playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;



        handCards = new List<GameObject>();
        pengGangCards = new List<List<GameObject>>();
        putCards = new List<GameObject>();
        flowerCards = new List<GameObject>();
        if (GlobalData.hasHeap)
        {
            heapCards = new List<GameObject>();
        }
        myselfCamera = GameObject.Find("SelfCardCamera").GetComponent<Camera>();
        defaultCardIdList = new List<int>();
        var handFirstCard = transform.Find("Card/Card1");
        var handSecondCard = transform.Find("Card/Card2");
        var handLastCard = transform.Find("Card/Card13");
        getCardContainer = transform.Find("Card/GetCard");
        var pengGangFirstCard = transform.Find("PengGangCard/PengGangCard1");
        var pengGangSecondCard = transform.Find("PengGangCard/PengGangCard2");
        var putFirstCard = transform.Find("PutCard/PutCard1");
        var putSecondCard = transform.Find("PutCard/PutCard2");
        var putThirdCard = transform.Find("PutCard/PutCard3");

        handCardContainer = transform.Find("Card");
        firstCard = transform.Find("Card/Card1");
        handCardGap = handSecondCard.localPosition - handFirstCard.localPosition;
        getHandCardGap = getCardContainer.localPosition - handLastCard.localPosition;
        sendCard = transform.Find("Card/SendCard");
        pengGangCardContainer = transform.Find("PengGangCard");
        pengGangGap = pengGangSecondCard.localPosition - pengGangFirstCard.localPosition;
        putCardContainer = transform.Find("PutCard");
        flowersCardContainer = transform.Find("FlowersCard");
        putCardHGap = putSecondCard.localPosition - putFirstCard.localPosition;
        putCardVGap = putThirdCard.localPosition - putFirstCard.localPosition;

        var flowerFirstCard = transform.Find("FlowersCard/FlowersCard1");
        var flowerSecondCard = transform.Find("FlowersCard/FlowersCard2");
        flowerCardHGap = flowerSecondCard.localPosition - flowerFirstCard.localPosition;

        if (GlobalData.hasHeap)
        {
            heapCardContainer = transform.Find("HeapCard");
            heapFirstCard = transform.Find("HeapCard/HeapCard1");
            var heapSecondCard = transform.Find("HeapCard/HeapCard2");
            var heapThirdCard = transform.Find("HeapCard/HeapCard3");
            heapHGap = heapThirdCard.localPosition - heapFirstCard.localPosition;
            heapVGap = heapSecondCard.localPosition - heapFirstCard.localPosition;
            foreach (Transform card in heapCardContainer)
            {
                defaultCardIdList.Add(card.gameObject.GetInstanceID());
            }
        }
        foreach (Transform card in handCardContainer)
        {
            defaultCardIdList.Add(card.gameObject.GetInstanceID());
        }
        foreach (Transform card in pengGangCardContainer)
        {
            defaultCardIdList.Add(card.gameObject.GetInstanceID());
        }
        foreach (Transform card in putCardContainer)
        {
            defaultCardIdList.Add(card.gameObject.GetInstanceID());
        }
        foreach (Transform card in flowersCardContainer)
        {
            defaultCardIdList.Add(card.gameObject.GetInstanceID());
        }
    }

    /// <summary>
    /// 点击的起始坐标
    /// </summary>
    Vector2 moveStart = Vector2.zero;
    // Update is called once per frame
    void Update()
    {
        if (_data == null)
        {
            return;
        }
        if (_data.userId != playerInfoProxy.UserInfo.UserID)
        {
            return;
        }
        if (Application.isMobilePlatform && Input.touchCount > 1)
        {//手机上点击多个直接跳过
            return;
        }
        if (!battleProxy.isStart || battleProxy.isReport)
        {
            return;
        }
        if (battleProxy.isForbit)
        {//正在发牌
            return;
        }
        if (!Input.GetMouseButtonDown(0))
        {
            if (Input.GetMouseButton(0))
            {
                var curPos = Input.mousePosition;
                if (curPos.y - moveStart.y > 100)
                {
                    moveStart = curPos;
                    Debug.Log("出牌");

                }
            }
            return;
        }
        moveStart = Input.mousePosition;
        RaycastHit hit;
        Ray ray = myselfCamera.ScreenPointToRay(Input.mousePosition);
        bool touchHandCard = Physics.Raycast(ray, out hit);
        if (!touchHandCard)
        {
            return;
        }
        GameObject touchCard = hit.transform.gameObject;
        if (touchCard.layer != GlobalData.SELF_HAND_CARDS)
        {
            return;
        }
        if (touchCard.transform.parent != handCardContainer)
        {
            return;
        }
        //播放选定动画
        if (selectCard != null && selectCard != touchCard)
        {
            selectCard.transform.localPosition = new Vector3(selectCard.transform.localPosition.x, selectCard.transform.localPosition.y, 0);
        }
        selectCard = touchCard;
        if (selectCard.transform.localPosition.z < 0.2f)
        {
            selectCard.transform.localPosition = new Vector3(selectCard.transform.localPosition.x, selectCard.transform.localPosition.y, 0.2f);
            return;
        }
        PutCard();
    }

    /// <summary>
    /// 从牌堆内获取对应的牌
    /// </summary>
    /// <param name="cardValue"></param>
    /// <returns></returns>
    public GameObject GetHeapCard(int cardValue)
    {
        var card = heapCards[0];
        ResourcesMgr.Instance.SetCardMesh(card, cardValue);
        heapCards.RemoveAt(0);

        if (dir == AreaDir.DOWN)
        {
            if (cardValue == battleProxy.treasureCardCode)
            {
                if (card.transform.childCount == 0)
                {
                    GameObject go = new GameObject();
                    go.AddComponent<SpriteRenderer>();
                    go.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/UI/treasureCardCode");
                    go.layer = LayerMask.NameToLayer("SelfHandCards");
                    go.transform.SetParent(card.transform);
                    go.transform.localPosition = new Vector3(-0.104f, -0.177f, 0.21f);
                    go.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 132f, 47));
                    go.transform.localScale = new Vector3(.8f, .8f, .8f);
                    Debug.Log(" 从自己的牌堆生成的精牌：" + cardValue);
                }
            }
        }
        else
        {
            if (cardValue != battleProxy.treasureCardCode)
            {
                if (card.transform.childCount > 0)
                {
                    for (int i = 0; i < card.transform.childCount; i++)
                    {
                        Destroy(card.transform.GetChild(i));
                    }
                }
            }
        }


        return card;
    }

    /// <summary>
    /// 出牌
    /// </summary>
    private void PutCard()
    {

        //出牌
        if (battleProxy.isSelfAction && battleProxy.playerActTipS2C != null && battleProxy.playerActTipS2C.acts.Contains(PlayerActType.PUT_CARD))
        {
            var cardValue = BattleAreaUtil.GetMeshCardValue(selectCard);
            if (battleProxy.playerActTipS2C.forbitCards.IndexOf(cardValue) != -1)
            {
                PopMsg.Instance.ShowMsg("吃牌后当前牌不能出牌");
                return;
            }
            PlayAMahjongC2S actC2S = new PlayAMahjongC2S();
            actC2S.mahjongCode = cardValue;
            NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.PLAYAMAHJONG_C2S.GetHashCode(), 0, actC2S);
            battleProxy.isSelfAction = false;

        }
    }

    /// <summary>
    /// 头像框对应的玩家数据
    /// </summary>
    public PlayerInfoVOS2C data
    {
        get { return _data; }
        set
        {
            _data = value;
            var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];

            sitOffset = (_data.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
            if (battleProxy.isStart)
            {
                BattleAreaUtil.InitPlayerCards(this);
            }
        }
    }

    /// <summary>
    /// 设置数据不更新牌面
    /// </summary>
    /// <param name="value"></param>
    public void SetData(PlayerInfoVOS2C value)
    {
        _data = value;
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        sitOffset = (_data.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
    }

    /// <summary>
    /// 播放单人发牌4张牌
    /// </summary>
    public IEnumerator PlaySendCardAnimator()
    {
        var sendCardTotal = _data.handCards.Count;
        //bool hasFlowerCard = false;
        //_data.handCards.ForEach(o => { if (battleProxy.flowerCardList.Contains(o)) hasFlowerCard = true; });
        if (_data.isBanker && battleProxy.GameStartTouchCard !=0 )
        {
            sendCardTotal += 1;
        }
        for (int i = 0; i < GlobalData.SEND_SINGLE; i++)
        {
            if (sendIndex + 1 > sendCardTotal)
            {
                yield break;
            }
            SendSingleCard();
            yield return new WaitForSeconds(0.08f);
            sendIndex += 1;
        }
    }

    /// <summary>
    /// 发送单张牌
    /// </summary>
    private void SendSingleCard()
    {
        int cardValue = 0;
        if (_data.handCards.Count > sendedCardNum)
        {
            cardValue = _data.handCards[sendedCardNum];
        }
        else
        {
            cardValue = _data.getCard;
        }



        GameObject addCard = null;

        if (GlobalData.hasHeap)
        {
            addCard = (ApplicationFacade.Instance.RetrieveMediator(Mediators.BATTLE_AREA_MEDIATOR) as BattleAreaMediator).GetHeapCard(cardValue);
            addCard.transform.SetParent(handCardContainer);
            addCard.transform.localScale = firstCard.localScale;
            addCard.transform.localEulerAngles = firstCard.localEulerAngles;
        }
        else
        {
            addCard = ResourcesMgr.Instance.GetFromPool(cardValue);
            addCard.transform.SetParent(handCardContainer);
            addCard.transform.localPosition = sendCard.localPosition;
            addCard.transform.localScale = firstCard.localScale;
            addCard.transform.localEulerAngles = firstCard.localEulerAngles;

        }

        if (dir == AreaDir.RIGHT)
        {
            addCard.layer = GlobalData.RIGHTHAND_CARDS;
        }
        if (dir == AreaDir.DOWN)
            addCard.layer = GlobalData.SELF_HAND_CARDS;
        if (dir == AreaDir.LEFT)
            addCard.layer = GlobalData.OTHER_CARDS;
        if (dir == AreaDir.UP)
            addCard.layer = GlobalData.OTHER_CARDS;

        addCard.name = "HandCard" + sendedCardNum;

        if (dir == AreaDir.DOWN)
        {
            addCard.layer = LayerMask.NameToLayer("SelfHandCards");
            if (cardValue == battleProxy.treasureCardCode)
            {
                if (addCard.transform.childCount == 0)
                {
                    GameObject go = new GameObject();
                    go.AddComponent<SpriteRenderer>();
                    go.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/UI/treasureCardCode");
                    go.layer = LayerMask.NameToLayer("SelfHandCards");
                    go.transform.SetParent(addCard.transform);
                    go.transform.localPosition = new Vector3(-0.085f, -0.175f, 0.18f);
                    go.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 0, 180f));
                    go.transform.localScale = new Vector3(1, 1, 1);
                    //Debug.Log("玩家：" + _data.userId + " 已生成精牌。");
                }
            }
        }

        Transform targetTransform = null;
        if (sendedCardNum + 1 > GlobalData.PLAYER_CARD_NUM)
        {
            targetTransform = transform.Find("Card/GetCard");
            getCard = addCard;
        }
        else
        {
            targetTransform = transform.Find("Card/Card" + (sendedCardNum + 1));
            handCards.Add(addCard);
        }
        addCard.transform.DOKill();
        addCard.transform.DOLocalMove(targetTransform.localPosition, 0.17f);
        GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio("Voices/Effect/SendCard"));
        sendedCardNum++;


    }

    /// <summary>
    /// 播放牌面合上再打开的动画
    /// </summary>
    public IEnumerator PlayCloseCardAction()
    {
        int rotateAngle = GlobalData.SitCardCloseArr[sitOffset];
        foreach (Transform item in handCardContainer)
        {
            if (item.gameObject.activeSelf)
            {
                item.localEulerAngles = new Vector3(rotateAngle, 0, 0);
            }
        }
        StartCoroutine(AudioSystem.Instance.PlayEffectAudio("Voices/Effect/RotateAllCard"));
        yield return new WaitForSeconds(0.83f);
        if (_data.userId == playerInfoProxy.UserInfo.UserID)
        {
            _data.handCards.Sort();
            for (int i = 0; i < handCards.Count; i++)
            {
                handCards[i].name = "HandCard" + i;
            }
            handCards.Sort(BattleAreaUtil.CompareCard);

            var TempList = new List<GameObject>();               //精牌排在前面
            foreach (var item in handCards)
            {
                if (item.transform.childCount > 0)
                {
                    TempList.Add(item);
                }
            }
            foreach (var item in TempList)
            {
                handCards.Remove(item);
            }
            TempList.Sort(BattleAreaUtil.CompareCard);
            handCards.Sort(BattleAreaUtil.CompareCard);

            foreach (var item in handCards)
            {
                TempList.Add(item);
            }
            handCards = TempList;

        }
        BattleAreaUtil.ResortHandGangGetCard(this);
        foreach (Transform item in handCardContainer)
        {
            if (item.gameObject.activeSelf)
            {
                item.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
    }

    /// <summary>
    /// 播放直接暗杠
    /// </summary>
    public void PlayCommonAnGang()
    {
        List<GameObject> gangCards = new List<GameObject>();
        if (getCard != null)
        {
            gangCards.Add(getCard);
            getCard.transform.DOKill();
        }
        getCard = null;
        if (_data.userId == playerInfoProxy.UserInfo.UserID)
        {//自己暗杠找到真正的牌暗杠
            for (int i = 0; i < handCards.Count;)
            {
                if (BattleAreaUtil.GetMeshCardValue(handCards[i]) == battleProxy.playerActS2C.actCard)
                {
                    gangCards.Add(handCards[i]);
                    handCards.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
            // BattleAreaUtil.ResortCard(this, false);
        }
        else
        {//不是自己暗杠随机找牌暗杠
            var randomIndex = UnityEngine.Random.Range(0, handCards.Count - 3);
            gangCards.Add(handCards[randomIndex]);
            gangCards.Add(handCards[randomIndex + 1]);
            gangCards.Add(handCards[randomIndex + 2]);
            handCards.RemoveAt(randomIndex);
            handCards.RemoveAt(randomIndex);
            handCards.RemoveAt(randomIndex);
            foreach (GameObject gangCard in gangCards)
            {
                ResourcesMgr.Instance.SetCardMesh(gangCard, battleProxy.playerActS2C.actCard);
            }
        }
        foreach (GameObject card in gangCards)
        {
            card.transform.SetParent(pengGangCardContainer);
        }
        pengGangCards.Add(gangCards);
        BattleAreaUtil.ResortCard(this);
    }

    /// <summary>
    /// 播放回头暗杠
    /// </summary>
    public void PlayBackAnGang()
    {
        List<GameObject> gangCards = new List<GameObject>();
        if (_data.userId == playerInfoProxy.UserInfo.UserID)
        {//自己暗杠找到真正的牌暗杠
            for (int i = 0; i < handCards.Count;)
            {
                if (BattleAreaUtil.GetMeshCardValue(handCards[i]) == battleProxy.playerActS2C.actCard)
                {
                    gangCards.Add(handCards[i]);
                    handCards.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
        else
        {//不是自己暗杠随机找牌暗杠
            var randomIndex = UnityEngine.Random.Range(0, handCards.Count - 4);
            gangCards.Add(handCards[randomIndex]);
            gangCards.Add(handCards[randomIndex + 1]);
            gangCards.Add(handCards[randomIndex + 2]);
            gangCards.Add(handCards[randomIndex + 3]);
            handCards.RemoveAt(randomIndex);
            handCards.RemoveAt(randomIndex);
            handCards.RemoveAt(randomIndex);
            handCards.RemoveAt(randomIndex);
            foreach (GameObject gangCard in gangCards)
            {
                ResourcesMgr.Instance.SetCardMesh(gangCard, battleProxy.playerActS2C.actCard);
            }
        }
        foreach (GameObject card in gangCards)
        {
            card.transform.SetParent(pengGangCardContainer);
        }
        pengGangCards.Add(gangCards);
        BattleAreaUtil.ResortCard(this);
    }

    /// <summary>
    /// 播放摸牌动画
    /// </summary>
    public IEnumerator PlayGetCard()
    {
        GameObject cardItem = null;
        if (GlobalData.hasHeap)
        {
            cardItem = (ApplicationFacade.Instance.RetrieveMediator(Mediators.BATTLE_AREA_MEDIATOR) as BattleAreaMediator).GetHeapCard(battleProxy.playerActS2C.actCard);
            cardItem.transform.SetParent(handCardContainer);
            cardItem.transform.localScale = sendCard.transform.localScale;
            cardItem.transform.localEulerAngles = sendCard.transform.localEulerAngles;
            //if (cardItem.transform.childCount > 0)
            //{
            //    Destroy(cardItem.transform.GetChild(0).gameObject);
            //}
        }
        else
        {
            cardItem = ResourcesMgr.Instance.GetFromPool(battleProxy.playerActS2C.actCard);
            cardItem.transform.SetParent(handCardContainer);
            cardItem.transform.localPosition = sendCard.transform.localPosition;
            cardItem.transform.localScale = sendCard.transform.localScale;
            cardItem.transform.localEulerAngles = sendCard.transform.localEulerAngles;
            //if (cardItem.transform.childCount > 0)
            //{
            //    Destroy(cardItem.transform.GetChild(0).gameObject);
            //}

        }

        Vector3 targetPos = Vector3.zero;
        if (handCards.Count > 0)
        {
            targetPos = handCards[handCards.Count - 1].transform.localPosition + getHandCardGap;
        }
        else
        {
            targetPos = getCardContainer.localPosition;
        }

        if (_data.userId == playerInfoProxy.UserInfo.UserID)
        {
            targetPos.y = 0;
            targetPos.z = 0;
        }
        if (getCard != null)
        {
            handCards.Add(getCard);
            getCard.transform.DOKill();
            getCard = null;
        }
        if (_data.userId == playerInfoProxy.UserInfo.UserID)
        {
            if (battleProxy.playerActS2C.actCard == battleProxy.treasureCardCode)
            {
                GameObject go = new GameObject();
                go.AddComponent<SpriteRenderer>();
                go.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/UI/treasureCardCode");
                go.layer = GlobalData.SELF_HAND_CARDS;
                go.transform.SetParent(cardItem.transform);
                go.transform.localPosition = new Vector3(-0.104f, -0.177f, 0.21f);
                go.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 132f, 47));
                go.transform.localScale = new Vector3(.8f, .8f, .8f);
            }

        }
        else
        {
            if (battleProxy.playerActS2C.actCard == battleProxy.treasureCardCode)
            {
                if (cardItem.transform.childCount > 0)
                {
                    Destroy(cardItem.transform.GetChild(0).gameObject);
                }

            }
        }

        if (battleProxy.isFlowerGetCard )
        {
            handCards.Add(cardItem);
            battleProxy.isFlowerGetCard = false;
        } 
        else
        {
            getCard = cardItem;
        }

        //if (data.handCards.Count % 3 == 0)
        //{
        //    handCards.Add(cardItem);
        //}
        //else
        //{
        //    getCard = cardItem;
        //}


        if (handCards.Count != _data.handCards.Count)
        {
            Debug.LogError(string.Format("显示和数据不一致handCards.count = {0} , _data.handCards.Count = {1}", handCards.Count, _data.handCards.Count));
            for (int i = 0; i < handCards.Count; i++)
            {
                Debug.Log(string.Format("handCards = {0}",BattleAreaUtil.GetMeshCardValue( handCards[i])));
            }
            for (int i = 0; i < _data.handCards.Count; i++)
            {
                Debug.Log(string.Format("handCards = {0}", _data.handCards[i]));
            }

        }
        if (battleProxy.isSkipTween)
        {
            cardItem.transform.localPosition = targetPos;
            //if (_data.userId == playerInfoProxy.UserInfo.UserID)
            //{
            //    cardItem.layer = GlobalData.SELF_HAND_CARDS;
            //}
            //else
            //{
            //    cardItem.layer = GlobalData.OTHER_CARDS;
            //}
            BattleAreaUtil.ResortHandGangGetCard(this);
        }
        else
        {
            if (battleProxy.playerActS2C.flowerAct)
            {
                battleProxy.isForbit = true;
                yield return new WaitForSeconds(3f);
                battleProxy.isForbit = false;
            }
            cardItem.transform.DOLocalMove(targetPos, 0.1f);
            //if (_data.userId == playerInfoProxy.UserInfo.UserID)
            //{
            //    cardItem.layer = GlobalData.SELF_HAND_CARDS;
            //}
            //else
            //{
            //    cardItem.layer = GlobalData.OTHER_CARDS;
            //}
            battleProxy.isForbit = true;
            yield return new WaitForSeconds(0.2f);
            BattleAreaUtil.ResortHandGangGetCard(this);
            battleProxy.isForbit = false;
        }
    }

    /// <summary>
    /// 播放过的动作
    /// </summary>
    public void PlayPass()
    {
        BattleAreaUtil.ResortCard(this);
    }

    /// <summary>
    /// 播放碰的动作
    /// </summary>
    public void PlayPeng(GameObject pengedCard)
    {
        List<GameObject> pengCards = new List<GameObject>();
        pengedCard.transform.DOKill();
        pengCards.Add(pengedCard);
        if (_data.userId == playerInfoProxy.UserInfo.UserID)
        { //自己碰找到真正的牌碰
            for (int i = 0; i < handCards.Count;)
            {
                if (BattleAreaUtil.GetMeshCardValue(handCards[i]) == battleProxy.playerActS2C.actCard)
                {
                    pengCards.Add(handCards[i]);
                    handCards.RemoveAt(i);
                }
                else
                {
                    i++;
                }
                if (pengCards.Count == 3)
                {
                    break;
                }
            }
        }
        else
        {
            var randomIndex = Random.Range(0, handCards.Count - 2);
            pengCards.Add(handCards[randomIndex]);
            pengCards.Add(handCards[randomIndex + 1]);
            handCards.RemoveAt(randomIndex);
            handCards.RemoveAt(randomIndex);
            foreach (GameObject pengCard in pengCards)
            {
                ResourcesMgr.Instance.SetCardMesh(pengCard, battleProxy.playerActS2C.actCard);
            }
        }
        foreach (GameObject card in pengCards)
        {
            card.transform.SetParent(pengGangCardContainer);
        }
        pengGangCards.Add(pengCards);
        BattleAreaUtil.ResortCard(this);
    }

    /// <summary>
    /// 播放被碰的动作
    /// </summary>
    public GameObject PlayPenged()
    {
        GameObject pengedCard = null;
        for (int i = putCards.Count - 1; i >= 0; i--)
        {
            if (BattleAreaUtil.GetMeshCardValue(putCards[i]) == battleProxy.playerActS2C.actCard)
            {
                pengedCard = putCards[i];
                putCards.RemoveAt(i);
                break;
            }
        }
        BattleAreaUtil.ResortPutCard(this);
        return pengedCard;
    }

    /// <summary>
    /// 播放吃的动作
    /// </summary>
    public void PlayChi(GameObject chiedCard)
    {
        chiedCard.transform.DOKill();
        List<GameObject> chiCards = new List<GameObject>();
        chiCards.Add(chiedCard);
        List<int> chiCardArr = new List<int>();
        chiCardArr.Add(battleProxy.playerActS2C.actCard);
        chiCardArr.AddRange(battleProxy.playerActS2C.chiCards);
        chiCardArr.Sort();
        if (_data.userId == playerInfoProxy.UserInfo.UserID)
        { //自己碰找到真正的牌碰
            for (int i = 0; i < battleProxy.playerActS2C.chiCards.Count; i++)
            {
                for (int j = 0; j < handCards.Count; j++)
                {
                    if (BattleAreaUtil.GetMeshCardValue(handCards[j]) == battleProxy.playerActS2C.chiCards[i])
                    {
                        chiCards.Add(handCards[j]);
                        handCards.RemoveAt(j);
                        break;
                    }
                }
            }
        }
        else
        {
            var randomIndex = UnityEngine.Random.Range(0, handCards.Count - 2);
            chiCards.Add(handCards[randomIndex]);
            chiCards.Add(handCards[randomIndex + 1]);
            handCards.RemoveAt(randomIndex);
            handCards.RemoveAt(randomIndex);
        }
        for (int i = 0; i < chiCards.Count; i++)
        {
            ResourcesMgr.Instance.SetCardMesh(chiCards[i], chiCardArr[i]);
        }
        foreach (GameObject card in chiCards)
        {
            Transform trans = pengGangCardContainer;
            card.transform.SetParent(trans);
        }
        pengGangCards.Add(chiCards);
        BattleAreaUtil.ResortCard(this);
    }

    /// <summary>
    /// 播放被吃的动作
    /// </summary>
    public GameObject PlayChied()
    {
        GameObject pengedCard = null;
        for (int i = putCards.Count - 1; i >= 0; i--)
        {
            if (BattleAreaUtil.GetMeshCardValue(putCards[i]) == battleProxy.playerActS2C.actCard)
            {
                pengedCard = putCards[i];
                putCards.RemoveAt(i);
                break;
            }
        }
        BattleAreaUtil.ResortPutCard(this);
        return pengedCard;
    }

    /// <summary>
    /// 播放直接碰杠动作
    /// </summary>
    public void PlayCommonPengGang()
    {
        GameObject pengedCard = null;
        pengedCard = getCard;
        getCard = null;
        if (_data.userId != playerInfoProxy.UserInfo.UserID)
        {
            ResourcesMgr.Instance.SetCardMesh(pengedCard, battleProxy.playerActS2C.actCard);
        }

        for (int i = 0; i < pengGangCards.Count; i++)
        {
            if (BattleAreaUtil.GetMeshCardValue(pengGangCards[i][0]) == battleProxy.playerActS2C.actCard)
            {
                pengGangCards[i].Add(pengedCard);
                pengedCard.transform.SetParent(pengGangCardContainer);
                for (int j = 0; j < pengGangCards[i].Count; j++)
                {
                    ResourcesMgr.Instance.SetCardMesh(pengGangCards[i][j], battleProxy.playerActS2C.actCard);
                }
                break;
            }
        }

        BattleAreaUtil.ResortCard(this);
    }

    /// <summary>
    /// 播放回头碰杠动作
    /// </summary>
    public void PlayBackPengGang()
    {
        GameObject pengedCard = null;
        if (_data.userId == playerInfoProxy.UserInfo.UserID)
        {
            for (int i = 0; i < handCards.Count; i++)
            {
                if (BattleAreaUtil.GetMeshCardValue(handCards[i]) == battleProxy.playerActS2C.actCard)
                {
                    pengedCard = handCards[i];
                    handCards.RemoveAt(i);
                    break;
                }
            }
        }
        else
        {
            var randomIndex = UnityEngine.Random.Range(0, handCards.Count - 1);
            pengedCard = handCards[randomIndex];
            handCards.RemoveAt(randomIndex);
        }

        for (int i = 0; i < pengGangCards.Count; i++)
        {
            if (BattleAreaUtil.GetMeshCardValue(pengGangCards[i][0]) == battleProxy.playerActS2C.actCard)
            {
                pengGangCards[i].Add(pengedCard);
                pengedCard.transform.SetParent(pengGangCardContainer);
                for (int j = 0; j < pengGangCards[i].Count; j++)
                {
                    ResourcesMgr.Instance.SetCardMesh(pengGangCards[i][j], battleProxy.playerActS2C.actCard);
                }
                break;
            }
        }
        BattleAreaUtil.ResortCard(this);
    }

    /// <summary>
    /// 播放出牌动作
    /// </summary>
    public IEnumerator PlayPutCard()
    {
        var cardValue = battleProxy.playerActS2C.actCard;
        GameObject putCard = null;
        if (getCard != null)
        {
            handCards.Add(getCard);
            getCard.transform.DOKill();
            getCard = null;
        }
        if (_data.userId == playerInfoProxy.UserInfo.UserID)
        {
            for (int i = 0; i < handCards.Count; i++)
            {
                if (BattleAreaUtil.GetMeshCardValue(handCards[i]) == battleProxy.playerActS2C.actCard)
                {
                    putCard = handCards[i];
                    //_data.putCards.Add(BattleAreaUtil.GetMeshCardValue(handCards[i]));
                    handCards.RemoveAt(i);
                    break;
                }
            }
            if (_data.userId == playerInfoProxy.UserInfo.UserID)
            {
                for (int i = 0; i < handCards.Count; i++)
                {
                    handCards[i].name = "HandCard" + i;
                }
                handCards.Sort(BattleAreaUtil.CompareCard);

                var TempList = new List<GameObject>();               //精牌排在前面
                foreach (var item in handCards)
                {
                    if (item.transform.childCount > 0)
                    {
                        TempList.Add(item);
                    }
                }
                foreach (var item in TempList)
                {
                    handCards.Remove(item);
                }
                TempList.Sort(BattleAreaUtil.CompareCard);
                handCards.Sort(BattleAreaUtil.CompareCard);

                foreach (var item in handCards)
                {
                    TempList.Add(item);
                }
                handCards = TempList;
            }
        }
        else
        {
            var randomIndex = Random.Range(0, handCards.Count - 1);
            putCard = handCards[randomIndex];
            //_data.putCards.Add(BattleAreaUtil.GetMeshCardValue(handCards[randomIndex]));
            handCards.RemoveAt(randomIndex);

            ResourcesMgr.Instance.SetCardMesh(putCard, cardValue);
            //ResourcesMgr.Instance.SetCardMesh(handCards[randomIndex], cardValue);
        }

        if (dir != AreaDir.DOWN)
        {
            if (cardValue == battleProxy.treasureCardCode)
            {
                GameObject go = new GameObject();
                go.AddComponent<SpriteRenderer>();
                go.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/UI/treasureCardCode");
                go.layer = LayerMask.NameToLayer("OtherCards");
                go.transform.SetParent(putCard.transform);
                go.transform.localPosition = new Vector3(-0.104f, -0.177f, 0.21f);
                go.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 132f, 47));
                go.transform.localScale = new Vector3(.8f, .8f, .8f);
            }
        }


        //////如果是花牌则加到花牌集合里面，否则加到打出的牌的集合里面
        if (battleProxy.flowerCardList.Contains(cardValue))
        {
            flowerCards.Add(putCard);
        }
        else
        {
            putCards.Add(putCard);
        }

        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var sitIndex = (_data.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio(string.Format("Voices/{0}/Card/{1}", _data.sex == 1 ? "Man" : "Woman", cardValue)));
        if (battleProxy.isSkipTween)
        {
            BattleAreaUtil.ResortCard(this);
            ApplicationFacade.Instance.SendNotification(NotificationConstant.TING_UPDATE);
            ApplicationFacade.Instance.SendNotification(NotificationConstant.SHOW_CARD_ARROW, this);
            if (cardValue != battleProxy.treasureCardCode)
            {
                if (putCard.transform.childCount > 0)
                {
                    Destroy(putCard.transform.GetChild(0).gameObject);
                }
            }
            yield break;
        }
        battleProxy.isForbit = true;
        yield return new WaitForSeconds(AudioSystem.Instance.perAudioLength);
        putCard.transform.DOKill();
        if (sitIndex == 1)
        {
            putCard.transform.localEulerAngles = new Vector3(0, -60, 90);
            putCard.transform.DOScale(Vector3.one * 2, 0.14f);
        }
        else if (sitIndex == 2)
        {
            putCard.transform.localEulerAngles = new Vector3(0, 0, 180);
            putCard.transform.DOScale(Vector3.one * 2, 0.14f);
        }
        else if (sitIndex == 3)
        {
            putCard.transform.localEulerAngles = new Vector3(0, 60, -90);
            putCard.transform.DOScale(Vector3.one * 3, 0.14f);
        }
        else
        {
            putCard.transform.DOScale(Vector3.one * 2, 0.14f);
        }
        putCard.transform.DOLocalMove(sendCard.localPosition, 0.14f);
        yield return new WaitForSeconds(0.15f);
        List<Vector3> anglePosition = new List<Vector3>();

        //putCards.ForEach(o => { o.transform.SetParent(putCardContainer); });
        putCard.transform.SetParent(putCardContainer);

        anglePosition = BattleAreaUtil.GetPutCardPosition(this, putCards.Count - 1);
        putCard.transform.localEulerAngles = anglePosition[0];
        putCard.transform.DOLocalMove(anglePosition[1], 0.1f);
        putCard.transform.DOScale(Vector3.one, 0.1f);
        //yield return new WaitForSeconds(0.1f);
        GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio("Voices/Effect/PutCard"));
        BattleAreaUtil.ResortCard(this);
        ApplicationFacade.Instance.SendNotification(NotificationConstant.TING_UPDATE);
        battleProxy.isForbit = false;
        ApplicationFacade.Instance.SendNotification(NotificationConstant.SHOW_CARD_ARROW, this);

        if (cardValue != battleProxy.treasureCardCode)
        {
            if (putCard.transform.childCount > 0)
            {
                Destroy(putCard.transform.GetChild(0).gameObject);
            }
        }        
    }

    /// <summary>
    /// 播放出花牌动作
    /// </summary>
    public IEnumerator PlayPutFlowerCard()
    {
        var cardValue = battleProxy.playerActS2C.actCard;
        //Debug.Log("断线重连是的actCard："+cardValue);
        GameObject putCard = null;
        if (getCard != null)
        {
            handCards.Add(getCard);
            getCard.transform.DOKill();
            getCard = null;
        }
        if (_data.userId == playerInfoProxy.UserInfo.UserID)
        {
            for (int i = 0; i < handCards.Count; i++)
            {
                if (BattleAreaUtil.GetMeshCardValue(handCards[i]) == battleProxy.playerActS2C.actCard)
                {
                    putCard = handCards[i];
                    handCards.RemoveAt(i);
                    break;
                }
            }
            for (int i = 0; i < handCards.Count; i++)
            {
                handCards[i].name = "HandCard" + i;
            }
            handCards.Sort(BattleAreaUtil.CompareCard);

            var TempList = new List<GameObject>();               //精牌排在前面
            foreach (var item in handCards)
            {
                if (item.transform.childCount > 0)
                {
                    TempList.Add(item);
                }
            }
            foreach (var item in TempList)
            {
                handCards.Remove(item);
            }
            TempList.Sort(BattleAreaUtil.CompareCard);
            handCards.Sort(BattleAreaUtil.CompareCard);

            foreach (var item in handCards)
            {
                TempList.Add(item);
            }
            handCards = TempList;
        }
        else
        {
            var randomIndex = Random.Range(0, handCards.Count - 1);
            putCard = handCards[randomIndex];
            handCards.RemoveAt(randomIndex);

            ResourcesMgr.Instance.SetCardMesh(putCard, cardValue);
        }
        
        //////如果是花牌则加到花牌集合里面，否则加到打出的牌的集合里面
        if (battleProxy.flowerCardList.Contains(cardValue))
        {
            flowerCards.Add(putCard);
        }
        else
        {
            putCards.Add(putCard);
        }

        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var sitIndex = (_data.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio(string.Format("Voices/{0}/Card/{1}", _data.sex == 1 ? "Man" : "Woman", cardValue)));
        if (battleProxy.isSkipTween)
        {
            BattleAreaUtil.ResortCard(this);
            ApplicationFacade.Instance.SendNotification(NotificationConstant.TING_UPDATE);
            if (cardValue != battleProxy.treasureCardCode)
            {
                if (putCard.transform.childCount > 0)
                {
                    Destroy(putCard.transform.GetChild(0).gameObject);
                }
            }
            yield break;
        }
        battleProxy.isForbit = true;
        yield return new WaitForSeconds(AudioSystem.Instance.perAudioLength);
        putCard.transform.DOKill();
        if (sitIndex == 1)
        {
            putCard.transform.localEulerAngles = new Vector3(0, -60, 90);
            putCard.transform.DOScale(Vector3.one * 3, 0.14f);
        }
        else if (sitIndex == 2)
        {
            putCard.transform.localEulerAngles = new Vector3(0, 0, 180);
            putCard.transform.DOScale(Vector3.one * 3, 0.14f);
        }
        else if (sitIndex == 3)
        {
            putCard.transform.localEulerAngles = new Vector3(0, 60, -90);
            putCard.transform.DOScale(Vector3.one * 3, 0.14f);
        }
        else
        {
            putCard.transform.DOScale(Vector3.one * 3, 0.14f);
        }
        putCard.transform.DOLocalMove(sendCard.localPosition, 0.14f);
        yield return new WaitForSeconds(0.25f);
        List<Vector3> anglePosition = new List<Vector3>();

        flowerCards.ForEach(o => { o.transform.SetParent(flowersCardContainer); });
        //putCard.transform.SetParent(putCardContainer);
        anglePosition = BattleAreaUtil.GetPutCardPosition(this, flowerCards.Count - 1);
        putCard.transform.localEulerAngles = anglePosition[0];
        putCard.transform.DOLocalMove(anglePosition[1], 0.1f);
        putCard.transform.DOScale(Vector3.one, 0.1f);
        yield return new WaitForSeconds(0.1f);
        GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio("Voices/Effect/PutCard"));
        BattleAreaUtil.ResortCard(this);
        ApplicationFacade.Instance.SendNotification(NotificationConstant.TING_UPDATE);
        battleProxy.isForbit = false;
        yield return new WaitForEndOfFrame();
        
    }

    /// <summary>
    /// 播放直杠动作
    /// </summary>
    /// <param name="zhiGangedCard"></param>
    public void PlayZhiGang(GameObject zhiGangedCard)
    {
        zhiGangedCard.transform.DOKill();
        List<GameObject> gangCards = new List<GameObject>();
        gangCards.Add(zhiGangedCard);
        if (_data.userId == playerInfoProxy.UserInfo.UserID)
        {
            for (int i = 0; i < handCards.Count;)
            {
                if (BattleAreaUtil.GetMeshCardValue(handCards[i]) == battleProxy.playerActS2C.actCard)
                {
                    gangCards.Add(handCards[i]);
                    handCards.RemoveAt(i);
                }
                else
                {
                    i++;
                }
                if (gangCards.Count == 4)
                {
                    break;
                }
            }
        }
        else
        {
            var randomIndex = UnityEngine.Random.Range(0, handCards.Count - 3);
            gangCards.Add(handCards[randomIndex]);
            gangCards.Add(handCards[randomIndex + 1]);
            gangCards.Add(handCards[randomIndex + 2]);
            handCards.RemoveAt(randomIndex);
            handCards.RemoveAt(randomIndex);
            handCards.RemoveAt(randomIndex);
            foreach (GameObject gangCard in gangCards)
            {
                ResourcesMgr.Instance.SetCardMesh(gangCard, battleProxy.playerActS2C.actCard);
            }
        }
        foreach (GameObject card in gangCards)
        {
            card.transform.SetParent(pengGangCardContainer);
        }
        pengGangCards.Add(gangCards);
        BattleAreaUtil.ResortHandGangGetCard(this);
    }

    /// <summary>
    /// 播放被直杠动作
    /// </summary>
    public GameObject PlayZhiGanged()
    {
        GameObject zhiGangCard = null;
        for (int i = putCards.Count - 1; i >= 0; i--)
        {
            if (BattleAreaUtil.GetMeshCardValue(putCards[i]) == battleProxy.playerActS2C.actCard)
            {
                zhiGangCard = putCards[i];
                putCards.RemoveAt(i);
                break;
            }
        }
        BattleAreaUtil.ResortPutCard(this);
        return zhiGangCard;
    }

    /// <summary>
    /// 播放自摸胡牌动作
    /// </summary>
    public void PlaySelfHu()
    {
        if (getCard != null)
        {
            handCards.Add(getCard);
            getCard.transform.DOKill();
            getCard = null;
        }
        if (_data.userId == playerInfoProxy.UserInfo.UserID)
        {
            for (int i = 0; i < handCards.Count; i++)
            {
                handCards[i].name = "HandCard" + i;
            }
            handCards.Sort(BattleAreaUtil.CompareCard);

            var TempList = new List<GameObject>();               //精牌排在前面
            foreach (var item in handCards)
            {
                if (item.transform.childCount > 0)
                {
                    TempList.Add(item);
                }
            }
            foreach (var item in TempList)
            {
                handCards.Remove(item);
            }
            TempList.Sort(BattleAreaUtil.CompareCard);
            handCards.Sort(BattleAreaUtil.CompareCard);
            foreach (var item in handCards)
            {
                TempList.Add(item);
            }
            handCards = TempList;
        }
        BattleAreaUtil.ResortHandGangGetCard(this);
    }

    /// <summary>
    /// 播放被胡动作
    /// </summary>
    /// <returns></returns>
    public GameObject PlayHued()
    {
        GameObject huCard = null;
        for (int i = putCards.Count - 1; i >= 0; i--)
        {
            if (BattleAreaUtil.GetMeshCardValue(putCards[i]) == battleProxy.playerActS2C.actCard)
            {
                huCard = putCards[i];
                putCards.RemoveAt(i);
                break;
            }
        }
        BattleAreaUtil.ResortPutCard(this);
        return huCard;
    }

    /// <summary>
    /// 播放胡牌动作
    /// </summary>
    /// <param name="huedCard"></param>
    public void PlayHu(GameObject huedCard)
    {
        huedCard.transform.SetParent(handCardContainer);
        handCards.Add(huedCard);
        if (_data.userId == playerInfoProxy.UserInfo.UserID)
        {
            for (int i = 0; i < handCards.Count; i++)
            {
                handCards[i].name = "HandCard" + i;
            }
            handCards.Sort(BattleAreaUtil.CompareCard);

            var TempList = new List<GameObject>();               //精牌排在前面
            foreach (var item in handCards)
            {
                if (item.transform.childCount > 0)
                {
                    TempList.Add(item);
                }
            }
            foreach (var item in TempList)
            {
                handCards.Remove(item);
            }
            TempList.Sort(BattleAreaUtil.CompareCard);
            handCards.Sort(BattleAreaUtil.CompareCard);
            foreach (var item in handCards)
            {
                TempList.Add(item);
            }
            handCards = TempList;
        }
        BattleAreaUtil.ResortHandGangGetCard(this);
    }



    /// <summary>
    /// 回收卡牌资源
    /// </summary>
    public void SaveAllCard()
    {
        List<GameObject> needSaveCard = new List<GameObject>();
        foreach (Transform card in flowersCardContainer)
        {
            if (defaultCardIdList.Contains(card.gameObject.GetInstanceID()))
            {
                continue;
            }
            needSaveCard.Add(card.gameObject);
        }
        foreach (Transform card in handCardContainer)
        {
            if (defaultCardIdList.Contains(card.gameObject.GetInstanceID()))
            {
                if (card.transform.childCount > 0)
                {
                    Destroy(card.transform.GetChild(0).gameObject);
                }
                continue;
            }
            needSaveCard.Add(card.gameObject);
        }
        foreach (Transform card in pengGangCardContainer)
        {
            if (defaultCardIdList.Contains(card.gameObject.GetInstanceID()))
            {
                if (card.transform.childCount > 0)
                {
                    Destroy(card.transform.GetChild(0).gameObject);
                }
                continue;
            }
            needSaveCard.Add(card.gameObject);
        }
        foreach (Transform card in putCardContainer)
        {


            if (card.gameObject.name == "CardArrowIcon")
            {
                continue;
            }
            else
            {
                if (defaultCardIdList.Contains(card.gameObject.GetInstanceID()))
                {

                    if (card.transform.childCount > 0)
                    {
                        Destroy(card.transform.GetChild(0).gameObject);
                    }

                    continue;
                }
                needSaveCard.Add(card.gameObject);
            }

        }
        if (GlobalData.hasHeap)
        {
            foreach (Transform card in heapCardContainer)
            {
                if (defaultCardIdList.Contains(card.gameObject.GetInstanceID()))
                {
                    if (card.transform.childCount > 0)
                    {
                        Destroy(card.transform.GetChild(0).gameObject);
                    }
                    continue;
                }

                needSaveCard.Add(card.gameObject);
            }
            heapCards.Clear();
        }
        foreach (GameObject item in needSaveCard)
        {
            ResourcesMgr.Instance.Add2Pool(item);
        }

        pengGangCards.Clear();
        handCards.Clear();
        putCards.Clear();
        flowerCards.Clear();
        getCard = null;
        sendIndex = 0;
        sendedCardNum = 0;
    }
}

public enum AreaDir
{
    LEFT,
    RIGHT,
    DOWN,
    UP
}