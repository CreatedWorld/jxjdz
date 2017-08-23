﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// 设置View
/// </summary>
public class SettingView : UIView
{
    /// <summary>
    /// 关闭按钮
    /// </summary>
    private Button closeButton;
    /// <summary>
    /// 切换账号按钮
    /// </summary>
    private Button switchAccountButton;
    /// <summary>
    /// 音效按钮
    /// </summary>
    private Slider soundSlider;
    /// <summary>
    /// 音乐按钮
    /// </summary>
    private Slider musicSlider;
    /// <summary>
    /// 语言选择Toggle
    /// </summary>
    private Toggle languageToggle;
    /// <summary>
    /// 背景图片
    /// </summary>
    private Image frame;
    public Button CloseButton
    {
        get
        {
            return closeButton;
        }

        set
        {
            closeButton = value;
        }
    }
    public Button SwitchAccountButton
    {
        get
        {
            return switchAccountButton;
        }

        set
        {
            switchAccountButton = value;
        }
    }

    public Slider SoundSlider
    {
        get
        {
            return soundSlider;
        }
    }

    public Slider MusicSlider
    {
        get
        {
            return musicSlider;
        }
    }

    public Toggle LanguageToggle
    {
        get
        {
            return languageToggle;
        }
        set
        {
            languageToggle = value;
        }
    }

    public Image Frame
    {
        get
        {
            return frame;
        }
        set
        {
            frame = value;
        }
    }

    public override void OnInit()
    {
        this.ViewRoot = this.LaunchUIView("Prefab/UI/Setting/SettingView");
        this.Frame = this.ViewRoot.transform.FindChild("Frame").GetComponent<Image>();
        this.CloseButton = this.ViewRoot.transform.FindChild("CloseButton").GetComponent<Button>();
        this.SwitchAccountButton = this.ViewRoot.transform.FindChild("SwitchAccountButton").GetComponent<Button>();
        this.soundSlider = this.ViewRoot.transform.FindChild("SoundInfo").FindChild("SoundSlider").GetComponent<Slider>();
        this.musicSlider = this.ViewRoot.transform.FindChild("SoundInfo").FindChild("MusicSlider").GetComponent<Slider>();
        this.LanguageToggle = this.ViewRoot.transform.FindChild("LanguageInfo").FindChild("LanguageToggle").GetComponent<Toggle>();
        ApplicationFacade.Instance.RegisterMediator(new SettingMediator(Mediators.HALL_SETTING, this));
        this.soundSlider.value = PlayerPrefs.GetFloat(PrefsKey.SOUNDSET);
        this.musicSlider.value = PlayerPrefs.GetFloat(PrefsKey.MUSICSET);
        this.LanguageToggle.isOn = PlayerPrefs.GetInt(PrefsKey.LUANAGE) > 0 ? true:false ;
    }
    public override void OnShow()
    {
        base.OnShow();
        UIManager.Instance.ShowUIMask(UIViewID.SETTING_VIEW);
        UIManager.Instance.ShowDOTween(this.ViewRoot.GetComponent<RectTransform>());
    }

    public override void OnHide()
    {
        UIManager.Instance.HidenDOTween(this.ViewRoot.GetComponent<RectTransform>(), base.OnHide);
    }
    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Setting/SettingView");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ApplicationFacade.Instance.RemoveMediator(Mediators.HALL_SETTING);
    }
}