using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class StopWatch : MonoBehaviour
{
    Stopwatch stopWatch = new Stopwatch();
    System.TimeSpan savedTime = new System.TimeSpan();

    // Start is called before the first frame update
    void Start()
    {
        stopWatch.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public System.TimeSpan Stop()
    {
        stopWatch.Stop();
        return stopWatch.Elapsed.Add(savedTime);
    }

    public void PauseStopWatch()
    {
        stopWatch.Stop();
    }

    public void ResumeStopWatch()
    {
        stopWatch.Start();
    }

    public System.TimeSpan SaveTime()
    {
        return stopWatch.Elapsed;
    }

    public void SetSavedTime(System.TimeSpan _time)
    {
        savedTime = _time;
    }
}
