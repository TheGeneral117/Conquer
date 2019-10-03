using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyThis : MonoBehaviour
{
    public float _time;

    void Start()
    {
        Destroy(gameObject, _time);
    }
}
