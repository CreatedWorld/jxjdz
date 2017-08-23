using System;
using System.IO;
using System.Net;
using ProtoBuf;
using UnityEngine;

namespace LZR.Data.NetWork.Client
{
    /// <summary>
    /// NSocket
    /// </summary>
    public sealed class NSocket
    {
        private IPEndPoint mServerAddress = null;
        /// <summary>
        /// 服务器地址
        /// </summary>
        public IPEndPoint ServerAddress
        {
            get
            {
                if (mServerAddress == null)
                    return mServerAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 65520);
                return mServerAddress;
            }
            set
            {
                if (IsConnected)
                    throw new Exception("非法操作!已建立连接,不允许修改服务器地址");
                else
                    mServerAddress = value;
            }
        }

        private Action<string> mConsole = Debug.Log;
        /// <summary>
        /// 输出
        /// </summary>
        public Action<string> Console
        {
            get { return mConsole; }
            set { mConsole = value; }
        }

        private Action<string> mLogError = Debug.LogError;
        /// <summary>
        /// 输出
        /// </summary>
        public Action<string> LogError
        {
            get { return mLogError; }
            set { mLogError = value; }
        }

        private NSocketModel mNSocketModel = null;
        /// <summary>
        /// 连接类
        /// </summary>
        private NSocketModel NSocketModel
        {
            get
            {
                if (mNSocketModel == null)
                    return mNSocketModel = new NSocketModel(this);
                return mNSocketModel;
            }
            set
            {
                mNSocketModel = value;
            }
        }

        private bool mIsConnected = false;
        /// <summary>
        /// 获取一个Boollean值是否已连接
        /// </summary>
        public bool IsConnected 
        {
            get
            {
                return mIsConnected;
            }
            internal set
            {
                mIsConnected = value;
            }
        }

        private int mMaxReConnectNums = 3;
        /// <summary>
        /// 连接断开时自动重连次数
        /// 为0则无限循环重连直至连接成功
        /// </summary>
        public int MaxReConnectNums
        {
            get
            {
                return mMaxReConnectNums;
            }
            set
            {
                if (IsConnected)
                    throw new Exception("非法操作!已建立连接,不允许修改服务器地址");
                else mMaxReConnectNums = value;
            }
        }

        private Action mOnConnectSuccessful = null;
        /// <summary>
        /// 当连接成功
        /// </summary>
        public Action OnConnectSuccessful
        {
            get { return mOnConnectSuccessful ?? (mOnConnectSuccessful = () => { }); }
            set
            {
                mOnConnectSuccessful = value;
            }
        }

        private Action mOnDisconnect = null;
        /// <summary>
        /// 当连接断开
        /// </summary>
        public Action OnDisconnect
        {
            get { return mOnDisconnect ?? (mOnDisconnect = () => { }); }
            set
            {
                mOnDisconnect = value;
            }
        }

        private Action mOnConnectFailed = null;
        /// <summary>
        /// 当重连失败
        /// </summary>
        public Action OnConnectFailed
        {
            get { return mOnConnectFailed ?? (mOnConnectFailed = () => { }); }
            set
            {
                mOnConnectFailed = value;
            }
        }

        private Action<int, int, byte[]> mReceiveBuff = null;
        /// <summary>
        /// 接收到新消息
        /// </summary>
        public Action<int, int, byte[]> OnReceiveBuff
        {
            get { return mReceiveBuff ?? (mReceiveBuff = (i, i1, arg3) => { }); }
            set
            {
                mReceiveBuff = value;
            }
        }

        private Action<int> mOnRquestTimeOut = null;
        /// <summary>
        /// 消息超时回调
        /// </summary>
        public Action<int> OnRquestTimeOut
        {
            get
            {
                if (mOnRquestTimeOut == null)
                    mOnRquestTimeOut = i => {Console("协议ID:"+i+",超时未回应"); };
                return mOnRquestTimeOut;
            }
            set
            {
                mOnRquestTimeOut = value;
            }
        }

        private int mRquestTimeOut = 5;
        /// <summary>
        /// 消息回应超时时间
        /// </summary>
        public int RquestTimeOut
        {
            get { return mRquestTimeOut; }
            set { mRquestTimeOut = value; }
        }

        private static int userid = 0;
        /// <summary>
        /// 用户ID
        /// 在登陆后
        /// 登陆服务器会
        /// 返回该参数
        /// 然后对此参数进行复制
        /// </summary>
        public static int UserId
        {
            get
            {
                return userid;
            }
            set
            {
                userid = value;
            }
        }

        /// <summary>
        /// 房间id
        /// 发送消息给服务器时需要添加该数据
        /// </summary>
        public static int RoomId;


        /// <summary>
        /// 构造模块
        /// </summary>
        /// <param name="serverAddress">服务器地址</param>
        public NSocket(IPEndPoint serverAddress)
        {
            if (serverAddress == null)
                throw new Exception("服务器地址为空,无法创建新连接");
            IsConnected = false;
            this.ServerAddress = serverAddress;
        }

        /// <summary>
        /// 启动该连接
        /// </summary>
        public void StartTcpConnection()
        {
            if (ServerAddress == null)
                throw new Exception("服务器地址为空,无法启动连接");
            if (IsConnected) throw new Exception("已连接,请断开后重试");
            NSocketModel.TcpConnecting();
        }

        /// <summary>
        /// 停止该连接
        /// </summary>
        public void StopTcpConnection()
        {
            NSocketModel.Close();
        }

        /// <summary>
        /// 热切换服务器连接
        /// </summary>
        public void HotChangeConnect(IPEndPoint serverAddress)
        {
            StopTcpConnection();
            this.ServerAddress = serverAddress;
            StartTcpConnection();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="channel">通信协议</param>
        /// <param name="type">数据包类型</param>
        /// <param name="tobject">要发送的内容</param>
        public void SendBuff<T>(int channel, int type, T tobject,bool OffCheckTimeOut = false)
        {
            if (type < 0 || type > 1) throw new Exception("数据类型异常");
            NSocketModel.AddSendBuff(channel, type, tobject,OffCheckTimeOut);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="buff">字节流</param>
        /// <returns>T类型</returns>
        public static T DeSerializes<T>(byte[] buff)
        {
            using (MemoryStream ms = new MemoryStream(buff))
            {
                T t = Serializer.Deserialize<T>(ms);
                return t;
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static byte[] Serialize<T>(T t)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize<T>(ms, t);
                return ms.ToArray();
            }
        }
    }
}
