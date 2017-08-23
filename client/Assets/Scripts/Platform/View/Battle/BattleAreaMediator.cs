using Platform.Model.Battle;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections;
using System.Collections.Generic;
using Platform.Model;
using UnityEngine;
using Platform.Net;
using System;
using Platform.Utils;
using DG.Tweening;

/// <summary>
/// 战斗区域中介
/// </summary>
public class BattleAreaMediator : Mediator, IMediator
{
    /// <summary>
    /// 游戏数据中介
    /// </summary>
    private GameMgrProxy gameMgrProxy;
    /// <summary>
    /// 用户信息数据中介
    /// </summary>
    private PlayerInfoProxy playerInfoProxy;
    /// <summary>
    /// 战斗模块数据中介
    /// </summary>
    private BattleProxy battleProxy; 

    public BattleAreaMediator(string mediatorName, object viewComponent) : base(mediatorName, viewComponent)
    {

    }

    public BattleMgr View
    {
        get
        {
            return (BattleMgr)ViewComponent;
        }
    }

    public override IList<string> ListNotificationInterests()
    {
        IList<string> list = new List<string>();
        list.Add(NotificationConstant.MEDI_BATTLE_SENDCARD);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAY_COMMONANGANG);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAY_BACKANGANG);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAYGETCARD);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAYPASS);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAYPENG);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAYCHI);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAY_COMMONPENGGANG);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAY_BACKPENGGANG);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAYPUTCARD);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAYZHIGANG);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAYHU);
        list.Add(NotificationConstant.MEDI_BATTLEVIEW_SHOWMATCHRESULT);
        list.Add(NotificationConstant.MEDI_BATTLEREA_STARTRECORD);
        list.Add(NotificationConstant.MEDI_BATTLEREA_STOPRECORD);
        list.Add(NotificationConstant.MEDI_BATTLE_PLAYPUTFLOWERCARD);
        list.Add(NotificationConstant.SHOW_CARD_ARROW);
        return list;
    }


    public override void OnRegister()
    {
        base.OnRegister();
        battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
        gameMgrProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.GAMEMGR_PROXY) as GameMgrProxy;
        playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;

        View.recorder.OnReComplete = SendAudioToServer;
        View.recorder.OnGetComplete = PlayAudio;
        AudioSystem.Instance.PlayBgm(Resources.Load<AudioClip>("Voices/Bgm/BattleBgm"));

        InitView();
    }

    public override void OnRemove()
    {
        base.OnRemove();
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case NotificationConstant.MEDI_BATTLE_SENDCARD:
                GameMgr.Instance.StartCoroutine(PlaySendCardAnimator());
                break;
            case NotificationConstant.MEDI_BATTLE_PLAY_COMMONANGANG:
                PlayCommonAnGang();
                break;
            case NotificationConstant.MEDI_BATTLE_PLAY_BACKANGANG:
                PlayBackAnGang();
                break;
            case NotificationConstant.MEDI_BATTLE_PLAYGETCARD:
                PlayGetCard();
                break;
            case NotificationConstant.MEDI_BATTLE_PLAYPASS:
                PlayPass();
                break;
            case NotificationConstant.MEDI_BATTLE_PLAYPENG:
                PlayPeng();
                break;
            case NotificationConstant.MEDI_BATTLE_PLAYCHI:
                PlayChi();
                break;
            case NotificationConstant.MEDI_BATTLE_PLAY_COMMONPENGGANG:
                PlayCommonPengGang();
                break;
            case NotificationConstant.MEDI_BATTLE_PLAY_BACKPENGGANG:
                PlayBackPengGang();
                break;
            case NotificationConstant.MEDI_BATTLE_PLAYPUTCARD:
                PlayPutCard();
                break;
            case NotificationConstant.MEDI_BATTLE_PLAYPUTFLOWERCARD:
                PlayPutFlowerCard();
                break;
            case NotificationConstant.MEDI_BATTLE_PLAYZHIGANG:
                PlayZhiGang();
                break;
            case NotificationConstant.MEDI_BATTLE_PLAYHU:
                PlayHu((bool)notification.Body);
                break;
            case NotificationConstant.MEDI_BATTLEVIEW_SHOWMATCHRESULT:
                SaveAllCard();
                View.cardArrowIcon.SetActive(false);
                break;
            case NotificationConstant.MEDI_BATTLEREA_STARTRECORD:
                View.recorder.Recording();
                break;
            case NotificationConstant.MEDI_BATTLEREA_STOPRECORD:
                View.recorder.StopRecording();
                break;
            case NotificationConstant.READY_NEXT:
                foreach (BattleAreaItem areaItem in View.battleAreaItems)
                {
                    areaItem.SaveAllCard();
                }
                View.cardArrowIcon.SetActive(false);
                ResourcesMgr.Instance.RecoveryAll();
                break;
            case NotificationConstant.SHOW_CARD_ARROW:
                UpdateCardArrow(notification.Body as BattleAreaItem);
                break;

        }
    }

    /// <summary>
    /// 初始化界面显示
    /// </summary>
    private void InitView()
    {
        if (battleProxy.isStart)
        {
            var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
            for (int i = 0; i < View.battleAreaItems.Count; i++)
            {
                View.battleAreaItems[i].data = battleProxy.playerSitInfoDic[GlobalData.GetNextSit(selfInfoVO.sit, i)];
            }
            battleProxy.PutFlowerCard();
            //battleProxy.PutFlowerCardHandler();
        }
    }

    /// <summary>
    /// 播放发牌动画
    /// </summary>
    private IEnumerator PlaySendCardAnimator()
    {
        //发牌之前先回收之前的牌
        foreach (BattleAreaItem areaItem in View.battleAreaItems)
        {
            areaItem.SaveAllCard();
        }
        View.cardArrowIcon.SetActive(false);
        if (GlobalData.hasHeap)
        {
            foreach (BattleAreaItem areaItem in View.battleAreaItems)
            {
                BattleAreaUtil.InitHeapCard(areaItem, GlobalData.CardWare.Length);
            }
        }
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var sendStartIndex = (battleProxy.BankerPlayerInfoVOS2C.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        for (int i = 0; i < View.battleAreaItems.Count; i++)
        {
            View.battleAreaItems[i].SetData(battleProxy.playerSitInfoDic[GlobalData.GetNextSit(selfInfoVO.sit, i)]);
        }
        //if (GlobalData.LoginServer != "127.0.0.1")
        //{
            int sendCount = Mathf.CeilToInt((float)GlobalData.PLAYER_CARD_NUM / GlobalData.SEND_SINGLE) * GlobalData.SIT_NUM;
            for (int i = 0; i < sendCount; i++)
            {
                GameMgr.Instance.StartCoroutine(View.battleAreaItems[(i + sendStartIndex) % GlobalData.SIT_NUM].PlaySendCardAnimator());
                yield return new WaitForSeconds(0.4f);
            }
            yield return new WaitForSeconds(0.5f);
        SendNotification(NotificationConstant.MEDI_BATTLE_UPDATEOOMMESSAGES);
            for (int i = 0; i < View.battleAreaItems.Count; i++)
            {
                GameMgr.Instance.StartCoroutine(View.battleAreaItems[i].PlayCloseCardAction());
            } 
        //}
        yield return new WaitForSeconds(0.83f);
        battleProxy.isForbit = false;

        SendNotification(NotificationConstant.MEDI_BATTLE_PLAYACTTIP);
    }

    /// <summary>
    /// 播放直接暗杠动画
    /// </summary>
    private void PlayCommonAnGang()
    {
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var actPlayerInfoVO = battleProxy.playerIdInfoDic[battleProxy.playerActS2C.userId];
        var actIndex = (actPlayerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        View.battleAreaItems[actIndex].PlayCommonAnGang();
        GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio(string.Format("Voices/{0}/Card/AnGang", actPlayerInfoVO.sex == 1 ? "Man" : "Woman")));
        View.cardArrowIcon.SetActive(false);
    }

    /// <summary>
    /// 播放回头暗杠动画
    /// </summary>
    private void PlayBackAnGang()
    {
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var actPlayerInfoVO = battleProxy.playerIdInfoDic[battleProxy.playerActS2C.userId];
        var actIndex = (actPlayerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        View.battleAreaItems[actIndex].PlayBackAnGang();
        GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio(string.Format("Voices/{0}/Card/AnGang", actPlayerInfoVO.sex == 1 ? "Man" : "Woman")));
        View.cardArrowIcon.SetActive(false);
    }

    /// <summary>
    /// 播放摸牌动作
    /// </summary>
    private void PlayGetCard()
    {
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var actPlayerInfoVO = battleProxy.playerIdInfoDic[battleProxy.playerActS2C.userId];
        var actIndex = (actPlayerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        GameMgr.Instance.StartCoroutine(View.battleAreaItems[actIndex].PlayGetCard());
        GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio("Voices/Effect/GetCard"));
    }

    /// <summary>
    /// 播放过动作
    /// </summary>
    private void PlayPass()
    {
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var actPlayerInfoVO = battleProxy.playerIdInfoDic[battleProxy.playerActS2C.userId];
        var actIndex = (actPlayerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        View.battleAreaItems[actIndex].PlayPass();
    }

    /// <summary>
    /// 播放碰牌动作
    /// </summary>
    private void PlayPeng()
    {
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var actPlayerInfoVO = battleProxy.playerIdInfoDic[battleProxy.playerActS2C.userId];
        var actIndex = (actPlayerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        var targetPlayerInfoVO = battleProxy.playerIdInfoDic[battleProxy.playerActS2C.targetUserId];
        var targetActIndex = (targetPlayerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        var pengedCard = View.battleAreaItems[targetActIndex].PlayPenged();
        View.battleAreaItems[actIndex].PlayPeng(pengedCard);
        GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio(string.Format("Voices/{0}/Card/Peng", actPlayerInfoVO.sex == 1 ? "Man" : "Woman")));
        View.cardArrowIcon.SetActive(false);
    }

    /// <summary>
    /// 播放吃牌动作
    /// </summary>
    private void PlayChi()
    {
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var actPlayerInfoVO = battleProxy.playerIdInfoDic[battleProxy.playerActS2C.userId];
        var actIndex = (actPlayerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        var targetPlayerInfoVO = battleProxy.playerIdInfoDic[battleProxy.playerActS2C.targetUserId];
        var targetActIndex = (targetPlayerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        var chiedCard = View.battleAreaItems[targetActIndex].PlayChied();
        View.battleAreaItems[actIndex].PlayChi(chiedCard);
        GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio(string.Format("Voices/{0}/Card/Chi", actPlayerInfoVO.sex == 1 ? "Man" : "Woman")));
        View.cardArrowIcon.SetActive(false);
    }

    /// <summary>
    /// 播放直接碰杠动作
    /// </summary>
    private void PlayCommonPengGang()
    {
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var actPlayerInfoVO = battleProxy.playerIdInfoDic[battleProxy.playerActS2C.userId];
        var actIndex = (actPlayerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        View.battleAreaItems[actIndex].PlayCommonPengGang();
        GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio(string.Format("Voices/{0}/Card/Gang", actPlayerInfoVO.sex == 1 ? "Man" : "Woman")));
        View.cardArrowIcon.SetActive(false);
    }

    /// <summary>
    /// 播放回头碰杠动作
    /// </summary>
    private void PlayBackPengGang()
    {
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var actPlayerInfoVO = battleProxy.playerIdInfoDic[battleProxy.playerActS2C.userId];
        var actIndex = (actPlayerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        View.battleAreaItems[actIndex].PlayBackPengGang();
        GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio(string.Format("Voices/{0}/Card/Gang", actPlayerInfoVO.sex == 1 ? "Man" : "Woman")));
        View.cardArrowIcon.SetActive(false);
    }

    /// <summary>
    /// 播放出牌动作
    /// </summary>
    private void PlayPutCard()
    {
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var actPlayerInfoVO = battleProxy.playerIdInfoDic[battleProxy.playerActS2C.userId];
        var actIndex = (actPlayerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        GameMgr.Instance.StartCoroutine(View.battleAreaItems[actIndex].PlayPutCard());
    }

    /// <summary>
    /// 播放出花牌动作
    /// </summary>
    private void PlayPutFlowerCard()
    {
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var actPlayerInfoVO = battleProxy.playerIdInfoDic[battleProxy.playerActS2C.userId];
        var actIndex = (actPlayerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        GameMgr.Instance.StartCoroutine(View.battleAreaItems[actIndex].PlayPutFlowerCard());  
    }

    /// <summary>
    /// 播放直杠动作
    /// </summary>
    private void PlayZhiGang()
    {
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var actPlayerInfoVO = battleProxy.playerIdInfoDic[battleProxy.playerActS2C.userId];
        var actIndex = (actPlayerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        var targetPlayerInfoVO = battleProxy.playerIdInfoDic[battleProxy.playerActS2C.targetUserId];
        var targetActIndex = (targetPlayerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        var gangedCard = View.battleAreaItems[targetActIndex].PlayZhiGanged();
        View.battleAreaItems[actIndex].PlayZhiGang(gangedCard);
        GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio(string.Format("Voices/{0}/Card/Gang", actPlayerInfoVO.sex == 1 ? "Man" : "Woman")));
        View.cardArrowIcon.SetActive(false);
    }

    /// <summary>
    /// 播放胡牌动作
    /// </summary>
    private void PlayHu(bool isSelf)
    {
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var actPlayerInfoVO = battleProxy.playerIdInfoDic[battleProxy.playerActS2C.userId];
        var actIndex = (actPlayerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        if (isSelf)
        {
            View.battleAreaItems[actIndex].PlaySelfHu();
            GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio(string.Format("Voices/{0}/Card/SelfHu", actPlayerInfoVO.sex == 1 ? "Man" : "Woman")));
        }
        else
        {
            var targetPlayerInfoVO = battleProxy.playerIdInfoDic[battleProxy.playerActS2C.targetUserId];
            var targetActIndex = (targetPlayerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
            var huedCard = View.battleAreaItems[targetActIndex].PlayHued();
            View.battleAreaItems[actIndex].PlayHu(huedCard);
            GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio(string.Format("Voices/{0}/Card/Hu", actPlayerInfoVO.sex == 1 ? "Man" : "Woman")));
        }
        View.cardArrowIcon.SetActive(false);
    }

    /// <summary>
    /// 更新已出牌的箭头
    /// </summary>
    /// <param name="areaItem"></param>
    private void UpdateCardArrow(BattleAreaItem areaItem)
    {
        if (areaItem.putCards.Count == 0)
        {
            return;
        }
        View.cardArrowIcon.SetActive(true);
        View.cardArrowIcon.transform.parent = areaItem.putCardContainer.transform;
        View.cardArrowIcon.transform.localPosition = new Vector3(areaItem.putCards[areaItem.putCards.Count - 1].transform.localPosition.x, areaItem.putCards[areaItem.putCards.Count - 1].transform.localPosition.y- 0.4f, areaItem.putCards[areaItem.putCards.Count - 1].transform.localPosition.z);
        if (areaItem.dir == AreaDir.DOWN)
        {
            View.cardArrowIcon.transform.localEulerAngles = new Vector3(90, 0f, 0);
        }
        else if (areaItem.dir == AreaDir.LEFT)
        {
            View.cardArrowIcon.transform.localEulerAngles = new Vector3(120, 90, 0);
        }
        else if (areaItem.dir == AreaDir.RIGHT)
        {
            View.cardArrowIcon.transform.localEulerAngles = new Vector3(120, 270, 0);
        }
        else if (areaItem.dir == AreaDir.UP)
        {
            View.cardArrowIcon.transform.localEulerAngles = new Vector3(118, 0, 0);
        }
        View.cardArrowIcon.transform.localScale = Vector3.one;
        View.cardArrowIcon.transform.DOKill();
        View.cardArrowIcon.transform.DOLocalMoveY(-0.8f, 1).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// 显示单局结算,隐藏回收桌面牌资源
    /// </summary>
    private void SaveAllCard()
    {
        foreach (BattleAreaItem areaItem in View.battleAreaItems)
        {
            areaItem.SaveAllCard();
        }
        View.cardArrowIcon.SetActive(false);
        //打开本局结算界面
        if (!battleProxy.isPlayHu)
        {
            UIManager.Instance.ShowUI(UIViewID.MATCH_RESULT_VIEW);
        }

    }

    /// <summary>
    /// 录音完成回调
    /// </summary>
    /// <param name="flag"></param>
    /// <param name="data"></param>
    private void SendAudioToServer(int flag, byte[] data)
    {
        if (flag == 1)
        {
            SendNotification(NotificationConstant.MEDI_BATTLEVIEW_HIDENRECORDING);
        }
        var sendVoiceC2S = new SendVoiceC2S();
        sendVoiceC2S.flag = flag;
        sendVoiceC2S.content = data;
        NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.SEND_VOICE_C2S.GetHashCode(), 0, sendVoiceC2S);
        battleProxy.perSendChatTime = gameMgrProxy.systemTime;
    }

    /// <summary>
    /// 播放声音
    /// </summary>
    /// <param name="obj"></param>
    private void PlayAudio(AudioPacket packet)
    {
        battleProxy.speekPacket.Enqueue(packet);
    }
    /// <summary>
    /// 回退重置单个玩家牌面
    /// </summary>
    private void BackPlayerCard()
    {
        foreach (BattleAreaItem areaItem in View.battleAreaItems)
        {
            areaItem.SaveAllCard();
        }
        View.cardArrowIcon.SetActive(false);
        InitView();
    }
    /// <summary>
    /// 获取当前摸到的牌堆的牌
    /// </summary>
    /// <returns></returns>
    public GameObject GetHeapCard(int cardValue)
    {
        int heapCardIndex = battleProxy.unGetHeapCardIndexs[0];
        battleProxy.unGetHeapCardIndexs.RemoveAt(0);
        foreach (BattleAreaItem areaItem in View.battleAreaItems)
        {
            if (heapCardIndex >= areaItem.heapStartIndex && heapCardIndex <= areaItem.heapEndIndex)
            {
                //Debug.Log(string.Format("方向:{0} 牌堆起始序号：{1} 牌堆结束序号:{2} 拿牌序号:{3}",areaItem.dir.ToString(),areaItem.heapStartIndex,areaItem.heapEndIndex,heapCardIndex));
                return areaItem.GetHeapCard(cardValue);
            }

        }



        return null;
    }
}
