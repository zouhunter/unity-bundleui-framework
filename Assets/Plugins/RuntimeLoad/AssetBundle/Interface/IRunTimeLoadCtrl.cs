using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public interface IRunTimeLoadCtrl {
    string Menu { get; }
    void InitlizeCreater(string menu);
    void GetGameObjectFromBundle(RunTimeTrigger trigger);
    void ClearLoaded();
}
