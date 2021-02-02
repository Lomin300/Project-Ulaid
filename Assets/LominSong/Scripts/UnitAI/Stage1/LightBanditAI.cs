using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightBanditAI : MonoBehaviour
{
    AI_enum state = AI_enum.trace;
    List<AI_enum> actList = new List<AI_enum>();

    #region 컴포넌트 파트
    public Transform centerPos;
    public Transform weaponPos;
    private Animator m_animator;
    private Animator m_cameraAni;
    private CharTableData m_charData;
    private GameObject player;
    private Rigidbody2D m_body2d;
    private SpriteRenderer m_spr;
    public ParticleSystem m_impCnt_particle;
    #endregion


    #region 1. Inference 추론 파트
    //1-1. 공격 추론
    Ray2D i_atkRay; //사거리 레이
    float i_atkParam = 0; //공격 가중치
    float i_moveParam = 0;
    #endregion

    #region 2. Decision 판단 파트
    //2-1. 행동 딜레이 선언
    public float m_decGlobalDelay = 1f;
    float m_decNowDelay;
    float m_decAddDelay;
    #endregion

    #region 3. Action 행동 파트
    //3-1. 행동 딜레이 선언
    public float m_actGlobalDelay = 1f; //매 행동에 따른 글로벌 딜레이
    float m_actNowDelay;
    float m_actAddDelay;

    //3-2. 공격 행동
    List<RaycastHit2D> a_atkHits = new List<RaycastHit2D>();
    [Range(0,100)]
    public float a_BossDoubleStroke_probability = 10f;
    private bool a_DoubleStroking;
    #endregion

    #region 4. 스킬 파트
    public List<IAISkill> m_skillList = new List<IAISkill>();
    

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        m_cameraAni = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        m_animator = GetComponent<Animator>();
        m_charData = GetComponent<CharTableData>();
        player = GameObject.FindGameObjectWithTag("Player");
        m_body2d = GetComponent<Rigidbody2D>();
        m_spr = GetComponent<SpriteRenderer>();
        m_impCnt_particle.Stop();

        foreach (IAISkill skill in m_skillList)
        {
            skill.OnEnter();
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //추론
        Inference();

        //판단
        if (m_decNowDelay <= 0)
            Decision();

        //행동
        if (m_actNowDelay <= 0)
            Action();

        m_decNowDelay -= Time.deltaTime;
        m_actNowDelay -= Time.deltaTime;
    }

    //추론
    void Inference()
    {
        checkParam();

        foreach (IAISkill skill in m_skillList) //스킬 추론
        {
            skill.Inference();
        }
    }

    //판단
    void Decision()
    {
        if (i_atkParam > i_moveParam)
            state = AI_enum.atk;
        else
            state = AI_enum.trace;

        foreach (IAISkill skill in m_skillList) //스킬 판단
        {
            if (skill.Decision())
                state = AI_enum.skill;
        }

        if (m_charData.m_curHP <= 0)
            state = AI_enum.die;

        if (Bandit._Instance.charTableData.m_curHP <= 0)
            state = AI_enum.idle;

        m_decNowDelay = m_decGlobalDelay + m_decAddDelay;
    }

    void Action()
    {
        switch (state)
        {
            case AI_enum.idle:
                m_animator.SetInteger("AnimState", 0);
                break;

            case AI_enum.trace:
                m_animator.SetInteger("AnimState", 2);
                A_MoveToTarget(player, m_charData.m_speed);
                StartCoroutine(DelayToIdle());
                break;

            case AI_enum.atk:
                m_animator.SetInteger("AnimState", 0);
                

                if (Random.Range(0, 100) <= a_BossDoubleStroke_probability)
                {
                    m_impCnt_particle.Play();
                    BattleSystem._Instance.DelayToStopParticle(m_impCnt_particle, 0.22f);
                    m_animator.SetTrigger("DoubleAttack");
                    StartCoroutine(DelayToAtk((int)Atk_case.bossAtk,0.5f));
                    StartCoroutine(DelayToDoubleStroke(0.75f));
                }

                else
                {
                    m_animator.SetTrigger("Attack");
                    StartCoroutine(DelayToAtk((int)Atk_case.bossAtk,0.35f));
                }
                break;

            case AI_enum.atkd:

                break;

            case AI_enum.die:
                m_animator.SetTrigger("Death");
                this.gameObject.tag = "Finish";
                this.enabled = false;
                m_charData.enabled = false;
                
                break;

            case AI_enum.skill:
                foreach (IAISkill skill in m_skillList)
                {
                    if (skill.Action())
                        break;
                }
                break;

        }



        m_actNowDelay = m_actGlobalDelay + m_actAddDelay;
    }

    #region 추론 영역 함수
    void checkParam()
    {
        I_AtkParam();
        I_MoveParam();

        //Debug.Log("ATKPARAM : " + i_atkParam);
        //Debug.Log("MOVEPARAM : " + i_moveParam);
    }

    void I_AtkParam() //추론 영역 공격 파람 
    {
        float playerDistance = Mathf.Sqrt(Vector2.SqrMagnitude(new Vector2(player.transform.position.x - this.transform.position.x, 
            player.transform.position.y - this.transform.position.y)));


        //사거리 RAY 설정
        i_atkRay.origin = centerPos.position;
        i_atkRay.direction = new Vector2(weaponPos.position.x - centerPos.position.x, 
            weaponPos.position.y - centerPos.position.y).normalized;

        a_atkHits.Clear();
        a_atkHits.AddRange(Physics2D.RaycastAll(i_atkRay.origin, i_atkRay.direction, m_charData.m_range+3));

        bool playerFinder = false;

        for (int i = 0; i < a_atkHits.Count; i++)
        {
            if (a_atkHits[i].transform.CompareTag("Player")) //플레이어 레이어를 찾아서
            {
                i_atkParam += 60 * Time.deltaTime;
                playerFinder = true;
            }
        }
 
        if(playerFinder == false)
            i_atkParam = 0; //사거리 밖이면 공격할 이유가 없기 때문에 가중치를 0으로 만든다.

        i_atkParam = Mathf.Clamp(i_atkParam, 0, 1000); //atkParam 안정화.
    }

    void I_MoveParam()
    {
        float playerDistance = Mathf.Sqrt(Vector2.SqrMagnitude(new Vector2(player.transform.position.x - this.transform.position.x,
            player.transform.position.y - this.transform.position.y)));

        if (m_charData.m_range < playerDistance)
            i_moveParam += 3*Time.deltaTime;
    }

    void I_lossedAction() //손해를 봤다고 판단되면 행동 속도를 바꿈.
    {
        m_actAddDelay += Random.Range(-m_actGlobalDelay * 0.15f, m_actGlobalDelay * 0.15f);
        m_actAddDelay = Mathf.Clamp(m_actAddDelay, -m_actGlobalDelay*0.6f, m_actGlobalDelay * 0.6f);
    }
    #endregion

    #region 행동 영역 함수
    void A_MoveToTarget(GameObject target, float speed)
    {
        Vector2 moveVec = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);

        moveVec = moveVec.normalized;
        moveVec.Set(moveVec.x * speed, moveVec.y * speed);
        m_body2d.velocity = moveVec;

        if (moveVec.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);

        i_moveParam = 0;
    }

    // X축만 타겟에게 접근
    // 미완성됨. 왜 플레이어와 가까워질수록 속도가 느려지는가? 의문
    /*void A_XMoveToTarget(GameObject target, float speed)
    {
        Vector2 moveVec = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y); //y축은 현재 오브젝트의 포지션

        moveVec = moveVec.normalized;
        moveVec.Set(moveVec.x * speed, moveVec.y);
        m_body2d.velocity = moveVec;

        if (moveVec.x <= 0)
            m_spr.flipX = false;
        else
            m_spr.flipX = true;
    }*/

    void A_BossAtk()
    {
        SoundManager._instance.PlaySound("Boss1_Atk" + Random.Range(1, 2), 1);

        a_atkHits.Clear();
        a_atkHits.AddRange(Physics2D.RaycastAll(i_atkRay.origin, i_atkRay.direction, m_charData.m_range));

        for (int i = 0; i < a_atkHits.Count; i++)
        {
            if (a_atkHits[i].transform.CompareTag("Player")) //플레이어 레이어를 찾아서
            {
                Debug.Log("Boss's Atk Hit!");
                if (CheckParring()) { }

                else if (CheckDashing()) { }

                else
                {
                    m_charData.AtkTarget(a_atkHits[i].transform.gameObject.GetComponent<CharTableData>(), false);
                    m_cameraAni.SetTrigger("Atked");
                    BattleSystem._Instance.KnockBack(this.gameObject, a_atkHits[i].transform.gameObject, 30);
                    SoundManager._instance.PlaySound("Player_Atked" + Random.Range(1, 3), 1);
                    break;
                }

            }
        }
    }

    void A_BossDoubleStroke()
    {

        SoundManager._instance.PlaySound("Boss1_Atk" + Random.Range(1, 2), 1);

        a_atkHits.Clear();
        a_atkHits.AddRange(Physics2D.RaycastAll(i_atkRay.origin, i_atkRay.direction, m_charData.m_range));

        for (int i = 0; i < a_atkHits.Count; i++)
        {
            if (a_atkHits[i].transform.CompareTag("Player")) //플레이어 레이어를 찾아서
            {
                
                 m_charData.AtkTarget(a_atkHits[i].transform.gameObject.GetComponent<CharTableData>(), false);
                 m_cameraAni.SetTrigger("Atked");
                 BattleSystem._Instance.KnockBack(this.gameObject, a_atkHits[i].transform.gameObject, 30);
                 SoundManager._instance.PlaySound("Player_Atked" + Random.Range(1, 3), 1);
                 break;
            }
        }
    }

    public IEnumerator DelayToAtk(int atkCase, float delay = 0.35f) //공격 애니메이션이 시작 후 딜레이를 준 다음 공격 판정을 내린다. 플레이어가 보고 피할 수 있도록.
    {

        yield return new WaitForSeconds(delay);

        switch(atkCase)
        {
            case (int)Atk_case.bossAtk:
                A_BossAtk();
                break;

            case (int)Atk_case.bossDoubleStroke:
                m_actNowDelay += delay;
                A_BossDoubleStroke();
                break;
        }
        
    }

    public IEnumerator DelayToDoubleStroke(float delay = 0.25f)
    {
        yield return new WaitForSeconds(delay);
        A_BossDoubleStroke();
    }

    public IEnumerator DelayToIdle()
    {

        yield return new WaitForSeconds(0.6f);

        m_animator.SetInteger("AnimState", 0);

    }

    #endregion

    #region 피동 함수 (피격 당하거나, 패링 체크 등)

    bool CheckParring()
    {
        if(Bandit._Instance.m_Parring >= 0)
        {
            Bandit._Instance.charTableData.m_curHP += Bandit._Instance.charTableData.m_parringHeal; //시야 범위 회복
            Bandit._Instance.charTableData.m_curHP = Mathf.Clamp(Bandit._Instance.charTableData.m_curHP, 0, Bandit._Instance.charTableData.m_maxHP); //최대치 벗어나지 않게
            SoundManager._instance.PlaySound("Player_Parring" + Random.Range(1, 3), 1); //패링 성공 사운드 출력
            Bandit._Instance.m_Parring = 0;
            Bandit._Instance.m_CounterAtk = true;
            Bandit._Instance.m_AniDelay = 0; //패링 애니메이션 초기화
            Bandit._Instance.m_animator.SetInteger("AnimState", 0);

            //플레이어가 패링 성공 시, 공격 손해로 간주함. 따라서 행동 속도를 변화함.
            I_lossedAction();

            return true;
        }
        
        return false;
    }

    bool CheckDashing()
    {
        if(Bandit._Instance.m_Dashing >= 0)
        {
            I_lossedAction();

            return true;
        }

        return false;
    }


    #endregion
}
