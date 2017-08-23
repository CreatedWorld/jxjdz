using Platform.Model;
using Platform.Model.Battle;
using Platform.Net;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Platform.View.Battle
{
    /// <summary>
    /// 聊天输入界面中介
    /// </summary>
    class ChatViewMediator : Mediator, IMediator
    {
        public ChatViewMediator(string mediatorName, object viewComponent) : base(mediatorName, viewComponent)
        {
        }

        public ChatView View
        {
            get { return (ChatView)ViewComponent; }
        }

        public override IList<string> ListNotificationInterests()
        {
            IList<string> list = new List<string>();
            return list;
        }

        bool emojiLight = true;
        public override void OnRegister()
        {
            base.OnRegister();
            View.closeBtn.onClick.AddListener(CloseHandler);

            //表情列表按钮注册
            for (int i = 0; i < View.faceList.childCount; i++)
            {
                var btn = View.faceList.GetChild(i).GetComponent<Button>();
                var index = i + 1;
                btn.onClick.AddListener(() =>
                {
                    SendFace(index);
                });

            }

            //常用语列表按钮注册
            //for (int i = 0; i < View.chatList.childCount; i++)
            //{
            //    var btn = View.chatList.GetChild(i).GetComponent<Button>();
            //    var index = i + 1;
            //    btn.onClick.AddListener(() =>
            //    {
            //        SendSpeaking(index);
            //    });
            //}

            View.SpeakingBtn.onClick.AddListener(OnClickSpeakingBtn);
            View.EmojiBtn.onClick.AddListener(OnClickEmojiBtn);
        }

        /// <summary>
        /// 显示或隐藏常用语列表
        /// </summary>
        private void OnClickSpeakingBtn()
        {
            if (!emojiLight)
            {
                emojiLight = true;
                View.SpeakingBtn.GetComponent<Image>().color = new Color(1,1,1,1);
                View.SpeakingGrid.SetActive(true);
                View.EmojiBtn.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                View.EmojiGrid.SetActive(false);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 显示或隐藏表情列表
        /// </summary>
        private void OnClickEmojiBtn()
        {
            if (emojiLight)
            {
                emojiLight = false;
                View.EmojiBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                View.EmojiGrid.SetActive(true);
                View.SpeakingBtn.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                View.SpeakingGrid.SetActive(false);
            }
            else
            {
                return;
               
            }
        }


        public override void OnRemove()
        {
            base.OnRemove();
        }

        public override void HandleNotification(INotification notification)
        {
        }

        /// <summary>
        /// 发送聊天内容
        /// </summary>
        //private void SendMsg()
        //{
        //    if (View.sendInput.text.Length == 0)
        //    {
        //        PopMsg.Instance.ShowMsg("请输入聊天内容");
        //    }
        //    else
        //    {
        //        GameMgrProxy gameMgrProxy =
        //            ApplicationFacade.Instance.RetrieveProxy(Proxys.GAMEMGR_PROXY) as GameMgrProxy;
        //        BattleProxy battleProxy =
        //            ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
        //        if (gameMgrProxy.systemTime - battleProxy.perSendChatTime < GlobalData.SendChatInvoke)
        //        {
        //            PopMsg.Instance.ShowMsg("请不要频繁发送");
        //            return;
        //        }
        //        SendChatC2S sendChatC2S = new SendChatC2S();
        //        sendChatC2S.content = View.sendInput.text;
        //        NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.SEND_CHAT_C2S.GetHashCode(), 0, sendChatC2S);
        //        View.sendInput.text = "";
        //        battleProxy.perSendChatTime = gameMgrProxy.systemTime;
        //        UIManager.Instance.HideUI(UIViewID.CHAT_VIEW);
        //    }
        //}

        /// <summary>
        /// 发送表情
        /// </summary>
        /// <param name="index"></param>
        private void SendFace(int index)
        {
            GameMgrProxy gameMgrProxy =
                    ApplicationFacade.Instance.RetrieveProxy(Proxys.GAMEMGR_PROXY) as GameMgrProxy;
            BattleProxy battleProxy =
                ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
            if (gameMgrProxy.systemTime - battleProxy.perSendChatTime < GlobalData.SendChatInvoke)
            {
                 PopMsg.Instance.ShowMsg("请不要频繁发送");
                
                return;
            }
            SendChatC2S sendChatC2S = new SendChatC2S();
            sendChatC2S.content = GlobalData.FACE_PREFIX + index;
            NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.SEND_CHAT_C2S.GetHashCode(), 0, sendChatC2S);
            battleProxy.perSendChatTime = gameMgrProxy.systemTime;
            UIManager.Instance.HideUI(UIViewID.CHAT_VIEW);
        }
        /// <summary>
        /// 发送常用语
        /// </summary>
        /// <param name="index"></param>
        private void SendSpeaking(int index)
        {
            //TODO...
            //Debug.Log("常用语 ："+ View.chatList.GetChild(index-1).name);
        }
        /// <summary>
        /// 关闭响应
        /// </summary>
        private void CloseHandler()
        {
            UIManager.Instance.HideUI(UIViewID.CHAT_VIEW);
        }
    }
}
