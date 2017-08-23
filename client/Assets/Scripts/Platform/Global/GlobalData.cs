using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全局常亮
/// </summary>
public static class GlobalData
{
    public static SDKPlatform sdkPlatform = SDKPlatform.LOCAL;
    /// <summary>
    /// 登录服务器
    /// </summary>
    public static string LoginServer = "119.23.124.253";
    /// <summary>
    /// 是否调试模式
    /// </summary>
    public static bool isDebugModel = false;
    /// <summary>
    /// 登录端口
    /// </summary>
    public static int LoginPort = 8129;//9009
    /// <summary>
    /// 登录用户名
    /// </summary>
    public static string UserName = "Username";
    /// <summary>
    /// 登录密码
    /// </summary>
    public static string UserPwd = "Password";  
    /// <summary>
    /// 角色id
    /// </summary>
    public static string UserMac = "PhoneMac";
    /// <summary>
    /// 头像Url
    /// </summary>
    public static string ImageUrl = "http://img.52fuqing.com/upload/news/20170303/20170303174534471.jpg";
    /// <summary>
    /// 版本号
    /// </summary>
    public static string VERSIONS = "8.11.1.0";//TimeHandle.Instance.GetDateTimeByTimestamp(TimeHandle.Instance.GetTimestamp()).ToString("yyyy-MM-dd HH:mm:ss");

    /// <summary>
    /// 是否有牌堆
    /// </summary>
    public static bool hasHeap = true;
    /// <summary>
    /// 牌局内玩家数量
    /// </summary>
    public static int SIT_NUM = 4;
    /// <summary>
    /// 座位号数组
    /// </summary>
    public static int[] NextSits = {1,2,3,4};
    /// <summary>
    /// 是否是第一局
    /// </summary>
    public static bool isFirst=false;
    /// <summary>
    /// 当前轮到哪个玩家出牌的id
    /// </summary>
    public static int optUserId = -1;

    /// <summary>
    /// 座位号
    /// </summary>
    public static int sit = 1;

    public static bool playGameBool = false;
    /// <summary>
    /// 牌型 1-万 2-同 3-条 4-东南西北 5-中发白
    /// </summary>
    public static readonly int[] CardValues =
    {
        11, 12, 13, 14, 15, 16, 17, 18, 19, //万子
        21, 22, 23, 24, 25, 26, 27, 28, 29, //同子
        31, 32, 33, 34, 35, 36, 37, 38, 39, //条子
        41, 42, 43, 44,//东 南 西 北 
        51, 52, 53, //中 发 白
        61,62,63,64,65,66,67,68,                        //春 夏 秋 冬 梅 兰 竹 菊
    };
    
    /// <summary>
    /// 麻将内所有牌                       				//箭牌 中 发 白
    /// </summary>
    public static readonly int[] CardWare =
    {
        11, 12, 13, 14, 15, 16, 17, 18, 19,
        11, 12, 13, 14, 15, 16, 17, 18, 19,
        11, 12, 13, 14, 15, 16, 17, 18, 19,
        11, 12, 13, 14, 15, 16, 17, 18, 19,
        21, 22, 23, 24, 25, 26, 27, 28, 29,
        21, 22, 23, 24, 25, 26, 27, 28, 29,
        21, 22, 23, 24, 25, 26, 27, 28, 29,
        21, 22, 23, 24, 25, 26, 27, 28, 29,
        31, 32, 33, 34, 35, 36, 37, 38, 39,
        31, 32, 33, 34, 35, 36, 37, 38, 39,
        31, 32, 33, 34, 35, 36, 37, 38, 39,
        31, 32, 33, 34, 35, 36, 37, 38, 39,
        41, 42, 43, 44,
        41, 42, 43, 44,
        41, 42, 43, 44,
        41, 42, 43, 44,
        51, 52, 53,
        51, 52, 53,
        51, 52, 53,
        51, 52, 53,
        61,62,63,64,65,66,67,68
    };

    /// <summary>
    /// 牌面对应的模型名称
    /// </summary>
    public static readonly string[] MeshNames =
    {
        "Mahjong_1W", "Mahjong_2W", "Mahjong_3W", "Mahjong_4W", "Mahjong_5W", "Mahjong_6W", "Mahjong_7W", "Mahjong_8W","Mahjong_9W",
        "Mahjong_1T", "Mahjong_2T", "Mahjong_3T", "Mahjong_4T", "Mahjong_5T", "Mahjong_6T", "Mahjong_7T", "Mahjong_8T","Mahjong_9T",
        "Mahjong_1S", "Mahjong_2S", "Mahjong_3S", "Mahjong_4S", "Mahjong_5S", "Mahjong_6S", "Mahjong_7S", "Mahjong_8S","Mahjong_9S",
        "Mahjong_D", "Mahjong_N", "Mahjong_X", "Mahjong_B",
        "Mahjong_HZ", "Mahjong_FC", "Mahjong_BB",
        "Mahjong_CH","Mahjong_XH","Mahjong_QH","Mahjong_DH","Mahjong_MH","Mahjong_LH","Mahjong_ZH","Mahjong_JH"
    };
    /// <summary>
    /// 表情数量
    /// </summary>
    public static int STICKER_NUM = 15;
    /// <summary>
    /// 表情持续时间
    /// </summary>
    public static float STICKER_LENGTH = 2f;
    /// <summary>
    /// 表情播放速度
    /// </summary>
    public static float STICKER_SPEED = .08f;

    /// <summary>
    /// 需要发牌的总数
    /// </summary>
    public static int SENDCARD_NUM = 53;

    /// <summary>
    /// 单次发牌数量
    /// </summary>
    public static int SEND_SINGLE = 4;

    /// <summary>
    /// 玩家手中默认的牌数
    /// </summary>
    public static int PLAYER_CARD_NUM = 13;

    /// <summary>
    /// 不同座位牌面盖下时的旋转角度
    /// </summary>
    public static int[] SitCardCloseArr = {180, 90, 45, 90};

    /// <summary>
    /// 服务器起始时间
    /// </summary>
    public static DateTime StartTime = DateTime.Parse("1970-1-1");

    /// <summary>
    /// 默认层
    /// </summary>
    public static readonly LayerMask LAYER_DEFAULT = 0;

    /// <summary>
    /// 其他牌层
    /// </summary>
    public static readonly LayerMask OTHER_CARDS = 8;

    /// <summary>
    /// 右边的牌
    /// </summary>
    public static readonly LayerMask RIGHTHAND_CARDS = 10;

    /// <summary>
    /// 自己手中的牌
    /// </summary>
    public static readonly LayerMask SELF_HAND_CARDS = 9;

    /// <summary>
    /// 不再显示字典
    /// </summary>
    public static Dictionary<DontShowAgain, bool> DontShowAgainDic;

    /// <summary>
    /// 背景音乐音量
    /// </summary>
    public static float BGMVolume = 1;
    /// <summary>
    /// 音效音量
    /// </summary>
    public static float AudioVolume = 1;
    /// <summary>
    /// 音效音量
    /// </summary>
    public static bool languagebool = false;
    /// <summary>
    /// 聊天输入最大文字数量
    /// </summary>
    public static int MaxCharNum = 15;
    /// <summary>
    /// 飘字提示最多显示的数量
    /// </summary>
    public static int PopMsgMax = 3;
    /// <summary>
    /// 聊天发送的时间间隔
    /// </summary>
    public static int SendChatInvoke = 3;
    /// <summary>
    /// 表情前缀
    /// </summary>
    public const string FACE_PREFIX = "#Face";
    /// <summary>
    /// 房间解散默认超时时间
    /// </summary>
    public const int DISLOVE_APPLY_TIMEOUT = 600;

    /// <summary>
    /// 聊天常用语
    /// </summary>
    public readonly static string[] Chat_Const = new string[]
    {
        "不要走 决战到天亮",
        "从早到晚 从黑到亮啊",
        "打麻将不但要有牌品还得要有气质",
        "都别点炮啊，这把哥要自摸",
        "快点吧等到黄花菜都凉了",
        "你的牌打的忒好了",
        "你看 那小子又开始点炮了",
        "请叫我炮哥 ",
        "我的内心几乎是崩溃的",
        "吓死宝宝了",
        "小牌一抓 啥事都忘",
        "要讲牌风 莫乱来",
        "要快 要快 不要像个老太太",
        "又短线了网络怎么这么差",
        "坐在麻将前 富不把贫嫌",
        "快点了，时间很宝贵的",
        "一路屁胡走向胜利",
        "上碰下自摸,大家要小心啊",
        "好汉不胡头三把",
        "先胡不算胡,后胡金满桌",
        "呀!打错了怎么办",
        "卡卡卡,卡的人火大呀",
        "很高兴能和大家一起打牌"
    };

    /// <summary>
    /// 获取当前序号后的座位号
    /// </summary>
    /// <param name="perSit"></param>
    /// <param name="addSit"></param>
    /// <returns></returns>
    public static int GetNextSit(int perSit, int addSit)
    {
        var perIndex = Array.IndexOf(NextSits, perSit);
        var nextIndex = (perIndex + addSit) % NextSits.Length;
        return NextSits[nextIndex];
    }

    /// <summary>  
    /// 日志最大数量  
    /// </summary>  
    public static int maxLogs = 2000;
    /// <summary>
    /// 日志数组
    /// </summary>
    public static ArrayList logs = new ArrayList();
    /// <summary>
    /// 分享链接地址
    /// </summary>
    public const string ShareUrl = "http://192.168.1.71/share.html";


    /// <summary>
    /// 暂时用来保存当前操作的牌
    /// </summary>
    public static int ActCard;
}

/// <summary>
/// 不再显示key
/// </summary>
public enum DontShowAgain
{
    EXIT_ROOM,
    DISSLOVE_ROOM
}

/// <summary>
/// 材质类型
/// </summary>
public enum MaterialName
{
    MAHJONG_BACK,
    MAHJONG_FRONT
}

/// <summary>
/// 对话框类型
/// </summary>
public enum DialogType
{
    ALERT,
    CONFIRM,
}

/// <summary>
/// 场景id
/// </summary>
public enum ESceneID
{
    SCENE_START = 0,
    //加载场景
    SCENE_LOGIN = 1,
    SCENE_HALL = 2,
    SCENE_BATTLE = 3
}

/// <summary>
/// 场景名称
/// </summary>
public class SceneName
{
    public const string START = "Start";
    public const string LOGIN = "Login";
    public const string HALL = "Hall";
    public const string BATTLE = "Battle";
}

/// <summary>
/// 提示框类型
/// </summary>
public enum AlertType
{
    Tip,
    Confirm
}

/// <summary>
/// 加载方式
/// </summary>
public enum LoadSceneType
{
    SYNC, //同步
    ASYNC //异步
}

public enum UIViewID
{
    LOGIN_VIEW,
    HALL_VIEW,
    BATTLE_VIEW,
    TOPMENU_VIEW,
    MIDDLEMENU_VIEW,
    BOTTOMMENU_VIEW,
    DIALOG_VIEW,
    //二级菜单
    PLATER_INFO_VIEW,
    MATCH_RESULT_VIEW,
    ROOM_RESULT_VIEW,
    CHAT_VIEW,
    DISLOVE_APPLY_VIEW,
    DISLOVE_STATISTICS_VIEW,
    SHOPPING_VIEW,
    SIGNIN_VIEW,
    HELP_VIEW,
    INVITE_VIEW,
    SHARE_VIEW,
    GRADE_VIEW,
    ACTIVITY_VIEW,
    SETTING_VIEW,
    COMPETITION_VIEW,
    RANKING_VIEW,
    CREATEROOM_VIEW,
    JOINROOM_VIEW,
    ATHLETICS_VIEW,
    MATCHING_VIEW,
    CUSTOMERSERVICE_VIEW,
    GRADEINFORMATION_VIEW,
}
/// <summary>
/// 游戏类型
/// </summary>
public enum GameMode
{
    FOUR_ROUND=4,       //4人局
    EIGHT_ROUND = 8,    //8人局
    SIXTEEN_ROUND = 16  //16人局
}
/// <summary>
/// 游戏规则
/// </summary>
public enum GameRule
{
    NOT_WORD = 1,       //无字
    WORD = 2,           //有字
}
/// <summary>
/// 
/// </summary>
public enum CapScore
{
    NO_CAP_SCORE=-1,
    CAP_SCORE=17,
}

/// <summary>
/// 本地保存数据key值
/// </summary>
public class PrefsKey
{
    /// <summary>
    /// ip
    /// </summary>
    public static string SERVERIP = "SERVERIP";
    /// <summary>
    /// 端口
    /// </summary>
    public static string SERVERPORT = "SERVERPORT";
    /// <summary>
    /// 用户名
    /// </summary>
    public static string USERNAME = "USERNAME";
    /// <summary>
    /// 用户id
    /// </summary>
    public static string USERMAC = "USERMAC";
    /// <summary>
    /// 音效设置
    /// </summary>
    public static string SOUNDSET = "SOUNDSET";
    /// <summary>
    /// 音乐设置
    /// </summary>
    public static string MUSICSET = "MUSICSET";
    /// <summary>
    /// 语言设置
    /// </summary>
    public static string LUANAGE = "LUANAGE";
    /// <summary>
    /// 头像URL
    /// </summary>
    public static string HEADURL = "HEADURL";
    /// <summary>
    /// 性别
    /// </summary>
    public static string SEX = "SEX";
}

/// <summary>
/// Socket连接类型
/// </summary>
public enum SocketType
{
    LOCAL,
    LOGIN,
    HALL,
    BATTLE
}
/// <summary>
/// 公告类型
/// </summary>
public enum HallNoticeType
{
    MENU_INFORMATION = 4,   //最新活动
    MENU_CONTACT = 3,       //联系我们
    MENU_ANNOUNCEMENT = 1,  //更新公告
    MENU_GENERALIZE = 5,    //推广专员
    HALL_CONTENT = 6,       //大厅公告图片
    HALL_NOTICE = 7,        //大厅公告文本
}

/// <summary>
/// 日志
/// </summary>
public struct LogVO
{
    public string message;
    public string stackTrace;
    public LogType type;
}

public class ItemType
{
    public static int ROOMCARD = 1;
}

public enum SDKPlatform
{
    LOCAL = 0,
    ANDROID,
    IOS
}

/// <summary>
/// 启动参数
/// </summary>
public class StartUpParam
{
    /// <summary>
    /// 启动类型
    /// </summary>
    public const string TYPE = "type";
    /// <summary>
    /// 房间号
    /// </summary>
    public const string ROOMID = "roomId";
}

/// <summary>
/// 启动类型
/// </summary>
public class StartUpType
{
    /// <summary>
    /// 加入房间
    /// </summary>
    public const string JOINROOM = "joinRoom";
}

