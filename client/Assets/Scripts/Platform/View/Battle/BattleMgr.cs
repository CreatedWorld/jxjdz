﻿using Platform.Model.Battle;
using Platform.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 战斗区域
/// </summary>
public class BattleMgr : MonoBehaviour
{
    /// <summary>
    /// 战位区域数组
    /// </summary>
    [HideInInspector]
    public List<BattleAreaItem> battleAreaItems;  
    /// <summary>
    /// 底部战位
    /// </summary>
    [HideInInspector]
    public BattleAreaItem downArea;
    /// <summary>
    /// 右侧战位
    /// </summary>
    [HideInInspector]
    public BattleAreaItem rightArea;
    /// <summary>
    /// 顶部战位
    /// </summary>
    [HideInInspector]
    public BattleAreaItem upArea;
    /// <summary>
    /// 左侧战位
    /// </summary>
    [HideInInspector]
    public BattleAreaItem leftArea;
    /// <summary>
    /// 当前牌的箭头
    /// </summary>
    [HideInInspector]
    public GameObject cardArrowIcon;
    /// <summary>
    /// 录取器
    /// </summary>
    public RecorderSystem recorder;

    private BattleProxy battleProxy;


    void Awake()
    {

        battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
        battleAreaItems = new List<BattleAreaItem>();
	    downArea = transform.Find("DownArea").gameObject.GetComponent<BattleAreaItem>();
        rightArea = transform.Find("RightArea").gameObject.GetComponent<BattleAreaItem>();
        upArea = transform.Find("UpArea").gameObject.GetComponent<BattleAreaItem>();
        cardArrowIcon = transform.Find("CardArrowIcon").gameObject;
        leftArea = transform.Find("LeftArea").gameObject.GetComponent<BattleAreaItem>();
        cardArrowIcon.SetActive(false);
        battleAreaItems.Add(downArea);
        battleAreaItems.Add(rightArea);
        battleAreaItems.Add(upArea);
        battleAreaItems.Add(leftArea);

        for (int i = 0; i < battleAreaItems.Count; i++)
        {
            battleAreaItems[i].heapStartIndex = i * GlobalData.CardWare.Length / GlobalData.SIT_NUM;
            battleAreaItems[i].heapEndIndex = battleAreaItems[i].heapStartIndex + GlobalData.CardWare.Length / GlobalData.SIT_NUM - 1;
        }
        recorder = new RecorderSystem();
        
        UIManager.Instance.ShowUI(UIViewID.BATTLE_VIEW);
    }

    private void Start()
    {
        ApplicationFacade.Instance.RegisterMediator(new BattleAreaMediator(Mediators.BATTLE_AREA_MEDIATOR, this));
    }

    // Update is called once per frame
    void Update () {
        //录音播放
        if (battleProxy.speekPacket.Count > 0)
        {
            StartCoroutine(PlaySpeek(battleProxy.speekPacket.Dequeue()));
        }
        recorder.Update();
    }

    //播放语音
    public IEnumerator PlaySpeek(AudioPacket packet)
    {
        ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_BATTLEVIEW_SHOWPLAYINGVOICE, packet.LocalId);
        StartCoroutine(AudioSystem.Instance.PlayEffectAudio(packet.Clip));
        yield return new WaitForSeconds(packet.Clip.length);
        ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_BATTLEVIEW_HIDENPLAYINGVOICE, packet.LocalId);
    }

    private void OnDestroy()
    {
        ApplicationFacade.Instance.RemoveMediator(Mediators.BATTLE_AREA_MEDIATOR);
        (ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy).Clear();
        //UIManager.Instance.InitUI();
    }

    /// <summary>
    /// 离开游戏提示重连
    /// </summary>
    /// <param name="pause"></param>
    private void OnApplicationPause(bool pause)
    {
        if (battleProxy.isStart && !battleProxy.isReport)
        {
            NetMgr.Instance.OnDisconnect();
        }
    }
}