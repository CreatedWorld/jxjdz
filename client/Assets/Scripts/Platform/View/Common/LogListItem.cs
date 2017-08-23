namespace Consolation
{
    using UnityEngine;
    using UnityEngine.UI;
    /// <summary>
    /// 日志节点
    /// </summary>
    class LogListItem:TableViewItem
    {
        /// <summary>
        /// 日志文本
        /// </summary>
        private Text logTxt;
        private void Awake()
        {
            logTxt = transform.GetComponent<Text>();
        }

        public override void Updata(object data)
        {
            base.Updata(data);
            if (data == null)
            {
                logTxt.text = "";
                return;
            }
            var log = (LogVO)data;
            logTxt.text = log.message + log.stackTrace;
            if (log.type == LogType.Error)
            {
                logTxt.color = Color.red;
            }
            else if (log.type == LogType.Warning)
            {
                logTxt.color = Color.yellow;
            }
            else
            {
                logTxt.color = Color.white;
            }
        }
    }
}
