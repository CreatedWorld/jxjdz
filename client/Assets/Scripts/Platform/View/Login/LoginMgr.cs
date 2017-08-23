using UnityEngine;
using Platform.Net;

public class LoginMgr : MonoBehaviour
{
    private LoginView loginView;

    public LoginView LoginView
    {
        get
        {
            return loginView;
        }

        set
        {
            loginView = value;
        }
    }

    void Awake()
    {
        NetMgr.Instance.StopAllTcpConnection();
        Debug.Log("断开所有与服务器连接");
        ApplicationFacade.Instance.RegisterMediator(new LoginMediator(Mediators.LOGIN_MEDIATOR, this));
    }
    void OnDestroy()
    {
        ApplicationFacade.Instance.RemoveMediator(Mediators.LOGIN_MEDIATOR);
    }
    /// <summary>
    /// 微信登陆回调
    /// </summary>
    /// <param name="str"></param>
    public void RespLoginResult(string str)
    {
        WXUserInfo wxUserInfo = JsonUtility.FromJson<WXUserInfo>(str);
        PlayerPrefs.SetString(PrefsKey.USERMAC, wxUserInfo.unionid);
        PlayerPrefs.SetString(PrefsKey.USERNAME,wxUserInfo.nickname);
        PlayerPrefs.SetString(PrefsKey.HEADURL,wxUserInfo.headimgurl);
        PlayerPrefs.SetInt(PrefsKey.SEX,int.Parse(wxUserInfo.sex));
        ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_LOGIN_WXLOGINSUCCEED);
    }
}