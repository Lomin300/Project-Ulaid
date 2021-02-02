using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Lighting : MonoBehaviour, IAISkill
{
    public GameObject prefab_Guid_Lighting;
    public GameObject prefab_Random_Lighting;

    public string targetTag = "Player";
    public float lighting_CoolDown = 20f;
    float playtime;
    bool trigger = false;
    CharTableData charTable;

    public float size_Guid_Lighting = 3;
    public float size_Random_Lighting = 10;


    private void Awake()
    {
        if (GetComponent<LightBanditAI>())
            GetComponent<LightBanditAI>().m_skillList.Add(this);
    }

    public void OnEnter()
    {
        charTable = GetComponent<CharTableData>();

        if (charTable != null) { }

        else
        {
            GetComponent<LightBanditAI>().m_skillList.Remove(this);
            Destroy(this);

        }
    }

    public bool Decision()
    {
        if (playtime >= lighting_CoolDown)
        {
            playtime -= lighting_CoolDown;

            trigger = true;
        }

        return trigger;
    }

    public bool Action()
    {
        

        if (trigger && charTable.m_curHP/ charTable.m_maxHP <= 0.35f)
        {
            SoundManager._instance.PlaySound("Storm_warning",10);
            GetComponent<Animator>().SetTrigger("Thunder");

            GuidLighting();
            RandomLighting();

            trigger = false;
            return true;
        }

        else if (trigger)
        {
            SoundManager._instance.PlaySound("Storm_warning", 10);

            GetComponent<Animator>().SetTrigger("Thunder");

            if (Random.Range(0, 2) == 0) //유도 번개
            {
                GuidLighting();
            }

            else //랜덤 번개
            {
                RandomLighting();
            }

            trigger = false;
            return true;
        }

        return false;
    }


    public void Inference()
    {
        playtime += Time.deltaTime;
    }

    void RandomLighting()
    {
        Tile_Lighting t_tile_Lighting;

        for(int i=0; i<size_Random_Lighting; i++)
        {
            t_tile_Lighting = prefab_Random_Lighting.transform.Find("Lighting_Head").GetComponent<Tile_Lighting>();
            t_tile_Lighting.atkerTag = this.transform.tag;
            t_tile_Lighting.targetTag = targetTag;

            Instantiate(prefab_Random_Lighting, new Vector3(Random.Range(-350, 350), -41, 0), Quaternion.identity, this.transform.parent);
        }
        
    }
    
    void GuidLighting()
    {
        StartCoroutine(ShotGuidLighting());
    }

    public IEnumerator ShotGuidLighting() //공격 애니메이션이 시작 후 딜레이를 준 다음 공격 판정을 내린다. 플레이어가 보고 피할 수 있도록.
    {
        Tile_Lighting t_tile_Lighting;

        for (int i=0; i<3; i++)
        {
            t_tile_Lighting = prefab_Random_Lighting.transform.Find("Lighting_Head").GetComponent<Tile_Lighting>();
            t_tile_Lighting.atkerTag = this.transform.tag;
            t_tile_Lighting.targetTag = targetTag;

            Instantiate(prefab_Random_Lighting, new Vector3(Bandit._Instance.transform.position.x, -41, 0), Quaternion.identity, this.transform.parent);

            yield return new WaitForSeconds(1f);
        }
    }
}
