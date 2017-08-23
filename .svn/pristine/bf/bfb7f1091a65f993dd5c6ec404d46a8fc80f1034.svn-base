using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;
using System.Collections;

public class ShoppingMediator : Mediator, IMediator
{
    public ShoppingMediator(string NAME, object viewComponent):base(NAME,viewComponent)
    {

    }

    public ShoppingView View
    {
        get
        {
            return (ShoppingView)ViewComponent;
        }
    }

    public override void OnRegister()
    {
        base.OnRegister();
        this.View.ButtonAddListening(this.View.CloseButton,
        () =>
        {
            UIManager.Instance.HideUI(UIViewID.SHOPPING_VIEW);
        });
        this.View.ButtonAddListening(this.View.ServiceButton,
            ()=>
            {
                UIManager.Instance.HideUI(UIViewID.SHOPPING_VIEW);
                UIManager.Instance.ShowUI(UIViewID.CUSTOMERSERVICE_VIEW);
            });
    }
    public override void OnRemove()
    {
        base.OnRemove();
        UIManager.Instance.DestroyUI(UIViewID.SHOPPING_VIEW);
    }
}

