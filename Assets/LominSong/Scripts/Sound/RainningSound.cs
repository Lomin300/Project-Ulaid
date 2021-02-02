using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainningSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager._instance.PlayLoopSound("RainningSound");
    }

    
}
