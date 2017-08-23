using Platform.Model;
using Platform.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 战绩信息View
/// </summary>
public class GradeScrollView : TableViewItem {

    /// <summary>
    /// 是否初始化
    /// </summary>
    private bool isInit;
    /// <summary>
    /// 房间ID
    /// </summary>
    private int roomID;
    /// <summary>
    /// 显示房间号
    /// </summary>
    private Text roomCodeTxt;
    /// <summary>
    /// 时间
    /// </summary>
    private Text timeTxt;
    /// <summary>
    /// 选择按钮
    /// </summary>
    private Button selectButton;
    /// <summary>
    /// 当前条目数据
    /// </summary>
    private GradeDataS2C scrollViewData;
    /// <summary>
    /// 输赢结果
    /// </summary>
    private Image resulteImg;
    /// <summary>
    /// 分数
    /// </summary>
    private Text score;

    private string roomIdStr;
    public Button SelectButton
    {
        get
        {
            return selectButton;
        }
    }

    public int RoomID
    {
        get
        {
            return roomID;
        }
    }

    public string RoomIdStr
    {
        get
        {
            return roomIdStr;
        }

        set
        {
            roomIdStr = value;
        }
    }

    public override void Updata(object data)
    {
        if (data == null)
        {
            return;
        }
        base.Updata(data);
        this.scrollViewData = (GradeDataS2C)data;
        if (scrollViewData.score > 0)
        {
            resulteImg.sprite = Resources.Load<Sprite>("Textures/UI/赢");
            score.text = "+" + scrollViewData.score;
            score.color = new Color(255, 241, 0);
        }
        if (scrollViewData.score < 0)
        {
            resulteImg.sprite = Resources.Load<Sprite>("Textures/UI/输");
            score.text = "" + scrollViewData.score;
            score.color = new Color(0, 171, 255);
        }
        if (scrollViewData.score == 0)
        {
            resulteImg.sprite = Resources.Load<Sprite>("Textures/UI/平");
            score.text = "0";
            score.color = new Color(0, 255, 0);
        }
        this.roomID = scrollViewData.roomID;
        this.roomCodeTxt.text = this.scrollViewData.roomCode;
        this.timeTxt.text = TimeHandle.Instance.GetDateTimeByTimestamp(this.scrollViewData.time).ToString("yy-MM-dd HH:mm:ss");
        //for (int i = 0;i < this.ScrollViewData.UsersInfo.Count ;i++)
        //{
        //    this.userNames[i].text = this.ScrollViewData.UsersInfo[i].userName + ":" + this.ScrollViewData.UsersInfo[i].score.ToString();
        //};
    }

    private void Awake()
    {
        resulteImg = transform.FindChild("Result").GetComponent<Image>();
        this.roomCodeTxt = this.transform.FindChild("RoomNumText").GetComponent<Text>();
        this.timeTxt = this.transform.FindChild("RoundTime").GetComponent<Text>();
        score = transform.FindChild("ScoreText").GetComponent<Text>();
        this.selectButton = this.transform.FindChild("RecordInformationButton").GetComponent<Button>();
        selectButton.onClick.AddListener(SelectHandler);
    }

    private void SelectHandler()
    {
        GetRoundInfoC2S package = new GetRoundInfoC2S();
        package.roomID = roomID;
        NetMgr.Instance.SendBuff<GetRoundInfoC2S>(SocketType.HALL, MsgNoC2S.REQUEST_GETROUNDINFOC_C2S.GetHashCode(), 0, package);
        UIManager.Instance.ShowUI(UIViewID.GRADEINFORMATION_VIEW);
    }
}
