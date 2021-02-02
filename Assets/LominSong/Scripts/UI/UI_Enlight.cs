using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Enlight : MonoBehaviour
{
    public GameObject prefab_EnlightEffect;
    public List<Sprite> sprites = new List<Sprite>();
    Image m_image;
    ParticleSystem prefab_particle;
    int m_playerEnlightFigureToInt;
    float m_playerEnlightFigure;

    // Start is called before the first frame update
    void Start()
    {
        m_image = GetComponent<Image>();
        prefab_particle = prefab_EnlightEffect.GetComponent<ParticleSystem>();
        prefab_particle.Stop();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Bandit._Instance.charTableData.m_curHP / Bandit._Instance.charTableData.m_maxHP == 1 && Bandit._Instance.m_Elighting > 0)
            m_playerEnlightFigureToInt = sprites.Count-1;
        else
        {
            m_playerEnlightFigure = ((Bandit._Instance.charTableData.m_curHP / Bandit._Instance.charTableData.m_maxHP) * 100) / (100 / (sprites.Count - 1));
            m_playerEnlightFigureToInt = (int)m_playerEnlightFigure;
        }
            

        if (m_playerEnlightFigureToInt == sprites.Count-1 && prefab_particle.isPlaying == false)
            prefab_particle.Play();
        else if(m_playerEnlightFigureToInt != sprites.Count - 1)
            prefab_particle.Stop();



        m_playerEnlightFigureToInt = Mathf.Clamp(m_playerEnlightFigureToInt, 0, sprites.Count - 1);

        m_image.sprite = sprites[m_playerEnlightFigureToInt];
    }
}
