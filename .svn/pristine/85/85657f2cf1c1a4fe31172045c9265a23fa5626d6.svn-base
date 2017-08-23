using Platform.Model.Battle;
using Platform.Net;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;
/// <summary>
/// 战报播放UI
/// </summary>
public class ReportView : MonoBehaviour
{
    /// <summary>
    /// 是否播放战报
    /// </summary>
    private bool isPlay = false;
    /// <summary>
    /// 加速速度列表
    /// </summary>
    private float[] speeds = {1f,2f,3f};
    /// <summary>
    /// 战斗数据
    /// </summary>
    private BattleProxy battleProxy;
    /// <summary>
    /// 游戏数据
    /// </summary>
    private GameMgrProxy gameMgrProxy;
    /// <summary>
    /// 当前播放速度
    /// </summary>
    private float curSpeed = 1;
    /// <summary>
    /// 当前播放到的动作序号
    /// </summary>
    private int curActionIndex = 0;
    /// <summary>
    /// 播放加速按钮
    /// </summary>
    private Button reportSpeedBtn;
    /// <summary>
    /// 播放按钮
    /// </summary>
    private Button pauseBtn;
    /// <summary>
    /// 播放按钮
    /// </summary>
    private Button playBtn;
    /// <summary>
    /// 返回按钮
    /// </summary>
    private Button returnBtn;


    void Awake ()
    {
        reportSpeedBtn = transform.Find("ReportSpeedBtn").GetComponent<Button>();
        pauseBtn = transform.Find("PauseBtn").GetComponent<Button>();
        playBtn = transform.Find("PlayBtn").GetComponent<Button>();
        returnBtn = transform.Find("ReturnBtn").GetComponent<Button>();

        curSpeed = speeds[0];
        battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
        gameMgrProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.GAMEMGR_PROXY) as GameMgrProxy;
        reportSpeedBtn.onClick.AddListener(SpeedHandler);
        pauseBtn.onClick.AddListener(PauseHandler);
        playBtn.onClick.AddListener(PlayHandler);
        returnBtn.onClick.AddListener(ReturnHandler);
        playBtn.gameObject.SetActive(false);
    }
    /// <summary>
    /// 速度设置
    /// </summary>
    private void SpeedHandler()
    {
        Debug.Log("加速");
        if (Time.timeScale == 0)
        {
            return;
        }
        var curIndex = Array.IndexOf<float>(speeds, curSpeed);
        curIndex = (curIndex + 1) % speeds.Length;
        curSpeed = speeds[curIndex];
        Time.timeScale = curSpeed;
    }
    /// <summary>
    /// 暂停播放
    /// </summary>
    private void PauseHandler()
    {
        Debug.Log("STOP");
        Time.timeScale = 0;
        pauseBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);
    }
    /// <summary>
    /// 继续播放
    /// </summary>
    private void PlayHandler()
    {
        Debug.Log("继续");
        Time.timeScale = curSpeed;
        pauseBtn.gameObject.SetActive(true);
        playBtn.gameObject.SetActive(false);
    }

    /// <summary>
    /// 退出播放
    /// </summary>
    private void ReturnHandler()
    {
        Debug.Log("退出");
        var loadInfo = new LoadSceneInfo(ESceneID.SCENE_HALL, LoadSceneType.ASYNC, LoadSceneMode.Additive);
        ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_GAMEMGR_LOADSCENE, loadInfo);
    }


    // Update is called once per frame
    void Update ()
    {
        if (!isPlay)
        {
            return;
        }
        if (curActionIndex >= battleProxy.report.actions.Count)
        {
            return;
        }
        var curAction = battleProxy.report.actions[curActionIndex];
        if (gameMgrProxy.scaleSystemTime > curAction.actionTime)
        {
            if (curAction.isActionTip)
            {
                NetMgr.Instance.OnClientReceiveBuff(MsgNoS2C.PUSH_PLAYER_ACTTIP.GetHashCode(), 0,
                    curAction.actTip);
            }
            else
            {
                NetMgr.Instance.OnClientReceiveBuff(MsgNoS2C.PUSH_PLAYER_ACT.GetHashCode(), 0,
                    curAction.act);
            }
            curActionIndex++;
            if (curActionIndex >= battleProxy.report.actions.Count)
            {
                Timer.Instance.AddTimer(1, 1, 3, ShowCompleteAlert);
            }
        }
      
	}

    /// <summary>
    /// 显示播放结束提示
    /// </summary>
    private void ShowCompleteAlert()
    {
        DialogMsgVO dialogMsgVO = new DialogMsgVO();
        dialogMsgVO.dialogType = DialogType.ALERT;
        dialogMsgVO.title = "播放提示";
        dialogMsgVO.content = "播放结束";
        dialogMsgVO.confirmCallBack = (() =>
        {
            var loadInfo = new LoadSceneInfo(ESceneID.SCENE_HALL, LoadSceneType.ASYNC, LoadSceneMode.Additive);
            ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_GAMEMGR_LOADSCENE, loadInfo);
        });
        DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
        dialogView.data = dialogMsgVO;
        this.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        reportSpeedBtn.onClick.RemoveListener(SpeedHandler);
        pauseBtn.onClick.RemoveListener(PauseHandler);
        playBtn.onClick.RemoveListener(PlayHandler);
        returnBtn.onClick.RemoveListener(ReturnHandler);
        Time.timeScale = 1;
        gameMgrProxy.ReviseScaleSystemTime();
    }

    /// <summary>
    /// 开始播放战报
    /// </summary>
    public void PlayReport()
    {
        isPlay = true;
    }
}

