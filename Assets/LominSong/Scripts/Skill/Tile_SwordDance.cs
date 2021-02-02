using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_SwordDance : MonoBehaviour
{
    public float lifeTime = 5;
    public float damage = 50;
    public float knockBack_scale = 150;
    public float speed = 50;
    public string atkerTag = "Boss";
    public string targetTag = "Player";
    public GameObject sonicWave;
    

    Rigidbody2D rigid2d;
    float playTime;
    Animator m_cameraAni;
    Transform targetPos;
    

    private void Awake()
    {

    }

    private void Start()
    {
        rigid2d = GetComponent<Rigidbody2D>();
        m_cameraAni = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        targetPos = GameObject.FindGameObjectWithTag(targetTag).transform;
        if (targetPos.position.x > this.transform.position.x)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
            
        }

        else 
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
            
    }

    void FixedUpdate()
    {
        playTime += Time.deltaTime;

        if (playTime >= lifeTime)
            this.gameObject.SetActive(false);

        this.transform.localPosition = new Vector2(this.transform.localPosition.x + Random.Range(-0.001f, 0.001f), this.transform.localPosition.y + Random.Range(-0.001f, 0.001f));

        if (this.transform.localScale.x == 1)
            rigid2d.velocity = Vector2.left * speed;

        else if (this.transform.localScale.x == -1)
            rigid2d.velocity = Vector2.right * speed;

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == targetTag && targetTag != null)
            targetTag = BattleSystem._Instance.AtkLoop(this.transform.parent.GetComponent<CharTableData>(), col.gameObject, damage, knockBack_scale, 0.8f);
    }
}
