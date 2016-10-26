using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public interface IUITween {
    void Play(bool forward = false);
    void Rewind();
    void TogglePause();
    void Kill();
}
