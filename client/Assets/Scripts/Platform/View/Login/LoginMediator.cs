using System.Collections.Generic;
using UnityEngine;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using Platform.Net;
using Platform.Model;
using UnityEngine.SceneManagement;
using Utils;
using System;

public class LoginMediator : Mediator, IMediator
{
    private LoginProxy loginProxy;
    public LoginMediator(string NAME, object viewComponent) : base(NAME,viewComponent)
    {
    }
    public LoginProxy LoginProxy
    {
        get
        {
            return loginProxy;
        }
    }
    public LoginMgr View
    {
        get
        {
            return (LoginMgr)ViewComponent;
        }
    }
    public override IList<string> ListNotificationInterests()
    {
        IList<string> list = new List<string>();
        list.Add(NotificationConstant.MEDI_LOGIN_SWITCHHALLSCENE);
        list.Add(NotificationConstant.MEDI_LOGIN_WXLOGINSUCCEED);
        return list;
    }


    public override void OnRegister()
    {
        base.OnRegister();
        this.loginProxy = Facade.RetrieveProxy(Proxys.LOGIN_PROXY) as LoginProxy;

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            Debug.Log(commandLineArgs);
            if (commandLineArgs.Length > 1)
            {
                GlobalData.LoginServer = commandLineArgs[1];
                GlobalData.LoginPort = int.Parse(commandLineArgs[2]);
                GlobalData.UserName = commandLineArgs[3];
                GlobalData.UserMac = commandLineArgs[4];
                LoginProxy.autoLogin = true;
                CheckUserAgreement();
                return;
            }
        }

        if (LoginProxy.autoLogin)
        {
            CheckUserAgreement();
        }
        else
        {
            this.View.LoginView = (LoginView)UIManager.Instance.ShowUI(UIViewID.LOGIN_VIEW);
            this.View.LoginView.ButtonAddListening(this.View.LoginView.LoginBtn, CheckUserAgreement);
            AudioSystem.Instance.PlayBgm(Resources.Load<AudioClip>("Voices/Bgm/HallBgm"));
        }
    }
    public override void OnRemove()
    {
        base.OnRemove();
        UIManager.Instance.DestroyUI(UIViewID.LOGIN_VIEW);
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case NotificationConstant.MEDI_LOGIN_SWITCHHALLSCENE:
                if (this.View.LoginView != null)
                {
                    this.View.LoginView.Anim.SetBool("LoginSucceed", true);
                }
                var loadInfo = new LoadSceneInfo(ESceneID.SCENE_HALL, LoadSceneType.ASYNC, LoadSceneMode.Additive);
                ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_GAMEMGR_LOADSCENE, loadInfo);
                break;
            case NotificationConstant.MEDI_LOGIN_WXLOGINSUCCEED:
                this.SendLoginSucceed();
                break;
            default:
                break;
        }
    }
    private void CheckUserAgreement()
    {
        if (LoginProxy.autoLogin)
        {
            this.LoginServerConnent();
            return;
        }
        switch (GlobalData.sdkPlatform)
        {
            case SDKPlatform.ANDROID:

                if (this.View.LoginView.Toggle.isOn)
                {
                    if (PlayerPrefs.HasKey(PrefsKey.USERMAC))
                    {
                        this.SendLoginSucceed();
                    }
                    else
                    {
                        AndroidSdkInterface.SendWeiXinLogin();
                    }
                }
                else
                {
                    LoginFailDialog();
                }
                break;
            case SDKPlatform.LOCAL:
                if (this.View.LoginView.Toggle.isOn)
                {
                    SendLoginSucceed();
                }
                else
                {
                    LoginFailDialog();
                }
                break;
        }
    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.KeypadEnter))
        //{
        //    CheckUserAgreement();
        //}
    }

    /// <summary>
    /// 登录服务器连接
    /// </summary>
    private void LoginServerConnent()
    {
        Debug.Log("登录服务器连接");
        if (loginProxy.autoLogin)
        {
            NetMgr.Instance.StopTcpConnection(SocketType.LOGIN);
            NetMgr.Instance.CreateConnect(SocketType.LOGIN, GlobalData.LoginServer, GlobalData.LoginPort, LoginConnectHandler);
            return;
        }
        GlobalData.LoginServer = View.LoginView.serverTxt.text;
        GlobalData.LoginPort = int.Parse(View.LoginView.portTxt.text);
        GlobalData.UserName = View.LoginView.nameTxt.text;
        GlobalData.UserMac = View.LoginView.macTxt.text;
        GlobalData.UserPwd = View.LoginView.pwdTxt.text;
        PlayerPrefs.SetString(PrefsKey.SERVERIP, GlobalData.LoginServer);
        PlayerPrefs.SetInt(PrefsKey.SERVERPORT, GlobalData.LoginPort);
        PlayerPrefs.SetString(PrefsKey.USERMAC, GlobalData.UserMac);
        PlayerPrefs.SetString(PrefsKey.USERNAME, GlobalData.UserName);
        NetMgr.Instance.StopTcpConnection(SocketType.LOGIN);
        NetMgr.Instance.CreateConnect(SocketType.LOGIN, GlobalData.LoginServer, GlobalData.LoginPort, LoginConnectHandler);
    }

    /// <summary>
    /// 连接成功回调
    /// </summary>
    private void LoginConnectHandler()
    {
        NetMgr.Instance.ConnentionDic[SocketType.LOGIN].OnConnectSuccessful = null;
        Debug.Log("登陆服务器连接成功:" + NetMgr.Instance.ConnentionDic[SocketType.LOGIN].ServerAddress);
        Timer.Instance.AddDeltaTimer(0, 1, 0, () => { this.SendLoginRequest(); });
        
    }

    /// <summary>
    /// 发送登陆请求
    /// </summary>
    private void SendLoginRequest()
    {
        LoginC2S package = new LoginC2S();
        switch (GlobalData.sdkPlatform)
        {
            case SDKPlatform.ANDROID:
                package.mac = PlayerPrefs.GetString(PrefsKey.USERMAC);
                package.name = PlayerPrefs.GetString(PrefsKey.USERNAME);
                package.imageUrl = PlayerPrefs.GetString(PrefsKey.HEADURL);
                package.sex = PlayerPrefs.GetInt(PrefsKey.SEX);
                break;
            case SDKPlatform.LOCAL:
                package.mac = GlobalData.UserMac;
                package.name = GlobalData.UserName;
                package.imageUrl = GlobalData.ImageUrl;
                package.sex = 1;
                break;
        }
        package.psw = GlobalData.UserPwd;
        NetMgr.Instance.SendBuff<LoginC2S>(SocketType.LOGIN, MsgNoC2S.REQUEST_LOGIN_C2S.GetHashCode(), 0, package);
        Debug.Log("发送登陆请求");
        loginProxy.autoLogin = false;
    }

    private void SendLoginSucceed()
    {
        if (View.LoginView.serverTxt.text == "127.0.0.1")
        {
            GlobalData.LoginServer = View.LoginView.serverTxt.text;
            GlobalData.LoginPort = int.Parse(View.LoginView.portTxt.text);
            PlayerPrefs.SetString(PrefsKey.SERVERIP, GlobalData.LoginServer);
            PlayerPrefs.SetInt(PrefsKey.SERVERPORT, GlobalData.LoginPort);
            SendNotification(NotificationConstant.MEDI_LOGIN_SWITCHHALLSCENE);
        }
        else
        {
            this.LoginServerConnent();
        }
    }
    private void LoginFailDialog()
    {
        DialogMsgVO dialogVO = new DialogMsgVO();
        dialogVO.dialogType = DialogType.ALERT;
        dialogVO.title = "登录失败";
        dialogVO.content = "请先阅读用户协议";
        DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
        dialogView.data = dialogVO;
    }
}