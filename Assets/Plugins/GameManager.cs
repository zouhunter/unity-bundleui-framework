using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : SingleManager<GameManager>
{
    public static bool IsQuit { get { return isQuit; } }
    private static bool isQuit;
    public static bool IsOn { get { return IsOn; } }
    private static bool isOn;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Facade.Instance.RegisterCommand<StartUpCommand>("StartUpCommand");
    }

    public void LunchFrameWork()
    {
        if (isOn)
        {
            return;
        }
        else
        {
            Facade.Instance.SendNotification("StartUpCommand");
            isOn = true;
        }
    }

    void OnApplicationQuit()
    {
        isQuit = true;
    }
}
