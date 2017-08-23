﻿using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

/// <summary>
///     UI管理器
/// </summary>
public class UIManager
{
    /// <summary>
    /// 蒙板关联的UI数组
    /// </summary>
    private List<UIViewID> maskBindUIList;
    /// <summary>
    ///     UI管理器单例
    /// </summary>
    private static UIManager instance;

    /// <summary>
    ///     UI集合父物体
    /// </summary>
    private readonly GameObject uiRoot;
    /// <summary>
    ///     弹出UI集合父物体
    /// </summary>
    private readonly GameObject popUpUIRoot;
    /// <summary>
    /// UI蒙板
    /// </summary>
    private readonly GameObject mask;

    /// <summary>
    ///     UIView集合,字典形式存储
    /// </summary>
    private readonly Dictionary<UIViewID, UIView> viewDic;
    /// <summary>
    /// 等待标志
    /// </summary>
    private readonly GameObject waitIcon;
    /// <summary>
    /// 旋转标志
    /// </summary>
    private readonly GameObject rotateIcon;

    private readonly Image backgournd;
    /// <summary>
    /// 截屏背景
    /// </summary>
    private RawImage screenBg;
    /// <summary>
    ///     构造函数,初始化UIRoot
    /// </summary>
    private UIManager()
    {
        uiRoot = GameObject.Find("UIRoot");
        popUpUIRoot = GameObject.Find("UIRoot/PopUpUI");
        mask = GameObject.Find("UIRoot/PopUpUI/Mask");
        backgournd = uiRoot.transform.FindChild("Backgournd").GetComponent<Image>();
        screenBg = uiRoot.transform.FindChild("ScreenBg").GetComponent<RawImage>();
        viewDic = new Dictionary<UIViewID, UIView>();
        maskBindUIList = new List<UIViewID>();
    }

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                instance = new UIManager();
            return instance;
        }
    }

    public GameObject UiRoot
    {
        get { return uiRoot; }
    }
    public GameObject PopUpUIRoot
    {
        get { return popUpUIRoot; }
    }

    public Image Backgournd
    {
        get
        {
            return backgournd;
        }
    }

    public void Update()
    {
        foreach (var dic in viewDic)
        {
            var view = dic.Value;
            if (view.IsShow)
            {
                view.Update();
            }
        }
    }

    public void FixedUpdate()
    {
        foreach (var dic in viewDic)
        {
            var view = dic.Value;
            if (view.IsShow)
            {
                view.FixedUpdate();
            }
        }
    }

    /// <summary>
    ///     初始化所有UI
    /// </summary>
    public void InitUI()
    {
        maskBindUIList.Clear();
        foreach (var dic in viewDic)
        {
            var view = dic.Value;
            view.IsInit = false;
            view.IsShow = false;
            view.OnDestroy();
            if (view.ViewRoot != null)
                Object.Destroy(view.ViewRoot);
            view.ViewRoot = null;
        }
        if (mask != null)
        {
            mask.SetActive(false);
        }
    }

    /// <summary>
    ///     显示UIView
    /// </summary>
    /// <param name="viewID">UIView的ID</param>
    public UIView ShowUI(UIViewID viewID)
    {
        if (viewDic.ContainsKey(viewID))
        {
            var view = viewDic[viewID];
            if (!view.IsInit)
            {
                view.OnInit();
                view.IsInit = true;
            }
            else
            {
                Debug.Log("View已初始化:" + viewID);
            }
            if (!view.IsShow)
            {
                view.OnShow();
                view.IsShow = true;
            }
            else
            {
                Debug.Log("View已打开:" + viewID);
            }
            BringToTop(viewID);
            return view;
        }
        Debug.LogError("无效ViewID:" + viewID);
        return null;
    }
    /// <summary>
    /// 设置UI到顶层
    /// </summary>
    public void BringToTop(UIViewID viewID)
    {
        if (viewDic.ContainsKey(viewID))
        {
            var view = viewDic[viewID];
            if (!view.IsInit)
            {
                return;
            }
            var nextUI = GetUIView(viewID);
            nextUI.ViewRoot.transform.SetSiblingIndex(nextUI.ViewRoot.transform.parent.childCount - 1);
        }
    }
    /// <summary>
    /// 设置UI到底层
    /// </summary>
    /// <param name="viewID"></param>
    public void BringToBottom(UIViewID viewID)
    {
        if (viewDic.ContainsKey(viewID))
        {
            var view = viewDic[viewID];
            if (!view.IsInit)
            {
                return;
            }
            var nextUI = GetUIView(viewID);
            nextUI.ViewRoot.transform.SetSiblingIndex(0);
        }
    }
    /// <summary>
    ///     隐藏UIView
    /// </summary>
    /// <param name="viewID">UIView的ID</param>
    public void HideUI(UIViewID viewID)
    {
        if (viewDic.ContainsKey(viewID))
        {
            var view = viewDic[viewID];
            if (view.IsShow)
            {
                view.OnHide();
                view.IsShow = false;
            }
            else
            {
                Debug.Log("View已关闭:" + viewID);
            }
        }
        else
        {
            Debug.LogError("无效ViewID:" + viewID);
        }
        maskBindUIList.Remove(viewID);
        if (maskBindUIList.Count == 0)
        {
            HidenUIMask();
        }
        else
        {
            ResortMaskLayer(maskBindUIList[maskBindUIList.Count - 1]);
        }
    }
    /// <summary>
    /// 参数回调
    /// </summary>
    /// <param name="viewID"></param>
    /// <param name="action"></param>
    public void HideUI(UIViewID viewID,Action action)
    {
        if (viewDic.ContainsKey(viewID))
        {
            var view = viewDic[viewID];
            if (view.IsShow)
            {
                view.OnHide(action);
                view.IsShow = false;
            }
            else
            {
                Debug.Log("View已关闭:" + viewID);
            }
        }
        else
        {
            Debug.LogError("无效ViewID:" + viewID);
        }
        maskBindUIList.Remove(viewID);
        if (maskBindUIList.Count == 0)
        {
            HidenUIMask();
        }
        else
        {
            ResortMaskLayer(maskBindUIList[maskBindUIList.Count - 1]);
        }
    }
    /// <summary>
    ///     删除UIView
    /// </summary>
    /// <param name="viewID">UIView的ID</param>
    public void DestroyUI(UIViewID viewID)
    {
        if (viewDic.ContainsKey(viewID))
        {
            var view = viewDic[viewID];
            view.OnDestroy();
            view.IsInit = false;
            view.IsShow = false;
        }
        else
        {
            Debug.LogError("无效ViewID:" + viewID);
        }
        maskBindUIList.Remove(viewID);
        if (maskBindUIList.Count == 0)
        {
            HidenUIMask();
        }
        else
        {
            var nextUI = GetUIView(maskBindUIList[maskBindUIList.Count - 1]);
            var viewIndex = nextUI.ViewRoot.transform.GetSiblingIndex();
            mask.transform.SetSiblingIndex(viewIndex - 1);
        }
    }

    /// <summary>
    ///     注册UIView
    /// </summary>
    /// <param name="viewID">UIView的ID</param>
    /// <param name="uiView">UIView对象</param>
    public void RegisterUI(UIViewID viewID, UIView uiView)
    {
        if (uiView != null)
            if (!viewDic.ContainsKey(viewID))
            {
                viewDic.Add(viewID, uiView);
                uiView.OnRegister();
            }
            else
            {
                Debug.LogError("该UI:" + viewID + "已存在,请勿重复注册");
            }
        else
            Debug.LogError("注册失败,无效ViewID:" + viewID);
    }

    /// <summary>
    ///     移除UIView注册
    /// </summary>
    /// <param name="viewID">UIView的ID</param>
    public void RemoveUI(UIViewID viewID)
    {
        if (viewDic.ContainsKey(viewID))
        {
            var view = viewDic[viewID];
            view.OnRemove();
            viewDic.Remove(viewID);
            view = null;
        }
        else
        {
            Debug.LogError("无效ViewID:" + viewID);
        }
    }

    /// <summary>
    ///     获取UIView
    /// </summary>
    /// <param name="viewID">UIView的ID</param>
    /// <returns></returns>
    public UIView GetUIView(UIViewID viewID)
    {
        if (viewDic.ContainsKey(viewID))
        {
            return viewDic[viewID];
        }
        Debug.LogError("无效ViewID:" + viewID);
        throw new NullReferenceException();
    }

    /// <summary>
    ///     创建UIView,UIRoot下
    /// </summary>
    /// <param PlayerName="path">路径</param>
    /// <returns></returns>
    public GameObject CreateUIView(string path)
    {
        return CreateUIView(path, popUpUIRoot.transform);
    }

    /// <summary>
    ///     创建UIView
    /// </summary>
    /// <param PlayerName="path">路径</param>
    /// <param PlayerName="parent">父物体</param>
    /// <returns></returns>
    public GameObject CreateUIView(string path, Transform parent)
    {
        var uiView = Object.Instantiate(Resources.Load<GameObject>(path), parent);
        UIViewResetTrans(uiView.transform);
        return uiView;
    }

    public GameObject CreateUIView(GameObject viewCache)
    {
        return CreateUIView(viewCache, popUpUIRoot.transform);
    }

    public GameObject CreateUIView(GameObject viewCache, Transform parent)
    {
        var uiView = Object.Instantiate(viewCache, parent);
        UIViewResetTrans(uiView.transform);
        return uiView;
    }

    /// <summary>
    ///     重新设置UI的变换
    /// </summary>
    /// <param name="trans">UIView物体</param>
    private void UIViewResetTrans(Transform trans)
    {
        trans.localPosition = Vector3.zero;
        trans.rotation = new Quaternion(0, 0, 0, 0);
        trans.localScale = Vector3.one;
    }

    /// <summary>
    /// 播放显示缓动
    /// </summary>
    /// <param name="rectTransform"></param>
    public void ShowDOTween(RectTransform rectTransform)
    {
        rectTransform.DOKill();
        rectTransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        rectTransform.DOScale(1f, 0.2f).SetEase(Ease.Linear);
        rectTransform.DOScale(1.1f, 0.125f).SetDelay(0.2f).SetEase(Ease.Linear);
        rectTransform.DOScale(0.9f, 0.125f).SetDelay(0.316f).SetEase(Ease.Linear);
        rectTransform.DOScale(1f, 0.125f).SetDelay(0.441f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// 播放隐藏缓动
    /// </summary>
    /// <param name="rectTransform"></param>
    public void HidenDOTween(RectTransform rectTransform,TweenCallback hidenCallback)
    {
        rectTransform.DOKill();
        rectTransform.localScale = Vector3.one;
        rectTransform.DOScale(1.1f, 0.2f).SetEase(Ease.Linear);
        rectTransform.DOScale(0f, 0.15f).SetDelay(0.2f).SetEase(Ease.Linear).OnComplete(hidenCallback);
    }

    /// <summary>
    /// 显示界面蒙板
    /// </summary>
    /// <param name="uiObj">蒙板关联的UIId</param>
    public void ShowUIMask(UIViewID uiid)
    {
        mask.SetActive(true);
        mask.GetComponent<Image>().DOKill();
        mask.GetComponent<Image>().color = Color.clear;
        mask.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0.7f), 0.3f);
        ResortMaskLayer(uiid);
        if (!maskBindUIList.Contains(uiid))
        {
            maskBindUIList.Add(uiid);
        }
    }

    /// <summary>
    /// 重排蒙板层次
    /// </summary>
    /// <param name="bindUIId"></param>
    private void ResortMaskLayer(UIViewID bindUIId)
    {
        var uiView = GetUIView(bindUIId);
        var childArr = new List<Transform>();
        var resultArr = new List<Transform>();
        foreach (Transform transform in popUpUIRoot.transform)
        {
            if (transform.gameObject != mask)
            {
                childArr.Add(transform);
            }
        }
        foreach (Transform transform in childArr)
        {
            if (transform != uiView.ViewRoot.transform)
            {
                resultArr.Add(transform);
            }
            else
            {
                resultArr.Add(mask.transform);
                resultArr.Add(uiView.ViewRoot.transform);
            }
        }
        for (int i = 0; i < resultArr.Count; i++)
        {
            resultArr[i].SetSiblingIndex(i);
        }
    }

    /// <summary>
    /// 缓动隐藏蒙板
    /// </summary>
    private void HidenUIMask()
    {
        mask.GetComponent<Image>().DOKill();
        mask.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0f), 0.5f).OnComplete(() =>
        {
            mask.SetActive(false);
        });
    }
    /// <summary>
    /// 是否需要截屏
    /// </summary>
    public bool needSaveScreen = false;
    /// <summary>
    /// 截屏回调
    /// </summary>
    private Action<Texture2D> saveCallBack;
    /// <summary>
    /// 开始截屏
    /// </summary>
    /// <param name="callBack">截屏回调</param>
    public void StartSaveScreen(Action<Texture2D> callBack)
    {
        saveCallBack = callBack;
        needSaveScreen = true;
    }
    /// <summary> 
    /// 获取截屏内容
    /// </summary>
    /// <returns></returns>
    public void SaveScreenTexture()
    {
        // 先创建一个的空纹理，大小可根据实现需要来设置  
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);

        // 读取屏幕像素信息并存储为纹理数据，  
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShot.Apply();
        needSaveScreen = false;
        saveCallBack(screenShot);
    }
    /// <summary>
    /// 保存截屏
    /// </summary>
    /// <summary>
    /// 保存截屏
    /// </summary>
    public void UpdateScreenBg(Texture2D screenShot)
    {
        screenBg.texture = screenShot;
        screenBg.gameObject.SetActive(true);
        screenBg.color = Color.white;
    }

    /// <summary>
    /// 隐藏截屏
    /// </summary>
    public void HidenScreenBg()
    {
        screenBg.DOColor(new Color(1, 1, 1, 0), 0.5f).OnComplete(ScreenHidenComplete);
    }

    /// <summary>
    /// 截屏内容隐藏完成
    /// </summary>
    private void ScreenHidenComplete()
    {
        screenBg.gameObject.SetActive(false);
        ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_GAMEMGR_SCENE_CHANGED);
    }

    /// <summary>
    /// 设置等待标志显示 TODO...
    /// </summary>
    /// <param name="value"></param>
    public void SetWaitIconActive(bool value)
    {
        if (GlobalData.LoginServer == "127.0.0.1")
        {
            return;
        }
        if (value)
        {
            if (!waitIcon.activeSelf)
            {
                waitIcon.SetActive(value);
                waitIcon.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                rotateIcon.SetActive(false);
                waitIcon.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0.4f), 0.1f).SetDelay(2f).OnComplete(() => {
                    rotateIcon.SetActive(true);
                });
            }
        }
        else
        {
            if (waitIcon.activeSelf)
            {
                waitIcon.GetComponent<Image>().DOKill();
                waitIcon.SetActive(value);
            }
        }
    }
}
