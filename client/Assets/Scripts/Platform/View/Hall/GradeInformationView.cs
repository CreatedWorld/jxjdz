using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 战绩查询
/// </summary>
public class GradeInformationView : UIView
{
    /// <summary>
    /// 关闭按钮
    /// </summary>
    private Button closeButton;
    /// <summary>
    /// 战绩信息滚动条,控制器组件
    /// </summary>
    //private TableView gradeTableView;
    /// <summary>
    /// 房间信息滚动条,控制器组件
    /// </summary>
    private TableView particularsTableView;
    /// <summary>
    /// 房间号ID
    /// </summary>
    private Text roomIDText;
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
    public Text RoomIDText
    {
        get
        {
            return roomIDText;
        }

        set
        {
            roomIDText = value;
        }
    }

    public TableView ParticularsTableView;

    public override void OnInit()
    {
        this.ViewRoot = this.LaunchUIView("Prefab/UI/Grade/GradeInformationView");
        this.ParticularsTableView = this.ViewRoot.transform.FindChild("Grade/ParticularsScrollView").GetComponent<TableView>();
        ApplicationFacade.Instance.RegisterMediator(new GradeInfoViewMediator(Mediators.HALL_GRADEINFO, this));
    }
    public override void OnShow()
    {
        base.OnShow();
        UIManager.Instance.ShowUIMask(UIViewID.GRADEINFORMATION_VIEW);
        UIManager.Instance.ShowDOTween(this.ViewRoot.GetComponent<RectTransform>());
    }

    public override void OnHide()
    {
        UIManager.Instance.HidenDOTween(this.ViewRoot.GetComponent<RectTransform>(),
            () =>
            {
                base.OnHide();
            });
    }
    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Grade/GradeInformationView");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ApplicationFacade.Instance.RemoveMediator(Mediators.HALL_GRADEINFO);
    }
}
