using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerResurrection : MonoBehaviour
{
    public static playerResurrection _Instance;

    public GameObject fadeOutPanel;
    [Range(0, 1)]
    public float hpValue = 0.5f;

    private void Awake()
    {
        _Instance = this;
    }

    public void Resurrection()
    {
        Bandit._Instance.enabled = true;
        Bandit._Instance.charTableData.m_curHP = Bandit._Instance.charTableData.m_maxHP * hpValue;
        Bandit._Instance.gameObject.tag = "Player";
        Bandit._Instance.m_animator.SetTrigger("Recover");

        SoundManager._instance.ChangeBGM("Stage1_BGM", 0.3f);
        SoundManager._instance.SetVolumeSFX(1f);

        BattleSystem._Instance.isDead = false;

    }
}
