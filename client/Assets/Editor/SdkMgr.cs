﻿using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class SdkMgr : EditorWindow
{
    [MenuItem("恩赐方/选择平台/本地", false, 2)]
    public static void CopyLocal()
    {
        DirectoryInfo androidFolder = new DirectoryInfo("Assets/Plugins/Android");
        if (androidFolder.Exists)
        {
            androidFolder.Delete(true);
        }
        ReplacePlatformScript("SDKPlatform.LOCAL");
    }
    [MenuItem("恩赐方/选择平台/本地SDK", false, 2)]
    public static void CopyLocalSDK()
    {
        DirectoryInfo sdkFolder = new DirectoryInfo("Sdk/Weixin/Android");
        DirectoryInfo androidFolder = new DirectoryInfo("Assets/Plugins/Android");
        if (androidFolder.Exists)
        {
            androidFolder.Delete(true);
        }
        CopyFolder(sdkFolder.FullName, new DirectoryInfo("Assets/Plugins").FullName);
        ReplacePlatformScript("SDKPlatform.LOCAL");
    }
    [MenuItem("恩赐方/选择平台/微信", false, 2)]
    public static void CopyWeiXin()
    {
        DirectoryInfo sdkFolder = new DirectoryInfo("Sdk/Weixin/Android");
        DirectoryInfo androidFolder = new DirectoryInfo("Assets/Plugins/Android");
        if (androidFolder.Exists)
        {
            androidFolder.Delete(true);
        }
        CopyFolder(sdkFolder.FullName, new DirectoryInfo("Assets/Plugins").FullName);
        ReplacePlatformScript("SDKPlatform.WEIXIN");
    }
    private static void CopyFolder(string strFromPath, string strToPath)
    {
        //如果源文件夹不存在，则创建
        if (!Directory.Exists(strFromPath))
        {
            Directory.CreateDirectory(strFromPath);
        }
        //取得要拷贝的文件夹名
        string strFolderName = strFromPath.Substring(strFromPath.LastIndexOf("\\") +
          1, strFromPath.Length - strFromPath.LastIndexOf("\\") - 1);
        //如果目标文件夹中没有源文件夹则在目标文件夹中创建源文件夹
        if (!Directory.Exists(strToPath + "\\" + strFolderName))
        {
            Directory.CreateDirectory(strToPath + "\\" + strFolderName);
        }
        //创建数组保存源文件夹下的文件名
        string[] strFiles = Directory.GetFiles(strFromPath);
        //循环拷贝文件
        for (int i = 0; i < strFiles.Length; i++)
        {
            //取得拷贝的文件名，只取文件名，地址截掉。
            string strFileName = strFiles[i].Substring(strFiles[i].LastIndexOf("\\") + 1, strFiles[i].Length - strFiles[i].LastIndexOf("\\") - 1);
            //开始拷贝文件,true表示覆盖同名文件
            File.Copy(strFiles[i], strToPath + "\\" + strFolderName + "\\" + strFileName, true);
        }
        //创建DirectoryInfo实例
        DirectoryInfo dirInfo = new DirectoryInfo(strFromPath);
        //取得源文件夹下的所有子文件夹名称
        DirectoryInfo[] ZiPath = dirInfo.GetDirectories();
        for (int j = 0; j < ZiPath.Length; j++)
        {
            //获取所有子文件夹名
            string strZiPath = ZiPath[j].ToString();
            //把得到的子文件夹当成新的源文件夹，从头开始新一轮的拷贝
            CopyFolder(strZiPath, strToPath + "\\" + strFolderName);
        }
    }
    private static void ReplacePlatformScript(string platformType)
    {
        FileInfo scriptFile = new FileInfo("Assets/Scripts/Platform/Global/GlobalData.cs");
        StreamReader reader = new StreamReader(scriptFile.FullName);
        string scriptStr = reader.ReadToEnd();
        reader.Close();
        string replaceStr = string.Format("    public static SDKPlatform sdkPlatform = {0};", platformType);
        Regex reg = new Regex(@"    public static SDKPlatform sdkPlatform = .*");
        scriptStr = reg.Replace(scriptStr, replaceStr);
        TextEditor textEditor = new TextEditor();
        textEditor.text = scriptStr;
        textEditor.OnFocus();
        textEditor.Copy();
        FileStream fs = new FileStream("Assets/Scripts/Platform/Global/GlobalData.cs", FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        //开始写入
        sw.Write(textEditor.text);
        //清空缓冲区
        sw.Flush();
        //关闭流
        sw.Close();
        fs.Close();
    }

    
}