using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ShockWave : MonoBehaviour, IAISkill
{
    public GameObject skillPrefab;
    public string targetTag = "Player";
    public float maxCoolDown = 5f;
    CharTableData charData;
    bool trigger = false;
    float coolDown;

    private void Awake()
    {
        if(GetComponent<LightBanditAI>())
            GetComponent<LightBanditAI>().m_skillList.Add(this);
    }

    public void OnEnter()
    {
        charData = GetComponent<CharTableData>();

        if (charData != null) 
        {
        }

        else
        {
            GetComponent<LightBanditAI>().m_skillList.Remove(this);
            Destroy(this);
        }
            
    }


    public void Inference()
    {
        coolDown -= Time.deltaTime;
    }

    public bool Decision()
    {
        if (charData.contiHurt >= 3 && coolDown <= 0)
        {
            trigger = true;
        }

        else
        {
            trigger = false;
        }

        return trigger;
    }

    public bool Action()
    {
        if(trigger)
        {
            BattleSystem._Instance.CreateParticleEffect(BattleSystem._Instance.boss_ShockWave_ParticleSystem, this.transform);
            SoundManager._instance.PlaySound("Storm_Warning", 6);
            GetComponent<Animator>().SetTrigger("ShockWave");
            StartCoroutine(DelayToSkill());

            trigger = false;
            return true;
        }

        return false;
    }

    public IEnumerator DelayToSkill(float delay = 0.45f) //공격 애니메이션이 시작 후 딜레이를 준 다음 공격 판정을 내린다. 플레이어가 보고 피할 수 있도록.
    {
        
        yield return new WaitForSeconds(delay);

        Tile_ShockWave t_tile_ShockWave;

        SoundManager._instance.PlaySound("ShockWave", 10);
        coolDown = maxCoolDown;

        t_tile_ShockWave = skillPrefab.transform.Find("ShockWave").GetComponent<Tile_ShockWave>();
        t_tile_ShockWave.atkerTag = this.transform.tag;
        t_tile_ShockWave.targetTag = targetTag;

        Instantiate(skillPrefab, this.transform);

        /*
        t_tile_ShockWave = t_gameobject.GetComponent<Tile_ShockWave>();
        Debug.Log(t_tile_ShockWave);
        t_tile_ShockWave.atkerTag = this.transform.tag;
        t_tile_ShockWave.targetTag = targetTag;*/
    }
}
