using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;
using Platform.Net;
using Platform.Model;
using UnityEngine.SceneManagement;
public class PlayerInfoProxy : Proxy, IProxy
{
    public PlayerInfoProxy(string NAME) : base(NAME,new UserInfoVO())
    {
    }

    public UserInfoVO UserInfo
    {
        get
        {
            return (UserInfoVO)base.Data;
        }
    }
    public override void OnRegister()
    {
        GameMgr.Instance.AddMsgHandler(MsgNoS2C.RESPONSE_GETUSERINFO_S2C, GetUserInfoResponse);
        GameMgr.Instance.AddMsgHandler(MsgNoS2C.RESPONSE_REFRESHUSERITEM_S2C, RefreshUserItem);
    }
    /// <summary>
    /// 更新用户数据
    /// </summary>
    /// <param name="bytes"></param>
    private void GetUserInfoResponse(byte[] bytes)
    {
        GetUserInfoS2C package = NetMgr.Instance.DeSerializes<GetUserInfoS2C>(bytes);
        this.UserInfo.UserName = package.userName;
        foreach(UserItem item in package.userItems)
        {
            if (!this.UserInfo.UserItems.ContainsKey(item.type))
            {
                this.UserInfo.UserItems.Add(item.type, item);
            }
            else
            {
                this.UserInfo.UserItems[item.type] = item;
            }
        }
        this.UserInfo.ShowID = package.showId;
        //string ip = package.ip;
        //ip = ip.Substring(0, ip.Length - 5);
        //ip = ip.Replace("/", "");
        this.UserInfo.LocalIP = package.ip;
        this.UserInfo.HeadIconUrl = package.imageUrl;
        this.UserInfo.Sex = package.sex;
        this.UserInfo.BoundAgency = package.boundAgency;
    }

    private void RefreshUserItem(byte[] bytes)
    {
        PushUserItem package = NetMgr.Instance.DeSerializes<PushUserItem>(bytes);
        foreach (UserItem item in package.userItems)
        {
            if (!this.UserInfo.UserItems.ContainsKey(item.type))
            {
                this.UserInfo.UserItems.Add(item.type, item);
            }
            else
            {
                this.UserInfo.UserItems[item.type] = item;
            }
        }
        ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_HALL_REFRESHITEM);
    }
}