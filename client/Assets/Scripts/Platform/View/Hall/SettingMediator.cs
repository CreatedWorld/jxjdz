﻿using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Platform.Model;
using Platform.Net;
using Platform.Model.Battle;

public class SettingMediator : Mediator, IMediator
{
    public SettingMediator(string NAME, object viewComponent):base(NAME,viewComponent)
    {

    }

    /// <summary>
    /// 战斗模块数据中介
    /// </summary>
    private BattleProxy battleProxy;
    /// <summary>
    /// 玩家信息数据
    /// </summary>
    private PlayerInfoProxy playerInfoProxy;

    public SettingView View
    {
        get
        {
            return (SettingView)ViewComponent;
        }
    }

    public override void OnRegister()
    {
        base.OnRegister();
        battleProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.BATTLE_PROXY) as BattleProxy;
        playerInfoProxy = ApplicationFacade.Instance.RetrieveProxy(Proxys.PLAYERINFO) as PlayerInfoProxy;

        this.View.ButtonAddListening(this.View.CloseButton,
        () =>
        {
            PlayerPrefs.SetFloat(PrefsKey.SOUNDSET, this.View.SoundSlider.value);
            PlayerPrefs.SetFloat(PrefsKey.MUSICSET, this.View.MusicSlider.value);
            PlayerPrefs.SetInt(PrefsKey.LUANAGE, this.View.LanguageToggle.isOn?1:-1);
            UIManager.Instance.HideUI(UIViewID.SETTING_VIEW);
        });
        //声音
        this.View.MusicSlider.onValueChanged.AddListener(
            (float value)
            =>
            {
                PlayerPrefs.SetFloat(PrefsKey.MUSICSET, value);
                AudioSystem.Instance.SetBgmAudioVolume(PlayerPrefs.GetFloat(PrefsKey.MUSICSET));
            });
        this.View.SoundSlider.onValueChanged.AddListener(
            (float value)=>
            {
             PlayerPrefs.SetFloat(PrefsKey.SOUNDSET,value);
            AudioSystem.Instance.SetEffectAudioVolume(PlayerPrefs.GetFloat(PrefsKey.SOUNDSET));
            });
        this.View.LanguageToggle.onValueChanged.AddListener(
            (bool isok)=>
            {
                //AudioSystem.Instance.SetLanguageType(isok);
            }
            );
        //设置界面退出提示框
        SettingPrompt();
    }

    public override void OnRemove()
    {
        base.OnRemove();
        UIManager.Instance.DestroyUI(UIViewID.SETTING_VIEW);
    }

    private void SettingPrompt()
    {
        if (SceneManager.GetActiveScene().name == SceneName.BATTLE)
        {
            this.View.Frame.overrideSprite = Resources.Load<Sprite>("Textures/UI/Prompt/弹窗背景");
            this.View.CloseButton.image.overrideSprite = Resources.Load<Sprite>("Textures/UI/Prompt/关闭按钮");
            this.View.CloseButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(283.3f,277.5f);
            this.View.CloseButton.image.SetNativeSize();
            this.View.ButtonAddListening(this.View.SwitchAccountButton,
                () =>
                {
                    if (playerInfoProxy.UserInfo.UserID != battleProxy.creatorId && battleProxy.isFirstMatch)
                    {
                        DialogMsgVO dialogMsgVO = new DialogMsgVO();
                        dialogMsgVO.title = "退出确认";
                        dialogMsgVO.content = "是否退出房间";
                        dialogMsgVO.dialogType = DialogType.CONFIRM;
                        dialogMsgVO.confirmCallBack = delegate { ConfirmExit(); };
                        DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
                        dialogView.data = dialogMsgVO;
                    }
                    else
                    {
                        DialogMsgVO dialogMsgVO = new DialogMsgVO();
                        dialogMsgVO.dialogType = DialogType.CONFIRM;
                        dialogMsgVO.title = "解散确认";
                        dialogMsgVO.content = "是否解散房间";
                        dialogMsgVO.confirmCallBack = delegate { ConfirmDissloution(); };
                        DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
                        dialogView.data = dialogMsgVO;
                    }
                }
                );
        }
        else
        {
            this.View.ButtonAddListening(this.View.SwitchAccountButton,
                () =>
                {
                    DialogMsgVO dialogVO = new DialogMsgVO();
                    dialogVO.dialogType = DialogType.CONFIRM;
                    dialogVO.title = "退出";
                    dialogVO.content = "你确定要离开游戏吗？";
                    dialogVO.confirmCallBack = delegate { ExitHall(); };
                    DialogView dialogView = UIManager.Instance.ShowUI(UIViewID.DIALOG_VIEW) as DialogView;
                    dialogView.data = dialogVO;
                }
                );
        }
    }

    /// <summary>
    /// 退出房间确认回调
    /// </summary>
    private void ConfirmExit()
    {
        var exitC2S = new ExitRoomC2S();
        NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.EXIT_C2S.GetHashCode(), 0, exitC2S);
    }

    /// <summary>
    /// 解散房间确认回调
    /// </summary>
    private void ConfirmDissloution()
    {
        if (battleProxy.isStart)
        {
            var disloveC2S = new ApplyDissolveRoomC2S();
            NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.DISLOVEAPPLY_C2S.GetHashCode(), 0, disloveC2S, false);
        }
        else
        {
            if (battleProxy.creatorId == playerInfoProxy.UserInfo.UserID)
            {
                var disloveC2S = new DissolveRoomC2S();
                NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.DISSOLUTION_C2S.GetHashCode(), 0, disloveC2S);
            }
            else
            {
                var disloveC2S = new ApplyDissolveRoomC2S();
                NetMgr.Instance.SendBuff(SocketType.BATTLE, MsgNoC2S.DISLOVEAPPLY_C2S.GetHashCode(), 0, disloveC2S);
            }
        }
    }


    private void ExitHall()
    {
        UIManager.Instance.Backgournd.color = new Color(1, 1, 1, 0);
        UIManager.Instance.Backgournd.gameObject.SetActive(true);
        UIManager.Instance.Backgournd.DOColor(new Color(1, 1, 1, 1), 0.5f).SetEase(Ease.Linear).OnComplete(
            () =>
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    PlayerPrefs.DeleteKey(PrefsKey.USERMAC);
                    PlayerPrefs.DeleteKey(PrefsKey.USERNAME);
                    PlayerPrefs.DeleteKey(PrefsKey.HEADURL);
                    PlayerPrefs.DeleteKey(PrefsKey.SEX);
                }
                var loadInfo = new LoadSceneInfo(ESceneID.SCENE_LOGIN, LoadSceneType.SYNC, LoadSceneMode.Single);
                ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_GAMEMGR_LOADSCENE, loadInfo);
            });
        PlayerPrefs.SetFloat(PrefsKey.SOUNDSET, this.View.SoundSlider.value);
        PlayerPrefs.SetFloat(PrefsKey.MUSICSET, this.View.MusicSlider.value);
        PlayerPrefs.SetInt(PrefsKey.LUANAGE, this.View.LanguageToggle.isOn ? 1 : -1);
    }
}