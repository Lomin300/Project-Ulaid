using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInit : MonoBehaviour
{
    public string backgroundMusic;
    [Range(0f, 1f)]
    public float bgmVolume;


    // Start is called before the first frame update
    void Start()
    {
        SoundManager._instance.ChangeBGM(backgroundMusic, bgmVolume);
        SoundManager._instance.masterVolumeSFX = 1;
        InventoryManager._instance.UpdateInventoryUI();
    }

}
