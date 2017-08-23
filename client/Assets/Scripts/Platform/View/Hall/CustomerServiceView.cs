using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerServiceView : UIView {

    private Button closeButton;

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

    public override void OnInit()
    {
        this.ViewRoot = this.LaunchUIView("Prefab/UI/CustomerService/CustomerService");
        this.CloseButton = this.ViewRoot.transform.FindChild("CloseButton").GetComponent<Button>();
        ApplicationFacade.Instance.RegisterMediator(new CustomerServiceMediator(Mediators.HALL_CUSTOMERSERVICE, this));
    }
    public override void OnShow()
    {
        base.OnShow();
        UIManager.Instance.ShowUIMask(UIViewID.CUSTOMERSERVICE_VIEW);
        UIManager.Instance.ShowDOTween(this.ViewRoot.GetComponent<RectTransform>());
    }

    public override void OnHide()
    {
        UIManager.Instance.HidenDOTween(this.ViewRoot.GetComponent<RectTransform>(), base.OnHide);

    }
    public override void OnRegister()
    {
        this.ViewRootCache = Resources.Load<GameObject>("Prefab/UI/CustomerService/CustomerService");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ApplicationFacade.Instance.RemoveMediator(Mediators.HALL_CUSTOMERSERVICE);
    }
}
