using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public interface IRunTimeLoadCtrl {
    string Menu { get; }
    string Url { get; }
    void InitlizeCreater(string menu, string url);
    void GetGameObjectFromBundle(RunTimeTrigger trigger);
    void ClearLoaded();
}
