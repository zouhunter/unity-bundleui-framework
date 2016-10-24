using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//public static class MD5TableParse {
//    /// <summary>
//    /// 便利文件目录生成md5csv文件
//    /// </summary>
//    /// <param name="dir"></param>
//    public static void GetMD5CSV(string dir,string fileName)
//    {
//        string bundleText = Path.Combine(dir + "/Config", fileName);
//        FileUtility.InitFileDiractory(dir);
//        if (File.Exists(bundleText)) File.Delete(bundleText);
//        List<string> streamFiles = new List<string>();
//        FileUtility.Recursive(dir,action: (x)=> { streamFiles.Add(x); });
//        FileStream fs = new FileStream(bundleText, FileMode.CreateNew);
//        StreamWriter sw = new StreamWriter(fs);
//        sw.WriteLine("filename" + "," + "md5");
//        for (int i = 0; i < streamFiles.Count; i++)
//        {
//            string file = streamFiles[i];
//            //string ext = Path.GetExtension(file);
//            if (file == fileName) continue;
//            if (file.EndsWith(".meta") || file.Contains(".DS_Store")) continue;

//            string md5 = FileUtility.md5file(file);
//            string value = "";
//            /////从本地读取文件有问题
//            value = file.Replace(dir, string.Empty);

//            sw.WriteLine(value + "," + md5);
//        }
//        sw.Close(); fs.Close();

//#if UNITY_EDITOR
//       UnityEditor.AssetDatabase.Refresh();
//#endif
//    }
//}
