using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class PlayerSight : MonoBehaviour
{
    public CharTableData charData;
    public Light2D light2D;
    public float elightRadius = 1000;
    float radius;
    [Header("시야 범위 설정. 플레이어의 시야 범위 수치를 조정합니다.")]
    public float lightValue = 250;
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        if(charData.m_armor>=100) //개안 중이면
            radius = elightRadius;
        else //평소에, 
            radius = charData.m_curHP / charData.m_maxHP * lightValue;

        light2D.pointLightOuterRadius = Mathf.Lerp(light2D.pointLightOuterRadius, radius, Time.deltaTime * 2);

        /*currentFill = bossTableData.m_curHP / bossTableData.m_maxHP;
        fillBar.fillAmount = Mathf.Lerp(fillBar.fillAmount, currentFill, Time.deltaTime * 2);
        bossName.text = bossTableData.m_unitName;*/
    }
}
