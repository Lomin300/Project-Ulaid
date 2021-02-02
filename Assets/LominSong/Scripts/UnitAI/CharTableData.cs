using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharTableData : MonoBehaviour
{
    [Header("<유닛 스텟 정의>")]
    [Tooltip("0번은 플레이어부터 각자 오브젝트 ID 코드")]
    public int m_id;
    [Tooltip("UI상에 노출될 닉네임")]
    public string m_unitName;
    [Space]
    [Tooltip("(High to High) 최대 생명력. 시야 비율에 관여됩니다. 시야 계산식 : 현재HP / 최대HP")]
    public float m_maxHP;
    [Tooltip("(High to High) 현재 생명력. 여기에 입력되는 수치는 플레이어 시작 생명력에 연관됩니다.")]
    public float m_curHP;
    [Space]
    [Tooltip("(High to High) 방어력. 수치 1당 1% 데미지 감소")]
    public float m_armor;
    [Space]
    [Tooltip("(High to High) 기본 공격 데미지")]
    public float m_damage;
    [Tooltip("(High to Fast) 공격 속도")]
    public float m_atkSpd;
    [Space]
    [Tooltip("(High to Fast) 이동 속도")]
    public float m_speed;
    [Tooltip("(High to Slow) 이동 빈도. 여기에 입력되는 값은 몬스터 이동 빈도입니다. 몬스터 Action의 이동 빈도는 Act Global Delay + m_moveFrq")]
    public float m_moveFrq;
    [Space]
    [Tooltip("(High to Long) 사정거리.")]
    public float m_range;

    [Header("<Player Only>")]
    [Tooltip("(High to High) 패링 성공 시 HP회복량")]
    public float m_parringHeal;
    [Tooltip("(High to Long) 패링 쿨타임")]
    public float m_parringCoolDown;
    [Tooltip("(High to Long) 패링 판정 시간")]
    public float m_parringJudg;
    [Tooltip("(High to Long) 대쉬 거리")]
    public float m_dashSize;
    [Tooltip("(High to Cool-Down Time) 대쉬 쿨타임")]
    public float m_dashCoolDown;
    [Tooltip("(High to Long) 대쉬 지속 시간")]
    public float m_dashTime;
    [Tooltip("(High to Long) 개안 지속 시간")]
    public float m_EnlightTime;
    [Tooltip("(High to Cool-Down Time) 개안 쿨타임")]
    public float m_EnlightCoolDown;


    //내부 파라미터
    [HideInInspector]
    public int hurtCount; //피격 카운트
    [HideInInspector]
    public int contiHurt; //연속된 피격 카운트. 타격 성공 시, 0으로 초기화
    

    #region <공격 스크립트> 대상에게 피해를 줌.
    public void AtkTarget(CharTableData target, bool through = false)
    {
        if (through) //관통 데미지 여부
            target.m_curHP -= this.m_damage;
        else
            target.m_curHP -= this.m_damage*(100 - target.m_armor)/100;

        Hurt(target);
    }

    public void AtkTarget(CharTableData target, float deal, bool through = false)
    {
        if (through)
            target.m_curHP -= deal;
        else
            target.m_curHP -= deal * (100 - target.m_armor)/100;

        Hurt(target);
    }

    public void AtkTarget(float deal, bool through = false) //타겟을 넣지 않으면 본인을 대상으로 피해를 입겠다.
    {
        if (through)
            this.m_curHP -= deal;
        else
            this.m_curHP -= deal * (100 - this.m_armor) / 100;

        this.gameObject.GetComponent<Animator>().SetTrigger("Hurt");
        this.hurtCount++;
        this.contiHurt++;
    }

    protected void Hurt(CharTableData target) //피격 애니메이션 및 기타
    {
        if (target.m_id == 0)
        {
            BattleSystem._Instance.NaturallyHurtAnimation(target.gameObject);
            
        }
            
        //target.gameObject.GetComponent<Animator>().SetTrigger("Hurt");
        target.hurtCount++;
        target.contiHurt++;
        this.contiHurt = 0;
    }
    #endregion
}
