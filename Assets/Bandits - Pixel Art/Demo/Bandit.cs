using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bandit : MonoBehaviour {

    public static Bandit _Instance;
    public Animator m_camera;
    public Transform centerPos;
    public Transform weaponPos;
    public Transform forwardPos;
    private GameObject dustObject;
    private BoxCollider2D m_boxCollider2D;
    public ParticleSystem m_parringParticle;
    public ParticleSystem m_hurtParticle;

    [SerializeField] float      m_jumpForce = 2.0f;
    [SerializeField] float m_disableNum = 0.2f;

    [HideInInspector]
    public Animator             m_animator;
    [HideInInspector]
    public Rigidbody2D         m_body2d;
    private Sensor_Bandit       m_groundSensor;
    private bool                m_grounded = false;
    private bool                m_combatIdle = false;
    private bool                m_isDead = false;

    //이동 제한을 위한 달리는 애니메이션 클립 정보
    public AnimationClip        m_AttackAniClip;
    public AnimationClip        m_ParringAniClip;
    //애니메이션의 딜레이를 기억함.
    [HideInInspector]
    public float m_AniDelay = 0;
    // -- Handle input and movement --
    private float inputX;
    private GameObject walkingSound;

    
    // 유닛 스텟
    public CharTableData charTableData;
    // 유닛 공격
    private float m_atking;
    private float m_atkCount;
    private Ray2D m_atkRay;
    public float nomalAtkStopTime;
    public float counterAtkStopTime;
    [HideInInspector]
    public bool alwaysCountAtk;

    //유닛 패링
    [HideInInspector]
    public float m_Parring;
    [HideInInspector]
    public bool m_CounterAtk;
    [HideInInspector]
    public float m_parringCoolDown;

    //유닛 대쉬
    [HideInInspector]
    public float m_Dashing;
    private float m_DashCoolDown;

    //유닛 개안
    [HideInInspector]
    public float m_Elighting;
    [HideInInspector]
    public float m_ElightCoolDown;
    [HideInInspector]
    public float m_originArmor;



    public List<RaycastHit2D> atkHits = new List<RaycastHit2D>();

    private void Awake()
    {
        _Instance = this;
    }

    // Use this for initialization
    void Start () {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
        m_atkRay = new Ray2D();
        dustObject = this.transform.Find("Effect_WalkingDust").gameObject;
        m_boxCollider2D = this.gameObject.GetComponent<BoxCollider2D>();
        m_parringParticle.Stop();
        //m_hurtParticle.Stop();
        m_originArmor = charTableData.m_armor;
        walkingSound = SoundManager._instance.PlayLoopSound("Player_Move");
    }
	
	// Update is called once per frame
	void Update () {
        Player_State();

        if(m_AniDelay<=0)
            Player_KeyInput();

        if(m_AniDelay<=0) //키 입력에서 딜레이가 추가될 수 있기 때문에 아래서 한번 더 검사해준다.
            Player_Move();


    }

    //상태 변화 체크 알고리즘
    private void FixedUpdate()
    {
        CheckParam();
    }

    private void Player_State()
    {
        //Check if character just landed on the ground
        /*if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }*/

        if (m_AniDelay > 0)
            dustObject.SetActive(false);

    }

    private void Player_Move()
    {
        inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        CharacterFlip();
      
        // Move
        m_body2d.velocity = new Vector2(inputX * charTableData.m_speed, m_body2d.velocity.y);

        if(inputX!=0)
        {
            walkingSound.SetActive(true);
        }
        else
        {
            walkingSound.SetActive(false);
        }

        //Set AirSpeed in animator
        //m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);
    }
    
    private void Player_KeyInput()
    {

        // -- Handle Animations --
        //Death
        if (Input.GetKeyDown("e"))
        {
            if (!m_isDead)
                m_animator.SetTrigger("Death");
            else
                m_animator.SetTrigger("Recover");

            m_isDead = !m_isDead;
        }

        else if(Input.GetKeyDown("1"))
        {
            InventoryManager._instance.Add("Finds_Eye");
        }

        else if (Input.GetKeyDown("2"))
        {
            InventoryManager._instance.Add("Intangible Sword");
        }
        
        else if (Input.GetKeyDown("3"))
        {
            InventoryManager._instance.Add("Nuadas_Touch");
        }

        //Hurt
        else if (Input.GetKeyDown("q"))
        {
            InventoryManager._instance.Use(0);
        }

        else if(Input.GetKeyDown("w"))
        {
            InventoryManager._instance.Use(1);
        }
        //m_animator.SetTrigger("Hurt");

        else if(Input.GetKeyDown("e"))
        {
            if (InventoryManager._instance.m_freeze)
                InventoryManager._instance.Defrost();
            else
                InventoryManager._instance.Freeze();
        }

        //Attack
        else if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("x"))
        {
            m_animator.SetTrigger("Attack" + m_atkCount % 3);
            m_atkCount++;
            m_animator.SetFloat("AtkSpeed", charTableData.m_atkSpd);

            m_atking = m_AniDelay = FindAniDelay(m_AttackAniClip) / charTableData.m_atkSpd; //어택애니메이션 클립 재생 시간만큼 딜레이

            if (m_CounterAtk)
                SoundManager._instance.PlaySound("Player_CounterSwing");
            else
                SoundManager._instance.PlaySound("Player_Swing" + Random.Range(1, 4), 1);


            m_body2d.velocity = new Vector2(0, 0); //이동 가속도를 제한. 이동 중 공격 시 가속도가 남아있는 것을 제거
        }

        //Parring
        else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown("c") && m_parringCoolDown <= 0)
        {
            m_animator.SetTrigger("Parring");
            m_animator.SetFloat("ParringSpeed", 1 / charTableData.m_parringJudg);

            m_AniDelay = m_Parring = FindAniDelay(m_ParringAniClip) * charTableData.m_parringJudg; //어택애니메이션 클립 재생 시간만큼 딜레이
            m_parringCoolDown = charTableData.m_parringCoolDown;

            m_body2d.velocity = new Vector2(0, 0); //이동 가속도를 제한. 이동 중 공격 시 가속도가 남아있는 것을 제거
        }

        //Change between idle and combat idle
        else if (Input.GetKeyDown("f"))
            m_combatIdle = !m_combatIdle;

        //Jump
        else if (Input.GetKeyDown("space") && m_DashCoolDown <= 0/* && m_grounded*/)
        {
            /*m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            //m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_body2d.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);
            m_groundSensor.Disable(m_disableNum);*/

            m_animator.SetInteger("AnimState", 2);
            m_animator.SetTrigger("Dash");
            m_DashCoolDown = charTableData.m_dashCoolDown;
            m_body2d.velocity = new Vector2(0, 0);
            m_Dashing = m_AniDelay = charTableData.m_dashTime;
            PlayerDash();

        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);
    }

    //Player_Move() 에서 사용됨.
    private void CharacterFlip()
    {
        /*if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            if(weaponPos.localPosition.x<0) //즉 무기 좌표가 왼쪽을 보고 있는 방향이라면 다시 오른쪽으로 회전 시켜준다.
                weaponPos.localPosition = new Vector2(-1 * weaponPos.localPosition.x, weaponPos.localPosition.y);
        }
            
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            if (weaponPos.localPosition.x > 0) //즉 무기 좌표가 오른쪽을 보고 있는 방향이라면 다시 왼쪽으로 회전 시켜준다.
                weaponPos.localPosition = new Vector2(-1 * weaponPos.localPosition.x, weaponPos.localPosition.y);
        }*/

        if(inputX > 0)
        {
            dustObject.SetActive(true);
            transform.localScale = new Vector3(-1, 1, 1);
            //transform.rotation = Quaternion.Euler(0, 180, 0);

        }
        else if(inputX < 0)
        {
            dustObject.SetActive(true);
            transform.localScale = new Vector3(1, 1, 1);
            //transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            dustObject.SetActive(false);
        }
            
    }


    //Player_KeyInput() 에서 사용됨
    private float FindAniDelay(AnimationClip tClip) //애니메이션 클립의 재생 시간을 리턴해준다.
    {
        return tClip.length;
    }

    void CheckParam()
    {
        if(m_atking>=0)
            PlayerAtk();
        if (m_Dashing <= 0)
            m_boxCollider2D.isTrigger = false;
            //PlayerDash();

        if (charTableData.m_curHP >= charTableData.m_maxHP && m_ElightCoolDown <= 0) 
            StartEnlight();

        else if (m_Elighting <= 0)
            EndEnlight();

        if (charTableData.m_curHP <= 0)
            PlayerDeath();

        if (alwaysCountAtk)
            m_CounterAtk = true;

        m_atking -= Time.deltaTime;
        m_Parring -= Time.deltaTime;
        m_AniDelay -= Time.deltaTime;
        m_DashCoolDown -= Time.deltaTime;
        m_parringCoolDown -= Time.deltaTime;
        m_ElightCoolDown -= Time.deltaTime;
        m_Dashing -= Time.deltaTime;
        m_Elighting -= Time.deltaTime;
    }

    protected void PlayerAtk()
    {
        
        m_atkRay.origin = centerPos.position;
        m_atkRay.direction = new Vector2(weaponPos.position.x - centerPos.position.x, weaponPos.position.y - centerPos.position.y).normalized;

        //Debug.DrawRay(m_atkRay.origin, m_atkRay.direction*charTableData.m_range, Color.red);

        atkHits.Clear();
        atkHits.AddRange(Physics2D.RaycastAll(m_atkRay.origin, m_atkRay.direction, charTableData.m_range));

        for(int i=0; i<atkHits.Count; i++)
        {
            if (atkHits[i].transform.CompareTag("Boss")) //보스 레이어를 찾아서
            {
                Debug.Log("Is Hit!");
                
                if(m_CounterAtk)
                {
                    m_camera.SetTrigger("CounterAtk");

                    StartCoroutine(BattleSystem._Instance.DelayToTimeReset(0.01f*nomalAtkStopTime));
                    Time.timeScale = 0.01f;

                    charTableData.AtkTarget(atkHits[i].transform.gameObject.GetComponent<CharTableData>(), charTableData.m_damage * 1.5f, false);

                    BattleSystem._Instance.KnockBack(this.gameObject, atkHits[i].transform.gameObject, 60);

                    SoundManager._instance.PlaySound("Player_CounterAtk",3);

                    BattleSystem._Instance.CreateParticleEffect(BattleSystem._Instance.player_Cunt_ParticleSystem, this.transform);

                }
                else
                {
                    m_camera.SetTrigger("Atk");

                    StartCoroutine(BattleSystem._Instance.DelayToTimeReset(0.01f*counterAtkStopTime));
                    Time.timeScale = 0.01f;

                    charTableData.AtkTarget(atkHits[i].transform.gameObject.GetComponent<CharTableData>(), false); //몬스터의 피를 깎음

                    BattleSystem._Instance.KnockBack(this.gameObject, atkHits[i].transform.gameObject, 30);

                    SoundManager._instance.PlaySound("Player_Atk" + Random.Range(1, 4), 1);

                    

                    BattleSystem._Instance.CreateParticleEffect(BattleSystem._Instance.player_Atk_ParticleSystem, this.transform);
                }
                
                m_atking = 0; //해당 공격 스크립트 동작이 
                break;
            }
        }

        m_CounterAtk = false;
    }

    protected void PlayerDash()
    {
        if (this.transform.localScale.x == 1)
            m_body2d.velocity = Vector2.left * charTableData.m_dashSize;

        else if (this.transform.localScale.x == -1)
            m_body2d.velocity = Vector2.right * charTableData.m_dashSize;

        m_boxCollider2D.isTrigger = true;

        SoundManager._instance.PlaySound("Player_Dash", 5);
    }

    protected void PlayerDeath()
    {
        m_animator.SetTrigger("Death");
        this.enabled = false;
    }

    public void StartEnlight()
    {
        SoundManager._instance.PlaySound("Enlight", 10);
        m_originArmor = charTableData.m_armor;
        charTableData.m_armor = 100;
        m_Dashing = charTableData.m_EnlightTime;

        m_ElightCoolDown = charTableData.m_EnlightCoolDown;
        m_Elighting = charTableData.m_EnlightTime;
    }

    public void EndEnlight()
    {
        charTableData.m_armor = m_originArmor;
    }
}
