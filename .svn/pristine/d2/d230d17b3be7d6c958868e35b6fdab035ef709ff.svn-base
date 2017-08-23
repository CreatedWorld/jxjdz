using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 战绩查询
/// </summary>
public class GradeView : UIView
{
    /// <summary>
    /// 关闭按钮
    /// </summary>
    private Button closeButton;
    /// <summary>
    /// 战绩信息滚动条
    /// </summary>
    private GameObject recordScrollView;
    /// <summary>
    /// 房间信息滚动条
    /// </summary>
    private GameObject particularsScrollView;
    /// <summary>
    /// 战绩信息滚动条,控制器组件
    /// </summary>
    //private TableView gradeTableView;
    /// <summary>
    /// 房间信息滚动条,控制器组件
    /// </summary>
    private TableView particularsTableView;
    /// <summary>
    /// 生成战绩信息滚动条信息集合
    /// </summary>
    private List<GradeScrollView> gradeScrollList;
    /// <summary>
    /// 生成房间信息滚动条信息集合
    /// </summary>
    private List<ParticularsScrollView> particularsScrollList;
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

    public TableView ParticularsTableView;


    public override void OnInit()
    {
        this.ViewRoot = this.LaunchUIView("Prefab/UI/Grade/GradeView");
        this.ParticularsTableView = this.ViewRoot.transform.FindChild("Grade/ParticularsScrollView").GetComponent<TableView>();
        ApplicationFacade.Instance.RegisterMediator(new GradeMediator(Mediators.HALL_GRADE, this));
      
    }
    public override void OnShow()
    {
        base.OnShow();
        UIManager.Instance.ShowUIMask(UIViewID.GRADE_VIEW);
        UIManager.Instance.ShowDOTween(this.ViewRoot.GetComponent<RectTransform>());
    }

    public override void OnHide()
    {
        UIManager.Instance.HidenDOTween(this.ViewRoot.GetComponent<RectTransform>(), 
            ()=> 
            {
                base.OnHide();
            });
    }
    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/Grade/GradeView");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ApplicationFacade.Instance.RemoveMediator(Mediators.HALL_GRADE);
    }
}
