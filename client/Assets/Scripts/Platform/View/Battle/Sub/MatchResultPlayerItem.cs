﻿using System.Collections;
using Platform.Model;
using Platform.Model.Battle;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 单局结算头像节点
/// </summary>
public class MatchResultPlayerItem : MonoBehaviour
{
    /// <summary>
    /// 节点玩家数据
    /// </summary>
    private PlayerMatchResultVOS2C _data;
    /// <summary>
    /// 手上的牌数组
    /// </summary>
    private List<GameObject> handCards;
    /// <summary>
    /// 牌之间的间距
    /// </summary>
    private Vector3 cardHGap;

    /// <summary>
    /// 本局获得的积分
    /// </summary>
    private int addScoreTxt;

    /// <summary>
    /// 庄家标志
    /// </summary>
    private GameObject bankerIcon;

    /// <summary>
    /// 头像
    /// </summary>
    private RawImage heroIcon;

    /// <summary>
    /// 玩家名称
    /// </summary>
    private Text nameTxt;
    /// <summary>
    /// 牌的容器
    /// </summary>
    private Transform cardContainer;
    /// <summary>
    /// 房主图标
    /// </summary>
    private Image fangzhuImg;
    /// <summary>
    /// 胡法
    /// </summary>
    private Image hufa;
    /// <summary>
    /// 玩家行背景
    /// </summary>
    private Image playerRowBg;
    /// <summary>
    /// 总分
    /// </summary>
    private Text tatolScoreText;
    /// <summary>
    /// 胡牌类型
    /// </summary>
    private Text huPaiTxet;

    // Use this for initialization
    void Awake()
    {
        heroIcon = transform.Find("HeroIcon").GetComponent<RawImage>();
        nameTxt = transform.Find("NameTxt").GetComponent<Text>();
        bankerIcon = transform.Find("BankerIcon").gameObject;
        fangzhuImg = transform.Find("fangzhu").GetComponent<Image>();
        playerRowBg = transform.Find("RowBG").GetComponent<Image>();
        hufa = transform.Find("oparatedetail/hufa").GetComponent<Image>();
        cardContainer = transform.Find("Card");
        huPaiTxet = transform.Find("huPaiType").GetComponent<Text>();
        tatolScoreText = transform.Find("tatolScore").GetComponent<Text>();
        var card1 = cardContainer.Find("Card1");
        var card2 = cardContainer.Find("Card2");
        handCards = new List<GameObject>();
        cardHGap = card2.localPosition - card1.localPosition;
    }

    /// <summary>
    /// 头像框对应的玩家数据
    /// </summary>
    public PlayerMatchResultVOS2C data
    {
        get { return _data; }
        set
        {
            if (this.cardContainer.childCount>0)
            {
                for (int i = 0; i < cardContainer.childCount; i++)
                {
                    if (cardContainer.transform.GetChild(i).gameObject.name != "Card1"
                     && cardContainer.transform.GetChild(i).gameObject.name != "Card2")
                    {
                        Destroy(cardContainer.transform.GetChild(i).gameObject); 
                    }
                }
            }
            _data = value;
            var battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
            var playerProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            var playerInfoVO = battleProxy.playerIdInfoDic[value.userId];
            
            GameMgr.Instance.StartCoroutine(DownIcon(playerInfoVO.headIcon));
            nameTxt.text = playerInfoVO.name;
            addScoreTxt = value.addScore;
            //Debug.Log("分数："+ addScoreTxt);
            if (value.userId == playerProxy.UserInfo.UserID)
            {
                playerRowBg.sprite = Resources.Load<Sprite>("Textures/UI/YellowBg");
            }
            else
            {
                playerRowBg.sprite = Resources.Load<Sprite>("Textures/UI/WhiteBg");
            }
            if (addScoreTxt < 0 )
            {
                tatolScoreText.GetComponent<Text>().font = Resources.Load<Font>("Fonts/AddScore_1") as Font;
            }
            else
            {
                tatolScoreText.GetComponent<Text>().font = Resources.Load<Font>("Fonts/AddScore") as Font;
            }
            tatolScoreText.text = addScoreTxt.ToString();
            bankerIcon.SetActive(playerInfoVO.userId == battleProxy.perBankerId);
            float cardNum = 0;
            if (_data.pengGangs.Count > 0)
            {
                for (int i = 0; i < _data.pengGangs.Count; i++)
                {
                for (int j = 0; j < _data.pengGangs[i].pengGangCards.Count; j++)
                {
                    if (j == _data.pengGangs[i].pengGangCards.Count-1 && _data.pengGangs[i].pengGangCards.Count - 1>=3)
                    {
                        GameObject cardItem = GetCardObject(_data.pengGangs[i].pengGangCards[j], 180);
                        cardItem.transform.SetParent(cardContainer);
                        cardItem.transform.localPosition = new Vector3(GetCardPositionByIndex(cardNum - 2).x, GetCardPositionByIndex(cardNum - 2).y+12f,1f);
                        cardItem.transform.localScale = new Vector3(0.44f, 0.5f, 0.6f);
                        cardItem.transform.localEulerAngles = new Vector3(0, 0, 180);
                        handCards.Add(cardItem);
                        GetTreasureCard(_data.pengGangs[i].pengGangCards[j], cardItem, battleProxy,true);
                    }
                    else
                    {
                    GameObject cardItem = GetCardObject(_data.pengGangs[i].pengGangCards[j],180);
                    cardItem.transform.SetParent(cardContainer);
                    cardItem.transform.localPosition = GetCardPositionByIndex(cardNum);
                    cardItem.transform.localScale = new Vector3(0.44f, 0.5f, 0.6f);
                    cardItem.transform.localEulerAngles = new Vector3(0, 0, 180);
                    handCards.Add(cardItem);
                    GetTreasureCard(_data.pengGangs[i].pengGangCards[j], cardItem, battleProxy, true);
                    cardNum++;      
                    }
                }
                cardNum += 0.1f;
                }
        
                cardNum += 0.3f;
            }

            if (battleProxy.matchResultS2C.huUserId.Contains(_data.userId))//胡牌不在手牌内
            {
                _data.handCards.Remove(battleProxy.matchResultS2C.huedCard);
            }
            for (int i = 0; i < _data.handCards.Count; i++)
            {
                GameObject cardItem = GetCardObject(_data.handCards[i],0);
                cardItem.transform.SetParent(cardContainer);
                cardItem.transform.localPosition = GetCardPositionByIndex(cardNum);
                cardItem.transform.localScale = new Vector3(0.44f, 0.5f, 0.6f);
                cardItem.transform.localEulerAngles = Vector3.zero;
                handCards.Add(cardItem);
                GetTreasureCard(_data.handCards[i], cardItem, battleProxy,false);
                cardNum++;
       
            }
            if (battleProxy.matchResultS2C.huUserId.Contains(data.userId))//添加胡别人的牌
            {
                cardNum += 0.7f;
                GameObject cardItem = GetCardObject(battleProxy.matchResultS2C.huedCard,0);
                cardItem.transform.SetParent(cardContainer);
                cardItem.transform.localPosition = GetCardPositionByIndex(cardNum);
                cardItem.transform.localScale = new Vector3(0.44f, 0.5f, 0.6f);//Vector3.one;
                cardItem.transform.localEulerAngles = Vector3.zero;
                handCards.Add(cardItem);
                GetTreasureCard(battleProxy.matchResultS2C.huedCard, cardItem, battleProxy,false);
            }
            if (battleProxy.matchResultS2C.ziMoUserId == _data.userId)//自摸
            {
                fangzhuImg.gameObject.SetActive(true);
                hufa.gameObject.SetActive(true);
                hufa.sprite = Resources.Load<Sprite>("Textures/UI/SelfHu");
                //fangzhuImg.sprite = Resources.Load<Sprite>("Textures/HuType/SelfHu");
            }
            else if (battleProxy.matchResultS2C.huedUserId == _data.userId)//点炮
            {
                hufa.gameObject.SetActive(true);
                hufa.sprite = Resources.Load<Sprite>("Textures/UI/Loser");
            }
            else if (battleProxy.matchResultS2C.huedUserId > 0 && battleProxy.matchResultS2C.huUserId.Contains(_data.userId))//接炮
            {
                hufa.gameObject.SetActive(true);
                hufa.sprite = Resources.Load<Sprite>("Textures/UI/Winner");
            }
            else
            {
                hufa.gameObject.SetActive(false);
            }

            //房主
            if(battleProxy.creatorId == playerInfoVO.userId)
            {
                fangzhuImg.gameObject.SetActive(true);
            }
            else
            {
                fangzhuImg.gameObject.SetActive(false);
            }
            foreach (var item in battleProxy.matchResultS2C.huUserId)
            {
                Debug.Log("胡牌人的ID"+ item);
            }
            if (battleProxy.matchResultS2C.huUserId.Contains(data.userId)&&data.anGangCount>0&&data.mingGangCount>0&& data.flowerCount>0)
            {
                huPaiTxet.text = " "+"明杠X" + data.mingGangCount + "  " + "暗杠X" + data.anGangCount + "  " + "花X" + data.flowerCount + " "+ "(" + data.huDesc + ")";
            }
            else if (battleProxy.matchResultS2C.huUserId.Contains(data.userId) && data.anGangCount > 0 && data.mingGangCount > 0)
            {
                huPaiTxet.text = " " + "明杠X" + data.mingGangCount + "  " + "暗杠X" + data.anGangCount  + " " + "(" + data.huDesc + ")";
            }
            else if (battleProxy.matchResultS2C.huUserId.Contains(data.userId) && data.anGangCount > 0 && data.flowerCount > 0)
            {
                huPaiTxet.text = " " + "暗杠X" + data.anGangCount + "  " + "花X" + data.flowerCount + " " + "(" + data.huDesc + ")";
            }
            else if (battleProxy.matchResultS2C.huUserId.Contains(data.userId) && data.anGangCount > 0 )
            {
                huPaiTxet.text = " " + "暗杠X" + data.anGangCount+" "  + "(" + data.huDesc + ")";
            }
            else if (battleProxy.matchResultS2C.huUserId.Contains(data.userId) && data.mingGangCount > 0 && data.flowerCount > 0)
            {
                huPaiTxet.text = " " + "明杠X" + data.mingGangCount + "  " + "花X" + data.flowerCount + " " + "(" + data.huDesc + ")";
            }
            else if (battleProxy.matchResultS2C.huUserId.Contains(data.userId) && data.mingGangCount > 0)
            {
                huPaiTxet.text = " " + "明杠X" + data.mingGangCount + " " + "(" + data.huDesc + ")";
            }
            else if (battleProxy.matchResultS2C.huUserId.Contains(data.userId) && data.flowerCount > 0)
            {
                huPaiTxet.text = "  " + "花X" + data.flowerCount+" " + "(" + data.huDesc + ")";
            }
            else if (battleProxy.matchResultS2C.huUserId.Contains(data.userId))
            {
                huPaiTxet.text = " " + "(" + data.huDesc + ")";
            }
            else if (data.anGangCount > 0 && data.mingGangCount > 0 && data.flowerCount > 0)
            {
                huPaiTxet.text = " " + "明杠X" + data.mingGangCount + "  " + "暗杠X" + data.anGangCount+"  " + "花X" + data.flowerCount;
            }
            else if (data.anGangCount > 0 && data.mingGangCount > 0)
            {
                huPaiTxet.text = " " + "明杠X" + data.mingGangCount + "  " + "暗杠X" + data.anGangCount;
            }
            else if (data.anGangCount > 0 && data.flowerCount > 0)
            {
                huPaiTxet.text = " " + "暗杠X" + data.anGangCount + "  " + "花X" + data.flowerCount;
            }
            else if (data.anGangCount > 0 )
            {
                huPaiTxet.text = " " + "暗杠X" + data.anGangCount;
            }
            else if (data.mingGangCount > 0 && data.flowerCount > 0)
            {
                huPaiTxet.text = " " + "明杠X" + data.mingGangCount + "  " + "花X" + data.flowerCount;
            }
            else if (data.mingGangCount > 0)
            {
                huPaiTxet.text = " " + "明杠X" + data.mingGangCount;
            }
            else if (data.flowerCount > 0)
            {
                huPaiTxet.text = "  " + "花X" + data.flowerCount;
            }
            else
            {
                huPaiTxet.text = "";
            }
            

        }
    }

    private void GetTreasureCard(int cardNumber,GameObject card,BattleProxy battleProxy,bool ispeng)
    {
        if (cardNumber == battleProxy.treasureCardCode)
        {
        GameObject go = new GameObject();
        go.AddComponent<Image>();
        go.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/UI/treasureCardCode");
        go.transform.SetParent(card.transform);
      
        go.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            if (ispeng)
            {
                go.transform.localPosition = new Vector3(-15.5f, -30.5f, 0);
                go.transform.localEulerAngles = new Vector3(0,0,180);
            }
            else
            {
                go.transform.localPosition = new Vector3(14.2f, 13.1f, 0);
                go.transform.localEulerAngles = Vector3.zero;
            }

        }
    }
    /// <summary>
    /// 获取牌面对象
    /// </summary>
    /// <param name="cardValue"></param>
    /// <returns></returns>
    private GameObject GetCardObject(int cardValue,int euler)
    {
        GameObject cardPerfab = Resources.Load<GameObject>("Prefab/UI/Battle/CardSelectBtn");

        //cardPerfab.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        var cardGameObject = Instantiate(cardPerfab);
        cardGameObject.transform.Find("CardFront").GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("Textures/Card/{0}", cardValue));
        cardGameObject.transform.Find("CardFront").localEulerAngles = new Vector3(0,0, euler);
        return cardGameObject;
    }

    /// <summary>
    /// 根据序号获取牌的位置
    /// </summary>
    private Vector3 GetCardPositionByIndex(float cardIndex)
    {
        return cardIndex * cardHGap;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    /// <summary>
    /// 异步加载头像
    /// </summary>
    /// <param name="headUrl"></param>
    /// <returns></returns>
    private IEnumerator DownIcon(string headUrl)
    {
        var www = new WWW(headUrl);
        yield return www;
        if (www.error == null)
            heroIcon.texture = www.texture;
    }



    /// <summary>
    /// 回收所有牌
    /// </summary>
    public void SaveAllCard()
    {
        foreach (var Cards in handCards)
        {
         Object.Destroy(Cards);
        }
        handCards.Clear();
    }

 
}