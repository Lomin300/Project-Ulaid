using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerCarmera : MonoBehaviour
{
    public float height; //높이
    private Transform playerPos;
    private Vector3 tempPos;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playerPos != null)
        {
            tempPos.Set(playerPos.position.x, playerPos.position.y + height, this.transform.position.z);
            this.transform.position = tempPos;
        }

        else
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        }
        
    }


}
