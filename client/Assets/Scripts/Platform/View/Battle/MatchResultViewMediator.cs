﻿using System.Collections;
using System.Collections.Generic;
using Platform.Model.Battle;
using Platform.Net;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;
using Platform.Model;
using UnityEngine.SceneManagement;
using System;
using Platform.Utils;

namespace Platform.View.Battle
{
    /// <summary>
    /// 单局结算界面中介
    /// </summary>
    internal class MatchResultViewMediator : Mediator, IMediator
    {
        /// <summary>
        ///     战斗模块数据中介
        /// </summary>
        private BattleProxy battleProxy;
        /// <summary>
        /// 玩家信息数据
        /// </summary>
        private PlayerInfoProxy playerInfoProxy;
        public MatchResultViewMediator(string mediatorName, object viewComponent) : base(mediatorName, viewComponent)
        {
        }

        public MatchResultView View
        {
            get { return (MatchResultView) ViewComponent; }
        }

        public override IList<string> ListNotificationInterests()
        {
            IList<string> list = new List<string>();
            return list;
        }


        public override void OnRegister()
        {
            base.OnRegister();
            battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
            playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
            View.startNextBtn.onClick.AddListener(OnStartNextClick);
            View.getoutRoomBtn.onClick.AddListener(OnGetOutRoomBtnClick);
            //View.shareBtn.onClick.AddListener(OnShaderClick);
            InitUI();
        }

        public override void OnRemove()
        {
            base.OnRemove();
            View.startNextBtn.onClick.RemoveListener(OnStartNextClick);
            View.getoutRoomBtn.onClick.RemoveListener(OnGetOutRoomBtnClick);
        }

        public override void HandleNotification(INotification notification)
        {
        }

        /// <summary>
        ///     初始化UI数据
        /// </summary>
        private void InitUI()
        {
            var matchResultS2C = battleProxy.matchResultS2C;
            for (var i = 0; i < matchResultS2C.resultInfos.Count; i++)
            {
                View.playerItems[i].data = matchResultS2C.resultInfos[i];
            }
            //if (matchResultS2C.huUserId.Contains(playerInfoProxy.UserInfo.UserID))
            //{
            //    View.titleIcon.gameObject.SetActive(false);
            //    var effectPerfab = Resources.Load<GameObject>("Effect/WinEffect/WinEffect");
            //    View.actEffect = GameObject.Instantiate(effectPerfab);
            //    var perPosition = View.actEffect.GetComponent<Transform>().localPosition;
            //    View.actEffect.GetComponent<Transform>().SetParent(View.ViewRoot.GetComponent<RectTransform>());
            //    View.actEffect.GetComponent<Transform>().localPosition = perPosition;
            //    View.actEffect.GetComponent<Transform>().localScale = Vector3.one;
            //    View.actEffect.GetComponent<Animator>().enabled = true;
            //    View.ViewRoot.GetComponent<Animator>().Play("MatchResultOpen",0,0);
            //    GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio("Voices/Effect/Win"));
            //}
            //else
            //{
            //    View.titleIcon.gameObject.SetActive(true);
            //    View.titleIcon.sprite = Resources.Load<Sprite>("Textures/MatchFailtTitle");
            //    View.ViewRoot.GetComponent<Animator>().enabled = true;
            //    View.ViewRoot.GetComponent<Animator>().Play("MatchResultOpen", 0, 0);
            //    GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio("Voices/Effect/Failt"));
            //}
            var hallProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.HALL_PROXY) as HallProxy;
            if (battleProxy.curInnings == hallProxy.HallInfo.Innings)//是否是最后一局
            {
               // View.startNextBtnTxt.text = "牌局结算";
            }
            else
            {
               // View.startNextBtnTxt.text = "开始游戏";
            }
        }

        /// <summary>
        /// 上次点击准备的时间
        /// </summary>
        private float perClickTime = 0;
        /// <summary>
        /// 开始下一局准备
        /// </summary>
        private void OnStartNextClick()
        {
            SendNotification(NotificationConstant.MEDI_BATTLE_UPDATEOOMMESSAGES);
            if (Time.time - perClickTime < 1)
            {
                return;
            }
            perClickTime = Time.time;
            GameMgr.Instance.StartCoroutine(PlayCloseEffect());
        }

        private void OnGetOutRoomBtnClick()
        {
            var loadInfo = new LoadSceneInfo(ESceneID.SCENE_HALL, LoadSceneType.ASYNC, LoadSceneMode.Additive);
            SendNotification(NotificationConstant.MEDI_GAMEMGR_LOADSCENE, loadInfo);
        }
        /// <summary>
        ///     播放关闭窗口动作,并移除窗口
        /// </summary>
        /// <returns></returns>
        private IEnumerator PlayCloseEffect()
        {
            //View.ViewRoot.GetComponent<Animator>().Play("MatchResultClose",0,0);
            yield return new WaitForSeconds(0.4f);
            foreach (MatchResultPlayerItem matchResultPlayerItem in View.playerItems)
            {
                matchResultPlayerItem.SaveAllCard();
            }
            if (battleProxy.roomResultS2C != null)//最后一局
            {
                UIManager.Instance.ShowUI(UIViewID.ROOM_RESULT_VIEW);
            }
            else
            {
                ApplicationFacade.Instance.SendNotification(NotificationConstant.READY_NEXT);

                battleProxy.AddInnings();
                var readyC2S = new ReadyC2S();
                NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.READY_C2S.GetHashCode(), 0, readyC2S);
            }
            UIManager.Instance.HideUI(UIViewID.MATCH_RESULT_VIEW);
        }
        /// <summary>
        ///     调用微信分享
        /// </summary>
        private void OnShaderClick()
        {
            if (GlobalData.sdkPlatform == SDKPlatform.ANDROID)
            {
                string desc = "快来江西景德镇翻精软四粒吧";
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
}