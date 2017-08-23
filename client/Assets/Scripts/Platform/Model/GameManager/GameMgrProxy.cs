using System;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;
/// <summary>
/// 管理模块数据
/// </summary>
public class GameMgrProxy : Proxy, IProxy
{
    /// <summary>
    /// 网络延时
    /// </summary>
    public int pingBackMS;
    /// <summary>
    /// 异步加载数据
    /// </summary>
    public AsyncOperation async;
    private long _systemTime;
    private long _systemDateUT;
    private float _scaleSystemDateUT;
    private DateTime _systemDateTime;

    public GameMgrProxy(string NAME) : base(NAME)
    {
    }

    /// <summary>
    /// 服务器当前时间,不受时间缩放影响
    /// </summary>
    public long systemTime
    {
        get
        {
            return _systemTime + TimeHandle.Instance.GetTimestamp() - _systemDateUT;
        }
        set
        {
            _systemDateUT = TimeHandle.Instance.GetTimestamp();
            _scaleSystemDateUT = Time.time * 1000;
            _systemTime = value;
        }
    }

    /// <summary>
    /// 服务器日期时间,不受时间缩放影响
    /// </summary>
    public DateTime systemDateTime
    {
        get
        {
            _systemDateTime = TimeHandle.Instance.GetDateTimeByTimestamp(systemTime);
            return _systemDateTime;
        }
    }

    /// <summary>
    /// 服务器当前时间,受时间缩放影响
    /// </summary>
    public long scaleSystemTime
    {
        get
        {
            return _systemTime + (long)(Time.time * 1000 - _scaleSystemDateUT);
        }
    }

    /// <summary>
    /// 修正缩放时间戳
    /// </summary>
    public void ReviseScaleSystemTime()
    {
        var curSystemTime = systemTime;
        _scaleSystemDateUT = _systemTime + Time.time * 1000 - curSystemTime;
    }
}
