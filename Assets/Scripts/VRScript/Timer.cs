using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    public float LimitTime;
    public TextMeshProUGUI text_timer;

    private void Update()
    {
        LimitTime += Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(LimitTime);
        text_timer.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }
}

