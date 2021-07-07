using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{

    public static TimerController instance;
    public static float savedTime;
    public Text timeCounter;
    private TimeSpan timePlaying;
    public bool timerGoing;
    public float elapsedTime;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if (AreaController.firstLoad)
        {
            elapsedTime = 0f;
        }
        if (AreaController.firstLoad == false)
        {
            elapsedTime = savedTime;
        }
    }

    public void BeginTimer()
    {        
        timerGoing = true;
        StartCoroutine(UpdateTimer());
    }

    public void StopTimer()
    {
        timerGoing = false;
        savedTime = elapsedTime;
        PlayerPrefs.SetFloat("TimerSave", elapsedTime); PlayerPrefs.Save();
    }

    private IEnumerator UpdateTimer()
    {
        while(timerGoing)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = "Time: " + timePlaying.ToString("mm' : 'ss' . 'ff");
            timeCounter.text = timePlayingStr;
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
