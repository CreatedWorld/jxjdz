﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 加入房间中间View
/// </summary>
public class MiddleMenuView : UIView
{
    /// <summary>
    /// 排行榜按钮
    /// </summary>
    private Button rankingButton;
    /// <summary>
    /// 创建房间按钮
    /// </summary>
    private Button createRoomButton;
    /// <summary>
    /// 加入房间按钮
    /// </summary>
    private Button joinRoomButton;
    /// <summary>
    /// 公告图片
    /// </summary>
    private RawImage noticContent;
    /// <summary>
    /// 公告文本总控件
    /// </summary>
    private Transform notice;
    /// <summary>
    /// 公告标题
    /// </summary>
    private Text noticeTitle;
    /// <summary>
    /// 公告内容
    /// </summary>
    private Text noticeText;
    /// <summary>
    /// 跑马灯文本
    /// </summary>
    private Text announcementText;

    /// <summary>
    /// 排行榜按钮
    /// </summary>
    public Button RankingButton
    {
        get
        {
            return rankingButton;
        }
        set
        {
            rankingButton = value;
        }
    }
    /// <summary>
    /// 创建房间按钮
    /// </summary>
    public Button CreateRoomButton
    {
        get
        {
            return createRoomButton;
        }

        set
        {
            createRoomButton = value;
        }
    }
    /// <summary>
    /// 加入房间按钮
    /// </summary>
    public Button JoinRoomButton
    {
        get
        {
            return joinRoomButton;
        }

        set
        {
            joinRoomButton = value;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public RawImage NoticContent
    {
        get
        {
            return noticContent;
        }

        set
        {
            noticContent = value;
        }
    }

    public Text AnnouncementText
    {
        get
        {
            return announcementText;
        }

        set
        {
            announcementText = value;
        }
    }

    public Text NoticeTitle
    {
        get
        {
            return noticeTitle;
        }

        set
        {
            noticeTitle = value;
        }
    }

    public Text NoticeText
    {
        get
        {
            return noticeText;
        }

        set
        {
            noticeText = value;
        }
    }

    public Transform Notice
    {
        get
        {
            return notice;
        }

        set
        {
            notice = value;
        }
    }

    public override void OnInit()
    {
        this.ViewRoot = this.LaunchUIView("Prefab/UI/Hall/MiddleMenuView", UIManager.Instance.GetUIView(UIViewID.HALL_VIEW).ViewRoot.transform);
        this.RankingButton = this.ViewRoot.transform.FindChild("Ranking").FindChild("RankingButton").GetComponent<Button>();
        this.CreateRoomButton = this.ViewRoot.transform.FindChild("CreateRoom").FindChild("CreateRoomButton").GetComponent<Button>();
        this.JoinRoomButton = this.ViewRoot.transform.FindChild("JoinRoom").FindChild("JoinRoomButton").GetComponent<Button>();
        this.NoticContent = this.ViewRoot.transform.FindChild("Notice").FindChild("ContentImage").GetComponent<RawImage>();
        this.Notice = this.ViewRoot.transform.FindChild("Notice").FindChild("NoticeText");
        this.NoticeTitle = this.Notice.FindChild("Title").GetComponent<Text>();
        //this.NoticeText = this.Notice.FindChild("ContentText").GetComponent<Text>();
        //this.NoticeText = this.ViewRoot.transform.FindChild("Announcement/Mask/Text").GetComponent<Text>();
        this.AnnouncementText = this.ViewRoot.transform.FindChild("Announcement").FindChild("Mask").FindChild("Text").GetComponent<Text>();
    }
    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Hall/MiddleMenuView");
    }
}