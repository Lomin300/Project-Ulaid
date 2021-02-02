using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thundering : MonoBehaviour
{
    public Animator thunder;
    public float freq; //빈도
    [Range(0,100)]
    public float prob; //확률

    float m_nowfreq;

    private void Start()
    {
        m_nowfreq = freq;
    }

    void FixedUpdate()
    {
        if (m_nowfreq <= 0)
            CheckThunder();

        m_nowfreq -= Time.deltaTime;
    }

    void CheckThunder()
    {
        if(Random.Range(0,101)>= (100-prob))
        {
            thunder.SetTrigger("Thundering");
            SoundManager._instance.PlaySound("Background_Storm", 0.5f);
        }

        m_nowfreq = freq;
    }
}
