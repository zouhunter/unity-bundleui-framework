using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TextureLoader {
    private static string baseUrl_
    {
        get {
            return "file:///" + Application.streamingAssetsPath + "/";
        }
    }
    private static string localbasePath_
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }

    private string pictureLoadUrl;
    private string pictureLoadFile;

    private MonoBehaviour holder;
    public TextureLoader(MonoBehaviour holder)
    {
         this.holder = holder;
    }

    public void SetSceneUrl(string _file)
    {
        pictureLoadUrl = baseUrl_ + _file + "/";
        pictureLoadFile = localbasePath_ + _file + "/";
    }
    /// <summary>
    /// 加载Sprite
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="loadSprite"></param>
    public void LoadSpriteAsync(string fileName, UnityAction<Sprite> loadSprite)
    {
        if (loadSprite == null) return;
        UnityAction<Texture> fun = (x) =>
        {
            Sprite sprite = Sprite.Create((Texture2D)x, new Rect(0, 0, x.width, x.height), Vector2.one * 0.5f);
            loadSprite(sprite);
        };
        LoadTextureAsync(fileName, fun);
    }
    /// <summary>
    /// 异步加载图片
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="loadTexture"></param>
    public void LoadTextureAsync(string fileName,UnityAction<Texture> loadTexture)
    {
        if (loadTexture == null) return;
        if (File.Exists(pictureLoadFile + fileName))
        {
            holder.StartCoroutine(DownLandTexture("file:///" + pictureLoadFile,fileName, loadTexture));
        }
        else
        {
            holder.StartCoroutine(DownLandTexture(pictureLoadUrl,fileName, loadTexture, true));
        }
    }
    /// <summary>
    /// 协程下载图片
    /// </summary>
    /// <returns></returns>
    IEnumerator DownLandTexture(string urlpath,string fileName,UnityAction<Texture> loadTexture,bool needSave = false)
    {
        WWW www = new WWW(urlpath + fileName);
        yield return www;
        if (www.error==null && www.isDone)
        {
            loadTexture(www.texture);
            if(needSave)
            {
                File.WriteAllBytes(pictureLoadFile + fileName, www.bytes);
            }
        }
        else
        {
            Debug.LogWarning(www.error);
        }
    }
}
