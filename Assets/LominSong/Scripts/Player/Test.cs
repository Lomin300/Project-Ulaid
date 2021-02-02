using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject parents;



    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(parents.transform.position, -Vector3.forward, 100 * Time.deltaTime);
    }
}
