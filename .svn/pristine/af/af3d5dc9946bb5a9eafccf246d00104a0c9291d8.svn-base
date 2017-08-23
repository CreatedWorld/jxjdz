using System.Collections;
using System;
using UnityEngine;
/// <summary>
/// 获取手机电量
/// </summary>
public class BatteryAndTime : MonoBehaviour
{
    string _time = string.Empty;
    string _battery = string.Empty;

    void Start()
    {
        StartCoroutine("UpdataTime");
        StartCoroutine("UpdataBattery");
    }

    void OnGUI()
    {
        GUILayout.Label(_time, GUILayout.Width(100), GUILayout.Height(100));
        GUILayout.Label(_battery, GUILayout.Width(100), GUILayout.Height(100));
    }

    IEnumerator UpdataTime()
    {
        DateTime now = DateTime.Now;
        _time = string.Format("{0}:{1}", now.Hour, now.Minute);
        yield return new WaitForSeconds(60f - now.Second);
        while (true)
        {
            now = DateTime.Now;
            _time = string.Format("{0}:{1}", now.Hour, now.Minute);
            yield return new WaitForSeconds(60f);
        }
    }

    IEnumerator UpdataBattery()
    {
        while (true)
        {
            _battery = GetBatteryLevel().ToString();
            yield return new WaitForSeconds(300f);
        }
    }

    int GetBatteryLevel()
    {
        try
        {
            string CapacityString = System.IO.File.ReadAllText("/sys/class/power_supply/battery/capacity");
            return int.Parse(CapacityString);
        }
        catch (Exception e)
        {
            Debug.Log("Failed to read battery power; " + e.Message);
        }
        return -1;
    }
}