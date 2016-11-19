using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MediatorTrigger:RunTimeTrigger {
    public string[] messageKey;
    public object data;
}
