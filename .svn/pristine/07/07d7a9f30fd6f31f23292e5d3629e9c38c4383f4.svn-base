using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Platform.Model;
using System;
using Platform.Net;
/// <summary>
/// 对战信息View
/// </summary>
struct player
{
   public  string score;
    public string name;
    public Sprite rectangleImg;
    public Sprite ineframeImg;
    public string headWWW;
    public Color color;
}
public class ParticularsScrollView : TableViewItem
{
    /// <summary>
    /// 是否初始化
    /// </summary>
    private bool isInit;


    /// <summary>
    /// 房间号
    /// </summary>
    private int roomNum;

    /// <summary>
    /// 时间
    /// </summary>
    private Text time;

    /// <summary>
    /// 玩家1分数
    /// </summary>
    private Text score_1;

    /// <summary>
    /// 玩家2分数
    /// </summary>
    private Text score_2;

    /// <summary>
    /// 玩家3分数
    /// </summary>
    private Text score_3;

    /// <summary>
    /// 玩家4分数
    /// </summary>
    private Text score_4;


    /// <summary>
    /// 玩家1名字
    /// </summary>
    private Text playername_1;

    /// <summary>
    /// 玩家2名字
    /// </summary>
    private Text playername_2;

    /// <summary>
    /// 玩家3名字
    /// </summary>
    private Text playername_3;

    /// <summary>
    /// 玩家4名字
    /// </summary>
    private Text playername_4;


    /// <summary>
    /// 玩家1背景矩形
    /// </summary>
    private Image rectangleImg_1;

    /// <summary>
    /// 玩家2背景矩形
    /// </summary>
    private Image rectangleImg_2;

    /// <summary>
    /// 玩家3背景矩形
    /// </summary>
    private Image rectangleImg_3;

    /// <summary>
    /// 玩家4背景矩形
    /// </summary>
    private Image rectangleImg_4;

    /// <summary>
    /// 玩家1背景线框
    /// </summary>
    private Image ineframeImg_1;

    /// <summary>
    /// 玩家2背景线框
    /// </summary>
    private Image ineframeImg_2;

    /// <summary>
    /// 玩家3背景线框
    /// </summary>
    private Image ineframeImg_3;

    /// <summary>
    /// 玩家4背景线框
    /// </summary>
    private Image ineframeImg_4;

    /// <summary>
    /// 玩家1头像
    /// </summary>
    private RawImage headRawImg_1;

    /// <summary>
    /// 玩家2头像
    /// </summary>
    private RawImage headRawImg_2;

    /// <summary>
    /// 玩家3头像
    /// </summary>
    private RawImage headRawImg_3;

    /// <summary>
    /// 玩家4头像
    /// </summary>
    private RawImage headRawImg_4;

    /// <summary>
    /// 对战信息数据
    /// </summary>
    private RoundData particularsScrollData;

    /// <summary>
    /// 打开战绩信息的按钮
    /// </summary>
    private Button recordInformationButton;
    /// <summary>
    /// 四个玩家的数据
    /// </summary> 
    private Dictionary<int, player> playerDic;

    /// <summary>
    /// 背景矩形
    /// </summary>
    private Dictionary<int, Image> rectangleImgDic;

    /// <summary>
    /// 背景线框
    /// </summary>
    private Dictionary<int, Image> ineframeImgDic;

    /// <summary>
    /// 局数
    /// </summary>
    private Text gamesNumberText;

    private int gamesNumber;

    private int reportId;
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

    public Button PlaybackButton
    {
        get
        {
            return PlaybackButton;
        }
    }
    public Button RecordInformationButton
    {
        get
        {
            return recordInformationButton;
        }
    }





    public RoundData gradeData;

    public ParticularsScrollView()
    {
        playerDic = new Dictionary<int, player>();
        this.IsInit = false;
    }
    public override void Updata(object data)
    {
        if (data == null)
        {
            return;
        }
        base.Updata(data);
        //this.particularsScrollData = (ParticularsScrollData)data;
        gradeData = (RoundData)data;

        for (int i = 0; i < gradeData.usersInfo.Count; i++)
        {
            if (gradeData.usersInfo[i].score > 0)
            {
                player _player;
                _player = new player();
         
                _player.rectangleImg = Resources.Load<Sprite>("Textures/UI/黄色矩形");
                _player.ineframeImg = Resources.Load<Sprite>("Textures/UI/黄色头像框");
                _player.score = "+" + gradeData.usersInfo[i].score;
                _player.name = gradeData.usersInfo[i].userName;
                _player.headWWW = gradeData.usersInfo[i].avatar;
                _player.color = new Color(255, 241, 0);
                playerDic[i] = _player;
                //score.color = new Color(255, 241, 0);
            }
            if (gradeData.usersInfo[i].score < 0)
            {
                player _player;
                _player = new player();
                _player.rectangleImg = Resources.Load<Sprite>("Textures/UI/蓝色矩形");
                _player.ineframeImg = Resources.Load<Sprite>("Textures/UI/蓝色头像框");
                _player.score = "" + gradeData.usersInfo[i].score;
                _player.name = gradeData.usersInfo[i].userName;
                _player.headWWW = gradeData.usersInfo[i].avatar;
                _player.color = new Color(0, 171, 255);
                playerDic[i] = _player;
                //score.color = new Color(0, 171, 255);
            }
            if (gradeData.usersInfo[i].score == 0)
            {
                player _player;
                _player = new player();
                _player.score = "0";
                _player.name = gradeData.usersInfo[i].userName;
                _player.headWWW = gradeData.usersInfo[i].avatar;
                _player.color = new Color(0, 255, 0);
                playerDic[i] = _player;
                //score.color = new Color(0, 255, 0);
            }

        }
        score_1.text = playerDic[0].score;
        score_1.color = playerDic[0].color;
        score_2.text = playerDic[1].score;
        score_2.color = playerDic[1].color;
        score_3.text = playerDic[2].score;
        score_3.color = playerDic[2].color;
        score_4.text = playerDic[3].score;
        score_4.color = playerDic[3].color;
        rectangleImg_1.sprite = playerDic[0].rectangleImg;
        rectangleImg_2.sprite = playerDic[1].rectangleImg;
        rectangleImg_3.sprite = playerDic[2].rectangleImg;
        rectangleImg_4.sprite = playerDic[3].rectangleImg;
        ineframeImg_1.sprite = playerDic[0].ineframeImg;
        ineframeImg_2.sprite = playerDic[1].ineframeImg;
        ineframeImg_3.sprite = playerDic[2].ineframeImg;
        ineframeImg_4.sprite = playerDic[3].ineframeImg;
        playername_1.text = playerDic[0].name;
        playername_2.text = playerDic[1].name;
        playername_3.text = playerDic[2].name;
        playername_4.text = playerDic[3].name;
        GameMgr.Instance.StartCoroutine(DownIcon(playerDic[0].headWWW, headRawImg_1));
        GameMgr.Instance.StartCoroutine(DownIcon(playerDic[1].headWWW, headRawImg_2));
        GameMgr.Instance.StartCoroutine(DownIcon(playerDic[2].headWWW, headRawImg_3));
        GameMgr.Instance.StartCoroutine(DownIcon(playerDic[3].headWWW, headRawImg_4));

        reportId = gradeData.reportId;
        gamesNumber = GradeInfoViewMediator.gamesNumberDic[gradeData] + 1;
        gamesNumberText.text = System.Convert.ToString(gamesNumber);
        time.text = SwitchTime(gradeData.time);
        roomNum = gradeData.roomID;
    }

    private void Awake()
    {
        if (!this.IsInit)
        {
            // resulteImg = transform.FindChild("Result").GetComponent<Image>();
            // roomNumText = transform.FindChild("RoomNumText").GetComponent<Text>();
            time = transform.FindChild("RoundTime").GetComponent<Text>();
            //score = transform.FindChild("ScoreText").GetComponent<Text>();
            score_1 = transform.FindChild("Head_BG_1/ScoreText").GetComponent<Text>();
            score_2 = transform.FindChild("Head_BG_2/ScoreText").GetComponent<Text>();
            score_3 = transform.FindChild("Head_BG_3/ScoreText").GetComponent<Text>();
            score_4 = transform.FindChild("Head_BG_4/ScoreText").GetComponent<Text>();
            rectangleImg_1 = transform.FindChild("Head_BG_1/color_BG").GetComponent<Image>();
            rectangleImg_2 = transform.FindChild("Head_BG_2/color_BG").GetComponent<Image>();
            rectangleImg_3 = transform.FindChild("Head_BG_3/color_BG").GetComponent<Image>();
            rectangleImg_4 = transform.FindChild("Head_BG_4/color_BG").GetComponent<Image>();
            ineframeImg_1 = transform.FindChild("Head_BG_1").GetComponent<Image>();
            ineframeImg_2 = transform.FindChild("Head_BG_2").GetComponent<Image>();
            ineframeImg_3 = transform.FindChild("Head_BG_3").GetComponent<Image>();
            ineframeImg_4 = transform.FindChild("Head_BG_4").GetComponent<Image>();
            playername_1 = transform.FindChild("Head_BG_1/PlayerName").GetComponent<Text>();
            playername_2 = transform.FindChild("Head_BG_2/PlayerName").GetComponent<Text>();
            playername_3 = transform.FindChild("Head_BG_3/PlayerName").GetComponent<Text>();
            playername_4 = transform.FindChild("Head_BG_4/PlayerName").GetComponent<Text>();
            headRawImg_1 = transform.FindChild("Head_Sprite_1").GetComponent<RawImage>();
            headRawImg_2 = transform.FindChild("Head_Sprite_2").GetComponent<RawImage>();
            headRawImg_3 = transform.FindChild("Head_Sprite_3").GetComponent<RawImage>();
            headRawImg_4 = transform.FindChild("Head_Sprite_4").GetComponent<RawImage>();
            gamesNumberText = transform.FindChild("RankingBG/RoundNum").GetComponent<Text>();
            this.recordInformationButton = this.transform.FindChild("PlaybackButton").GetComponent<Button>();
            recordInformationButton.onClick.AddListener(GradeInformationButtonEvent);


        }
    }
    private static String SwitchTime(long t)
    {
        DateTime dt = TimeHandle.Instance.GetDateTimeByTimestamp(t);
        return dt.ToString("hh:mm");
    }

    /// <summary>
    /// 绑定战绩详情按钮
    /// </summary>
    /// <param name="body"></param>
    private void GradeInformationButtonEvent()
    {
        PlayVideoC2S package = new PlayVideoC2S();
        package.round = reportId;
        package.roomId = roomNum;
        NetMgr.Instance.SendBuff<PlayVideoC2S>(SocketType.HALL, MsgNoC2S.REQUEST_PLAYVIDEO_C2S.GetHashCode(), 0, package);
    }
    IEnumerator DownIcon(string headUrl, RawImage rawImage)
    {
        WWW www = new WWW(headUrl);
        yield return www;
        rawImage.texture = www.texture;
        
    }





}
