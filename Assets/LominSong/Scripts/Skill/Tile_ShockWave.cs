using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_ShockWave : MonoBehaviour
{
    public AnimationClip clip;
    public float damage = 50;
    public float knockBack_scale = 150;
    public string atkerTag = "Boss";
    public string targetTag = "Player";
    float playTime;

    Animator m_cameraAni;

    private void Awake()
    {
        
    }

    private void Start()
    {
        m_cameraAni = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
    }

    void Update()
    {
        playTime += Time.deltaTime;
        if (playTime >= clip.length)
            this.gameObject.SetActive(false);

        this.transform.localPosition = new Vector2(Random.Range(-0.001f, 0.001f), Random.Range(-0.001f, 0.001f));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == targetTag && targetTag != null)
        {
            targetTag = BattleSystem._Instance.AtkLoop(this.transform.parent.parent.GetComponent<CharTableData>(), col.gameObject, damage, knockBack_scale, 1.6f);
        }
    }

    /*private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == targetTag)
        {
            if (BattleSystem._Instance.CheckParring()) { }
            else if (BattleSystem._Instance.CheckDashing()) { }
            else if (col.gameObject.GetComponent<CharTableData>() != null)
            {
                col.gameObject.GetComponent<CharTableData>().AtkTarget(damage);
                this.enabled = false;
                Debug.Log("충격파 데미지 받음");
            }


        }
    }*/
}
