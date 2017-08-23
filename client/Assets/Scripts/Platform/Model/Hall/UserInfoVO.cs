using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using Platform.Model;
using UnityEngine;
public class UserInfoVO
{
    /// <summary>
    /// 微信制定的唯一id
    /// </summary>
    private int userID;
    /// <summary>
    /// 角色名称
    /// </summary>
    private string userName;
    /// <summary>
    /// 货币
    /// </summary>
    private Dictionary<int, UserItem> userItems;
    /// <summary>
    /// 当前积分
    /// </summary>
    private int score;
    ///<summary>
    /// 性别
    /// </summary>
    private int sex;
    /// <summary>
    /// 本机IP
    /// </summary>
    private string localIP;
    /// <summary>
    /// 玩家显示ID
    /// </summary>
    private string showID;
    /// <summary>
    /// 微信的头像url地址
    /// </summary>
    private string headIconUrl;
    /// <summary>
    /// 头像缓存地址
    /// </summary>
    private Texture headIcon;
    /// <summary>
    /// 是否绑定代理
    /// </summary>
    private int boundAgency;
    public int UserID
    {
        get
        {
            return userID;
        }

        set
        {
            userID = value;
        }
    }

    public string UserName
    {
        get
        {
            return userName;
        }

        set
        {
            userName = value;
        }
    }

    public Dictionary<int, UserItem> UserItems
    {
        get
        {
            return userItems;
        }

        set
        {
            userItems = value;
        }
    }

    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
        }
    }

    public int Sex
    {
        get
        {
            return sex;
        }

        set
        {
            sex = value;
        }
    }

    public string LocalIP
    {
        get
        {
            return localIP;
        }

        set
        {
            localIP = value;
        }
    }

    public string ShowID
    {
        get
        {
            return showID;
        }

        set
        {
            showID = value;
        }
    }

    public string HeadIconUrl
    {
        get
        {
            return headIconUrl;
        }

        set
        {
            headIconUrl = value;
        }
    }

    public Texture HeadIcon
    {
        get
        {
            return headIcon;
        }

        set
        {
            headIcon = value;
        }
    }

    public int BoundAgency
    {
        get
        {
            return boundAgency;
        }

        set
        {
            boundAgency = value;
        }
    }

    public UserInfoVO()
    {
        this.UserItems = new Dictionary<int, UserItem>();
    } 
}

