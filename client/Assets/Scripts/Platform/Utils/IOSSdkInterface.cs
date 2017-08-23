using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Platform.Utils
{
    /// <summary>
    /// ios平台接口
    /// </summary>
    class IOSSdkInterface
    {
        /// <summary>
        /// 微信登陆
        /// </summary>
        [DllImport("__Internal")]
        public static extern void SendWeiXinLogin();

        /// <summary>
        /// 微信分享
        /// <param name="text">分享内容:{0}|{1}|{2}形式传入,{0}:分享连接,{1}:分享标题,{2}:分享描述</param>
        /// <param name="isTimeline">分享至朋友圈/好友 true:朋友圈 false:好友</param>
        /// </summary>
        [DllImport("__Internal")]
        public static extern void WeiXinShare(string url, string title, string desc, bool isTimeline);

        /// <summary>
        /// 微信分享截屏
        /// </summary>
        /// <param name="imgBase64">图片的base64编码</param>
        /// <param name="isTimeline">分享至朋友圈/好友 true:朋友圈 false:好友</param>
        [DllImport("__Internal")]
        public static extern void WeiXinShareScreen(string imgBase64, bool isTimeline);

        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <returns></returns>
        [DllImport("__Internal")]
        public static extern string GetVersion();


        /// <summary>
        /// 第三方支付接口
        /// <param name="payJson">支付信息JSON字符串</param>
        /// 支付结果回调
        /// result 支付结果
        /// UnityPlayer.UnitySendMessage("GameMgr","onPayResult",result)
        /// </summary>
        [DllImport("__Internal")]
        public static extern void otherPay(string payJson);
        /// <summary>
        /// 启动充值
        /// </summary>
        /// <param name="amount">数量</param>
        /// <param name="goodsName">购买的商品名称</param>
        /// <param name="playerID">玩家id</param>
        /// <param name="remark"></param>
        [DllImport("__Internal")]
        public static extern void FWPay(string amount, string goodsName, string playerID, string remark);

        /// <summary>
        /// 微信支付
        /// <param name="payJson">微信支付信息JSON对象</param>
        /// 支付结果回调
        /// result 支付结果
        /// UnityPlayer.UnitySendMessage("GameMgr","onPayResult",result)
        /// </summary>
        [DllImport("__Internal")]
        public static extern void weChatPay(string payJson);

        /// <summary>
        /// 支付宝支付
        /// <param name="payJson">支付宝支付信息JSON字符串</param>
        /// 支付结果回调
        /// result 支付结果
        /// UnityPlayer.UnitySendMessage("GameMgr","onPayResult",result)
        /// </summary>
        [DllImport("__Internal")]
        public static extern void aliPay(string payJson);
        /// <summary>
        /// 获取电池电量0-100
        /// </summary>
        [DllImport("__Internal")]
        public static extern int GetElectricity();

        /// <summary>
        /// 更新游戏
        /// </summary>
        [DllImport("__Internal")]
        public static extern void UpdateApp(string url);
    }
}
