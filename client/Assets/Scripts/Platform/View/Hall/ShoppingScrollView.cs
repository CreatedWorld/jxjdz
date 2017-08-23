using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 商场数据
/// </summary>
public class ShoppingScrollView : TableViewItem {
    /// <summary>
    /// 标题
    /// </summary>
    private Text title;
    /// <summary>
    /// 图片
    /// </summary>
    private Image buttomImage;
    public override void Updata(object data)
    {
        if (data == null)
        {
            return;
        }
        base.Updata(data);
        this.title.text = ((ShoppingScrollData)data).Title;
        string path = "Textures/UI/Shopping/" + ((ShoppingScrollData)data).ImageName;
        this.buttomImage.sprite = Resources.Load<Sprite>(path);
    }

    private void Awake()
    {
        this.title = this.transform.FindChild("InfoText").GetComponent<Text>();
        this.buttomImage = this.transform.FindChild("Button").GetComponent<Image>();
    }
}
