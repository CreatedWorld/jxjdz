﻿using System.Collections.Generic;
using DG.Tweening;
using Platform.Model.Battle;
using UnityEngine;
using UnityEngine.UI;
using System;
using Platform.Model;
using Utils;

/// <summary>
///     牌局内转圈界面
/// </summary>
public class MasterView : MonoBehaviour
{
    /// <summary>
    /// 方向图标数组
    /// </summary>
    private List<GameObject> dirIconArr;
    /// <summary>
    /// 方向图标位置数组
    /// </summary>
    //private List<Vector3> dirIconPostionArr;
    /// <summary>
    /// 战斗模块数据
    /// </summary>
    private BattleProxy battleProxy;
    /// <summary>
    /// 游戏模块数据
    /// </summary>
    private GameMgrProxy gameMgrProxy;
    /// <summary>
    /// 玩家信息模块数据
    /// </summary>
    private PlayerInfoProxy playerInfoProxy;
    /// <summary>
    /// 旋转的定时器id
    /// </summary>
    private int rotateTimeId;
    /// <summary>
    /// 倒计时定时器id
    /// </summary>
    private int remainTimeId;

    /// <summary>
    /// 箭头标志
    /// </summary>
    //private Image arrowIcon;
    /// <summary>
    /// 庄家标志
    /// </summary>
    private Image masterIcon;

    /// <summary>
    /// 时间-分
    /// </summary>
    private Image Time_m;
    /// <summary>
    /// 时间-秒
    /// </summary>
    private Image Time_s;
    /// <summary>
    /// 东-高亮
    /// </summary>
    private Image EastHighLight;
    /// <summary>
    /// 南-高亮
    /// </summary>
    private Image SouthHighLight;
    /// <summary>
    /// 西-高亮
    /// </summary>
    private Image WestHighLight;
    /// <summary>
    /// 北-高亮
    /// </summary>
    private Image NorthHighLight;
    // Use this for initialization
    void Awake()
    {
        dirIconArr = new List<GameObject>();
        //dirIconPostionArr = new List<Vector3>();
        battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
        gameMgrProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.GAMEMGR_PROXY) as GameMgrProxy;
        playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;
        Time_m = transform.Find("Time_m").GetComponent<Image>();
        Time_s = transform.Find("Time_s").GetComponent<Image>();
      //  masterIcon = transform.Find("MasterIcon").GetComponent<Image>();
        var eastIcon = transform.Find("bg/EastIcon").gameObject;
        var southIcon = transform.Find("bg/SouthIcon").gameObject;
        var westIcon = transform.Find("bg/WestIcon").gameObject;
        var northIcon = transform.Find("bg/NorthIcon").gameObject;
        dirIconArr.Add(eastIcon);
        dirIconArr.Add(southIcon);
        dirIconArr.Add(westIcon);
        dirIconArr.Add(northIcon);
        //dirIconPostionArr.Add(eastIcon.transform.localPosition);
        //dirIconPostionArr.Add(southIcon.transform.localPosition);
        //dirIconPostionArr.Add(westIcon.transform.localPosition);
        //dirIconPostionArr.Add(northIcon.transform.localPosition);
        //arrowIcon = transform.Find("ArrowIcon").GetComponent<Image>();
        //arrowEffect = transform.Find("ArrowIcon/ArrowEffect").gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    /// <summary>
    ///     开始播放庄家旋转动画
    /// </summary>
    public void PlayRotate()
    {
        gameObject.GetComponent<Animator>().Play(MasterAnimationName.MasterRotate, 0, 0);
    }

    /// <summary>
    ///     抛出开始旋转事件
    /// </summary>
    public void DispatchStartRotate()
    {
        GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio("Voices/Effect/MasterRotate",0,true));
        rotateTimeId = Timer.Instance.AddTimer(0.08f, 0, 0.08f, UpdateRotateAngle);
    }

    /// <summary>
    /// 更新旋转角度
    /// </summary>
    private void UpdateRotateAngle()
    {
        //arrowIcon.rectTransform.localEulerAngles = new Vector3(0,0, arrowIcon.rectTransform.localEulerAngles.z + 90);
    }

    /// <summary>
    ///     抛出停止旋转事件
    /// </summary>
    public void DispatchEndRotate()
    {
        AudioSystem.Instance.StopEffectAudio("Voices/Effect/MasterRotate");
        Timer.Instance.CancelTimer(rotateTimeId);
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var bankerInfoVO = battleProxy.BankerPlayerInfoVOS2C;
        var sitIndex = (bankerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        //arrowIcon.rectTransform.localEulerAngles = new Vector3(0, 0, 180 + sitIndex * 90);
    }

    /// <summary>
    ///     抛出显示庄家图标事件
    /// </summary>
    public void DispatchMasterShow()
    {
        GameMgr.Instance.StartCoroutine(AudioSystem.Instance.PlayEffectAudio("Voices/Effect/MasterConfirm"));
        ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_BATTLEVIEW_SHOWBANKERICON);
    }

    /// <summary>
    ///     开始显示东南西北
    /// </summary>
    public void DispatchShowDir()
    {
        UpdateMasterInfo();
    }

    /// <summary>
    ///     旋转完成隐藏旋转
    /// </summary>
    public void DispatchRotateComplete()
    {
        ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_BATTLE_SENDCARD);
    }

    /// <summary>
    /// 更新东西南北信息
    /// </summary>
    public void UpdateMasterInfo()
    {
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        var masterPlayerInfoVO = battleProxy.MasterPlayerInfoVOS2C;
        var sitIndex = (masterPlayerInfoVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;//计算方向偏移量
        //for (int i = 0; i < dirIconArr.Count; i++)
        //{
        //    dirIconArr[i].GetComponent<RectTransform>().localPosition =
        //        dirIconPostionArr[(i + sitIndex) % GlobalData.SIT_NUM];
        //}
    }

    /// <summary>
    /// 显示玩家操作提示
    /// </summary>
    public void ShowPlayActTip()
    {
        gameObject.GetComponent<Animator>().Stop();
        var selfInfoVO = battleProxy.playerIdInfoDic[playerInfoProxy.UserInfo.UserID];
        PlayerInfoVOS2C tipPlayerVO = null;
        if (battleProxy.playerActTipS2C != null)
        {
            tipPlayerVO = battleProxy.playerIdInfoDic[battleProxy.playerActTipS2C.optUserId];
        }
        if (battleProxy.playerActS2C != null)
        {
            tipPlayerVO = battleProxy.playerIdInfoDic[battleProxy.playerActS2C.userId];
        }
        if (tipPlayerVO == null)
        {
            return;
        }
        var sitIndex = (tipPlayerVO.sit - selfInfoVO.sit + GlobalData.SIT_NUM) % GlobalData.SIT_NUM;
        //arrowIcon.rectTransform.localEulerAngles = new Vector3(0, 0, 180 + sitIndex * 90);
        //arrowIcon.color = Color.white;
        //TODO 四个方向setActive(true)
        masterIcon.color = new Color(1, 1, 1, 0);
        remainTimeId = Timer.Instance.AddTimer(1, 0, 1, UpdateTipRemain);
        //remainTimeTxt.gameObject.SetActive(true);
        UpdateTipRemain();
        //arrowEffect.SetActive(false);
    }

    /// <summary>
    /// 设置剩余时间
    /// </summary>
    private void UpdateTipRemain()
    {
        if (battleProxy.playerActTipS2C == null)
        {
            //remainTimeTxt.gameObject.SetActive(false);
            return;
        }
        float curRemainTime = 0;
        if (battleProxy.isReport)
        {
            curRemainTime = battleProxy.playerActTipS2C.tipRemainTime - (gameMgrProxy.scaleSystemTime - battleProxy.playerActTipS2C.tipRemainUT) / 1000;
            Debug.Log(curRemainTime);
        }
        else
        {
            curRemainTime = battleProxy.playerActTipS2C.tipRemainTime - (gameMgrProxy.systemTime - battleProxy.playerActTipS2C.tipRemainUT) / 1000;
            Debug.Log(curRemainTime);
        }
        
        curRemainTime = Mathf.Max(curRemainTime, 0);
        curRemainTime = Mathf.Ceil(curRemainTime);
        //remainTimeTxt.gameObject.SetActive(true);
        if (battleProxy.isStart)
        {
            //remainTimeTxt.text = curRemainTime.ToString();
        }
        else
        {
            //remainTimeTxt.gameObject.SetActive(false);
            Timer.Instance.CancelTimer(remainTimeId);
        }
    }
}

/// <summary>
///     动作名称
/// </summary>
internal class MasterAnimationName
{
    public const string MasterRotate = "MasterRotate";

    public const string Empty = "Empty";
}