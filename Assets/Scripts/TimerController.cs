using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;

    [SerializeField] private TMP_Text timer;


    private TimeSpan timePlaying;
    private bool isRunning;

    private float elapsedTime;

    private void Awake()
    {
        instance = this;
        timer.gameObject.SetActive(true);
        timer.text = "Time: 00:00:00";
        isRunning = false;
    }


    public void BeginTimer()
    {
        isRunning = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());

    }

    public void EndTimer()
    {
        isRunning = false;
    }

    private IEnumerator UpdateTimer()
    {
        while (isRunning)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = "Time: " + timePlaying.ToString("mm':'ss'.'ff");
            timer.text = timePlayingStr;

            yield return null;
        }
    }

    public TimeSpan getTime()
    {
        return timePlaying;
    }
    // Update is called once per frame
}
