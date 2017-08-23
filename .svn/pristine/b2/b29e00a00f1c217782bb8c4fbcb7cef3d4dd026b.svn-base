using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class CreatHeapCard : Editor
{
    static float DownXpoint = -4.0f;
    static float DownXSpace = 0.52f;

    [MenuItem("恩赐方/摆牌")]
    public static void CreatCard()
    {
        CreateHeapCard();
    }

    [MenuItem("恩赐方/清除所有牌")]
    public static void ClearCard()
    {
        GameObject DownHeapCard = GameObject.Find("BattleMgr/DownArea/HeapCard");
        GameObject RightHeapCard = GameObject.Find("BattleMgr/RightArea/HeapCard");
        GameObject UpHeapCard = GameObject.Find("BattleMgr/UpArea/HeapCard");
        GameObject LeftHeapCard = GameObject.Find("BattleMgr/LeftArea/HeapCard");
        while (DownHeapCard.transform.childCount > 0)
        {
            DestroyImmediate(DownHeapCard.transform.GetChild(0).gameObject);
        }
        while (RightHeapCard.transform.childCount > 0)
        {
            DestroyImmediate(RightHeapCard.transform.GetChild(0).gameObject);
        }
        while (UpHeapCard.transform.childCount > 0)
        {
            DestroyImmediate(UpHeapCard.transform.GetChild(0).gameObject);
        }
        while (LeftHeapCard.transform.childCount > 0)
        {
            DestroyImmediate(LeftHeapCard.transform.GetChild(0).gameObject);
        }

    }

    public static void CreateHeapCard()
    {
        #region
        Transform DownHeapCard = GameObject.Find("BattleMgr/DownArea/HeapCard").transform;
        if (DownHeapCard.childCount ==0)
        {
            int dk = 1;
            for (int i = 0; i < 18; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    GameObject go = Instantiate(Resources.Load("Models/1S")) as GameObject;
                    go.transform.parent = DownHeapCard;
                    go.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    go.transform.localScale = new Vector3(1, 1, 1);
                    go.name = "PutCard" + (dk++);
                    if (j == 0)
                    {
                        go.transform.localPosition = new Vector3(DownXpoint + DownXSpace * i, 0.33f, 0);
                    }
                    else
                    {
                        go.transform.localPosition = new Vector3(DownXpoint + DownXSpace * i, 0, 0);
                    }
                }
            } 
        }
        #endregion
        #region
        Transform UpHeapCard = GameObject.Find("BattleMgr/UpArea/HeapCard").transform;
        if (UpHeapCard.childCount == 0)
        {
            int uk = 1;
            for (int i = 0; i < 18; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    GameObject go = Instantiate(Resources.Load("Models/1S")) as GameObject;
                    go.transform.parent = UpHeapCard;
                    go.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    go.transform.localScale = new Vector3(1, 1, 1);
                    go.name = "PutCard" + (uk++);
                    if (j == 0)
                    {
                        go.transform.localPosition = new Vector3(DownXpoint + DownXSpace * i, 0.33f, 0);
                    }
                    else
                    {
                        go.transform.localPosition = new Vector3(DownXpoint + DownXSpace * i, 0, 0);
                    }
                }
            }
        }
        #endregion
        #region
        Transform RightHeapCard = GameObject.Find("BattleMgr/RightArea/HeapCard").transform;
        if (RightHeapCard.childCount ==0)
        {
            int rk = 1;
            for (int i = 0; i < 18; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    GameObject go = Instantiate(Resources.Load("Models/1S")) as GameObject;
                    go.transform.parent = RightHeapCard;
                    go.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    go.transform.localScale = new Vector3(1, 1, 1);
                    go.name = "PutCard" + (rk++);
                    if (j == 0)
                    {
                        go.transform.localPosition = new Vector3(DownXpoint + DownXSpace * i, 0.33f, 0);
                    }
                    else
                    {
                        go.transform.localPosition = new Vector3(DownXpoint + DownXSpace * i, 0, 0);
                    }
                }
            } 
        }
        #endregion
        #region
        Transform LeftHeapCard = GameObject.Find("BattleMgr/LeftArea/HeapCard").transform;
        if (LeftHeapCard.childCount == 0)
        {
            int lk = 1;
            for (int i = 0; i < 18; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    GameObject go = Instantiate(Resources.Load("Models/1S")) as GameObject;
                    go.transform.parent = LeftHeapCard;
                    go.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    go.transform.localScale = new Vector3(1, 1, 1);
                    go.name = "PutCard" + (lk++);
                    if (j == 0)
                    {
                        go.transform.localPosition = new Vector3(DownXpoint + DownXSpace * i, 0.33f, 0);
                    }
                    else
                    {
                        go.transform.localPosition = new Vector3(DownXpoint + DownXSpace * i, 0, 0);
                    }
                }
            } 
        }
        #endregion
    }
    void OnInspectorUpdate()
    {
        this.Repaint();
    }
}
