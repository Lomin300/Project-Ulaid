using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowArrows : MonoBehaviour
{
    public AnimationClip animationClip;
    public List<GameObject> arrows = new List<GameObject>();
    public float delay;
    float playTime;

    void FixedUpdate()
    {
        playTime += Time.deltaTime;

        if(playTime >= animationClip.length+delay)
        {
            foreach (GameObject arrow in arrows)
                arrow.SetActive(true);

            Destroy(this.gameObject);
        }
    }
}
