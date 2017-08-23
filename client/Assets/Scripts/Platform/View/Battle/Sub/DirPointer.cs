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
public class DirPointer : MonoBehaviour
{
    /// <summary>
    /// 战斗模块数据
    /// </summary>
    private BattleProxy battleProxy;
    /// <summary>
    /// 游戏模块数据
    /// </summary>
    //private GameMgrProxy gameMgrProxy;
    /// <summary>
    /// 玩家信息模块数据
    /// </summary>
    private PlayerInfoProxy playerInfoProxy;
    /// <summary>
    /// 旋转的定时器id
    /// </summary>
    //private int rotateTimeId;
    /// <summary>
    /// 倒计时定时器id
    /// </summary>
    //private int remainTimeId;

    /// <summary>
    /// <summary>
    /// 时间-分
    /// </summary>
    public SpriteRenderer time_m;
    /// <summary>
    /// 时间-秒
    /// </summary>
    public SpriteRenderer time_s;
    /// <summary>
    /// 东-高亮
    /// </summary>
    public SpriteRenderer eastHighLight;
    /// <summary>
    /// 南-高亮
    /// </summary>
    public SpriteRenderer southHighLight;
    /// <summary>
    /// 西-高亮
    /// </summary>
    public SpriteRenderer westHighLight;
    /// <summary>
    /// 北-高亮
    /// </summary>
    public SpriteRenderer northHighLight;
    /// <summary>
    /// 东南西北的根
    /// </summary>
    public Transform dirRoot;
    /// <summary>
    /// 游戏模块数据
    /// </summary>
    private GameMgrProxy gameMgrProxy;
    /// <summary>
    /// 总时间背景
    /// </summary>
    public GameObject allTimeBg;

    private SpriteRenderer highlightSprite;

    private float eastSpritAlpha = 0;
    private float southSpritAlpha = 0;
    private float westSpritAlpha = 0;
    private float northSpritAlpha = 0;
    private Color highLightColor = new Color(1, 1, 1, 1);
    private Color gooutColor = new Color(1, 1, 1, 0);

    private Vector3 eastVec = new Vector3(0, 0, 0);
    private Vector3 southVec = new Vector3(0, 0, 90);
    private Vector3 westVec = new Vector3(0, 0, 180);
    private Vector3 northVec = new Vector3(0, 0, -90);

    private List<Sprite> ssSprite = new List<Sprite>();
    void Awake()
    {
        gameMgrProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.GAMEMGR_PROXY) as GameMgrProxy;
        battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
        playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;

        for (int i = 0; i < 10; i++)
        {
            ssSprite.Add(Resources.Load<Sprite>("Textures/saizi/"+i));
        }
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (GlobalData.sit.Equals(1))
        {
            dirRoot.transform.localEulerAngles = eastVec;
        }
        if (GlobalData.sit.Equals(4))
        {
            dirRoot.transform.localEulerAngles = southVec;
        }
        if (GlobalData.sit.Equals(3))
        {
            dirRoot.transform.localEulerAngles = westVec;
        }
        if (GlobalData.sit.Equals(2))
        {
            dirRoot.transform.localEulerAngles = northVec;
        }
        if (GlobalData.optUserId.Equals(1))
        {
            CurrentSitHighLight("eastHighLight");
            PlayTime();
        }
        if (GlobalData.optUserId.Equals(2))
        {
            CurrentSitHighLight("northHighLight");
            PlayTime();
        }
        if (GlobalData.optUserId.Equals(3))
        {
            CurrentSitHighLight("westHighLight");
            PlayTime();
        }
        if (GlobalData.optUserId.Equals(4))
        {
            CurrentSitHighLight("southHighLight");
            PlayTime();
        }



    }
    void PlayTime()
    {
        float curRemainTime = 0;
        if (battleProxy.playerActTipS2C != null)
        {
            curRemainTime = battleProxy.playerActTipS2C.tipRemainTime - (gameMgrProxy.scaleSystemTime - battleProxy.playerActTipS2C.tipRemainUT) / 1000;
        }

        if (curRemainTime < 0)
        {
            time_m.sprite = ssSprite[0];
            time_s.sprite = ssSprite[0];
        }
        else
        {
            time_m.sprite = ssSprite[(int)((curRemainTime / 10) % 10)];
            time_s.sprite = ssSprite[(int)((curRemainTime / 1) % 10)]; 
        }
    }

    /// <summary>
    /// 显示当前哪个玩家高亮
    /// </summary>
    /// <param name="s"></param>
    private void CurrentSitHighLight(string s)
    {
        switch (s)
        {
            case "eastHighLight":
                if (eastHighLight.color != highLightColor)
                {
                    
                    eastHighLight.color = highLightColor;
                    southHighLight.color = gooutColor;
                    westHighLight.color = gooutColor;
                    northHighLight.color = gooutColor;
                }

                break;
            case "southHighLight":
                if (southHighLight.color != highLightColor)
                {
                    southHighLight.color = highLightColor;
                    eastHighLight.color = gooutColor;
                    westHighLight.color = gooutColor;
                    northHighLight.color = gooutColor;
                }

                break;
            case "westHighLight":
                if (westHighLight.color != highLightColor)
                {
                    westHighLight.color = highLightColor;
                    southHighLight.color = gooutColor;
                    eastHighLight.color = gooutColor;
                    northHighLight.color = gooutColor;
                }

                break;
            case "northHighLight":
                if (northHighLight.color != highLightColor)
                {
                    northHighLight.color = highLightColor;
                    eastHighLight.color = gooutColor;
                    westHighLight.color = gooutColor;
                    southHighLight.color = gooutColor;
                }

                break;

        }

    }

    private void Twinkle()
    {

    }

}

