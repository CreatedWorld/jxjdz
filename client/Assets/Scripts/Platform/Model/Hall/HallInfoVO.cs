﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;
using Platform.Model;

public class HallInfoVO
{
    /// <summary>
    /// 排行榜玩家信息集合
    /// </summary>
    public List<UserInfo> userInfo;
    /// <summary>
    /// 开房间选择的局数
    /// </summary>
    private int innings;
    /// <summary>
    /// 开房间封顶的分数
    /// </summary>
    private int capScore;
    /// <summary>
    /// 房间号
    /// </summary>
    private string roomId;
    /// <summary>
    /// 座位
    /// </summary>
    private int seat;
    /// <summary>
    /// 七天登陆签到天数
    /// </summary>
    private int signInDay;
    /// <summary>
    /// 七天登陆的领取状态
    /// </summary>
    private int signInState;
    /// <summary>
    /// 游戏规则
    /// </summary>
    private GameRule gameRule;
    /// <summary>
    /// 游戏服务器IP
    /// </summary>
    private string battleSeverIP;
    /// <summary>
    /// 游戏服务器端口
    /// </summary>
    private int battleSeverPort;
    /// <summary>
    /// 公告
    /// </summary>
    private Queue<AnnouncementData> announcementQueue;
    /// <summary>
    /// 当前系统时间
    /// </summary>
    private long currentTime;
    /// <summary>
    /// 比赛开始时间
    /// </summary>
    private long startTime;
    /// <summary>
    /// 比赛结束时间
    /// </summary>
    private long endTime;
    /// <summary>
    /// 消息
    /// </summary>
    private Dictionary<HallNoticeType, NoticeConfigDataS2C> noticeList;
    /// <summary>
    /// 比赛场规则
    /// </summary>
    private GameRule competitionRule;
    /// <summary>
    /// 比赛场回合数
    /// </summary>
    private GameMode competitionRound;
    /// <summary>
    /// 支付方式
    /// </summary>
    private int payType;

    public int PayType
    {
        get
        {
            return payType;
        }
        set
        {
            payType = value;
        }
    }
    public int Innings
    {
        get
        {
            return innings;
        }

        set
        {
            innings = value;
        }
    }

    /// <summary>
    /// 房间封顶的分数
    /// </summary>
    public int CapScore
    {
        get
        {
            return capScore;
        }
        set
        {
            capScore = value;
        }
    }

    public string RoomCode
    {
        get
        {
            return roomId;
        }

        set
        {
            roomId = value;
        }
    }

    public int Seat
    {
        get
        {
            return seat;
        }

        set
        {
            seat = value;
        }
    }
    public int SignInDay
    {
        get
        {
            return signInDay;
        }

        set
        {
            signInDay = value;
        }
    }

    public int SignInState
    {
        get
        {
            return signInState;
        }

        set
        {
            signInState = value;
        }
    }

    public GameRule GameRule
    {
        get
        {
            return gameRule;
        }

        set
        {
            gameRule = value;
        }
    }

    public string BattleSeverIP
    {
        get
        {
            return battleSeverIP;
        }

        set
        {
            battleSeverIP = value;
        }
    }

    public int BattleSeverPort
    {
        get
        {
            return battleSeverPort;
        }

        set
        {
            battleSeverPort = value;
        }
    }
    public Queue<AnnouncementData> AnnouncementQueue
    {
        get
        {
            if (announcementQueue == null)
            {
                announcementQueue = new Queue<AnnouncementData>();
            }
            return announcementQueue;
        }

        set
        {
            announcementQueue = value;
        }
    }

    public long CurrentTime
    {
        get
        {
            return currentTime;
        }

        set
        {
            currentTime = value;
        }
    }

    public long StartTime
    {
        get
        {
            return startTime;
        }

        set
        {
            startTime = value;
        }
    }

    public long EndTime
    {
        get
        {
            return endTime;
        }

        set
        {
            endTime = value;
        }
    }

    public Dictionary<HallNoticeType, NoticeConfigDataS2C> NoticeList
    {
        get
        {
            if (noticeList == null)
            {
                noticeList = new Dictionary<HallNoticeType, NoticeConfigDataS2C>();
            }
            return noticeList;
        }

        set
        {
            noticeList = value;
        }
    }

    public GameRule CompetitionRule
    {
        get
        {
            return competitionRule;
        }

        set
        {
            competitionRule = value;
        }
    }

    public GameMode CompetitionRound
    {
        get
        {
            return competitionRound;
        }

        set
        {
            competitionRound = value;
        }
    }
}