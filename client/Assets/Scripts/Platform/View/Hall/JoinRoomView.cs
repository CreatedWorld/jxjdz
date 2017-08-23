using System;
using System.Collections;
using System.Collections.Generic;
using Platform.Model.Battle;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomView : UIView
{
    /// <summary>
    /// 房间号最大位数
    /// </summary>
    private int roomIDMAX;
    /// <summary>
    /// 关闭按钮
    /// </summary>
    private Button closeButton;
    /// <summary>
    /// 房间号
    /// </summary>
    private List<string> roomNums;
    /// <summary>
    /// 房号显示UIText组件
    /// </summary>
    private List<Text> roomNumTexts;
    /// <summary>
    /// 删除按钮
    /// </summary>
    private Button delButton;
    /// <summary>
    /// 重置按钮
    /// </summary>
    private Button resButton;
    /// <summary>
    /// 房号数字键盘
    /// </summary>
    private Transform numberTrans;
    /// <summary>
    /// 房号数字显示
    /// </summary>
    private Transform roomNumTrans;
    /// <summary>
    /// 自己的中介
    /// </summary>
    private JoinRoomMediator mediator;
    public int RoomIDMAX
    {
        get
        {
            return roomIDMAX;
        }

        set
        {
            roomIDMAX = value;
        }
    }

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

    public List<string> RoomNums
    {
        get
        {
            return roomNums;
        }

        set
        {
            roomNums = value;
        }
    }

    public List<Text> RoomNumTexts
    {
        get
        {
            return roomNumTexts;
        }

        set
        {
            roomNumTexts = value;
        }
    }

    public Button DelButton
    {
        get
        {
            return delButton;
        }

        set
        {
            delButton = value;
        }
    }

    public Button ResButton
    {
        get
        {
            return resButton;
        }

        set
        {
            resButton = value;
        }
    }

    public Transform NumberTrans
    {
        get
        {
            return numberTrans;
        }

        set
        {
            numberTrans = value;
        }
    }

    public Transform RoomNumTrans
    {
        get
        {
            return roomNumTrans;
        }

        set
        {
            roomNumTrans = value;
        }
    }

    public override void OnInit()
    {
        this.RoomNums = new List<string>();
        this.RoomNumTexts = new List<Text>();
        this.ViewRoot = this.LaunchUIView("Prefab/UI/JoinRoom/JoinRoomView");
        this.CloseButton = this.ViewRoot.transform.FindChild("CloseButton").GetComponent<Button>();
        this.NumberTrans = this.ViewRoot.transform.FindChild("Keyboard").FindChild("Number");
        this.RoomNumTrans = this.ViewRoot.transform.FindChild("RoomNum");
        this.DelButton = this.ViewRoot.transform.FindChild("Keyboard").FindChild("Delete").GetComponent<Button>();
        this.ResButton = this.ViewRoot.transform.FindChild("Keyboard").FindChild("Resume").GetComponent<Button>();
        mediator = new JoinRoomMediator(Mediators.HALL_JOINROOM, this);
        ApplicationFacade.Instance.RegisterMediator(mediator);
    }
    public override void OnShow()
    {
        base.OnShow();
        UIManager.Instance.ShowUIMask(UIViewID.JOINROOM_VIEW);
        UIManager.Instance.ShowDOTween(this.ViewRoot.GetComponent<RectTransform>());
    }

    public override void OnHide()
    {
        UIManager.Instance.HidenDOTween(this.ViewRoot.GetComponent<RectTransform>(), base.OnHide);

    }
    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/JoinRoom/JoinRoomView");
    }

    public override void Update()
    {
        mediator.Update();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ApplicationFacade.Instance.RemoveMediator(Mediators.HALL_JOINROOM);
    }
}
