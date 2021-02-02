using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    public Image fillBar;
    public Text bossName;
    private GameObject bossGameObject;
    public CharTableData bossTableData;
    public float currentFill;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(bossGameObject == null)
        {
            
            bossGameObject = GameObject.FindWithTag("Boss");

            if(bossGameObject!=null)
                bossTableData = bossGameObject.GetComponent<CharTableData>();

            else
            {
                fillBar.fillAmount = 0;
                bossName.text = " ";
            }
            
            
        }
        else
        {
            currentFill = bossTableData.m_curHP / bossTableData.m_maxHP;
            fillBar.fillAmount = Mathf.Lerp(fillBar.fillAmount, currentFill, Time.deltaTime * 2);
            bossName.text = bossTableData.m_unitName;
        }
    }
}
