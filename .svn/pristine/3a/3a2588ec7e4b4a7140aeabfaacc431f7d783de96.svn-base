using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class Timer
    {
        private static Timer ins;
        /// <summary>
        /// 定时器字典
        /// </summary>
        private Dictionary<int, Tick> tickDict = new Dictionary<int, Tick>();
        private float curtime;
        /// <summary>
        /// 延迟定时器字典
        /// </summary>
        private Dictionary<int, Tick> deltaTickDic = new Dictionary<int,Tick>();
        /// <summary>
        /// 当前延迟的时间
        /// </summary>
        private float deltaTime;
        /// <summary>
        /// 未缩放时间
        /// </summary>
        private float unscaledTime;
        private int timerid;

        List<int> dellist = new List<int>();
        List<int> templist = new List<int>();

        public static Timer Instance
        {
            get
            {
                if (ins == null)
                {
                    ins = new Timer();
                }
                return ins;
            }
        }

        public bool Init()
        {
            timerid = 0;
            curtime = 0;
            unscaledTime = 0;
            deltaTime = 0;
            return true;
        }

        /// <summary>
        /// 添加定时器,该定时器不受时间缩放影响
        /// </summary>
        /// <param name="interval">定时器时间间隔</param>
        /// <param name="count">定时器执行次数,0不限次数</param>
        /// <param name="start">定时器开始等待时间</param>
        /// <param name="func">定时器回调方法</param>
        /// <returns></returns>
        public int AddTimer(float interval, int count, float start, TimerCallBack func)
        {
            if (interval < 0 || count < 0 || start < 0)
            {
                Debug.LogError("error add timer args:" + interval + count + start);
                return 0;
            }
            Tick tick = new Tick();
            if (tick == null)
	        {
		        return 0;
	        }
            timerid++;
            tick.tid = timerid;
            tick.interval = interval;
            tick.start = unscaledTime + start;
            tick.count = count;     
            tick.cbfunc = func;
            tickDict.Add(tick.tid, tick);
            return tick.tid;
        }
        /// <summary>
        /// 添加定时器,该定时器受时间缩放影响
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="count"></param>
        /// <param name="start"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public int AddDeltaTimer(float interval, int count, float start, TimerCallBack func)
        {
            if (interval < 0 || count < 0 || start < 0)
            {
                Debug.LogError("error add timer args:" + interval + count + start);
                return 0;
            }
            Tick tick = new Tick();
            if (tick == null)
            {
                return 0;
            }
            timerid++;
            tick.tid = timerid;
            tick.interval = interval;
            tick.start = deltaTime + start;
            tick.count = count;
            tick.cbfunc = func;
            deltaTickDic.Add(tick.tid, tick);
            return tick.tid;
        }

        /// <summary>
        /// 设置定时器为锁死状态
        /// </summary>
        /// <param name="tid"></param>
        public void SetFixTimer(int tid)
        {
            Tick tick = null;
            if (tickDict.ContainsKey(tid))
            {
                tick = tickDict[tid];
            }
            else if (deltaTickDic.ContainsKey(tid))
            {
                tick = deltaTickDic[tid];
            }
            if (tick == null)
            {
                Debug.LogError("pause timer no id:" + tid);
                return;
            }
            tick.fix = true;
        }

        /// <summary>
        /// 取消所有定时器,标记为锁死的除外
        /// </summary>
        public void CancelAllTimer()
        {
            List<int> _list = new List<int>();
            foreach (var item in tickDict)
            {
                Tick tick = item.Value;
                if (tick.fix == false)
                {
                    _list.Add(tick.tid);
                }
            }

            foreach (var item in deltaTickDic)
            {
                Tick tick = item.Value;
                if (tick.fix == false)
                {
                    _list.Add(tick.tid);
                }
            }

            foreach (var item in _list)
            {
                int tid = item;
                CancelTimer(tid);
            }
        }
        
        /// <summary>
        /// 取消单个定时器
        /// </summary>
        /// <param name="tid"></param>
        public void CancelTimer(int tid)
        {
            if (tickDict.ContainsKey(tid))
            {
                tickDict.Remove(tid);
            }
            else if (deltaTickDic.ContainsKey(tid))
            {
                deltaTickDic.Remove(tid);
            }
            else
            {
                //Output.Error("do delete timer no id:", tid);
            }
        }

        /// <summary>
        /// 暂停单个定时器
        /// </summary>
        /// <param name="tid"></param>
        public void PauseTimer(int tid)
        {
            Tick tick = null;
            if (tickDict.ContainsKey(tid))
            {
                tick = tickDict[tid];
            }
            else if (deltaTickDic.ContainsKey(tid))
            {
                tick = deltaTickDic[tid];
            }
            if (tick == null)
            {
                Debug.LogError("pause timer no id:" + tid);
                return;
            }
            tick.pause = true;
        }

        /// <summary>
        /// 暂停所有定时器
        /// </summary>
        public void PauseAllTimer()
        {
            List<int> _list = new List<int>();
            foreach (var item in tickDict)
            {
                Tick tick = item.Value;
                _list.Add(tick.tid);
            }
            foreach (var item in deltaTickDic)
            {
                Tick tick = item.Value;
                _list.Add(tick.tid);
            }

            foreach (var item in _list)
            {
                int tid = item;
                PauseTimer(tid);
            }
        }

        /// <summary>
        /// 恢复单个定时器
        /// </summary>
        /// <param name="tid"></param>
        public void RecoverTimer(int tid)
        {
            Tick tick = null;
            if (tickDict.ContainsKey(tid))
            {
                tick = tickDict[tid];
            }
            else if (deltaTickDic.ContainsKey(tid))
            {
                tick = deltaTickDic[tid];
            }
            if (tick == null)
            {
                Debug.LogError("recover timer no id:" + tid);
                return;
            }
            tick.pause = false;
        }

        /// <summary>
        /// 恢复所有定时器
        /// </summary>
        public void RecoverAllTimer()
        {
            List<int> _list = new List<int>();
            foreach (var item in tickDict)
            {
                Tick tick = item.Value;
                _list.Add(tick.tid);
            }
            foreach (var item in deltaTickDic)
            {
                Tick tick = item.Value;
                _list.Add(tick.tid);
            }

            foreach (var item in _list)
            {
                int tid = item;
                RecoverTimer(tid);
            }
        }

        public void DoUpdate()
        {
            if (deltaTickDic.Count <= 0) return;

            dellist.Clear();
            templist.Clear();

            foreach (var item in deltaTickDic)
            {
                Tick tick = item.Value;
                templist.Add(tick.tid);
            }

            deltaTime += Time.deltaTime;
            
            foreach (var item in templist)
            {
                int tid = item;
                Tick tick;
                if (!deltaTickDic.TryGetValue(tid, out tick))
                {
                    Debug.LogError("find timer no id:" + tid);
                    continue;
                };

                if (tick.pause == true) continue;
                if (tick.start <= deltaTime)
                {
                    tick.cbfunc();
                    tick.count--;
                    tick.start += tick.interval;
                    if (tick.count == 0)
                    {
                        dellist.Add(tick.tid);
                    }
                }
            }

            foreach (int tid in dellist)
            {
                if (deltaTickDic.ContainsKey(tid) == false)
                {
                    //Output.Error("auto delete timer no id:", tid);
                    continue;
                }
                deltaTickDic.Remove(tid);
            }
        }

        public void DoFixUpdate()
        {
            if (tickDict.Count <= 0) return;

            dellist.Clear();
            templist.Clear();

            foreach (var item in tickDict)
            {
                Tick tick = item.Value;
                templist.Add(tick.tid);
            }

            curtime = Time.unscaledTime;
            unscaledTime = curtime;
            foreach (var item in templist)
            {
                int tid = item;
                Tick tick;
                if (!tickDict.TryGetValue(tid, out tick))
                {
                    //Debug.LogError("find timer no id:" + tid);
                    continue;
                };

                if (tick.pause == true) continue;
                if (tick.start <= curtime)
                {
                    tick.cbfunc();
                    tick.count--;
                    tick.start += tick.interval;
                    if (tick.count == 0)
                    {
                        dellist.Add(tick.tid);
                    }
                }
            }

            foreach (int tid in dellist)
	        {
                if (tickDict.ContainsKey(tid) == false)
                {
                    //Output.Error("auto delete timer no id:", tid);
                    continue;
                }
                tickDict.Remove(tid);
	        }
        }

    }


}
