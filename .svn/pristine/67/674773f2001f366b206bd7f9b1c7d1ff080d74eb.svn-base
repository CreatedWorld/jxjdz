using System;
using System.Net;
using LZR.Data.NetWork.Client;
using Platform.Model.Battle;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Utils;
using Platform.Model;

namespace Platform.Net
{
    /// <summary>
    /// 消息管理
    /// </summary>
    class NetMgr
    {
        /// <summary>
        /// 是否显示断线重连提示
        /// </summary>
        private bool isShowTimeOut = false;
        private static NetMgr mInstance = null;
        /// <summary>
        /// 消息管理类
        /// </summary>
        public static NetMgr Instance
        {
            get
            {
                if (null == mInstance)
                {
                    mInstance = new NetMgr();
                    return mInstance;
                }
                return mInstance;
            }
            private set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                mInstance = value;
            }
        }

        public NetMgr()
        {
            var timerId = Timer.Instance.AddTimer(20, 0, 0, SendBeat);
            Timer.Instance.SetFixTimer(timerId);
        }

        /// <summary>
        /// 网络模块
        /// </summary>
        public Dictionary<SocketType, NSocket> ConnentionDic = new Dictionary<SocketType, NSocket>();

        /// <summary>
        /// 构建连接
        /// </summary>
        public NSocket CreateConnect(SocketType socketType,string server,int port,Action connectSucess=null)
        {
            var connention = new NSocket(new IPEndPoint(IPAddress.Parse(server), port));
            connention.MaxReConnectNums = 0;
            connention.OnConnectFailed = OnConnectFailed;
            if (connectSucess != null)
            {
                connention.OnConnectSuccessful = connectSucess;
            }
            else
            {
                connention.OnConnectSuccessful = OnConnectSuccessful;
            }
            connention.OnDisconnect = delegate { Timer.Instance.AddTimer(0, 1, 0, () => { OnDisconnect(connention); }); };
            connention.OnReceiveBuff = OnReceiveBuff;
            connention.OnRquestTimeOut = OnSendBuffTimeOut;
            ConnentionDic.Add(socketType, connention);
            connention.StartTcpConnection();
            return connention;
        }

        /// <summary>
        /// 停止连接
        /// </summary>
        /// <param name="socketType"></param>
        public void StopTcpConnection(SocketType socketType)
        {
            if (!ConnentionDic.ContainsKey(socketType))
            {
                return;
            }
            var connention = ConnentionDic[socketType];
            connention.StopTcpConnection();
            ConnentionDic.Remove(socketType);
        }

        /// <summary>
        /// 断开所有服务器连接
        /// </summary>
        public void StopAllTcpConnection()
        {
            foreach (KeyValuePair<SocketType, NSocket> pair in ConnentionDic)
            {
                pair.Value.StopTcpConnection();
            }
            ConnentionDic.Clear();
        }

        /// <summary>
        /// 当连接成功
        /// </summary>
        void OnConnectSuccessful()
        {
            //绑定连接成功回调
        }

        /// <summary>
        /// 当连接断开
        /// </summary>
        void OnDisconnect(NSocket socket)
        {
            if (!ConnentionDic.ContainsValue(socket))
            {
                return;
            }
            if (isShowTimeOut)
            {
                return;
            }
            isShowTimeOut = true;
            DialogMsgVO dialogMsgVO = new DialogMsgVO();
            dialogMsgVO.title = "连接提示";
            dialogMsgVO.content = "您已断开连接是否重新连接";
            dialogMsgVO.dialogType = DialogType.CONFIRM;
            dialogMsgVO.confirmCallBack = delegate { ConfirmReConnect(); };
            dialogMsgVO.cancelCallBack = delegate { CancelReConnect(); };
            DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
            dialogView.data = dialogMsgVO;
        }

        /// <summary>
        /// 确定重连
        /// </summary>
        void ConfirmReConnect()
        {
            isShowTimeOut = false;
            //foreach (KeyValuePair<SocketType, NSocket> keyValuePair in ConnentionDic)
            //{
            //    keyValuePair.Value.ReConnecting();
            //}
            if (SceneManager.GetActiveScene().name != SceneName.LOGIN)
            {
                LoginProxy loginProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.LOGIN_PROXY) as LoginProxy;
                loginProxy.autoLogin = true;
            }
            StopAllTcpConnection();
            var loadInfo = new LoadSceneInfo(ESceneID.SCENE_LOGIN, LoadSceneType.SYNC, LoadSceneMode.Single);
            ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_GAMEMGR_LOADSCENE, loadInfo);
        }

        /// <summary>
        /// 取消重连
        /// </summary>
        void CancelReConnect()
        {
            isShowTimeOut = false;
            var loadInfo = new LoadSceneInfo(ESceneID.SCENE_LOGIN, LoadSceneType.SYNC, LoadSceneMode.Single);
            ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_GAMEMGR_LOADSCENE, loadInfo);
        }

        /// <summary>
        /// 当连接失败
        /// 即重连后依然连接不上
        /// </summary>
        void OnConnectFailed()
        {
            //连接服务器失败了
        }

        /// <summary>
        /// 当接收到新数据
        /// </summary>
        /// <param name="channel">协议ID</param>
        /// <param name="type">数据类型</param>
        /// <param name="buff">数据内容</param>
        void OnReceiveBuff(int channel, int type, byte[] buff)
        {
            ReciveMsgVO msgVo = new ReciveMsgVO();
            msgVo.channel = channel;
            msgVo.type = type;
            msgVo.bytes = buff;
            GameMgr.Instance.ReciveMsgPool.Enqueue(msgVo);
        }

        /// <summary>
        /// 发送心跳包
        /// </summary>
        void SendBeat()
        {
            if (isShowTimeOut)
            {
                return;
            }
            if (GlobalData.LoginServer == "127.0.0.1")
            {
                return;
            }
            if (ConnentionDic.ContainsKey(SocketType.HALL))
            {
                //ConnentionDic[SocketType.HALL].SendBuff(MsgNoC2S.HALL_BEAT_C2S.GetHashCode(), 0, new HallBeatC2S(), false);
                SendBuff(SocketType.HALL, MsgNoC2S.HALL_BEAT_C2S.GetHashCode(), 0, new HallBeatC2S());
            }
            if (ConnentionDic.ContainsKey(SocketType.BATTLE))
            {
                //ConnentionDic[SocketType.BATTLE].SendBuff(MsgNoC2S.BATTLE_BEAT_C2S.GetHashCode(), 0, new BattleBeatC2S(), false);
                SendBuff(SocketType.BATTLE, MsgNoC2S.BATTLE_BEAT_C2S.GetHashCode(), 0, new BattleBeatC2S());
            }
        }

        /// <summary>
        /// 发送数据包
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="socketType">socket类型</param>
        /// <param name="channel">通信协议[PID]</param>
        /// <param name="type">内容的类型,0-Protobuf1,Json</param>
        /// <param name="tbuff">传递的内容</param>
        /// <param name="offCheckTimeOut">是否超时</param>
        public void SendBuff<T>(SocketType socketType, int channel, int type, T tbuff, bool offCheckTimeOut = false)
        {
            SendMsgVO msgVO = new SendMsgVO();
            msgVO.socketType = socketType;
            msgVO.channel = channel;
            msgVO.type = type;
            msgVO.tbuff = tbuff;
            msgVO.offCheckTimeOut = offCheckTimeOut;
            GameMgr.Instance.SendMsgPool.Enqueue(msgVO);
        }

        /// <summary>
        /// 当发送数据超时未回应
        /// </summary>
        public void OnSendBuffTimeOut(int channel)
        {
            Debug.Log("协议ID:" + channel + ",超时未回应");
            Timer.Instance.AddTimer(0, 1, 0, () => { OnRquestTimeOut(); });
        }

        /// <summary>
        /// 连接超时
        /// </summary>
        /// <param name="socket"></param>
        void OnRquestTimeOut()
        {
            //if (isShowTimeOut)
            //{
            //    return;
            //}
            //isShowTimeOut = true;
            //DialogMsgVO dialogMsgVO = new DialogMsgVO();
            //dialogMsgVO.title = "连接提示";
            //dialogMsgVO.content = "连接超时是否重新连接";
            //dialogMsgVO.dialogType = DialogType.CONFIRM;
            //dialogMsgVO.confirmCallBack = delegate { ConfirmReConnect(); };
            //dialogMsgVO.cancelCallBack = delegate { CancelReConnect(); };
            //DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
            //dialogView.data = dialogMsgVO;
        }

        public void OnDisconnect()
        {
            DialogMsgVO dialogMsgVO = new DialogMsgVO();
            dialogMsgVO.title = "连接提示";
            dialogMsgVO.content = "您已离线，是否重新进入游戏";
            dialogMsgVO.dialogType = DialogType.CONFIRM;
            dialogMsgVO.confirmCallBack = delegate { ConfirmReConnect(); };
            dialogMsgVO.cancelCallBack = delegate { CancelReConnect(); };
            DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
            dialogView.data = dialogMsgVO;
        }

        /// <summary>
        /// 反序列化
        /// 将Byte[]反序列化为类或结构
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns></returns>
        public T DeSerializes<T>(byte[] buff)
        {
            T result = NSocket.DeSerializes<T>(buff);
            ClientAIMgr.Instance.ShowReciveMsgLog(result);
            return result;
        }

        /// <summary>
        /// 客户端模拟接到消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="type"></param>
        /// <param name="msg"></param>
        public void OnClientReceiveBuff<T>(int channel, int type, T msg)
        {
            byte[] buffs = NSocket.Serialize<T>(msg);
            OnReceiveBuff(channel, type, buffs);
        }


        /// <summary>
        /// 当关闭
        /// </summary>
        public void OnDisable()
        {
            foreach (KeyValuePair<SocketType, NSocket> pair in ConnentionDic)
            {
                pair.Value.StopTcpConnection();
            }
        }
    }
}
