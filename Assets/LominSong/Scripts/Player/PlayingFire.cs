using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class PlayingFire : MonoBehaviour
{
    public Light2D light2D;
    public float minReScaleTime;
    public float maxReScaleTime;
    public float minSize;
    public float maxSize;

    float timer;

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if(timer>=Random.Range(minReScaleTime,maxReScaleTime))
        {
            light2D.pointLightInnerRadius = Random.Range(minSize, maxSize);
            timer = 0;
        }
            
    }
}
