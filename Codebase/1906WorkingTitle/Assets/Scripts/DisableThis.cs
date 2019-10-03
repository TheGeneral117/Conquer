using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableThis : MonoBehaviour
{
    public uint time;

    private void Update()
    {
        if(time <= 0)
        {
            time += 180;
            gameObject.SetActive(false);
        }
        else
        {
            time--;
        }
    }

    public void AddTime(uint _time)
    {
        time += _time;
    }

    public float GetTime()
    {
        return time;
    }
}

