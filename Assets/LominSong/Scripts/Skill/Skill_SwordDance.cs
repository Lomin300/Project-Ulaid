using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_SwordDance : MonoBehaviour, IAISkill
{
    public GameObject skillPrefab;
    public GameObject sonicWavePrefab;
    public float maxCoolDown = 5f;
    public float overRange = 50;
    public string targetTag = "Player";
    CharTableData charData;
    GameObject centerPos;
    GameObject weaponPos;
    bool trigger = false;
    float coolDown;
    Ray2D ray2d;
    List<RaycastHit2D> a_atkHits = new List<RaycastHit2D>();


    private void Awake()
    {
        if (GetComponent<LightBanditAI>())
            GetComponent<LightBanditAI>().m_skillList.Add(this);
    }

    public void OnEnter()
    {
        charData = GetComponent<CharTableData>();
        centerPos = this.transform.Find("CenterPos").gameObject;
        weaponPos = this.transform.Find("WeaponPos").gameObject;

        if (charData != null && weaponPos != null && centerPos != null)
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
        ray2d.origin = centerPos.transform.position;
        ray2d.direction = new Vector2(weaponPos.transform.position.x - centerPos.transform.position.x,
        weaponPos.transform.position.y - centerPos.transform.position.y).normalized;

        a_atkHits.Clear();
        a_atkHits.AddRange(Physics2D.RaycastAll(ray2d.origin, ray2d.direction, 1000));

        for (int i = 0; i < a_atkHits.Count; i++)
        {
            if (a_atkHits[i].transform.CompareTag(targetTag))
            {
                if (new Vector2(a_atkHits[i].transform.position.x - centerPos.transform.position.x,
                    a_atkHits[i].transform.position.y - centerPos.transform.position.y).magnitude > overRange) //타겟<-this 벡터의 크기가 range보다 길때, trigger = true;  
                {
                    trigger = true;
                    break;
                }

                else
                    trigger = false;
                
            }
        }

        return trigger;
    }

    public bool Action()
    {
        if (trigger && coolDown <= 0)
        {
            BattleSystem._Instance.CreateParticleEffect(BattleSystem._Instance.boss_SwordDance_ParticleSystem, this.transform);
            Instantiate(sonicWavePrefab, this.transform);
            SoundManager._instance.PlaySound("SwordDance_Warning", 8);
            GetComponent<Animator>().SetTrigger("SwordDance");
            StartCoroutine(DelayToSkill());

            trigger = false;
            return true;
        }

        return false;
    }

    public IEnumerator DelayToSkill(float delay = 0.45f) //공격 애니메이션이 시작 후 딜레이를 준 다음 공격 판정을 내린다. 플레이어가 보고 피할 수 있도록.
    {

        yield return new WaitForSeconds(delay);

        Tile_SwordDance t_tile_SwordDance;

        SoundManager._instance.PlaySound("SwordDance", 4);
        coolDown = maxCoolDown;

        t_tile_SwordDance = skillPrefab.GetComponent<Tile_SwordDance>();
        t_tile_SwordDance.atkerTag = this.transform.tag;
        t_tile_SwordDance.targetTag = targetTag;

        Instantiate(skillPrefab, this.transform);

        
    }
}
