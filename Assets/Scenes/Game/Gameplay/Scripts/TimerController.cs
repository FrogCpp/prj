using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private DiologTracker dt;

    private float _timer = 60 * 20;

    void Update()
    {
        if (_timer < 0.0f) return;

        _timer -= Time.deltaTime;

        text.text = Parse(_timer);

        if (_timer < 0.0f)
        {
            text.text = "00.00.00";
            dt.EndLevel();
        }
    }

    private string Parse(float time)
    {
        float roundedTime = (float)Mathf.Floor(time * 10) / 10;

        int minutes = (int)(roundedTime / 60);
        int seconds = (int)(roundedTime % 60);
        int tenths = (int)((roundedTime * 10) % 10);

        return $"{minutes:00}:{seconds:00}.{tenths}";
    }
}
