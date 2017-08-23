using System.Collections.Generic;
using Platform.Utils;
using UnityEngine;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine.SceneManagement;
using Utils;
using Platform.Model;
using Platform.Net;
using System.Collections;
using DG.Tweening;
/// <summary>
/// ������ģ������ģ��
/// </summary>
public class GameMgrMediator  : Mediator, IMediator
{
    /// <summary>
    /// ����ģ������ʵ�����
    /// </summary>
    private GameMgrProxy gameManagerProxy;
    /// <summary>
    /// �첽����ˢ�¶�ʱ��id
    /// </summary>
    private int UpdateTimerId;
    public GameMgrMediator(string NAME,object viewComponent) : base(NAME, viewComponent)
    {
    }

    /// <summary>
    /// ����ģ���ܹ�������
    /// </summary>
    public GameMgr View
    {
        get
        {
            return (GameMgr)ViewComponent;
        }
    }
    /// <summary>
    /// ����ģ����Ϣע��
    /// </summary>
    /// <returns></returns>
    public override IList<string> ListNotificationInterests()
    {
        IList<string> list = new List<string>();
        list.Add(NotificationConstant.MEDI_GAMEMGR_LOADSCENE);
        list.Add(NotificationConstant.MEDI_GAMEMGR_CONNECTSERVER);
        return list;
    }

    /// <summary>
    /// ����ģ���ʼ��
    /// </summary>
    public override void OnRegister()
    {
        base.OnRegister();
        this.gameManagerProxy = Facade.RetrieveProxy(Proxys.GAMEMGR_PROXY) as GameMgrProxy;
    }

    /// <summary>
    /// ����ģ����Ӧ��Ϣ
    /// </summary>
    /// <param PlayerName="notification"></param>
    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case NotificationConstant.MEDI_GAMEMGR_LOADSCENE:
                SwitchScene((LoadSceneInfo)notification.Body);
                break;
            case NotificationConstant.MEDI_GAMEMGR_CONNECTSERVER:
                GetUserInfoC2S userInfoPackage = new GetUserInfoC2S();
                GetCheckInInfoC2S userSignInPackage = new GetCheckInInfoC2S();
                ReConnectC2S reConnectC2S = new ReConnectC2S();
                NetMgr.Instance.SendBuff<GetCheckInInfoC2S>(SocketType.HALL, MsgNoC2S.REQUEST_GETSIGNININFO_C2S.GetHashCode(), 0, userSignInPackage);
                NetMgr.Instance.SendBuff<GetUserInfoC2S>(SocketType.HALL, MsgNoC2S.REQUEST_GETUSERINFO_C2S.GetHashCode(), 0, userInfoPackage);
                NetMgr.Instance.SendBuff<ReConnectC2S>(SocketType.HALL, MsgNoC2S.RECONNECT_C2S.GetHashCode(), 0, reConnectC2S);
                
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ÿ֡�ص�
    /// </summary>
    public void Update()
    {
        if (gameManagerProxy != null && gameManagerProxy.async != null && gameManagerProxy.async.isDone)//�������
        {
            UIManager.Instance.HidenScreenBg();
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            gameManagerProxy.async = null;
            if (SceneManager.GetActiveScene().name == SceneName.BATTLE || SceneManager.GetActiveScene().name == SceneName.LOGIN)
            {
                ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_HALL_REFRESHHALLNOTICE);
            }
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
            Timer.Instance.CancelTimer(UpdateTimerId);
        }
    }

    /// <summary>
    /// �л�����
    /// </summary>
    /// <param PlayerName="sceneName"></param>
    private void SwitchScene(LoadSceneInfo info)
    {
        Timer.Instance.CancelAllTimer();
        GameMgr.Instance.StopAllCoroutines();
        ResourcesMgr.Instance.ClearPool();
        AudioSystem.Instance.StopAllAudio();
        DOTween.KillAll(true);
        switch (info.type)
        {
            case (LoadSceneType.SYNC):
                LoadSceneSync(info);
                break;
            case (LoadSceneType.ASYNC):
                if (SceneManager.GetActiveScene().buildIndex != ESceneID.SCENE_START.GetHashCode())
                {
                    UIManager.Instance.StartSaveScreen(UIManager.Instance.UpdateScreenBg);
                }
                GameMgr.Instance.StartCoroutine(LoadSceneAsync(info));
                break;
            default:
                Debug.LogError("������Ϣ: SceneName:" + info.sceneID.ToString() + " Type:" + info.type.ToString() + " Model:" + info.mode.ToString());
                break;
        }        
    }

    /// <summary>
    /// ֱ�Ӽ��س���
    /// </summary>
    /// <param name="info">������Ϣ</param>
    public void LoadSceneSync(LoadSceneInfo info)
    {
        UIManager.Instance.InitUI();
        SceneManager.LoadScene((int)info.sceneID, info.mode);
    }
    /// <summary>
    /// �첽���س���
    /// </summary>
    /// <param name="info">������Ϣ</param>
    /// <returns></returns>
    public IEnumerator LoadSceneAsync(LoadSceneInfo info)
    {
        yield return new WaitForEndOfFrame();
        UIManager.Instance.InitUI();
        UpdateTimerId = Timer.Instance.AddTimer(0, 0, 0, Update);
        gameManagerProxy.async = SceneManager.LoadSceneAsync((int)info.sceneID, info.mode);
        yield return this.gameManagerProxy.async;
    }
}