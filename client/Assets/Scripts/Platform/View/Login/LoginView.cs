using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoginView : UIView
{
    private Button loginBtn;
    private Toggle toggle;
    private Text versions;
    private CanvasGroup canvasGroup;
    private Animator anim;
    /// <summary>
    /// 登录输入区域
    /// </summary>
    public GameObject loginInputArea;
    /// <summary>
    /// 登录ip
    /// </summary>
    public InputField serverTxt;
    /// <summary>
    /// 登录端口
    /// </summary>
    public InputField portTxt;
    /// <summary>
    /// 用户id
    /// </summary>
    public InputField macTxt;
    /// <summary>
    /// 登录名称
    /// </summary>
    public InputField nameTxt;
    /// <summary>
    /// 登录密码
    /// </summary>
    public InputField pwdTxt;
    /// <summary>
    /// 登录中介
    /// </summary>
    private LoginMediator mediator;

    public Button LoginBtn
    {
        get
        {
            return loginBtn;
        }

        set
        {
            loginBtn = value;
        }
    }

    public Toggle Toggle
    {
        get
        {
            return toggle;
        }

        set
        {
            toggle = value;
        }
    }

    public CanvasGroup CanvasGroup
    {
        get
        {
            return canvasGroup;
        }

        set
        {
            canvasGroup = value;
        }
    }

    public Animator Anim
    {
        get
        {
            return anim;
        }

        set
        {
            anim = value;
        }
    }

    public override void OnInit()
    {
        this.ViewRoot = this.LaunchUIView("Prefab/UI/Login/LoginView");
        this.LoginBtn = this.ViewRoot.transform.FindChild("LoginButton").GetComponent<Button>();
        this.Toggle = this.ViewRoot.transform.FindChild("Toggle").GetComponent<Toggle>();
        this.versions = this.ViewRoot.transform.FindChild("Versions").GetComponent<Text>();
        this.CanvasGroup = this.ViewRoot.GetComponent<CanvasGroup>();
        this.Anim = this.ViewRoot.GetComponent<Animator>();
        loginInputArea = ViewRoot.transform.FindChild("LoginInputArea").gameObject;
        serverTxt = ViewRoot.transform.FindChild("LoginInputArea/ServerValueTxt").GetComponent<InputField>();
        portTxt = ViewRoot.transform.FindChild("LoginInputArea/PortValueTxt").GetComponent<InputField>();
        macTxt = ViewRoot.transform.FindChild("LoginInputArea/MacValueTxt").GetComponent<InputField>();
        nameTxt = ViewRoot.transform.FindChild("LoginInputArea/UserNameValueTxt").GetComponent<InputField>();
        pwdTxt = ViewRoot.transform.FindChild("LoginInputArea/PwdValueTxt").GetComponent<InputField>();
        serverTxt.text = GlobalData.LoginServer;
        portTxt.text = GlobalData.LoginPort.ToString();
        nameTxt.text = GlobalData.UserName;
        macTxt.text = GlobalData.UserMac;
        pwdTxt.text = GlobalData.UserPwd;
        macTxt.characterLimit = 6;
        nameTxt.characterLimit = 6;
        if (GlobalData.UserMac == "PhoneMac")
        {
            nameTxt.text = GetRandAccount();
            macTxt.text = nameTxt.text;
        }
        //this.versions.text = GlobalData.VERSIONS;
    }

    string GetRandAccount()
    {
        int curtv = (int)Mathf.Floor((float)(DateTime.Now.Subtract(DateTime.Parse("1970-1-1")).TotalMilliseconds / 1000));
        int ranval = UnityEngine.Random.Range(10, 99);
        string randacc = String.Format("{0:X}", curtv) + String.Format("{0:X}", ranval);
        randacc = randacc.Substring(randacc.Length - 6);
        return randacc;
    }

    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Login/LoginView");
    }

    public override void Update()
    {
        if (mediator == null)
        {
            mediator = ApplicationFacade.Instance.RetrieveMediator(Mediators.LOGIN_MEDIATOR) as LoginMediator;
        }
        mediator.Update();
    }
}
