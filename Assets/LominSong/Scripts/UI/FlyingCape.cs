using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCape : MonoBehaviour
{
    public Animator cape;
    // Start is called before the first frame update
    public void Fly()
    {
        cape.SetTrigger("Fly");
        SoundManager._instance.PlaySound("Title_click", 1);
    }
}
