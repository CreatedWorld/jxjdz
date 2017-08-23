using Platform.Model;
using Platform.Model.Battle;
using Platform.Net;
using System.Collections;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

namespace Consolation
{
    /// <summary>  
    /// A console to display Unity's debug logs in-game.  
    /// </summary>  
    class LogConsole : MonoBehaviour
    {
        //fps相关
        public float f_UpdateInterval = 0.5F;
        private float f_LastInterval;
        private int i_Frames = 0;
        private float f_Fps;
        private GUIStyle style;
       
        /// <summary>
        /// 日志列表
        /// </summary>
        private TableView logList;
        /// <summary>
        /// 日志显示隐藏
        /// </summary>
        private Button logBtn;
        /// <summary>
        /// 复制按钮
        /// </summary>
        private Button copyBtn;
        /// <summary>
        /// 日志清除
        /// </summary>
        private Button clearBtn;
        /// <summary>
        /// 调试按钮
        /// </summary>
        private Button debugBtn;

        private void Awake()
        {
            style = new GUIStyle();
            style.fontSize = 30;
            style.normal.textColor = Color.yellow;

            logList = transform.Find("LogList").GetComponent<TableView>();
            logBtn = transform.Find("LogBtn").GetComponent<Button>();
            copyBtn = transform.Find("CopyBtn").GetComponent<Button>();
            clearBtn = transform.Find("ClearBtn").GetComponent<Button>();
            debugBtn = transform.Find("DebugBtn").GetComponent<Button>();

            logBtn.onClick.AddListener(UpdateLogActive);
            copyBtn.onClick.AddListener(CopyLogs);
            clearBtn.onClick.AddListener(ClearLog);
            debugBtn.onClick.AddListener(DebugHandler);

        }

        void Start()
        {
            f_LastInterval = Time.realtimeSinceStartup;
            i_Frames = 0;
        }

        void Update()  
        {  
            ++i_Frames;
            if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
            {
                f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);
                i_Frames = 0;
                f_LastInterval = Time.realtimeSinceStartup;
            }
            UpdateUsed();

        }
        //Memory
        private string sUserMemory;
        private string s;
        private uint MonoUsedM;
        private uint AllMemory;
        void UpdateUsed()
        {
            sUserMemory = "";
            MonoUsedM = Profiler.GetMonoUsedSize() / 1000000;
            AllMemory = Profiler.GetTotalAllocatedMemory() / 1000000;


            sUserMemory += "MonoUsed:" + MonoUsedM + "M" + "\n";
            sUserMemory += "AllMemory:" + AllMemory + "M" + "\n";
            sUserMemory += "UnUsedReserved:" + Profiler.GetTotalUnusedReservedMemory() / 1000000 + "M" + "\n";


            s = "";
            s += " MonoHeap:" + Profiler.GetMonoHeapSize() / 1000 + "k";
            s += " MonoUsed:" + Profiler.GetMonoUsedSize() / 1000 + "k";
            s += " Allocated:" + Profiler.GetTotalAllocatedMemory() / 1000 + "k";
            s += " Reserved:" + Profiler.GetTotalReservedMemory() / 1000 + "k";
            s += " UnusedReserved:" + Profiler.GetTotalUnusedReservedMemory() / 1000 + "k";
            s += " UsedHeap:" + Profiler.usedHeapSize / 1000 + "k";
        }

        void OnGUI()  
        {
            GUI.Label(new Rect(0, 0, 200, 200), " FPS:" + f_Fps.ToString("f2"), style);

            GUI.Label(new Rect(0, 50, 200, 200), sUserMemory, style);
        }

        /// <summary>
        /// 清除日志
        /// </summary>
        private void ClearLog()
        {
            GlobalData.logs.Clear();
            logList.DataProvider = GlobalData.logs;
        }
        /// <summary>
        /// 日志显示隐藏
        /// </summary>
        private void UpdateLogActive()
        {
            foreach(Transform child in transform)
            {
                if (child == logBtn.transform)
                {
                    continue;
                }
                child.gameObject.SetActive(!child.gameObject.activeSelf);
            }
            if (logList.gameObject.activeSelf)
            {
                logList.DataProvider = GlobalData.logs;
            }
        }
        /// <summary>
        /// debug调试
        /// </summary>
        private void DebugHandler()
        {
            GameObject effectPerfab = Resources.Load<GameObject>("Effect/ChiEffect/ChiEffect");
            GameObject actEffect = Instantiate(effectPerfab);

            var perPosition = actEffect.GetComponent<RectTransform>().localPosition;
            actEffect.GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>());
            actEffect.GetComponent<RectTransform>().localPosition = Vector3.zero;
            actEffect.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
            actEffect.GetComponent<Animator>().enabled = true;




        }

        /// <summary>
        /// 复制日志
        /// </summary>
        void CopyLogs()
        {
            string result = "";
            foreach (LogVO log in GlobalData.logs)
            {
                if (result != "")
                {
                    result += "\n";
                }
                result += log.message + log.stackTrace;
            }
            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidSdkInterface.CopyToClip(result);
            }
            if (!Application.isMobilePlatform)
            {
                TextEditor textEditor = new TextEditor();
                textEditor.text = result;
                textEditor.OnFocus();
                textEditor.Copy();
            }
        }
    }
}