using UnityEngine;
public class HallMgr : MonoBehaviour {
    private HallView hallView;
    private TopMenuView topView;
    private MiddleMenuView middleView;
    public TopMenuView TopView
    {
        get
        {
            return topView;
        }

        set
        {
            topView = value;
        }
    }

    public MiddleMenuView MiddleView
    {
        get
        {
            return middleView;
        }

        set
        {
            middleView = value;
        }
    }


    public HallView HallView
    {
        get
        {
            return hallView;
        }

        set
        {
            hallView = value;
        }
    }

    void Awake()
    {
        ApplicationFacade.Instance.RegisterMediator(new HallMediator(Mediators.HALL_MEDIATOR,this));
    }
    void Start ()
    {
        ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_HALL_REFRESHUSERINFO);
        //ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_HALL_REFRESHHALLNOTICE);
    }
    void Update () {
        this.RollAnnouncement();
    }
    void OnDestroy()
    {
        ApplicationFacade.Instance.RemoveMediator(Mediators.HALL_MEDIATOR);
        UIManager.Instance.Backgournd.gameObject.SetActive(false);
    }
    /// <summary>
    /// 跑马灯
    /// </summary>
    private void RollAnnouncement()
    {
        if (this.MiddleView.AnnouncementText == null || this.MiddleView.AnnouncementText.text == "")
        {
            return;
        }
        this.MiddleView.AnnouncementText.transform.Translate(Vector3.right * Time.deltaTime * -0.5f);
        if (this.MiddleView.AnnouncementText.rectTransform.localPosition.x < -510)
        {
            ApplicationFacade.Instance.SendNotification(NotificationConstant.MEDI_HALL_ANNOUNCEMENTFINISH);
        }
    }
}
