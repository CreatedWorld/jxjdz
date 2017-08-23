using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class UIView : IUIView
{
    public delegate void UIViewAction(params object[] parameter);
    private GameObject viewRootCache;
    public GameObject ViewRootCache
    {
        get
        {
            return viewRootCache;
        }
        set
        {
            viewRootCache = value;
        }
    }
    private GameObject viewRoot;
    public GameObject ViewRoot
    {
        get
        {
            return viewRoot;
        }
        set
        {
            viewRoot = value;
        }
    }
    private bool isInit;
    public bool IsInit
    {
        get
        {
            return isInit;
        }
        set
        {
            isInit = value;
        }
    }
    private bool isShow;
    public bool IsShow
    {
        get
        {
            return isShow;
        }
        set
        {
            isShow = value;
        }
    }
    public UIView()
    {
        this.ViewRootCache = null;
    }

    public virtual void Update()
    {
        
    }

    public virtual void FixedUpdate()
    {
        
    }
    public abstract void OnInit();
    public abstract void OnRegister();
    public virtual void OnShow()
    {
        this.ViewRoot.SetActive(true);
    }
    public virtual void OnHide()
    {
        this.ViewRoot.SetActive(false);
    }
    public virtual void OnHide(Action callBack)
    {
        UIManager.Instance.HidenDOTween(this.ViewRoot.GetComponent<RectTransform>(),
        () =>
        {
            callBack();
        });
    }
    public virtual void OnDestroy()
    {
        if (this.ViewRoot != null)
        {
            GameObject.Destroy(this.ViewRoot);
        }
    }
    public virtual void OnRemove()
    {
        if (this.ViewRoot != null)
        {
            GameObject.Destroy(this.ViewRoot);
        }
        if (this.ViewRootCache != null)
        {
            this.ViewRootCache = null;
        }
    }
    public GameObject LaunchUIView(string path, Transform parent = null)
    {
        GameObject viewRoot = null;
        if (this.ViewRootCache == null)
        {
            if (parent == null)
            {
                viewRoot = UIManager.Instance.CreateUIView(path);
            }
            else
            {
                viewRoot = UIManager.Instance.CreateUIView(path, parent);
            }
        }
        else
        {
            if (parent == null)
            {
                viewRoot = UIManager.Instance.CreateUIView(this.ViewRootCache);
            }
            else
            {
                viewRoot = UIManager.Instance.CreateUIView(this.ViewRootCache, parent);
            }
        }
        return viewRoot;
    }
    public void ButtonAddListening(Button button, UnityAction action,bool isScale = false)
    {
        button.onClick.AddListener(action);
        //if (isScale)
        //{
        //    this.ButtonAddScaleResult(button);
        //}
    }
    public void ButtonAddListening(Button button, UIViewAction callBack, bool isScale,params object[] parameter)
    {
        button.onClick.AddListener(()=> { callBack(parameter);});
        //if (isScale)
        //{
        //    this.ButtonAddScaleResult(button);
        //}
    }

    private void ButtonAddScaleResult(Button button)
    {
        EventTrigger buttonTrigger = button.GetComponent<EventTrigger>();
        buttonTrigger.triggers = new List<EventTrigger.Entry>();

        EventTrigger.Entry pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener(
            (BaseEventData arg0) => 
            {
                button.transform.localScale = new Vector3(1.15f,1.15f,1.15f);
            });

        EventTrigger.Entry pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener(
            (BaseEventData arg0) =>
            {
                button.transform.localScale = new Vector3(1f, 1f, 1f);
            });
        buttonTrigger.triggers.Add(pointerUp);
        buttonTrigger.triggers.Add(pointerDown);
    }
}
