using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem _Instance;
    public GameObject endingPanel;
    public GameObject fadeOutPanel;
    public AudioClip winBgmClip;
    [Space]
    public ParticleSystem player_Hurt_ParticleSystem;
    public ParticleSystem player_Atk_ParticleSystem;
    public ParticleSystem player_Cunt_ParticleSystem;
    public ParticleSystem boss_ShockWave_ParticleSystem;
    public ParticleSystem boss_SwordDance_ParticleSystem;
    public LoadScene clearSceneLoad;
    public LoadScene deadSceneLoad;

    private CharTableData playerTable;

    private GameObject bossObject;
    private CharTableData bossTable;
    Animator m_cameraAni;

    [HideInInspector]
    public bool isEnd;
    [HideInInspector]
    public bool isDead;

    private void Awake()
    {
        _Instance = this;
    }

    private void Start()
    {
        m_cameraAni = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if(bossObject == null)
        {
            bossObject = GameObject.FindGameObjectWithTag("Boss");
            bossTable = bossObject.GetComponent<CharTableData>();
        }

        else
        {
            if (bossTable.m_curHP <= 0 && isEnd == false)
            {
                BossDeathProcedure();
            }
                
        }

        if(playerTable == null)
        {
            playerTable = Bandit._Instance.charTableData;
        }

        else
        {
            if(playerTable.m_curHP <= 0 && isDead == false)
            {
                PlayerDeathProcedure();
            }
        }
    }

    public bool CheckParring()
    {
        if (Bandit._Instance.m_Parring >= 0)
        {
            Bandit._Instance.charTableData.m_curHP += Bandit._Instance.charTableData.m_parringHeal; //시야 범위 회복
            Bandit._Instance.charTableData.m_curHP = Mathf.Clamp(Bandit._Instance.charTableData.m_curHP, 0, Bandit._Instance.charTableData.m_maxHP); //최대치 벗어나지 않게
            SoundManager._instance.PlaySound("Player_Parring" + Random.Range(1, 3), 1); //패링 성공 사운드 출력
            //Bandit._Instance.m_Parring = 0;
            Bandit._Instance.m_CounterAtk = true;
            Bandit._Instance.m_AniDelay = 0; //패링 애니메이션 초기화
            Bandit._Instance.m_animator.SetInteger("AnimState", 0);
            Bandit._Instance.m_parringParticle.Play();
            StartCoroutine(DelayToStopParticle(Bandit._Instance.m_parringParticle, 0.22f));

            return true;
        }

        return false;
    }

    public bool CheckDashing()
    {
        if (Bandit._Instance.m_Dashing >= 0)
        {
            return true;
        }

        return false;
    }

    public bool KnockBack(GameObject pusher, GameObject victim, float scale, float delayTime = 0.5f) //#주의) victim이 rigidbody2D가 있어야함.
    {
        Vector2 pushPos = (victim.transform.position - pusher.transform.position).normalized;
        Rigidbody2D vicRigid = victim.GetComponent<Rigidbody2D>();

        if (vicRigid == null)
            return false;

        pushPos.x *= scale;

        if (victim.tag == "Player")
        {
            Bandit._Instance.m_AniDelay = delayTime;
            Bandit._Instance.m_animator.SetInteger("AnimState", 0);
            Bandit._Instance.m_animator.SetTrigger("Hurt");

            CreateParticleEffect(player_Hurt_ParticleSystem, Bandit._Instance.transform);
        }
            

        vicRigid.velocity = Vector2.zero;

        vicRigid.velocity = pushPos;

        return true;
    }




    public void NaturallyHurtAnimation(GameObject target)
    {
        if (CheckAniState(target, "Idle"))
            target.GetComponent<Animator>().SetTrigger("Hurt");
        
    }


    public bool CheckAniState(GameObject target, string ClipName)
    {
        Animator target_ani = target.GetComponent<Animator>();
        if (!target_ani.GetCurrentAnimatorStateInfo(0).IsName(ClipName))
            return true;

        return false;
    }



    public string AtkLoop(CharTableData atker, GameObject victim, float damage, float knockBack_Scale, float knockBack_Delay) //스킬 어택 알고리즘
    {
        string targetTag = victim.tag;

        if(targetTag == "Player") //희생자가 플레이어라면
        {
            if (CheckParring()) { }
            else if (CheckDashing()) { }
            else if (victim.gameObject.GetComponent<CharTableData>() != null)
            {
                //Bandit._Instance.m_hurtParticle.Play();
                //DelayToStopPaticle(Bandit._Instance.m_hurtParticle, 0.22f);

                atker.AtkTarget(Bandit._Instance.charTableData, damage);

                KnockBack(atker.gameObject, victim, knockBack_Scale, knockBack_Delay);

                m_cameraAni.SetTrigger("Atked");

                targetTag = "null";
            }
        }

        else if (targetTag == "Boss") //희생자가 보스라면
        {
            atker.AtkTarget(Bandit._Instance.charTableData, damage);
            KnockBack(atker.gameObject, victim, knockBack_Scale, knockBack_Delay);
            m_cameraAni.SetTrigger("Atked");
            targetTag = "null";
        }

        
        return victim.tag;
    }

    

    public void CreateParticleEffect(ParticleSystem particleSystem, Transform transform, float playTime = 0f)
    {
        GameObject temp_Particle_GameObject = Instantiate(particleSystem.gameObject, transform);

        if(playTime == 0)
            StartCoroutine(DelayToDisableObject(temp_Particle_GameObject, particleSystem.main.duration));
        else
            StartCoroutine(DelayToDisableObject(temp_Particle_GameObject, playTime));
    }

    public void BossDeathProcedure()
    {


        isEnd = true;

        StartCoroutine(DelayToChangeTempPanel(4f, winBgmClip, bossObject));
    }

    public void PlayerDeathProcedure()
    {
        isDead = true;
        Bandit._Instance.gameObject.tag = "PlayerDead";
        SoundManager._instance.PlaySound("Player_Die", 1);
        SoundManager._instance.SetVolumeSFX(0f);

        if (InventoryManager._instance.Search("Nuadas_Touch"))
        {
            InventoryManager._instance.Delete("Nuadas_Touch", 9999);
            StartCoroutine(DelayToPlayerResurrection(2f));
        }

        else
        {
            deadSceneLoad.enabled = true;
        }

    }


    #region 코루틴 모음
    public IEnumerator DelayToStopParticle(ParticleSystem particleSystem, float delay) //파티클 시스템 멈춤
    {

        yield return new WaitForSeconds(delay);

        particleSystem.Stop();
    }

    public IEnumerator DelayToDisableObject(GameObject gameObject, float delay) //오브젝트 디스에이블
    {

        yield return new WaitForSeconds(delay);

        gameObject.SetActive(false);
    }

    public IEnumerator DelayToChangeTempPanel(float delay, AudioClip audioClip = null, GameObject disableObject = null) //씬 이동
    {

        yield return new WaitForSeconds(delay);

        if(audioClip)
        {
            SoundManager._instance.ChangeBGM(audioClip, 3);
            SoundManager._instance.SetVolumeSFX(0.1f);
        }
        
        if(disableObject)
            disableObject.SetActive(false);


        InventoryManager._instance.Add(Random.Range(0, 3), Random.Range(0, 2));
        InventoryManager._instance.Add(Random.Range(0, 3), Random.Range(0, 2));
        InventoryManager._instance.Add(Random.Range(0, 3), Random.Range(0, 2));

        clearSceneLoad.enabled = true;
    }

    public IEnumerator DelayToTimeReset(float delay)
    {
        yield return new WaitForSeconds(delay);

        Time.timeScale = 1f;
    }

    public IEnumerator DelayToPlayerResurrection(float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        playerResurrection._Instance.Resurrection();
    }

    #endregion
}
