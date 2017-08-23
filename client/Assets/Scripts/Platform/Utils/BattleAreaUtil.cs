﻿using DG.Tweening;
using Platform.Model.Battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Utils
{
    /// <summary>
    /// 牌面排序相关工具类
    /// </summary>
    class BattleAreaUtil
    {
        /// <summary>
        /// 获取麻将对应的牌值
        /// </summary>
        /// <param name="obj">麻将对象</param>
        /// <returns></returns>
        public static int GetMeshCardValue(GameObject obj)
        {
            var meshFilter = obj.GetComponent<MeshFilter>();
            var nameIndex = Array.IndexOf(GlobalData.MeshNames, meshFilter.sharedMesh.name);
            return GlobalData.CardValues[nameIndex];
        }

        /// <summary>
        /// 重排牌的位置
        /// </summary>
        public static void ResortCard(BattleAreaItem areaItem)
        {
            ResortHandGangGetCard(areaItem);
            ResortPutCard(areaItem);
            ResorFlowerCard(areaItem);
        }

        /// <summary>
        /// 重排碰杠 手牌 摸到的牌位置
        /// </summary>
        public static void ResortHandGangGetCard(BattleAreaItem areaItem)
        {
            var battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            areaItem.selectCard = null;
            //碰杠牌排序
            Vector3 pengGangPos = new Vector3(0, 0, 0); // Vector3.zero;
            GameObject lastPengGangCard = null;
            int pengone = 0;//碰标志
            int mgangone = 0;
            var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
            //selfInfoVO.pengGangCards[0].p
            if (areaItem.pengGangCards.Count != areaItem.data.pengGangCards.Count)
            {
                Debug.LogError("显示与数据不一致");
            }
            for (int i = 0; i < areaItem.pengGangCards.Count; i++)
            {
                if (i > 0)
                {
                    pengGangPos += areaItem.pengGangGap * 1.2f;
                }

                if (areaItem.pengGangCards[i].Count != areaItem.data.pengGangCards[i].pengGangCards.Count)
                {
                    Debug.LogError("显示与数据不一致");
                }
                for (int j = 0; j < areaItem.pengGangCards[i].Count; j++)
                {
                    if (j > 0)
                    {
                        pengGangPos += areaItem.pengGangGap;
                    }
                    if (areaItem.data.pengGangCards[i].specialMahjong == -1)//暗杠
                    {
                        areaItem.pengGangCards[i][j].transform.localEulerAngles = new Vector3(180, 0, 0);
                        if (GetMeshCardValue(areaItem.pengGangCards[i][j]) == battleProxy.treasureCardCode)
                        {
                            if (areaItem.pengGangCards[i][j].transform.childCount > 0)
                            {
                                UnityEngine.Object.Destroy(areaItem.pengGangCards[i][j].transform.GetChild(0).gameObject);
                            }
                        }
                    }
                    else if (areaItem.data.pengGangCards[i].specialMahjong == -2)//明杠
                    {
                        if (mgangone == 0)
                        {
                            areaItem.pengGangCards[i][j].transform.localEulerAngles = new Vector3(180, 0, 0);
                            mgangone++;
                            if (GetMeshCardValue(areaItem.pengGangCards[i][j]) == battleProxy.treasureCardCode)
                            {
                                if (areaItem.pengGangCards[i][j].transform.childCount > 0)
                                {
                                    UnityEngine.Object.Destroy(areaItem.pengGangCards[i][j].transform.GetChild(0).gameObject);
                                }
                            }
                        }
                        else {
                            areaItem.pengGangCards[i][j].transform.localEulerAngles = new Vector3(0, 0, 0);
                            if (GetMeshCardValue(areaItem.pengGangCards[i][j]) == battleProxy.treasureCardCode)
                            {
                                if (areaItem.dir == AreaDir.DOWN)
                                {
                                    if (areaItem.pengGangCards[i][j].transform.childCount > 0)
                                    {
                                        areaItem.pengGangCards[i][j].layer = GlobalData.SELF_HAND_CARDS;
                                    }
                                }
                                else
                                {
                                    if (areaItem.pengGangCards[i][j].transform.childCount > 0)
                                    {
                                        areaItem.pengGangCards[i][j].layer = GlobalData.OTHER_CARDS;
                                    }
                                }
                            }
                        }
                    }
                    else if (areaItem.data.pengGangCards[i].specialMahjong == -3 )//碰
                    {
                        if (pengone == 0)
                        {
                            areaItem.pengGangCards[i][j].transform.localEulerAngles = new Vector3(180, 0, 0);
                            pengone++;
                            if (GetMeshCardValue(areaItem.pengGangCards[i][j]) == battleProxy.treasureCardCode)
                            {
                                if (areaItem.pengGangCards[i][j].transform.childCount > 0)
                                {
                                    UnityEngine.Object.Destroy(areaItem.pengGangCards[i][j].transform.GetChild(0).gameObject);
                                }
                            }
                        }
                        else
                        {
                            areaItem.pengGangCards[i][j].transform.localEulerAngles = new Vector3(0, 0, 0);
                            if (GetMeshCardValue(areaItem.pengGangCards[i][j]) == battleProxy.treasureCardCode)
                            {
                                if (areaItem.dir == AreaDir.DOWN)
                                {
                                    if (areaItem.pengGangCards[i][j].transform.childCount > 0)
                                    {
                                        areaItem.pengGangCards[i][j].layer = GlobalData.SELF_HAND_CARDS;
                                    }
                                }
                                else
                                {
                                    if (areaItem.pengGangCards[i][j].transform.childCount > 0)
                                    {
                                        areaItem.pengGangCards[i][j].layer = GlobalData.OTHER_CARDS;
                                    }
                                }
                            }
                        }

                    }
                    else if (GetMeshCardValue(areaItem.pengGangCards[i][j]) == areaItem.data.pengGangCards[i].specialMahjong)//吃
                    {
                        areaItem.pengGangCards[i][j].transform.localEulerAngles = new Vector3(180, 0, 0);
                        if (areaItem.pengGangCards[i][j].transform.childCount > 0)
                        {
                            UnityEngine.Object.Destroy(areaItem.pengGangCards[i][j].transform.GetChild(0).gameObject);
                        }
                    }
                    else
                    {
                        areaItem.pengGangCards[i][j].transform.localEulerAngles = new Vector3(0, 0, 0);
                        if (GetMeshCardValue(areaItem.pengGangCards[i][j]) == battleProxy.treasureCardCode)
                        {
                            if (areaItem.dir == AreaDir.DOWN)
                            {
                                if (areaItem.pengGangCards[i][j].transform.childCount > 0)
                                {
                                    areaItem.pengGangCards[i][j].layer = GlobalData.SELF_HAND_CARDS;
                                }
                            }
                            else
                            {
                                if (areaItem.pengGangCards[i][j].transform.childCount > 0)
                                {
                                    areaItem.pengGangCards[i][j].layer = GlobalData.OTHER_CARDS;
                                }
                            }
                        }
                    }
                     
                    areaItem.pengGangCards[i][j].transform.localScale = Vector3.one;

                    if (areaItem.dir == AreaDir.RIGHT)
                    {
                        areaItem.pengGangCards[i][j].layer = GlobalData.RIGHTHAND_CARDS;
                    }
                    else
                    {
                        if (areaItem.data.userId == playerInfoProxy.UserInfo.UserID)
                        {
                            areaItem.pengGangCards[i][j].layer = GlobalData.SELF_HAND_CARDS;
                        }
                        else
                        {
                            areaItem.pengGangCards[i][j].layer = GlobalData.OTHER_CARDS;
                        }

                    }

                    if (areaItem.dir == AreaDir.UP)
                    {
                        //areaItem.pengGangCards[i][j].transform.localRotation = Quaternion.Euler(new Vector3(0,180,0));
                    }
                    areaItem.pengGangCards[i][j].transform.localPosition = pengGangPos;
                    lastPengGangCard = areaItem.pengGangCards[i][j];
                }
            }

            //手中的牌排序
            Vector3 handPos = Vector3.zero;
            if (lastPengGangCard != null)
            {
                Vector3 pengGangWorldPos = lastPengGangCard.transform.position;
                handPos = areaItem.handCardContainer.InverseTransformPoint(pengGangWorldPos);
                handPos += areaItem.handCardGap;
                handPos.y = 0;
                handPos.z = 0;
            }
            if (areaItem.handCards.Count != areaItem.data.handCards.Count)
            {
                Debug.LogError(string.Format("显示与数据不一致 {0} {1}", areaItem.handCards.Count, areaItem.data.handCards.Count));
                foreach (var ah in areaItem.handCards)
                {
                    Debug.Log(areaItem.handCards.Count + " areaItem.handCards = " + ah);
                }
                foreach (var adh in areaItem.data.handCards)
                {
                    Debug.Log(areaItem.data.handCards.Count + " areaItem.data.handCards = " + adh);
                }
            }
            for (int i = 0; i < areaItem.handCards.Count; i++)
            {
                areaItem.handCards[i].transform.localScale = Vector3.one;
                areaItem.handCards[i].transform.localEulerAngles = Vector3.zero;
                areaItem.handCards[i].transform.localPosition = handPos;
                if (areaItem.data.userId == playerInfoProxy.UserInfo.UserID)
                {
                    areaItem.handCards[i].layer = GlobalData.SELF_HAND_CARDS;
                }
                else
                {
                    //areaItem.handCards[i].layer = GlobalData.OTHER_CARDS;
                    if (areaItem.dir == AreaDir.RIGHT)
                    {
                        areaItem.handCards[i].layer = GlobalData.RIGHTHAND_CARDS;
                    }
                    else
                        areaItem.handCards[i].layer = GlobalData.OTHER_CARDS;
                }
                if (i + 1 < areaItem.handCards.Count)
                {
                    handPos += areaItem.handCardGap;
                }
                else
                {
                    handPos += areaItem.getHandCardGap;
                }
            }
            //摸到的牌
            if (areaItem.getCard != null)
            {
                if (areaItem.data.getCard == 0)
                {
                    Debug.LogError("显示与数据不一致000");
                }
                areaItem.getCard.transform.localScale = Vector3.one;
                areaItem.getCard.transform.localEulerAngles = Vector3.zero;
                areaItem.getCard.transform.localPosition = handPos;
                if (areaItem.data.userId == playerInfoProxy.UserInfo.UserID)
                {
                    areaItem.getCard.layer = GlobalData.SELF_HAND_CARDS;
                    //摸到的精牌
                    if (areaItem.data.getCard == battleProxy.treasureCardCode)
                    {
                        if (areaItem.getCard.transform.childCount == 0)
                        {
                            GameObject go = new GameObject();
                            go.AddComponent<SpriteRenderer>();
                            go.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/UI/treasureCardCode");
                            go.layer = LayerMask.NameToLayer("SelfHandCards");
                            go.transform.SetParent(areaItem.getCard.transform);
                            go.transform.localPosition = new Vector3(-0.104f, -0.177f, 0.21f);
                            go.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 132f, 47));
                            go.transform.localScale = new Vector3(.8f, .8f, .8f);
                        }
                    }

                }
                else
                {
                    //areaItem.getCard.layer = GlobalData.OTHER_CARDS;
                    if (areaItem.dir == AreaDir.RIGHT)
                    {
                        areaItem.getCard.layer = GlobalData.RIGHTHAND_CARDS;
                    }
                    else
                        areaItem.getCard.layer = GlobalData.OTHER_CARDS;
                }
            }
            else
            {
                if (areaItem.data.getCard != 0)
                {
                    Debug.LogError("显示与数据不一致111");
                }
            }
        }

        /// <summary>
        /// 重排已出的牌
        /// </summary>
        public static void ResortPutCard(BattleAreaItem areaItem)
        {

            for (int i = 0; i < areaItem.putCards.Count; i++)
            {
                var anglePosition = GetPutCardPosition(areaItem, i);
                areaItem.putCards[i].transform.SetParent(areaItem.putCardContainer);
                areaItem.putCards[i].transform.localScale = Vector3.one;
                areaItem.putCards[i].transform.localEulerAngles = anglePosition[0];
                areaItem.putCards[i].transform.localPosition = anglePosition[1];
                //areaItem.putCards[i].layer = GlobalData.OTHER_CARDS;
                if (areaItem.dir == AreaDir.RIGHT)
                {
                    areaItem.putCards[i].layer = GlobalData.RIGHTHAND_CARDS;
                }
                else
                    areaItem.putCards[i].layer = GlobalData.OTHER_CARDS;

                if (areaItem.putCards[i].transform.childCount > 0)
                {
                    areaItem.putCards[i].transform.GetChild(0).gameObject.layer = areaItem.putCards[i].layer;
                    areaItem.putCards[i].transform.GetChild(0).localPosition = new Vector3(-0.115f, -0.184f, 0.233f);
                    areaItem.putCards[i].transform.GetChild(0).localEulerAngles = new Vector3(-90, 73, 106);
                    areaItem.putCards[i].transform.GetChild(0).localScale = new Vector3(0.8f, 0.8f, 0.8f);
                }
            }
        }

        /// <summary>
        /// 重排已出的花牌
        /// </summary>
        /// <param name="areaItem"></param>
        public static void ResorFlowerCard(BattleAreaItem areaItem)
        {
            var battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            float y = 0;
            for (int i = 0; i < areaItem.flowerCards.Count; i++)
            {
                areaItem.flowerCards[i].transform.SetParent(areaItem.flowersCardContainer);
                if (areaItem.dir == AreaDir.DOWN)
                {
                    areaItem.flowerCards[i].layer = GlobalData.SELF_HAND_CARDS; 
                }

                if (i<4)
                {
                    if (areaItem.dir == AreaDir.DOWN)
                    {
                        areaItem.flowerCards[i].transform.localPosition = new Vector3(0 - 0.551f * i, y, 0);
                        areaItem.flowerCards[i].transform.localEulerAngles = Vector3.zero;
                        areaItem.flowerCards[i].transform.localScale = Vector3.one;
                    }
                    if (areaItem.dir == AreaDir.LEFT)
                    {
                        areaItem.flowerCards[i].transform.localPosition = new Vector3(0 - 0.551f * i, y, 0);
                        areaItem.flowerCards[i].transform.localEulerAngles = Vector3.zero;
                        areaItem.flowerCards[i].transform.localScale = Vector3.one;
                    }
                    if (areaItem.dir == AreaDir.RIGHT)
                    {
                        areaItem.flowerCards[i].transform.localPosition = new Vector3(0 - 0.551f * i, y, 0);
                        areaItem.flowerCards[i].transform.localEulerAngles = Vector3.zero;
                        areaItem.flowerCards[i].transform.localScale = Vector3.one;
                    }
                    if (areaItem.dir == AreaDir.UP)
                    {
                        areaItem.flowerCards[i].transform.localPosition = new Vector3(0 - 0.551f * i, y, 0);
                        areaItem.flowerCards[i].transform.localEulerAngles = Vector3.zero;
                        areaItem.flowerCards[i].transform.localScale = Vector3.one;
                    }
                }
                else
                {
                    y = -0.32f;
                    if (areaItem.dir == AreaDir.DOWN)
                    {
                        areaItem.flowerCards[i].transform.localPosition = new Vector3(0 - 0.551f * (i-4), y, 0);
                        areaItem.flowerCards[i].transform.localEulerAngles = Vector3.zero;
                        areaItem.flowerCards[i].transform.localScale = Vector3.one;
                    }
                    if (areaItem.dir == AreaDir.LEFT)
                    {
                        areaItem.flowerCards[i].transform.localPosition = new Vector3(0 - 0.551f * (i - 4), y, 0);
                        areaItem.flowerCards[i].transform.localEulerAngles = Vector3.zero;
                        areaItem.flowerCards[i].transform.localScale = Vector3.one;
                    }
                    if (areaItem.dir == AreaDir.RIGHT)
                    {
                        areaItem.flowerCards[i].transform.localPosition = new Vector3(0 - 0.551f * (i - 4), y, 0);
                        areaItem.flowerCards[i].transform.localEulerAngles = Vector3.zero;
                        areaItem.flowerCards[i].transform.localScale = Vector3.one;
                    }
                    if (areaItem.dir == AreaDir.UP)
                    {
                        areaItem.flowerCards[i].transform.localPosition = new Vector3(0 - 0.551f * (i - 4), y, 0);
                        areaItem.flowerCards[i].transform.localEulerAngles = Vector3.zero;
                        areaItem.flowerCards[i].transform.localScale = Vector3.one;
                    }
                }
            }

        }
        /// <summary>
        /// 获取当前牌的角度和坐标
        /// </summary>
        /// <param name="areaItem"></param>
        /// <param name="cardIndex"></param>
        /// <returns></returns>
        public static List<Vector3> GetPutCardPosition(BattleAreaItem areaItem, int cardIndex)
        {
            var anglePositionArr = new List<Vector3>();
            var battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
            int column = cardIndex % areaItem.putCardHNum;
            int row = cardIndex / areaItem.putCardHNum;
            var sitIndex = (areaItem.data.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
            if (sitIndex == 2)
            {//顶部
                anglePositionArr.Add(new Vector3(0, -180, 0));
            }
            else
            {
                anglePositionArr.Add(Vector3.zero);
            }
            anglePositionArr.Add(column * areaItem.putCardHGap + row * areaItem.putCardVGap);
            return anglePositionArr;
        }

        /// <summary>
        /// 获取当前花牌的角度和坐标
        /// </summary>
        /// <param name="areaItem"></param>
        /// <param name="cardIndex"></param>
        /// <returns></returns>
        public static List<Vector3> GetFlowerCardPosition(BattleAreaItem areaItem, int cardIndex)
        {
            var anglePositionArr = new List<Vector3>();
            var battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
            var sitIndex = (areaItem.data.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
            if (sitIndex == 2)
            {//顶部
                anglePositionArr.Add(new Vector3(0, -180, 0));
            }
            else
            {
                anglePositionArr.Add(Vector3.zero);
            }
            anglePositionArr.Add(cardIndex * areaItem.flowerCardHGap);
            return anglePositionArr;
        }

        /// <summary>
        /// 牌面排序
        /// </summary>
        public static int CompareCard(GameObject card1, GameObject card2)
        {
            var cardValue1 = GetMeshCardValue(card1);
            var cardValue2 = GetMeshCardValue(card2);

            if (cardValue1 < cardValue2)
            {
                return -1;
            }
            else if (cardValue1 > cardValue2)
            {
                return 1;
            }
            else
            {
                int cardIndex1 = int.Parse(card1.name.Replace("HandCard", ""));
                int cardIndex2 = int.Parse(card2.name.Replace("HandCard", ""));
                return cardIndex1 < cardIndex2 ? -1 : 1;
            }
        }

        /// <summary>
        /// 初始化所有牌面
        /// </summary>
        public static void InitPlayerCards(BattleAreaItem areaItem)
        {
            var battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            //恢复碰杠的牌
            Vector3 pengGangPos = Vector3.zero;
            GameObject lastPengGangCard = null;
            int pengOne = 0;//碰(盖牌一次)的标志
            int mgangone = 0;
            int covertIndex = -1;//需要盖住的牌面序号
            
            var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
            for (int i = 0; i < areaItem.data.pengGangCards.Count; i++)
            {
                areaItem.data.pengGangCards[i].pengGangCards.Sort();
            }
            for (int i = 0; i < areaItem.data.pengGangCards.Count; i++)
            {
                if (i > 0)
                {
                    pengGangPos += areaItem.pengGangGap * 1.2f;
                }
                if (areaItem.data.pengGangCards[i].targetUserId == areaItem.data.userId || areaItem.data.pengGangCards[i].targetUserId == 0)
                {
                    covertIndex = -1;
                }
                else
                {
                    var targetPlayerInfoVO = battleProxy.playerIdInfoDic[areaItem.data.pengGangCards[i].targetUserId];
                    var targetIndex = (targetPlayerInfoVO.sit - areaItem.data.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
                    covertIndex = GlobalData.SIT_NUM - targetIndex;
                }

                var cards = new List<GameObject>();
                for (int j = 0; j < areaItem.data.pengGangCards[i].pengGangCards.Count; j++)
                {
                    var card = ResourcesMgr.Instance.GetFromPool(areaItem.data.pengGangCards[i].pengGangCards[j]);
                    if (j > 0)
                    {
                        pengGangPos += areaItem.pengGangGap;
                    }
                    card.transform.SetParent(areaItem.pengGangCardContainer);
                    card.transform.localScale = Vector3.one;
                    //if (covertIndex == j || covertIndex == -1)
                    //{
                    //    card.transform.localEulerAngles = new Vector3(180, 0, 0);
                    //}
                    //else
                    //{
                    //    card.transform.localEulerAngles = Vector3.zero;
                    //}
                    //吃=牌面值，暗杠=-1，明杠=-2，碰=-3
                    if (areaItem.data.pengGangCards[i].specialMahjong == areaItem.data.pengGangCards[i].pengGangCards[j])
                    {
                        card.transform.localEulerAngles = new Vector3(180, 0, 0);
                        if (areaItem.data.pengGangCards[i].specialMahjong == battleProxy.treasureCardCode)
                        {
                            if (card.transform.childCount > 0)
                            {
                                UnityEngine.Object.Destroy(card.transform.GetChild(0).gameObject);
                            }
                        }
                    }
                    else
                    {
                        card.transform.localEulerAngles = new Vector3(0, 0, 0);
                    }
                    if (areaItem.data.pengGangCards[i].specialMahjong == -1)//暗杠
                    {
                        card.transform.localEulerAngles = new Vector3(180, 0, 0);
                    }
                    if (areaItem.data.pengGangCards[i].specialMahjong == -2)//明杠
                    {
                        if (mgangone == 0)
                        {
                            card.transform.localEulerAngles = new Vector3(180, 0, 0);
                            mgangone++;
                        }
                        else
                        {
                            card.transform.localEulerAngles = new Vector3(0, 0, 0);
                        }
                    }
                    if (areaItem.data.pengGangCards[i].specialMahjong == -3)//碰
                    {
                        if (pengOne == 0)
                        {
                            card.transform.localEulerAngles = new Vector3(180, 0, 0);
                            pengOne++;
                        }
                        else
                        {
                            card.transform.localEulerAngles = new Vector3(0, 0, 0);
                        }
                    }

                    card.transform.localPosition = pengGangPos;

                    //card.layer = GlobalData.OTHER_CARDS;

                    if (areaItem.dir == AreaDir.RIGHT)
                    {
                        card.layer = GlobalData.RIGHTHAND_CARDS;
                        if (card.transform.childCount > 0)
                        {
                            card.transform.GetChild(0).gameObject.layer = GlobalData.RIGHTHAND_CARDS;
                        }
                    }
                    else if (areaItem.dir == AreaDir.DOWN)
                    {
                        card.layer = GlobalData.SELF_HAND_CARDS;
                    }
                    else
                    {
                        card.layer = GlobalData.OTHER_CARDS;
                        if (card.transform.childCount > 0)
                        {
                            card.transform.GetChild(0).gameObject.layer = GlobalData.OTHER_CARDS;
                        }
                    }
                    if (AreaDir.UP == areaItem.dir)
                    {
                        //card.transform.eulerAngles = new Vector3(0, 180, 0);
                    }
                    lastPengGangCard = card;
                    cards.Add(card);

                }

               
                areaItem.pengGangCards.Add(cards);
            }
            //恢复手中的牌
            Vector3 handPos = Vector3.zero;
            if (lastPengGangCard != null)
            {
                Camera cam;
                if (areaItem.data.userId == playerInfoProxy.UserInfo.UserID)
                {
                    cam = areaItem.myselfCamera;

                }
                else
                {
                    cam = Camera.main;
                }
                Vector3 pengGangWorldPos = lastPengGangCard.transform.position;
                handPos = areaItem.handCardContainer.InverseTransformPoint(pengGangWorldPos);
                handPos += areaItem.handCardGap;
                handPos.y = 0;
                handPos.z = 0;
            }
            for (int i = 0; i < areaItem.data.handCards.Count; i++)
            {
                var card = ResourcesMgr.Instance.GetFromPool(areaItem.data.handCards[i]);
                card.transform.SetParent(areaItem.handCardContainer);
                card.transform.localScale = Vector3.one;
                card.transform.localEulerAngles = Vector3.zero;
                card.transform.localPosition = handPos;
                if (areaItem.data.userId == playerInfoProxy.UserInfo.UserID)
                {
                    card.layer = GlobalData.SELF_HAND_CARDS;
                    Debug.Log(string.Format("恢复的手牌有：{0} 张,id为：{1}", areaItem.data.handCards.Count, areaItem.data.userId));

                }
                else
                {
                    if (card.transform.childCount > 0)
                    {
                        card.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("OtherCards");
                    }
                    //card.layer = GlobalData.OTHER_CARDS;
                    if (areaItem.dir == AreaDir.RIGHT)
                    {
                        card.layer = GlobalData.RIGHTHAND_CARDS;
                    }
                    else
                        card.layer = GlobalData.OTHER_CARDS;
                }
                if (i + 1 < areaItem.data.handCards.Count)
                {
                    handPos += areaItem.handCardGap;
                }
                else
                {
                    handPos += areaItem.getHandCardGap;
                }
                areaItem.handCards.Add(card);
            }
            //恢复摸到的牌
            if (areaItem.data.getCard > 0)
            {
                var card = ResourcesMgr.Instance.GetFromPool(areaItem.data.getCard);
                card.transform.SetParent(areaItem.handCardContainer);
                card.transform.localScale = Vector3.one;
                card.transform.localEulerAngles = Vector3.zero;
                card.transform.localPosition = handPos;
                areaItem.getCard = card;
                if (areaItem.data.userId == playerInfoProxy.UserInfo.UserID)
                {
                    areaItem.getCard.layer = GlobalData.SELF_HAND_CARDS;
                }
                else
                {
                    if (areaItem.getCard.transform.childCount > 0)
                    {
                        areaItem.getCard.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("OtherCards");
                    }
                    //areaItem.getCard.layer = GlobalData.OTHER_CARDS;
                    if (areaItem.dir == AreaDir.RIGHT)
                    {
                        areaItem.getCard.layer = GlobalData.RIGHTHAND_CARDS;
                    }
                    else
                        areaItem.getCard.layer = GlobalData.OTHER_CARDS;
                }
            }
            //恢复已出的牌
            Vector3 putPos = Vector3.zero;
            //清空一下
            areaItem.flowerCards.Clear();
            for (int i = 0; i < areaItem.data.putCards.Count; i++)
            {
                var card = ResourcesMgr.Instance.GetFromPool(areaItem.data.putCards[i]);
                int column = i % areaItem.putCardHNum;
                int row = i / areaItem.putCardHNum;
                card.transform.SetParent(areaItem.putCardContainer);
                card.transform.localScale = Vector3.one;
                var sitIndex = (areaItem.data.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
                if (sitIndex == 2)
                {//顶部
                    card.transform.localEulerAngles = new Vector3(0, -180, 0);
                }
                else
                {
                    card.transform.localEulerAngles = Vector3.zero;
                }
                card.transform.localPosition = putPos + column * areaItem.putCardHGap + row * areaItem.putCardVGap;

                if (areaItem.dir == AreaDir.RIGHT)
                {
                    card.layer = GlobalData.RIGHTHAND_CARDS;
                }
                else
                    card.layer = GlobalData.OTHER_CARDS;

                if (card.transform.childCount > 0)
                {
                    card.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("OtherCards");
                }
                areaItem.putCards.Add(card);

            }

            //已出花牌的重连
            float y = 0;
            for (int i = 0; i < areaItem.data.flowerCards.Count; i++)
            {
                var card = ResourcesMgr.Instance.GetFromPool(areaItem.data.flowerCards[i]);
                areaItem.flowerCards.Add(card);
                card.transform.SetParent(areaItem.flowersCardContainer);
                if (areaItem.dir == AreaDir.DOWN)
                {
                    card.layer = GlobalData.SELF_HAND_CARDS; 
                }
                if (i<4)
                {
                    if (areaItem.dir == AreaDir.DOWN)
                    {
                        areaItem.flowerCards[i].transform.localPosition = new Vector3(0 - 0.551f * i, y, 0);
                        areaItem.flowerCards[i].transform.localEulerAngles = Vector3.zero;
                        areaItem.flowerCards[i].transform.localScale = Vector3.one;
                    }
                    if (areaItem.dir == AreaDir.LEFT)
                    {
                        areaItem.flowerCards[i].transform.localPosition = new Vector3(0 - 0.551f * i, y, 0);
                        areaItem.flowerCards[i].transform.localEulerAngles = Vector3.zero;
                        areaItem.flowerCards[i].transform.localScale = Vector3.one;
                    }
                    if (areaItem.dir == AreaDir.RIGHT)
                    {
                        areaItem.flowerCards[i].transform.localPosition = new Vector3(0 - 0.551f * i, y, 0);
                        areaItem.flowerCards[i].transform.localEulerAngles = Vector3.zero;
                        areaItem.flowerCards[i].transform.localScale = Vector3.one;
                    }
                    if (areaItem.dir == AreaDir.UP)
                    {
                        areaItem.flowerCards[i].transform.localPosition = new Vector3(0 - 0.551f * i, y, 0);
                        areaItem.flowerCards[i].transform.localEulerAngles = Vector3.zero;
                        areaItem.flowerCards[i].transform.localScale = Vector3.one;
                    }
                    if (areaItem.flowerCards[i].transform.childCount > 0)
                    {
                        UnityEngine.Object.Destroy(areaItem.flowerCards[i].transform.GetChild(0).gameObject);
                    }
                }
                else
                {
                    y = -0.32f;
                    if (areaItem.dir == AreaDir.DOWN)
                    {
                        areaItem.flowerCards[i].transform.localPosition = new Vector3(0 - 0.551f * (i-4), y, 0);
                        areaItem.flowerCards[i].transform.localEulerAngles = Vector3.zero;
                        areaItem.flowerCards[i].transform.localScale = Vector3.one;
                    }
                    if (areaItem.dir == AreaDir.LEFT)
                    {
                        areaItem.flowerCards[i].transform.localPosition = new Vector3(0 - 0.551f * (i - 4), y, 0);
                        areaItem.flowerCards[i].transform.localEulerAngles = Vector3.zero;
                        areaItem.flowerCards[i].transform.localScale = Vector3.one;
                    }
                    if (areaItem.dir == AreaDir.RIGHT)
                    {
                        areaItem.flowerCards[i].transform.localPosition = new Vector3(0 - 0.551f * (i - 4), y, 0);
                        areaItem.flowerCards[i].transform.localEulerAngles = Vector3.zero;
                        areaItem.flowerCards[i].transform.localScale = Vector3.one;
                    }
                    if (areaItem.dir == AreaDir.UP)
                    {
                        areaItem.flowerCards[i].transform.localPosition = new Vector3(0 - 0.551f * (i - 4), y, 0);
                        areaItem.flowerCards[i].transform.localEulerAngles = Vector3.zero;
                        areaItem.flowerCards[i].transform.localScale = Vector3.one;
                    }
                    if (areaItem.flowerCards[i].transform.childCount > 0)
                    {
                        UnityEngine.Object.Destroy(areaItem.flowerCards[i].transform.GetChild(0).gameObject);
                    }

                }
            }
            //恢复牌堆的牌
            if (GlobalData.hasHeap)
            {
                InitHeapCard(areaItem, battleProxy.leftCard);
            }
        }


        /// <summary>
        /// 生成牌堆内的牌
        /// </summary>
        /// <param name="areaItem"></param>
        public static void InitHeapCard(BattleAreaItem areaItem, int leftCard)
        {
            var battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
            if (!battleProxy.unGetHeapCardIndexs.Contains(areaItem.heapStartIndex) && !battleProxy.unGetHeapCardIndexs.Contains(areaItem.heapEndIndex))
            {
                return;
            }
            List<int> addHeapIndexArr = new List<int>();
            //先将发牌位置右侧的牌堆生成
            int recivedCard = GlobalData.CardWare.Length - leftCard;
            for (int i = Math.Max(battleProxy.sendHeapStartIndex + recivedCard, areaItem.heapStartIndex); i <= areaItem.heapEndIndex; i++)
            {
                if (!battleProxy.unGetHeapCardIndexs.Contains(i))
                {
                    continue;
                }
                addHeapIndexArr.Add(i);
            }
            //再生成发牌位置左侧的牌堆
            for (int i = areaItem.heapStartIndex; i < Math.Min(battleProxy.sendHeapStartIndex, areaItem.heapEndIndex + 1); i++)
            {
                if (!battleProxy.unGetHeapCardIndexs.Contains(i))
                {
                    continue;
                }
                addHeapIndexArr.Add(i);
            }
            foreach (int i in addHeapIndexArr)
            {
                var card = ResourcesMgr.Instance.GetFromPool(65);
                int column = (i - areaItem.heapStartIndex) / 2;
                int row = i % 2;
                card.transform.SetParent(areaItem.heapCardContainer);
                card.transform.localEulerAngles = Vector3.zero;
                card.transform.localScale = Vector3.one;
                card.transform.localPosition = areaItem.heapFirstCard.localPosition + column * areaItem.heapHGap + row * areaItem.heapVGap;
                //if (areaItem.dir != AreaDir.RIGHT)
                //{
                //    card.layer = GlobalData.OTHER_CARDS;
                //}
                //else
                //{
                //    card.layer = GlobalData.RIGHTHAND_CARDS;
                //}
                areaItem.heapCards.Add(card);
                if (areaItem.dir == AreaDir.RIGHT)
                {
                    card.layer = GlobalData.RIGHTHAND_CARDS;
                }
                if (areaItem.dir == AreaDir.DOWN)
                {
                    card.layer = GlobalData.SELF_HAND_CARDS;
                }
            }

        }

        /// <summary>
        /// 获取当前牌内能吃的组合数组
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static List<List<int>> GetCanChiArr(int card)
        {
            var result = new List<List<int>>();
            var battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            var handCards = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID].handCards;

            if (card < 40 || card > 45)//不是东南西北的必须要是顺子才能吃，东南西北任意3个都可以组成刻子
            {
                int[] temp = {
                    card + 1,
                    card + 2,
                    card - 1,
                    card + 1,
                    card - 2,
                    card - 1
                };
                bool isContainAll = true;

                int count = temp.Length / 2;
                int index = 0;
                List<int> canSelectCard = new List<int>();
                for (int i = 0; i < count; i++)
                {
                    isContainAll = true;
                    canSelectCard = new List<int>();
                    for (int j = 0; j < 2; j++)
                    {
                        index = 2 * i + j;
                        canSelectCard.Add(temp[index]);
                        if (!handCards.Contains(temp[index]))
                        {
                            isContainAll = false;
                            break;
                        }
                    }
                    if (isContainAll)
                    {
                        canSelectCard.Remove(card);
                        canSelectCard.Sort();
                        result.Add(canSelectCard);
                    }

                }
                if (result.Count == 0)
                {
                    Debug.Log("chiCard===========" + card);
                    for (int i = 0; i < handCards.Count; i++)
                    {
                        Debug.Log("handCards===========" + handCards[i]);
                    }
                }
            }
            else//东南西北吃
            {

                List<int> fengList = new List<int>();
                fengList.Add(41);
                fengList.Add(42);
                fengList.Add(43);
                fengList.Add(44);
                List<int> ListCombination = new List<int>();

                for (int k = 0; k < handCards.Count; k++)
                {
                    if (fengList.Contains(handCards[k]))
                    {
                        if (!ListCombination.Contains(handCards[k]) && handCards[k] != card)
                        {
                            ListCombination.Add(handCards[k]);
                        }
                    }
                }

                if (ListCombination.Count > 1)
                {
                    for (int i = 0; i < ListCombination.Count; i++)
                    {
                        List<int> list = new List<int>();
                        if (i + 1 == ListCombination.Count)
                        {
                            if (ListCombination.Count != 2)
                            {
                                list.Add(ListCombination[i]);
                                list.Add(ListCombination[0]);
                                result.Add(list);
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (i + 1 < ListCombination.Count)
                        {
                            list.Add(ListCombination[i]);
                            list.Add(ListCombination[i + 1]);
                            Debug.Log("= " + ListCombination[i] + "   =" + ListCombination[i + 1]);
                            result.Add(list);
                        }

                        //ListCombination.RemoveAt(0);
                        //if (ListCombination.Count == 1) break;
                    }
                }
                foreach (var v in result)
                {
                    foreach (var item in v)
                    {
                        Debug.Log("zuhe:" + item);
                    }
                    //Debug.Log("------------");
                }
            }
            return result;
        }
    }
}