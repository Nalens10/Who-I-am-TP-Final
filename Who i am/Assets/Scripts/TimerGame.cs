using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerGame : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text timerGameOver;
    private float timeElapsed;


    private void Update()
    {
        TimePlayer();
    }

    private void TimePlayer()
    {
        timeElapsed += Time.deltaTime;
        int minutes = (int)(timeElapsed / 60f);
        int seconds = (int)(timeElapsed - minutes * 60f);
        int cents = (int)((timeElapsed - (int)timeElapsed) * 100f);

        SetTimerUI(minutes, seconds, cents);
    }


    private void SetTimerUI(int minutes, int seconds, int cents)
    {
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, cents);
        timerGameOver.text = "Tiempo Jugado: " + string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, cents);
    }
}
