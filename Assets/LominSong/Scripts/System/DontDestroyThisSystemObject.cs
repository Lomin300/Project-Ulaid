using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyThisSystemObject : MonoBehaviour
{
    public List<GameObject> dontDestroyObjects = new List<GameObject>();
    void Start()
    {
        foreach (GameObject dontDestroyObject in dontDestroyObjects)
        {
            DontDestroyOnLoad(dontDestroyObject);
        }
    }
}
