using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Lighting : MonoBehaviour
{
    public AnimationClip clip;
    Animator m_cameraAni;
    public float damage = 50;
    public float knockBack_scale = 150;
    public string atkerTag = "Boss";
    public string targetTag = "Player";
    float playTime;

    private void Start()
    {
        m_cameraAni = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        
        SoundManager._instance.PlaySound("Storm", 0.1f);
    }

    void Update()
    {
        playTime += Time.deltaTime;
        if (playTime >= clip.length+0.1f)
            this.transform.parent.gameObject.SetActive(false);

        this.transform.localPosition = new Vector2(this.transform.localPosition.x+Random.Range(-0.001f, 0.001f), this.transform.localPosition.y+Random.Range(-0.001f, 0.001f));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == targetTag && targetTag != null)
        {
            targetTag = BattleSystem._Instance.AtkLoop(GameObject.FindGameObjectWithTag(atkerTag).GetComponent<CharTableData>(), col.gameObject, damage, knockBack_scale, 0.5f);
        }
    }
}
