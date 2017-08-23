using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCapScoreInfo
{
    public GameCapScoreInfo(Toggle toggle, CapScore cap)
    {
        Toggle = toggle;
        Cap = cap;
    }
    /// <summary>
    /// 游戏类型
    /// </summary>
    public CapScore Cap { get; private set; }
    /// <summary>
    /// 复选框控件
    /// </summary>
    public Toggle Toggle { get; private set; }
}
