﻿using System.Collections.Generic;
using Platform.Net;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;
using UnityEngine.SceneManagement;
using Platform.Global;
using Platform.Model.VO.BattleVO;
using System;
using Utils;
using LZR.Data.NetWork.Client;

namespace Platform.Model.Battle
{
    /// <summary>
    /// 牌局数据代理
    /// </summary>
    internal class BattleProxy : Proxy, IProxy
    {

        /// <summary>
        /// 当前是第几局
        /// </summary>
        public int curInnings = 1;
        /// <summary>
        /// 是否跳过缓动
        /// </summary>
        public bool isSkipTween;
        /// <summary>
        /// 本局是否已开始
        /// </summary>
        public bool isStart;
        /// <summary>
        /// 房间房主id
        /// </summary>
        public int creatorId;

        /// <summary>
        /// 宝牌
        /// </summary>
        public int treasureCardCode;

        /// <summary>
        /// 本局结算信息
        /// </summary>
        public PushMatchResultS2C matchResultS2C;
        /// <summary>
        /// 之前的庄家id
        /// </summary>
        public int perBankerId;

        /// <summary>
        /// 当前播放的玩家动作
        /// </summary>
        public PushPlayerActS2C playerActS2C;

        /// <summary>
        /// 推送的玩家操作提示
        /// </summary>
        public PushPlayerActTipS2C playerActTipS2C;

        /// <summary>
        /// 玩家信息字典{LocalId:玩家信息VO}
        /// </summary>
        public Dictionary<int, PlayerInfoVOS2C> playerIdInfoDic;

        /// <summary>
        /// 玩家信息字典{Sit:玩家信息VO}
        /// </summary>
        public Dictionary<int, PlayerInfoVOS2C> playerSitInfoDic;

        /// <summary>
        /// 是否轮到自己出手
        /// </summary>
        public bool isSelfAction;
        /// <summary>
        /// 房间结算信息
        /// </summary>
        public PushRoomResultS2C roomResultS2C;
        /// <summary>
        /// 剩余牌数
        /// </summary>
        public int leftCard = 144;
        /// <summary>
        /// 语音缓存队列
        /// </summary>
        public Queue<AudioPacket> speekPacket = new Queue<AudioPacket>();
        /// <summary>
        /// 胡牌类型
        /// </summary>
        public List<PlayerActType> huTypes = new List<PlayerActType>();
        /// <summary>
        /// 当前播放的战报
        /// </summary>
        public PlayReportS2C report;
        /// <summary>
        /// 战报开始时的本地时间
        /// </summary>
        public float reportLocalTime;
        /// <summary>
        /// 禁止期间未播放的动作队列
        /// </summary>
        private List<ForbitActionVO> forbitActions;
        /// <summary>
        /// 当前是否播放战报
        /// </summary>
        public bool isReport = false;
        /// <summary>
        /// 之前发送语言聊天的时间
        /// </summary>
        public long perSendChatTime = 0;
        /// <summary>
        /// 是否正在播放胡牌动画
        /// </summary>
        public bool isPlayHu = false;
        /// <summary>
        /// 解散申请人的ID
        /// </summary>
        public int disloveApplyUserId;
        /// <summary>
        /// 本局开始时间
        /// </summary>
        public long startTime;
        /// <summary>
        /// 申请解散剩余时间
        /// </summary>
        public int disloveRemainTime;
        /// <summary>
        /// 解散剩余时间戳
        /// </summary>
        public long disloveRemainUT;
        /// <summary>
        /// 同意的玩家id数组
        /// </summary>
        public List<int> agreeIds = new List<int>();
        /// <summary>
        /// 不同意的玩家id数组
        /// </summary>
        public List<int> refuseIds = new List<int>();
        /// <summary>
        /// 是否有申请解散
        /// </summary>
        public bool hasDisloveApply;

        /// <summary>
        /// 听的牌数组
        /// </summary>
        public List<int> tingCards = new List<int>();
        /// <summary>
        /// 牌堆内未拿的牌序号数组
        /// </summary>
        public List<int> unGetHeapCardIndexs;
        /// <summary>
        /// 是否禁止操作正在发牌
        /// </summary>
        private bool _isForbit = false;
        /// <summary>
        /// 花牌
        /// </summary>
        public List<int> flowerCardList = new List<int>();
        /// <summary>
        /// 重连时的花牌
        /// </summary>
        public List<int> hadPutFlowerCards = new List<int>();

        public int GameStartTouchCard = 0;
        /// <summary>
        /// 替换花牌的牌
        /// </summary>
        ///public List<int> replaceFlowerCards = new List<int>();

        /// <summary>
        /// 色子点数（第一个表示方向，第二个表示第几摞开始拿）
        /// </summary>
        public List<int> dices = new List<int>();
        /// <summary>
        /// 是否禁止操作
        /// </summary>

        public bool isForbit
        {
            get
            {
                return _isForbit;
            }

            set
            {
                _isForbit = value;
                if (!value)
                {
                    PlayActionArr();
                }
            }
        }

        //是否是刚发完13张的补花
        public bool initPutFlowerCard = true;
        

        /// <summary>
        /// 拿牌的牌堆起始序号,自己的牌堆第一张牌为0，需要将后端的起始序号转为自己的起始序号
        /// </summary>
        public int sendHeapStartIndex = 0;

        public BattleProxy(string NAME) : base(NAME)
        {
            flowerCardList.Add(61);
            flowerCardList.Add(62);
            flowerCardList.Add(63);
            flowerCardList.Add(64);
            flowerCardList.Add(65);
            flowerCardList.Add(66);
            flowerCardList.Add(67);
            flowerCardList.Add(68);

            huTypes.Add(PlayerActType.CHI_HU);
            huTypes.Add(PlayerActType.QIANG_AN_GANG_HU);
            huTypes.Add(PlayerActType.QIANG_PENG_GANG_HU);
            huTypes.Add(PlayerActType.QIANG_ZHI_GANG_HU);
            huTypes.Add(PlayerActType.SELF_HU);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.ENTER_ROOMSERVER_S2C, CreateRoomHandler);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.JOIN_ROOM_S2C, JoinInRoomHandler);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.PUSH_JOIN, PushJoinHandler);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.EXIT_S2C, ExitHandler);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.DISSOLUTION_S2C, DissolutionHandler);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.PUSH_READY, PushReadyHandler);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.GAME_START_S2C, GameStartHandler);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.PUSH_PLAYER_ACTTIP, PushPlayerActTipHandler);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.PUSH_PLAYER_ACT, PushPlayerActHandler);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.PUSH_MATCH_END, PushMatchEndHandler);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.PUSH_ROOM_END, PushRoomEndHandler);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.PUSH_VOICE, PushVoiceHandler);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.PUSH_CHAT, PushChatHandler);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.GET_PLAYERINFO_S2C, GetUserInfoHandler);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.DISLOVEAPPLY_S2C, DisloveApplyHandler);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.CANCEL_DISSLOVEAPPLY_S2C, DisloveCancelHandler);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.DISSLOVEROOM_CONFIRM_S2C, DisloveConfirmHandler);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.RESPONSE_PLAYVIDEO_S2C, PlayVideo);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.ACTERROR_S2C, ActErrorHandler);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.PLAYAMAHJONG_S2C, PlayAmahjongHandler);
            GameMgr.Instance.AddMsgHandler(MsgNoS2C.PLAYAFLOWERMAHJONG_S2C, PlayFlowerCardHandler);
        }

        /// <summary>
        /// 播放缓存的动作
        /// </summary>
        private void PlayActionArr()
        {
            isSkipTween = true;
            for (int i = 0; i < forbitActions.Count; i++)
            {
                if (forbitActions[i].isActTip)
                {
                    PushPlayerActTipHandler(forbitActions[i].bytes);
                }
                else
                {
                    PushPlayerActHandler(forbitActions[i].bytes);
                }
            }
            forbitActions.Clear();
            isSkipTween = false;
        }

        /// <summary>
        /// 庄家VOS2C
        /// </summary>
        public PlayerInfoVOS2C BankerPlayerInfoVOS2C
        {
            get
            {
                foreach (var playerInfoVOS2C in playerIdInfoDic)
                    if (playerInfoVOS2C.Value.isBanker)
                        return playerInfoVOS2C.Value;
                return null;
            }
        }

        /// <summary>
        /// 东家VOS2C
        /// </summary>
        public PlayerInfoVOS2C MasterPlayerInfoVOS2C
        {
            get
            {
                foreach (var playerInfoVOS2C in playerIdInfoDic)
                {
                    if (playerInfoVOS2C.Value.isMaster)
                    {
                        return playerInfoVOS2C.Value;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 创建房间返回
        /// </summary>
        /// <param PlayerName="bytes"></param>
        private void CreateRoomHandler(byte[] bytes)
        {
            playerActTipS2C = null;
            playerIdInfoDic = new Dictionary<int, PlayerInfoVOS2C>();
            var selfInfoVO = new PlayerInfoVOS2C();
            var hallProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.HALL_PROXY) as HallProxy;
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            selfInfoVO.headIcon = playerInfoProxy.UserInfo.HeadIconUrl;
            selfInfoVO.isBanker = false;
            selfInfoVO.isMaster = false;
            selfInfoVO.isReady = false;
            selfInfoVO.userId = playerInfoProxy.UserInfo.UserID;
            selfInfoVO.name = playerInfoProxy.UserInfo.UserName;
            selfInfoVO.score = playerInfoProxy.UserInfo.Score;
            selfInfoVO.sex = playerInfoProxy.UserInfo.Score;
            selfInfoVO.sit = hallProxy.HallInfo.Seat;
            tingCards.Clear();
            //GlobalData.sitNumber = 1;
            playerIdInfoDic.Add(selfInfoVO.userId, selfInfoVO);
            UpdatePlayerSitDic();
            curInnings = 1;
            creatorId = playerInfoProxy.UserInfo.UserID;
            isStart = false;
            isSelfAction = false;
            hasDisloveApply = false;
            agreeIds.Clear();
            refuseIds.Clear();
            UIManager.Instance.HideUI(UIViewID.CREATEROOM_VIEW,
                () =>
                {
                    var loadInfo = new LoadSceneInfo(ESceneID.SCENE_BATTLE, LoadSceneType.ASYNC, LoadSceneMode.Additive);
                    SendNotification(NotificationConstant.MEDI_GAMEMGR_LOADSCENE, loadInfo);
                });

            var readyC2S = new ReadyC2S();
            NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.READY_C2S.GetHashCode(), 0, readyC2S);
        }

        /// <summary>
        /// 加入房间返回
        /// </summary>
        /// <param PlayerName="bytes">消息体</param>
        private void JoinInRoomHandler(byte[] bytes)
        {
            forbitActions = new List<ForbitActionVO>();
            var hallProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.HALL_PROXY) as HallProxy;
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            playerIdInfoDic = new Dictionary<int, PlayerInfoVOS2C>();
            var joinRoomS2C = NetMgr.Instance.DeSerializes<JoinRoomS2C>(bytes);
            creatorId = joinRoomS2C.createId;
            isStart = joinRoomS2C.isStart;
            startTime = joinRoomS2C.startTime;
            hasDisloveApply = joinRoomS2C.hasDisloveApply;
            agreeIds = joinRoomS2C.agreeIds;
            refuseIds = joinRoomS2C.refuseIds;
            disloveRemainTime = joinRoomS2C.disloveRemainTime;
            //Debug.Log("disloveRemainTime ==== "+ disloveRemainTime);
            disloveRemainUT = joinRoomS2C.disloveRemainUT;
            tingCards = joinRoomS2C.tingCards;
            treasureCardCode = joinRoomS2C.treasureCardCode;

            if (joinRoomS2C.agreeIds.Count > 0)
            {
                disloveApplyUserId = joinRoomS2C.agreeIds[0];
            }
            int putCardNum = 0;
            List<int> treasurCarList = new List<int>();
            foreach (var voS2C in joinRoomS2C.playInfoArr)
            {
                
                if (voS2C.userId == playerInfoProxy.UserInfo.UserID)
                {

                    for (int i = 0; i < voS2C.handCards.Count; i++)
                    {
                        if (voS2C.handCards[i] == treasureCardCode)
                        {
                            treasurCarList.Add(voS2C.handCards[i]);
                            voS2C.handCards.RemoveAt(i);
                        }
                    }
                    if (isStart)
                    {//已开局牌局手中的牌自动排序

                        voS2C.handCards.Sort();
                    }
                    voS2C.handCards.InsertRange(0, treasurCarList);

                }


                playerIdInfoDic.Add(voS2C.userId, voS2C);
                putCardNum += voS2C.handCards.Count;
                if (voS2C.getCard > 0)
                {
                    putCardNum += 1;
                }
                foreach (PengGangCardVO pengGangCardVo in voS2C.pengGangCards)
                {
                    putCardNum += pengGangCardVo.pengGangCards.Count;
                }
                putCardNum += voS2C.putCards.Count;
            }
            dices = joinRoomS2C.dices;
            if (dices.Count > 0)
            {

                List<int> playerIndex = new List<int>();
                playerIndex.Add(dices[1] * 2);
                playerIndex.Add(36 + (dices[1] * 2));
                playerIndex.Add(2 * 36 + (dices[1] * 2));
                playerIndex.Add(3 * 36 + (dices[1] * 2));

                int cardIndex = (dices[0] + 4) % 4;
                if (playerIdInfoDic[playerInfoProxy.UserInfo.UserID].sit == 1)
                {
                    if (cardIndex == 1)
                    {
                        sendHeapStartIndex = playerIndex[0];
                    }
                    if (cardIndex == 2)
                    {
                        sendHeapStartIndex = playerIndex[1];
                    }
                    if (cardIndex == 3)
                    {
                        sendHeapStartIndex = playerIndex[2];
                    }
                    if (cardIndex == 0)
                    {
                        sendHeapStartIndex = playerIndex[3];
                    }
                }
                else if (playerIdInfoDic[playerInfoProxy.UserInfo.UserID].sit == 2)
                {
                    if (cardIndex == 1)
                    {
                        sendHeapStartIndex = playerIndex[3];
                    }
                    if (cardIndex == 2)
                    {
                        sendHeapStartIndex = playerIndex[0];
                    }
                    if (cardIndex == 3)
                    {
                        sendHeapStartIndex = playerIndex[1];
                    }
                    if (cardIndex == 0)
                    {
                        sendHeapStartIndex = playerIndex[2];
                    }
                }
                else if (playerIdInfoDic[playerInfoProxy.UserInfo.UserID].sit == 3)
                {
                    if (cardIndex == 1)
                    {
                        sendHeapStartIndex = playerIndex[2];
                    }
                    if (cardIndex == 2)
                    {
                        sendHeapStartIndex = playerIndex[3];
                    }
                    if (cardIndex == 3)
                    {
                        sendHeapStartIndex = playerIndex[0];
                    }
                    if (cardIndex == 0)
                    {
                        sendHeapStartIndex = playerIndex[1];
                    }
                }
                else
                {
                    if (cardIndex == 1)
                    {
                        sendHeapStartIndex = playerIndex[1];
                    }
                    if (cardIndex == 2)
                    {
                        sendHeapStartIndex = playerIndex[2];
                    }
                    if (cardIndex == 3)
                    {
                        sendHeapStartIndex = playerIndex[3];
                    }
                    if (cardIndex == 0)
                    {
                        sendHeapStartIndex = playerIndex[0];
                    }
                }
                Debug.Log("joinroom sendHeapStartIndex = " + sendHeapStartIndex);
            }
             
            if (joinRoomS2C.optUserId != 0)
            {

                GlobalData.sit = playerIdInfoDic[playerInfoProxy.UserInfo.UserID].sit;
                GlobalData.optUserId = playerIdInfoDic[joinRoomS2C.optUserId].sit;
                Debug.Log(string.Format("当前玩家的座位号：{0}，轮到{1}家出牌", GlobalData.sit, GlobalData.optUserId));
            }
            else
            {
                Debug.Log("joinRoomS2C.optUserId = 0");
            }

            //获取重连的花牌
            hadPutFlowerCards = playerIdInfoDic[playerInfoProxy.UserInfo.UserID].flowerCards;

            UpdatePlayerSitDic();
            //Debug.Log("加入房间， 玩家id："+playerInfoProxy.UserInfo.UserID + "  sit = " + joinRoomS2C.playInfoArr[playerInfoProxy.UserInfo.UserID].sit);
            hallProxy.HallInfo.RoomCode = joinRoomS2C.roomCode;
            hallProxy.HallInfo.Innings = joinRoomS2C.innings;
            curInnings = joinRoomS2C.curInnings;
            playerActTipS2C = joinRoomS2C.playerTipAct;
            if (playerActTipS2C != null)
            {
                isSelfAction = playerActTipS2C.optUserId == playerInfoProxy.UserInfo.UserID;
            }
            leftCard = joinRoomS2C.leftCardCount;
            if (isStart)
            {
                if (GlobalData.hasHeap)
                {
                    InitHeapCardIndexs(leftCard);
                }
            }
            if (UIManager.Instance.GetUIView(UIViewID.JOINROOM_VIEW).IsShow)
            {
                UIManager.Instance.HideUI(UIViewID.JOINROOM_VIEW, EnterBattle);
            }
            else
            {
                EnterBattle();
            }

            UIManager.Instance.HideUI(UIViewID.MATCHING_VIEW,
                () =>
                {
                    var loadInfo = new LoadSceneInfo(ESceneID.SCENE_BATTLE, LoadSceneType.ASYNC, LoadSceneMode.Additive);
                    SendNotification(NotificationConstant.MEDI_GAMEMGR_LOADSCENE, loadInfo);
                });
        }

        /// <summary>
        /// 进入战斗场景
        /// </summary>
        private void EnterBattle()
        {
            var loadInfo = new LoadSceneInfo(ESceneID.SCENE_BATTLE, LoadSceneType.ASYNC,
                               LoadSceneMode.Additive);
            SendNotification(NotificationConstant.MEDI_GAMEMGR_LOADSCENE, loadInfo);
        }

        /// <summary>
        /// 推送玩家加入房间
        /// </summary>
        /// <param PlayerName="bytes"></param>
        private void PushJoinHandler(byte[] bytes)
        {
            var pushJoinS2C = NetMgr.Instance.DeSerializes<PushJoinS2C>(bytes);
            playerIdInfoDic.Add(pushJoinS2C.playerInfo.userId, pushJoinS2C.playerInfo);
            UpdatePlayerSitDic();
            SendNotification(NotificationConstant.MEDI_BATTLEVIEW_UPDATESINGLEHEAD, pushJoinS2C.playerInfo);
        }

        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param PlayerName="bytes"></param>
        private void ExitHandler(byte[] bytes)
        {
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            var exitS2C = NetMgr.Instance.DeSerializes<ExitRoomS2C>(bytes);
            if (exitS2C.clientCode != ErrorCode.SUCCESS)
            {
                DialogMsgVO dialogMsgVO = new DialogMsgVO();
                dialogMsgVO.title = "退出";
                dialogMsgVO.dialogType = DialogType.ALERT;
                dialogMsgVO.content = "房间开局后不可中途退出";
                DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
                dialogView.data = dialogMsgVO;
                return;
            }
            NSocket.RoomId = 0;
            if (exitS2C.userId == playerInfoProxy.UserInfo.UserID)
            { //自己退出

                PopMsg.Instance.ShowMsg("成功退出房间");
                playerIdInfoDic = null;
                playerSitInfoDic = null;
                curInnings = 0;
                var loadInfo = new LoadSceneInfo(ESceneID.SCENE_HALL, LoadSceneType.ASYNC, LoadSceneMode.Additive);
                SendNotification(NotificationConstant.MEDI_GAMEMGR_LOADSCENE, loadInfo);
            }
            else
            {
                var exitPlayerInfoVO = playerIdInfoDic[exitS2C.userId];
                playerIdInfoDic.Remove(exitS2C.userId);
                UpdatePlayerSitDic();
                SendNotification(NotificationConstant.MEDI_BATTLEVIEW_UPDATESINGLEHEAD, exitPlayerInfoVO);
            }
        }

        /// <summary>
        /// 解散房间
        /// </summary>
        /// <param name="bytes"></param>
        private void DissolutionHandler(byte[] bytes)
        {
            var disloveS2C = NetMgr.Instance.DeSerializes<DissolveRoomS2C>(bytes);
            if (disloveS2C.clientCode != ErrorCode.SUCCESS)
            {
                return;
            }
            NSocket.RoomId = 0;
            if (roomResultS2C == null)
            {//房间未结算显示解散提示
                DialogMsgVO dialogMsgVO = new DialogMsgVO();
                dialogMsgVO.title = "解散提示";
                dialogMsgVO.dialogType = DialogType.ALERT;
                dialogMsgVO.content = "房间已解散";
                dialogMsgVO.confirmCallBack = (() =>
                {
                    playerIdInfoDic = null;
                    playerSitInfoDic = null;
                    curInnings = 0;
                    var loadInfo = new LoadSceneInfo(ESceneID.SCENE_HALL, LoadSceneType.ASYNC, LoadSceneMode.Additive);
                    SendNotification(NotificationConstant.MEDI_GAMEMGR_LOADSCENE, loadInfo);
                });
                DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
                dialogView.data = dialogMsgVO;
            }
            UIManager.Instance.HideUI(UIViewID.DISLOVE_STATISTICS_VIEW);
        }

        /// <summary>
        /// 推送玩家准备
        /// </summary>
        /// <param PlayerName="bytes"></param>
        private void PushReadyHandler(byte[] bytes)
        {
            var pushReadyS2C = NetMgr.Instance.DeSerializes<PushReadyS2C>(bytes);
            //Debug.Log("推送玩家准备时的id："+ pushReadyS2C.userId);
            var readyPlayerInfoVO = playerIdInfoDic[pushReadyS2C.userId];
            readyPlayerInfoVO.isReady = true;
            SendNotification(NotificationConstant.MEDI_READY_COMPLETE);
            SendNotification(NotificationConstant.MEDI_BATTLEVIEW_UPDATESINGLEHEAD, readyPlayerInfoVO);
        }

        private int GetCarHeapIndex(List<int> list, int i)
        {
            int index = 2;
            switch (i)
            {
                case 1:
                    index = list[0];
                    break;
                case 2:
                    index = list[1];
                    break;
                case 3:
                    index = list[2];
                    break;
                case 0:
                    index = list[3];
                    break;
            }
            return index;
        }
        /// <summary>
        /// 推送发牌
        /// </summary>
        /// <param PlayerName="bytes"></param>
        private void GameStartHandler(byte[] bytes)
        {


            forbitActions = new List<ForbitActionVO>();
            isForbit = true;
            isStart = true;
            var playerInfoProxy =
                ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            var gameStartS2C = NetMgr.Instance.DeSerializes<GameStart_S2C>(bytes);

            GameStartTouchCard = gameStartS2C.touchMahjongCode;
            startTime = gameStartS2C.startTime;

            if (GlobalData.LoginServer != "127.0.0.1")
            {
                dices = gameStartS2C.dices;
            }
            else
            {
                List<int> list = new List<int>() { 1, 5 };
                dices = list;
            }
            List<int> playerIndex = new List<int>();
            playerIndex.Add(dices[1] * 2);
            playerIndex.Add(36 + (dices[1] * 2));
            playerIndex.Add(2 * 36 + (dices[1] * 2));
            playerIndex.Add(3 * 36 + (dices[1] * 2));

            int cardIndex = (dices[0] + 4) % 4;
            if (playerIdInfoDic[playerInfoProxy.UserInfo.UserID].sit == 1)
            {
                if (cardIndex == 1)
                {
                    sendHeapStartIndex = playerIndex[0];
                }
                if (cardIndex == 2)
                {
                    sendHeapStartIndex = playerIndex[1];
                }
                if (cardIndex == 3)
                {
                    sendHeapStartIndex = playerIndex[2];
                }
                if (cardIndex == 0)
                {
                    sendHeapStartIndex = playerIndex[3];
                }
            }
            else if (playerIdInfoDic[playerInfoProxy.UserInfo.UserID].sit == 2)
            {
                if (cardIndex == 1)
                {
                    sendHeapStartIndex = playerIndex[3];
                }
                if (cardIndex == 2)
                {
                    sendHeapStartIndex = playerIndex[0];
                }
                if (cardIndex == 3)
                {
                    sendHeapStartIndex = playerIndex[1];
                }
                if (cardIndex == 0)
                {
                    sendHeapStartIndex = playerIndex[2];
                }
            }
            else if (playerIdInfoDic[playerInfoProxy.UserInfo.UserID].sit == 3)
            {
                if (cardIndex == 1)
                {
                    sendHeapStartIndex = playerIndex[2];
                }
                if (cardIndex == 2)
                {
                    sendHeapStartIndex = playerIndex[3];
                }
                if (cardIndex == 3)
                {
                    sendHeapStartIndex = playerIndex[0];
                }
                if (cardIndex == 0)
                {
                    sendHeapStartIndex = playerIndex[1];
                }
            }
            else
            {
                if (cardIndex == 1)
                {
                    sendHeapStartIndex = playerIndex[1];
                }
                if (cardIndex == 2)
                {
                    sendHeapStartIndex = playerIndex[2];
                }
                if (cardIndex == 3)
                {
                    sendHeapStartIndex = playerIndex[3];
                }
                if (cardIndex == 0)
                {
                    sendHeapStartIndex = playerIndex[0];
                }
            }


            //replaceFlowerCards = gameStartS2C.givenCards;
            //getFlowerCards = gameStartS2C.flowerCards;
            //         gameStartS2C.flowerCards.ForEach (o => {
            //	Debug.Log ("花牌：" + o);
            //});
            // gameStartS2C.pushPlayerActTipS2C.optUserId;
            tingCards.Clear();
            var isFirstMatch = true;
            foreach (KeyValuePair<int, PlayerInfoVOS2C> playerInfoVos2C in playerIdInfoDic)
            {//已经有庄家
                if (playerInfoVos2C.Value.isBanker)
                {
                    isFirstMatch = false;
                    break;
                }
            }
            var bankerPlayerInfoVO = playerIdInfoDic[gameStartS2C.bankerUserId]; //设置庄家

            curInnings = gameStartS2C.currentTimes;
            if (curInnings == 1)
            {
                bankerPlayerInfoVO.isMaster = true;
            }

            //if (gameStartS2C.bankerUserId == )
            //{

            //}

            foreach (KeyValuePair<int, PlayerInfoVOS2C> playerInfoVos2C in playerIdInfoDic)
            {
                playerInfoVos2C.Value.pengGangCards.Clear();
                playerInfoVos2C.Value.handCards.Clear();
                playerInfoVos2C.Value.putCards.Clear();
                playerInfoVos2C.Value.isBanker = playerInfoVos2C.Value.userId == gameStartS2C.bankerUserId;
                if (playerInfoVos2C.Value.userId == playerInfoProxy.UserInfo.UserID)
                {
                    playerInfoVos2C.Value.handCards.AddRange(gameStartS2C.handCards);
                    if (playerInfoProxy.UserInfo.UserID == gameStartS2C.bankerUserId && gameStartS2C.touchMahjongCode != 0)
                    {//自己是庄家,给自己加一张牌
                        playerInfoVos2C.Value.getCard = gameStartS2C.touchMahjongCode;
                    }
                }
                else
                {
                    for (int i = 0; i < GlobalData.PLAYER_CARD_NUM; i++)
                    {
                        playerInfoVos2C.Value.handCards.Add(GlobalData.CardValues[0]);
                    }
                    if (playerInfoVos2C.Value.userId == gameStartS2C.bankerUserId && gameStartS2C.touchMahjongCode!=0)
                    {//庄家再发一张牌
                        playerInfoVos2C.Value.getCard = GlobalData.CardValues[0];
                    }
                }
            }
            playerActTipS2C = gameStartS2C.pushPlayerActTipS2C;
            isSelfAction = gameStartS2C.pushPlayerActTipS2C.optUserId == playerInfoProxy.UserInfo.UserID;
            if (GlobalData.hasHeap)
            {
                InitHeapCardIndexs(GlobalData.CardWare.Length);
            }
            if (gameStartS2C.currentTimes == 1)
            {
                if (playerInfoProxy.UserInfo.UserID == gameStartS2C.bankerUserId)
                {
                    //GlobalData.sitNumber = 1;
                }
            }
            else
            {
                ///TODO...
                ///否则就从服务器获取当前出牌的座位号，目前只有摸牌的时候才知道
            }
            GlobalData.sit = playerIdInfoDic[playerInfoProxy.UserInfo.UserID].sit;
            GlobalData.optUserId = playerIdInfoDic[gameStartS2C.bankerUserId].sit;

            //精牌
            treasureCardCode = gameStartS2C.treasureCardCode;
            leftCard = gameStartS2C.leftCardCount;
            SendNotification(NotificationConstant.MEDI_BATTLE_SENDCARD);
            SendNotification(NotificationConstant.MEDI_BATTLE_SHOWJING);
            SendNotification(NotificationConstant.MEDI_BATTLEVIEW_UPDATEALLHEAD, isFirstMatch);
            SendNotification(NotificationConstant.MEDI_BATTLEVIEW_SHOWBANKERICON);

        }
        /// <summary>
        /// 当前是否第一局未开启
        /// </summary>
        public bool isFirstMatch
        {
            get
            {
                if (isStart)
                {
                    return false;
                }
                foreach (KeyValuePair<int, PlayerInfoVOS2C> playerInfoVos2C in playerIdInfoDic)
                {
                    if (playerInfoVos2C.Value.score != 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
         
        /// <summary>
        /// 初始化牌堆未拿牌的序号
        /// </summary>
        private void InitHeapCardIndexs(int leftCardValue)
        {
            Debug.Log("leftCardValue = "+ leftCardValue);
            Debug.Log("sendHeapStartIndex = "+ sendHeapStartIndex);
            unGetHeapCardIndexs = new List<int>();
            int recivedCard = GlobalData.CardWare.Length - leftCardValue;
            for (int i = sendHeapStartIndex + recivedCard; i < GlobalData.CardWare.Length; i++)
            {
                unGetHeapCardIndexs.Add(i);
            }
            int addStart = 0;
            if (sendHeapStartIndex + recivedCard > GlobalData.CardWare.Length - 1)
            {
                addStart = (sendHeapStartIndex + recivedCard) % GlobalData.CardWare.Length;
            }
            for (int i = addStart; i < sendHeapStartIndex; i++)
            {
                unGetHeapCardIndexs.Add(i);
            }
        }

        /// <summary>
        /// 推送玩家动作提示
        /// </summary>
        /// <param name="bytes">消息体</param>
        private void PushPlayerActTipHandler(byte[] bytes)
        {
            if (_isForbit)
            {
                var actionVO = new ForbitActionVO();
                actionVO.isActTip = true;
                actionVO.bytes = bytes;
                forbitActions.Add(actionVO);
                return;
            }
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            var curActTips = NetMgr.Instance.DeSerializes<PushPlayerActTipS2C>(bytes);
            if (playerActTipS2C != null && playerActTipS2C.optUserId == playerInfoProxy.UserInfo.UserID)
            {
                for (int i = 0; i < huTypes.Count; i++)
                {
                    if (playerActTipS2C.acts.IndexOf(huTypes[i]) != -1)
                    {//自己已接到胡牌推送,忽略后续的提示
                        return;
                    }
                }
            }


            //Debug.Log(string.Format("当前玩家的座位号：{0}，座位号：{1}", GlobalData.optUserId, GlobalData.sit));


            if (isSelfAction && GlobalData.isDebugModel)
            {
                if (playerActTipS2C.acts.Contains(PlayerActType.PASS))
                {
                    NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.GUO_C2S.GetHashCode(), 0, new GuoC2S());
                }
                else if (playerActTipS2C.acts.Contains(PlayerActType.PUT_CARD))
                {
                    var putC2S = new PlayAMahjongC2S();
                    putC2S.mahjongCode = playerActTipS2C.actCards[0];
                    NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.PLAYAMAHJONG_C2S.GetHashCode(), 0, putC2S);
                }
            }
            playerActTipS2C = curActTips;

            GlobalData.sit = playerIdInfoDic[playerInfoProxy.UserInfo.UserID].sit;
            GlobalData.optUserId = playerIdInfoDic[playerActTipS2C.optUserId].sit;
            //Debug.Log("battleproxy. playerActTipS2C.optUserId = "+ playerActTipS2C.optUserId);
            isSelfAction = playerActTipS2C.optUserId == playerInfoProxy.UserInfo.UserID;
            SendNotification(NotificationConstant.MEDI_BATTLE_PLAYACTTIP);
            if (flowerCardList.Contains(playerActTipS2C.actCards[0]) && playerActTipS2C != null)
            {
                PutFlowerCard(); 
            }
        }

        /// <summary>
        /// 打花牌
        /// </summary>
        public void PutFlowerCard()
        {
            if (playerActTipS2C != null)
            {
                List<int> flowers = new List<int>();
                var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
                PlayerInfoVOS2C selfPlayerInfoVO = playerIdInfoDic[playerInfoProxy.UserInfo.UserID];

                if (flowerCardList.Contains(playerActTipS2C.actCards[0]))
                {
                    PlayAFlowerMahjongC2S actC2S = new PlayAFlowerMahjongC2S();
                    flowers.Add(playerActTipS2C.actCards[0]);
                    actC2S.mahjongCode = flowers[0];
                    NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.PlayAFlowerMahjongC2S.GetHashCode(), 0, actC2S);
                    //Debug.Log("这是打的花牌：" + actC2S.mahjongCode);

                    //if (playerInfoProxy.UserInfo.UserID == playerActTipS2C.optUserId || isReport)
                    //{ //是自己找到对应的牌移除
                    //    selfPlayerInfoVO.handCards.Remove(playerActTipS2C.actCards[0]);
                    //}
                    //else
                    //{
                    //    var randomIndex = selfPlayerInfoVO.handCards.Count - 1;
                    //    selfPlayerInfoVO.handCards.RemoveAt(randomIndex);
                    //}
                }

            }
            else
            {
                Debug.Log("断线重连回来时playerActTipS2C = null");
            }

        }

        /// <summary>
        /// 动作提示延迟
        /// </summary>
        private void DelayPlayerActTip(byte[] bytes)
        {
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            var curActTips = NetMgr.Instance.DeSerializes<PushPlayerActTipS2C>(bytes);
            if (playerActTipS2C != null && playerActTipS2C.optUserId == playerInfoProxy.UserInfo.UserID)
            {
                for (int i = 0; i < huTypes.Count; i++)
                {
                    if (playerActTipS2C.acts.IndexOf(huTypes[i]) != -1)
                    {//自己已接到胡牌推送,忽略后续的提示
                        return;
                    }
                }
            }
            playerActTipS2C = curActTips;
            //有胡牌操作,过滤其他非胡牌操作
            for (int i = 0; i < huTypes.Count; i++)
            {
                if (playerActTipS2C.acts.IndexOf(huTypes[i]) != -1)
                {
                    for (int j = 0; j < playerActTipS2C.acts.Count;)
                    {
                        if (huTypes.IndexOf(playerActTipS2C.acts[j]) == -1)
                        {
                            playerActTipS2C.acts.RemoveAt(j);
                        }
                        else
                        {
                            j++;
                        }
                    }
                    break;
                }
            }
            isSelfAction = playerActTipS2C.optUserId == playerInfoProxy.UserInfo.UserID;
            if (isStart)
                SendNotification(NotificationConstant.MEDI_BATTLE_PLAYACTTIP);
        }

        /// <summary>
        /// 推送玩家动作
        /// </summary>
        /// <param PlayerName="bytes">消息体</param>
        private void PushPlayerActHandler(byte[] bytes)
        {
            if (_isForbit)
            {
                var actionVO = new ForbitActionVO();
                actionVO.isActTip = false;
                actionVO.bytes = bytes;
                forbitActions.Add(actionVO);
                return;
            }
            playerActTipS2C = null;
            playerActS2C = NetMgr.Instance.DeSerializes<PushPlayerActS2C>(bytes);

            //  leftCard =playerActS2C.leftCardCount;
            switch (playerActS2C.act)
            {
                case PlayerActType.ZHI_GANG:
                    ZhiGangActHandler();
                    break;
                case PlayerActType.BACK_AN_GANG:
                    BackAnGangActHandler();
                    break;
                case PlayerActType.COMMON_AN_GANG:
                    CommonAnGangActHandler();
                    break;
                case PlayerActType.BACK_PENG_GANG:
                    BackPengGangActHandler();
                    break;
                case PlayerActType.COMMON_PENG_GANG:
                    CommonPengGangActHandler();
                    break;
                case PlayerActType.GET_CARD:
                    GetCardActHandler();
                    break;
                case PlayerActType.QIANG_AN_GANG_HU:
                    HuActHandler(false);
                    break;
                case PlayerActType.QIANG_PENG_GANG_HU:
                    HuActHandler(false);
                    break;
                case PlayerActType.QIANG_ZHI_GANG_HU:
                    HuActHandler(false);
                    break;
                case PlayerActType.SELF_HU:
                    HuActHandler(true);
                    break;
                case PlayerActType.CHI_HU:
                    HuActHandler(false);
                    break;
                case PlayerActType.PENG:
                    PengActHandler();
                    break;
                case PlayerActType.PASS:
                    PassActHandler();
                    break;
                case PlayerActType.PUT_CARD:
                    PutCardActHandler();
                    break;
                case PlayerActType.CHI:
                    ChiActHandler();
                    break;
                case PlayerActType.PUT_FLOWER_CARD:
                    PutFlowerCardHandler();
                    break;
            }
        }

        public void PutFlowerCardHandler()
        {
            isFlowerGetCard = true;
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            var putCardPlayerVO = playerIdInfoDic[playerActS2C.userId];
            if (isReport)
            {
                //var playerCards = new List<PlayerCardVO>();
                //playerCards.Add(RecordCardInfo(putCardPlayerVO, PlayerCardType.GET, PlayerCardType.HAND, PlayerCardType.PUT));
                //reportActions.Add(playerCards);
            }
            putCardPlayerVO.flowerCards.Add(playerActS2C.actCard);
            //putCardPlayerVO.flowerCards.Add(playerActS2C.actCard);
            if (putCardPlayerVO.getCard != 0)
            {
                putCardPlayerVO.handCards.Add(putCardPlayerVO.getCard);
            }
            if (playerInfoProxy.UserInfo.UserID == playerActS2C.userId || isReport)
            { //是自己找到对应的牌移除
                putCardPlayerVO.handCards.Remove(playerActS2C.actCard);
            }
            else
            {
                var randomIndex = UnityEngine.Random.Range(0, putCardPlayerVO.handCards.Count - 1);
                putCardPlayerVO.handCards.RemoveAt(randomIndex);
            }

            putCardPlayerVO.handCards.Sort();
            putCardPlayerVO.getCard = 0;
            SendNotification(NotificationConstant.MEDI_BATTLE_PLAYPUTFLOWERCARD);
        }
        /// <summary>
        /// 推送直杠牌消息处理
        /// </summary>
        private void ZhiGangActHandler()
        {
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            var pengGangPlayerVOS2C = playerIdInfoDic[playerActS2C.userId];
            var pengGangCardVOS2C = new PengGangCardVO();
            pengGangCardVOS2C.pengGangCards.Add(playerActS2C.actCard);
            pengGangCardVOS2C.pengGangCards.Add(playerActS2C.actCard);
            pengGangCardVOS2C.pengGangCards.Add(playerActS2C.actCard);
            pengGangCardVOS2C.pengGangCards.Add(playerActS2C.actCard);
            pengGangCardVOS2C.targetUserId = playerActS2C.targetUserId;
            pengGangCardVOS2C.specialMahjong = -2;
            pengGangPlayerVOS2C.pengGangCards.Add(pengGangCardVOS2C);
            if (playerInfoProxy.UserInfo.UserID == playerActS2C.userId)
            {//是自己找到对应的牌移除
                pengGangPlayerVOS2C.handCards.Remove(playerActS2C.actCard);
                pengGangPlayerVOS2C.handCards.Remove(playerActS2C.actCard);
                pengGangPlayerVOS2C.handCards.Remove(playerActS2C.actCard);
            }
            else
            {//非自己随机找牌移除
                var randomIndex = UnityEngine.Random.Range(0, pengGangPlayerVOS2C.handCards.Count - 3);
                pengGangPlayerVOS2C.handCards.RemoveAt(randomIndex);
                pengGangPlayerVOS2C.handCards.RemoveAt(randomIndex);
                pengGangPlayerVOS2C.handCards.RemoveAt(randomIndex);
            }

            pengGangPlayerVOS2C.handCards.Sort();
            var targetPlayerVOS2C = playerIdInfoDic[playerActS2C.targetUserId];
            targetPlayerVOS2C.putCards.RemoveAt(targetPlayerVOS2C.putCards.Count - 1); //移除最后一张出牌
            SendNotification(NotificationConstant.MEDI_BATTLE_PLAYZHIGANG);
        }

        /// <summary>
        /// 推送回头暗杠消息处理
        /// </summary>
        private void BackAnGangActHandler()
        {
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            var pengGangPlayerVOS2C = playerIdInfoDic[playerActS2C.userId];
            var pengGangCardVOS2C = new PengGangCardVO();
            pengGangCardVOS2C.pengGangCards.Add(playerActS2C.actCard);
            pengGangCardVOS2C.pengGangCards.Add(playerActS2C.actCard);
            pengGangCardVOS2C.pengGangCards.Add(playerActS2C.actCard);
            pengGangCardVOS2C.pengGangCards.Add(playerActS2C.actCard);
            pengGangCardVOS2C.targetUserId = playerActS2C.targetUserId;
            pengGangCardVOS2C.specialMahjong = -1;
            pengGangPlayerVOS2C.pengGangCards.Add(pengGangCardVOS2C);
            if (playerInfoProxy.UserInfo.UserID == playerActS2C.userId)
            {//是自己找到对应的牌移除
                pengGangPlayerVOS2C.handCards.Remove(playerActS2C.actCard);
                pengGangPlayerVOS2C.handCards.Remove(playerActS2C.actCard);
                pengGangPlayerVOS2C.handCards.Remove(playerActS2C.actCard);
                pengGangPlayerVOS2C.handCards.Remove(playerActS2C.actCard);
            }
            else
            {//非自己随机找牌移除
                var randomIndex = UnityEngine.Random.Range(0, pengGangPlayerVOS2C.handCards.Count - 4);
                pengGangPlayerVOS2C.handCards.RemoveAt(randomIndex);
                pengGangPlayerVOS2C.handCards.RemoveAt(randomIndex);
                pengGangPlayerVOS2C.handCards.RemoveAt(randomIndex);
                pengGangPlayerVOS2C.handCards.RemoveAt(randomIndex);
            }

            pengGangPlayerVOS2C.handCards.Sort();
            SendNotification(NotificationConstant.MEDI_BATTLE_PLAY_BACKANGANG);
        }

        /// <summary>
        /// 推送暗杠消息处理
        /// </summary>
        private void CommonAnGangActHandler()
        {
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            var pengGangPlayerVOS2C = playerIdInfoDic[playerActS2C.userId];
            var pengGangCardVOS2C = new PengGangCardVO();
            pengGangCardVOS2C.pengGangCards.Add(playerActS2C.actCard);
            pengGangCardVOS2C.pengGangCards.Add(playerActS2C.actCard);
            pengGangCardVOS2C.pengGangCards.Add(playerActS2C.actCard);
            pengGangCardVOS2C.pengGangCards.Add(playerActS2C.actCard);
            pengGangCardVOS2C.targetUserId = playerActS2C.targetUserId;
            pengGangPlayerVOS2C.pengGangCards.Add(pengGangCardVOS2C);
            pengGangPlayerVOS2C.getCard = 0;
            pengGangCardVOS2C.specialMahjong = -1;
            if (playerInfoProxy.UserInfo.UserID == playerActS2C.userId)
            { //是自己找到对应的牌移除
                pengGangPlayerVOS2C.handCards.Remove(playerActS2C.actCard);
                pengGangPlayerVOS2C.handCards.Remove(playerActS2C.actCard);
                pengGangPlayerVOS2C.handCards.Remove(playerActS2C.actCard);

            }
            else
            {//非自己随机找牌移除
                var randomIndex = UnityEngine.Random.Range(0, pengGangPlayerVOS2C.handCards.Count - 3);
                pengGangPlayerVOS2C.handCards.RemoveAt(randomIndex);
                pengGangPlayerVOS2C.handCards.RemoveAt(randomIndex);
                pengGangPlayerVOS2C.handCards.RemoveAt(randomIndex);
            }

            pengGangPlayerVOS2C.handCards.Sort();
            SendNotification(NotificationConstant.MEDI_BATTLE_PLAY_COMMONANGANG);
        }

        /// <summary>
        /// 推送普通碰杠消息处理
        /// </summary>
        private void CommonPengGangActHandler()
        {
            var pengGangPlayerVOS2C = playerIdInfoDic[playerActS2C.userId];
            foreach (var pengGangCardVos2C in pengGangPlayerVOS2C.pengGangCards)
                if (pengGangCardVos2C.pengGangCards[0] == playerActS2C.actCard)
                {
                    pengGangCardVos2C.pengGangCards.Add(playerActS2C.actCard);
                    pengGangCardVos2C.specialMahjong = -2;
                }
            pengGangPlayerVOS2C.getCard = 0;
            SendNotification(NotificationConstant.MEDI_BATTLE_PLAY_COMMONPENGGANG);
        }

        /// <summary>
        /// 推送回头碰杠消息处理
        /// </summary>
        private void BackPengGangActHandler()
        {
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            var pengGangPlayerVOS2C = playerIdInfoDic[playerActS2C.userId];
            foreach (var pengGangCardVos2C in pengGangPlayerVOS2C.pengGangCards)
                if (pengGangCardVos2C.pengGangCards[0] == playerActS2C.actCard)
                {
                    pengGangCardVos2C.pengGangCards.Add(playerActS2C.actCard);
                    pengGangCardVos2C.specialMahjong = -2;
                }
            if (playerInfoProxy.UserInfo.UserID == playerActS2C.userId)
            { //是自己找到对应的牌移除
                pengGangPlayerVOS2C.handCards.Remove(playerActS2C.actCard);
            }
            else
            {
                var randomIndex = UnityEngine.Random.Range(0, pengGangPlayerVOS2C.handCards.Count - 1);
                pengGangPlayerVOS2C.handCards.RemoveAt(randomIndex);
            }

            pengGangPlayerVOS2C.getCard = 0;
            SendNotification(NotificationConstant.MEDI_BATTLE_PLAY_BACKPENGGANG);

        }
        /// <summary>
        /// 是否补花的摸牌
        /// </summary>
        public bool isFlowerGetCard = false;
        /// <summary>
        /// 推送摸牌消息处理
        /// </summary>
        private void GetCardActHandler()
        {
            leftCard -= 1;
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            
            var getCardPlayerVOS2C = playerIdInfoDic[playerActS2C.userId];
            if (getCardPlayerVOS2C.getCard != 0)
            {
                getCardPlayerVOS2C.handCards.Add(getCardPlayerVOS2C.getCard);
            }
            
            if (isFlowerGetCard )
            {
                if (getCardPlayerVOS2C.handCards.Count >= 12 || getCardPlayerVOS2C.pengGangCards.Count > 0)
                {
                    getCardPlayerVOS2C.getCard = playerActS2C.actCard;
                }
                else
                {
                    getCardPlayerVOS2C.handCards.Add(playerActS2C.actCard);
                }
            }
            else
            {
                getCardPlayerVOS2C.getCard = playerActS2C.actCard;
            }

            SendNotification(NotificationConstant.MEDI_BATTLE_PLAYGETCARD);
        }


        /// <summary>
        /// 推送胡牌消息处理
        /// </summary>
        private void HuActHandler(bool isSelf)
        {
            var huCardPlayerVOS2C = playerIdInfoDic[playerActS2C.userId];
            if (isSelf)
            { //自摸
                if (huCardPlayerVOS2C.getCard > 0)
                {
                    huCardPlayerVOS2C.handCards.Add(huCardPlayerVOS2C.getCard);
                    huCardPlayerVOS2C.getCard = 0;
                }
            }
            else
            {
                huCardPlayerVOS2C.handCards.Add(playerActS2C.actCard);
                var targetPlayerVOS2C = playerIdInfoDic[playerActS2C.targetUserId];
                targetPlayerVOS2C.putCards.RemoveAt(targetPlayerVOS2C.putCards.Count - 1); //移除最后一张出牌
            }
            playerActTipS2C = null;
            SendNotification(NotificationConstant.MEDI_BATTLE_PLAYHU, isSelf);
        }

        /// <summary>
        /// 推送碰牌消息处理
        /// </summary>
        private void PengActHandler()
        {
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            var pengPlayerVOS2C = playerIdInfoDic[playerActS2C.userId];
            var pengGangCardVOS2C = new PengGangCardVO();
            pengGangCardVOS2C.pengGangCards.Add(playerActS2C.actCard);
            pengGangCardVOS2C.pengGangCards.Add(playerActS2C.actCard);
            pengGangCardVOS2C.pengGangCards.Add(playerActS2C.actCard);
            pengGangCardVOS2C.targetUserId = playerActS2C.targetUserId;
            pengPlayerVOS2C.pengGangCards.Add(pengGangCardVOS2C);
            pengGangCardVOS2C.specialMahjong = -3;
            if (playerInfoProxy.UserInfo.UserID == playerActS2C.userId)
            { //是自己找到对应的牌移除
                pengPlayerVOS2C.handCards.Remove(playerActS2C.actCard);
                pengPlayerVOS2C.handCards.Remove(playerActS2C.actCard);
            }
            else
            {
                var randomIndex = UnityEngine.Random.Range(0, pengPlayerVOS2C.handCards.Count - 2);
                pengPlayerVOS2C.handCards.RemoveAt(randomIndex);
                pengPlayerVOS2C.handCards.RemoveAt(randomIndex);
            }

            pengPlayerVOS2C.handCards.Sort();

            var targetPlayerVOS2C = playerIdInfoDic[playerActS2C.targetUserId];
            targetPlayerVOS2C.putCards.RemoveAt(targetPlayerVOS2C.putCards.Count - 1); //移除最后一张出牌
            SendNotification(NotificationConstant.MEDI_BATTLE_PLAYPENG);
        }

        /// <summary>
        /// 推送过消息处理
        /// </summary>
        private void PassActHandler()
        {
            playerActTipS2C = null;
            SendNotification(NotificationConstant.MEDI_BATTLE_PLAYPASS);
        }

        /// <summary>
        /// 推送出牌消息处理
        /// </summary>
        private void PutCardActHandler()
        {
            
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            var putCardPlayerVO = playerIdInfoDic[playerActS2C.userId];
            if (isReport)
            {
                //var playerCards = new List<PlayerCardVO>();
                //playerCards.Add(RecordCardInfo(putCardPlayerVO, PlayerCardType.GET, PlayerCardType.HAND, PlayerCardType.PUT));
                //reportActions.Add(playerCards);
            }
            putCardPlayerVO.putCards.Add(playerActS2C.actCard);
            if (putCardPlayerVO.getCard != 0)
            {
                putCardPlayerVO.handCards.Add(putCardPlayerVO.getCard);
            }
            if (playerInfoProxy.UserInfo.UserID == playerActS2C.userId || isReport)
            { //是自己找到对应的牌移除
                putCardPlayerVO.handCards.Remove(playerActS2C.actCard);
            }
            else
            {
                var randomIndex = UnityEngine.Random.Range(0, putCardPlayerVO.handCards.Count - 1);
                putCardPlayerVO.handCards.RemoveAt(randomIndex);
            }
            putCardPlayerVO.handCards.Sort();
            putCardPlayerVO.getCard = 0;
            //tingCards.Clear();
            SendNotification(NotificationConstant.MEDI_BATTLE_PLAYPUTCARD);
            
        }

        /// <summary>
        /// 推送吃牌消息处理
        /// </summary>
        private void ChiActHandler()
        {
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            var pengPlayerVOS2C = playerIdInfoDic[playerActS2C.userId];
            var pengGangCardVOS2C = new PengGangCardVO();
            pengGangCardVOS2C.pengGangCards.Add(playerActS2C.actCard);
            pengGangCardVOS2C.pengGangCards.AddRange(playerActS2C.chiCards);
            pengGangCardVOS2C.pengGangCards.Sort();
            pengGangCardVOS2C.targetUserId = playerActS2C.targetUserId;
            pengPlayerVOS2C.pengGangCards.Add(pengGangCardVOS2C);
            pengGangCardVOS2C.specialMahjong = playerActS2C.actCard;
            if (playerInfoProxy.UserInfo.UserID == playerActS2C.userId)
            { //是自己找到对应的牌移除
                pengPlayerVOS2C.handCards.Remove(playerActS2C.chiCards[0]);
                pengPlayerVOS2C.handCards.Remove(playerActS2C.chiCards[1]);
            }
            else
            {
                var randomIndex = UnityEngine.Random.Range(0, pengPlayerVOS2C.handCards.Count - 2);
                pengPlayerVOS2C.handCards.RemoveAt(randomIndex);
                pengPlayerVOS2C.handCards.RemoveAt(randomIndex);
            }

            pengPlayerVOS2C.handCards.Sort();

            var targetPlayerVOS2C = playerIdInfoDic[playerActS2C.targetUserId];
            targetPlayerVOS2C.putCards.RemoveAt(targetPlayerVOS2C.putCards.Count - 1); //移除最后一张出牌
            SendNotification(NotificationConstant.MEDI_BATTLE_PLAYCHI);

            if (GlobalData.LoginServer == "127.0.0.1")
            {

            }
        }

        /// <summary>
        /// 出牌返回
        /// </summary>
        /// <param name="bytes"></param>
        private void PlayAmahjongHandler(byte[] bytes)
        {
            var playAmahjongS2C = NetMgr.Instance.DeSerializes<PlayAMahjongS2C>(bytes);
            tingCards.Clear();
            tingCards = playAmahjongS2C.tingCards;
            SendNotification(NotificationConstant.TING_UPDATE);
            //Debug.Log("收到开局听牌提示。。。。。。。。。。。。。。。。");
        }
        private void PlayFlowerCardHandler(byte[] bytes)
        {
            var playeFlowerCardS2C = NetMgr.Instance.DeSerializes<PlayAFlowerMahjongC2S>(bytes);
            Debug.Log("服务器告诉客户端已打出花牌：" + playeFlowerCardS2C.mahjongCode);
        }
        /// <summary>
        /// 推送本局结束
        /// </summary>
        /// <param PlayerName="bytes"></param>
        private void PushMatchEndHandler(byte[] bytes)
        {
            isStart = false;
            matchResultS2C = NetMgr.Instance.DeSerializes<PushMatchResultS2C>(bytes);
            perBankerId = BankerPlayerInfoVOS2C.userId;
            if (matchResultS2C.huUserId.Count == 1)
            { //本局结束,重置玩家状态数据
                foreach (KeyValuePair<int, PlayerInfoVOS2C> keyValuePair in playerIdInfoDic)
                {
                    keyValuePair.Value.isReady = false;
                    if (keyValuePair.Value.userId == matchResultS2C.huUserId[0])
                    { //新庄家
                        keyValuePair.Value.isBanker = true;
                    }
                    else
                    {
                        keyValuePair.Value.isBanker = false;
                    }
                }
            }
            else if (matchResultS2C.huUserId.Count > 1)
            {
                foreach (KeyValuePair<int, PlayerInfoVOS2C> keyValuePair in playerIdInfoDic)
                {
                    keyValuePair.Value.isReady = false;
                    if (keyValuePair.Value.userId == matchResultS2C.huedUserId)
                    { //新庄家
                        keyValuePair.Value.isBanker = true;
                    }
                    else
                    {
                        keyValuePair.Value.isBanker = false;
                    }
                }
            }
            foreach (PlayerMatchResultVOS2C playerMatchResultVO in matchResultS2C.resultInfos)
            {
                playerMatchResultVO.handCards.Sort();
                playerIdInfoDic[playerMatchResultVO.userId].score += playerMatchResultVO.addScore;
                //playerMatchResultVO.handCards.ForEach(o=> { Debug.Log("玩家的id："+playerMatchResultVO.userId+"  手中的牌：" +o); });

            }

            flowerCardList.Clear();
            flowerCardList.Add(61);
            flowerCardList.Add(62);
            flowerCardList.Add(63);
            flowerCardList.Add(64);
            flowerCardList.Add(65);
            flowerCardList.Add(66);
            flowerCardList.Add(67);
            flowerCardList.Add(68);
            dices.Clear();
            tingCards.Clear();
            SendNotification(NotificationConstant.TING_UPDATE);
            SendNotification(NotificationConstant.MEDI_BATTLEVIEW_UPDATEALLHEAD, false);
            SendNotification(NotificationConstant.MEDI_BATTLEVIEW_SHOWMATCHRESULT);
        }


        /// <summary>
        /// 推送房间结束
        /// </summary>
        /// <param PlayerName="bytes"></param>
        private void PushRoomEndHandler(byte[] bytes)
        {
            NSocket.RoomId = 0;
            roomResultS2C = NetMgr.Instance.DeSerializes<PushRoomResultS2C>(bytes);
            if (matchResultS2C == null)
            {
                UIManager.Instance.ShowUI(UIViewID.ROOM_RESULT_VIEW);
            }
        }

        /// <summary>
        /// 刷新座位字典
        /// </summary>
        private void UpdatePlayerSitDic()
        {
            playerSitInfoDic = new Dictionary<int, PlayerInfoVOS2C>();
            foreach (var playerInfoVO in playerIdInfoDic)
                playerSitInfoDic.Add(playerInfoVO.Value.sit, playerInfoVO.Value);
        }

        /// <summary>
        /// 增加局数
        /// </summary>
        public void AddInnings()
        {
            Resources.UnloadUnusedAssets();
            forbitActions.Clear();
            matchResultS2C = null;
            roomResultS2C = null;
            playerActTipS2C = null;
            playerActS2C = null;
            isForbit = false;
            isStart = false;
            tingCards.Clear();
            SendNotification(NotificationConstant.TING_UPDATE);
        }

        /// <summary>
        /// 推送语音消息处理
        /// </summary>
        public void PushVoiceHandler(byte[] bytes)
        {
            var pushVoiceS2C = NetMgr.Instance.DeSerializes<PushVoiceS2C>(bytes);
            RecorderSystem.GetAudioPacket(pushVoiceS2C.senderUserId, pushVoiceS2C.flag, pushVoiceS2C.content);
        }

        /// <summary>
        /// 推送聊天消息处理
        /// </summary>
        /// <param name="bytes"></param>
        private void PushChatHandler(byte[] bytes)
        {
            var pushChatS2C = NetMgr.Instance.DeSerializes<PushSendChatS2C>(bytes);
            if (pushChatS2C.content.Contains(GlobalData.FACE_PREFIX))
            {
                SendNotification(NotificationConstant.MEDI_BATTLEVIEW_SHOWFACE, pushChatS2C);
            }
            else
            {
                SendNotification(NotificationConstant.MEDI_BATTLEVIEW_SHOWCHAT, pushChatS2C);
            }
        }

        /// <summary>
        /// 获取战报内容
        /// </summary>
        /// <param name="bytes"></param>
        private void PlayVideo(byte[] bytes)
        {
            var gameMgrProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.GAMEMGR_PROXY) as GameMgrProxy;
            var playVideoS2C = NetMgr.Instance.DeSerializes<PlayVideoS2C>(bytes);
            report = PlayReportS2C.Paser(playVideoS2C.report);
            var addTime = gameMgrProxy.systemTime - report.startTime;
            foreach (ActionVO action in report.actions)
            {//将时间置为当前服务器时间
                action.actionTime += addTime;
                if (action.isActionTip)
                {
                    action.actTip.tipRemainUT += addTime;
                }
            }
            reportLocalTime = Time.time;
            gameMgrProxy.ReviseScaleSystemTime();
            isReport = true;
            NetMgr.Instance.OnClientReceiveBuff(MsgNoS2C.JOIN_ROOM_S2C.GetHashCode(), 0, report.joinInfo);
            SendNotification(NotificationConstant.MEDI_BATTLEVIEW_SHOW_REPORTVIEW);
        }

        /// <summary>
        /// 获取玩家信息返回
        /// </summary>
        /// <param name="bytes"></param>
        private void GetUserInfoHandler(byte[] bytes)
        {
            var getPlayerInfoS2C = NetMgr.Instance.DeSerializes<GetUserInfoByIdS2C>(bytes);
            var playerInfoView = UIManager.Instance.ShowUI(UIViewID.PLATER_INFO_VIEW) as PlayerInfoView;
            playerInfoView.data = getPlayerInfoS2C;
        }

        /// <summary>
        /// 获取申请解散消息处理
        /// </summary>
        /// <param name="bytes"></param>
        private void DisloveApplyHandler(byte[] bytes)
        {
            var disloveApplyS2C = NetMgr.Instance.DeSerializes<ApplyDissolveRoomS2C>(bytes);
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            var gameMgrProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.GAMEMGR_PROXY) as GameMgrProxy;
            disloveApplyUserId = disloveApplyS2C.userId;
            agreeIds.Add(disloveApplyS2C.userId);
            hasDisloveApply = true;
            disloveRemainTime = GlobalData.DISLOVE_APPLY_TIMEOUT;
            disloveRemainUT = gameMgrProxy.systemTime;
            UIManager.Instance.ShowUI(UIViewID.DISLOVE_STATISTICS_VIEW);
            SendNotification(NotificationConstant.UPDATE_DISLOVE_STATISTICS);
            if (disloveApplyS2C.userId != playerInfoProxy.UserInfo.UserID)
            {//自己的申请忽略
                UIManager.Instance.ShowUI(UIViewID.DISLOVE_APPLY_VIEW);

            }




        }

        /// <summary>
        /// 同意解散
        /// </summary>
        /// <param name="bytes"></param>
        private void DisloveConfirmHandler(byte[] bytes)
        {
            
            var disloveCancelS2C = NetMgr.Instance.DeSerializes<CancelDissolveRoomS2C>(bytes);
            var playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            var disloveConfirmS2C = NetMgr.Instance.DeSerializes<DissloveRoomConfirmS2C>(bytes);
            
            if (disloveCancelS2C.userId != 0)
            {
                agreeIds.Add(disloveConfirmS2C.userId);
                SendNotification(NotificationConstant.UPDATE_DISLOVE_STATISTICS);
            }
            //if (disloveConfirmS2C.userId == playerInfoProxy.UserInfo.UserID)
            //{
            //    UIManager.Instance.ShowUI(UIViewID.DISLOVE_STATISTICS_VIEW);
            //}
        }

        /// <summary>
        /// 拒绝解散
        /// </summary>
        /// <param name="bytes"></param>
        private void DisloveCancelHandler(byte[] bytes)
        {
            UIManager.Instance.HideUI(UIViewID.DISLOVE_STATISTICS_VIEW);
            UIManager.Instance.HideUI(UIViewID.DISLOVE_APPLY_VIEW);
            var disloveCancelS2C = NetMgr.Instance.DeSerializes<CancelDissolveRoomS2C>(bytes);
            SendNotification(NotificationConstant.UPDATE_DISLOVE_STATISTICS);
            DialogMsgVO dialogMsgVO = new DialogMsgVO();
            dialogMsgVO.title = "解散提示";
            dialogMsgVO.dialogType = DialogType.ALERT;
            dialogMsgVO.content = string.Format("{0}拒绝解散房间", playerIdInfoDic[disloveCancelS2C.userId].name);
            DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
            dialogView.data = dialogMsgVO;
            hasDisloveApply = false;
            agreeIds.Clear();
            refuseIds.Clear();
        }


        /// <summary>
        /// 出牌出错返回
        /// </summary>
        /// <param name="bytes"></param>
        private void ActErrorHandler(byte[] bytes)
        {
            var errorS2C = NetMgr.Instance.DeSerializes<ActErrorS2C>(bytes);
            if (errorS2C.errorCode == ErrorCode.FORBIDDEN_CARD)
            {
                PopMsg.Instance.ShowMsg("当前牌不允许出");
            }
            isSelfAction = true;
        }

        /// <summary>
        /// 清理数据
        /// </summary>
        public void Clear()
        {
            curInnings = 1;
            isStart = false;
            creatorId = 0;
            matchResultS2C = null;
            roomResultS2C = null;
            playerActS2C = null;
            playerActTipS2C = null;
            playerIdInfoDic = null;

            playerSitInfoDic = null;

            isSelfAction = false;
            roomResultS2C = null;
            leftCard = GlobalData.CardWare.Length;
            speekPacket = new Queue<AudioPacket>();
            report = null;
            reportLocalTime = 0;
            isReport = false;
            perSendChatTime = 0;
            isPlayHu = false;
        }
    }
}