using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Platform.Model;
/// <summary>
/// 对战信息View
/// </summary>
public class RankingScrollView : TableViewItem
{
    /// <summary>
    /// 是否初始化
    /// </summary>
    private bool isInit;
    /// <summary>
    /// 房间号
    /// </summary>
    private Text roomNumText;
    /// <summary>
    /// 回合
    /// </summary>
    private Text roundNumText;
    /// <summary>
    /// 时间
    /// </summary>
    private Text time;
    /// <summary>
    /// 参赛者信息
    /// </summary>
    private List<Text> userNames;
    /// <summary>
    /// 排行玩家信息
    /// </summary>
    private UserInfo userInfo;
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
    
    public Image MedalImage;
    public Text No1Text;
    public RawImage HeadImg;
    public Text UserName;
    public Text WinCount;
    private RawImage photoTexture;

    public RankingScrollView()
    {
        this.IsInit = false;
    }
    public override void Updata(object data)
    {
        if (data == null)
        {
            return;
        }
        base.Updata(data);
        this.userInfo = data as UserInfo;

        UserName.text = userInfo.userName;
        WinCount.text = userInfo.win.ToString();

        GameMgr.Instance.StartCoroutine(DownIcon(userInfo.imageUrl));
        if (userInfo.rankNum == 1)
        {
            MedalImage.sprite = Resources.Load<Sprite>("Textures/UI/第一名");
            MedalImage.GetComponent<Image>().enabled = true;
            No1Text.text = "";
        }else if(userInfo.rankNum == 2)
        {
            MedalImage.sprite = Resources.Load<Sprite>("Textures/UI/第二名");
            MedalImage.GetComponent<Image>().enabled = true;
            No1Text.text = "";
        }
        else if (userInfo.rankNum == 3)
        {
            MedalImage.sprite = Resources.Load<Sprite>("Textures/UI/第三名");
            MedalImage.GetComponent<Image>().enabled = true;
            No1Text.text = "";
        }
        else
        {
            MedalImage.GetComponent<Image>().enabled = false;
            No1Text.text = userInfo.rankNum.ToString();
        }
    }

    private void Awake()
    {
        MedalImage = transform.FindChild("RankingImage").GetComponent<Image>();
        No1Text = transform.FindChild("RankingImage/Text").GetComponent<Text>();
        HeadImg = transform.FindChild("PhotoMask/Photo").GetComponent<RawImage>();
        UserName = transform.FindChild("NameText").GetComponent<Text>();
        WinCount = transform.FindChild("WinText").GetComponent<Text>();

    }
    IEnumerator DownIcon(string headUrl)
    {
        WWW www = new WWW(headUrl);
        yield return www;
        if (www.error == null)
        {
            HeadImg.texture = www.texture;
        }
    }
}
