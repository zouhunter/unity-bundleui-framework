using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public interface IRunTimeLoadCtrl {
    string Menu { get; }
    void GetGameObjectFromBundle(RTBundleInfo trigger);
}
