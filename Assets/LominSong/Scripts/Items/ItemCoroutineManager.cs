using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCoroutineManager : MonoBehaviour
{
    public static ItemCoroutineManager _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public bool StartCoroutineWithOutOwnership(IEnumerator routin)
    {
        StartCoroutine(routin);

        return true;
    }


    public IEnumerator EndDuration_Item_AlwaysCounterAtk(float delay = 5f)
    {
        yield return new WaitForSeconds(delay);

        Bandit._Instance.alwaysCountAtk = false;
        Bandit._Instance.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

    }
}
