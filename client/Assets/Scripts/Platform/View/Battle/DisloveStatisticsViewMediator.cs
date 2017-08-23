﻿using Platform.Model.Battle;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Platform.View.Battle
{
    /// <summary>
    /// 聊天输入界面中介
    /// </summary>
    class DisloveStatisticsViewMediator : Mediator, IMediator
    {
        /// <summary>
        /// 倒计时定时器id
        /// </summary>
        private int timeId = 0;
        /// <summary>
        /// 牌局内数据
        /// </summary>
        private BattleProxy battleProxy;
        /// <summary>
        /// 游戏模块数据
        /// </summary>
        private GameMgrProxy gameMgrProxy;
        public DisloveStatisticsViewMediator(string mediatorName, object viewComponent) : base(mediatorName, viewComponent)
        {
        }

        public DisloveStatisticsView View
        {
            get { return (DisloveStatisticsView)ViewComponent; }
        }

        public override IList<string> ListNotificationInterests()
        {
            IList<string> list = new List<string>();
            list.Add(NotificationConstant.UPDATE_DISLOVE_STATISTICS);
            return list;
        }


        public override void OnRegister()
        {
            base.OnRegister();
            battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
            gameMgrProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.GAMEMGR_PROXY) as GameMgrProxy;
            //View.closeBtn.onClick.AddListener(CloseHandler);
            //View.falseBtn.onClick.AddListener(CloseHandler);
            //View.trueBtn.onClick.AddListener(CloseHandler);
            UpdateStatisticsInfo();
        }

        public override void OnRemove()
        {
            base.OnRemove();
        }

        public override void HandleNotification(INotification notification)
        {
            switch(notification.Name)
            {
 
                case NotificationConstant.UPDATE_DISLOVE_STATISTICS:
                    UpdateStatisticsInfo();
                    break;
            }
        }

        /// <summary>
        /// 更新申请情况
        /// </summary>
        private void UpdateStatisticsInfo()
        {
            View.nameTxtArr[0].text = battleProxy.playerIdInfoDic[battleProxy.disloveApplyUserId].name;
            for (int i = 0; i < GlobalData.SIT_NUM; i++)
            {
                if (battleProxy.playerSitInfoDic[i + 1].name == battleProxy.playerIdInfoDic[battleProxy.disloveApplyUserId].name)
                {
                    int h = 0;
                    for (int j = 1; j < GlobalData.SIT_NUM; j++)
                    {
                       
                        if (battleProxy.playerSitInfoDic[h + 1].userId == battleProxy.playerIdInfoDic[battleProxy.disloveApplyUserId].userId)
                        {
                            h++;
                        }
                        View.nameTxtArr[j].text = battleProxy.playerSitInfoDic[h + 1].name;

                        if (battleProxy.agreeIds.IndexOf(battleProxy.playerSitInfoDic[h + 1].userId) != -1)
                        {
                            View.statusTxtArr[j].text = "已同意";
                            View.statusTxtArr[j].color = new Color(255f / 255f, 244f / 255f, 92f / 255f);
                        }
                        else if (battleProxy.refuseIds.IndexOf(battleProxy.playerSitInfoDic[h + 1].userId) != -1)
                        {
                            View.statusTxtArr[j].text = "拒绝";
                            View.statusTxtArr[j].color = new Color(102f / 255f, 102f / 255f, 102f / 255f);
                        }
                        else
                        {
                            View.statusTxtArr[j].text = "未选择..";
                            View.statusTxtArr[j].color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                        }
                        h++;
                    }
                }
                
                
            }
            if (timeId == 0)
            {
                timeId = Timer.Instance.AddTimer(1, 0, 0, UpdateRemainTime);
            }
        }

        /// <summary>
        /// 更新剩余时间
        /// </summary>
        private void UpdateRemainTime()
        {
            long curRemainTime = battleProxy.disloveRemainTime * 1000 - (gameMgrProxy.systemTime - battleProxy.disloveRemainUT);
            curRemainTime = Math.Abs(curRemainTime/1000);
            View.remainTimeTxt.text = TimeHandle.Instance.ParseSecond((int)curRemainTime);
        }

        /// <summary>
        /// 关闭响应
        /// </summary>
        private void CloseHandler()
        {
            UIManager.Instance.HideUI(UIViewID.DISLOVE_STATISTICS_VIEW);
         

        }
    }
}