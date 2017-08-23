using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Interfaces;
using PureMVC.Patterns;

public class CustomerServiceMediator : Mediator,IMediator {

    public CustomerServiceMediator(string NAME, object viewComponent):base(NAME,viewComponent)
    {

    }

    public CustomerServiceView View
    {
        get
        {
            return (CustomerServiceView)ViewComponent;
        }
    }

    public override void OnRegister()
    {
        base.OnRegister();
        this.View.ButtonAddListening(this.View.CloseButton,
        () =>
        {
            UIManager.Instance.HideUI(UIViewID.CUSTOMERSERVICE_VIEW);
        });
    }
    public override void OnRemove()
    {
        base.OnRemove();
        UIManager.Instance.DestroyUI(UIViewID.CUSTOMERSERVICE_VIEW);
    }
}
