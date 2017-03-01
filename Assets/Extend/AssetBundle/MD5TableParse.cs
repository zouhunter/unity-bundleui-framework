using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
public static class MD5TableParse
{
    /// <summary>
    /// 便利文件目录生成md5csv文件
    /// </summary>
    /// <param name="dir"></param>
    public static void GetMD5CSV(string dir, string fileName)
    {
        string bundleText = Path.Combine(dir + "/Config", fileName);
        InitFileDiractory(dir);
        if (File.Exists(bundleText)) File.Delete(bundleText);
        List<string> streamFiles = new List<string>();
        RecursiveSub(dir, action: (x) => { streamFiles.Add(x); });
        FileStream fs = new FileStream(bundleText, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        sw.WriteLine("filename" + "," + "md5");
        for (int i = 0; i < streamFiles.Count; i++)
        {
            string file = streamFiles[i];
            //string ext = Path.GetExtension(file);
            if (file == fileName) continue;
            if (file.EndsWith(".meta") || file.Contains(".DS_Store")) continue;

            string md5 = md5file(file);
            string value = "";
            /////从本地读取文件有问题
            value = file.Replace(dir, string.Empty);

            sw.WriteLine(value + "," + md5);
        }
        sw.Close(); fs.Close();

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    public static void RecursiveSub(string path, string ignoreFileExt = ".meta", string ignorFolderEnd = "_files", Action<string> action = null)
    {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names)
        {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(ignoreFileExt)) continue;
            action(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs)
        {
            if (dir.EndsWith(ignorFolderEnd)) continue;
            RecursiveSub(dir, ignoreFileExt, ignorFolderEnd, action);
        }
    }
    /// <summary>
    /// 初始化文件旋转路经
    /// </summary>
    /// <param name="filePath"></param>
    public static void InitFileDiractory(string filePath)
    {
        string dir = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }
    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    public static string md5file(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();
            fs.Dispose();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }
}
