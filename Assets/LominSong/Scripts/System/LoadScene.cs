using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public Image loadingBar;
    public string loadSceneName;
    public float delay;
    public Animator fadeOut_Panel_Animator;


    // Start is called before the first frame update
    void Start()
    {
        InventoryManager._instance.ItemRecall();
        loadingBar.fillAmount = 0f;
        if (fadeOut_Panel_Animator)
            fadeOut_Panel_Animator.SetBool("FadeOut", true);
        StartCoroutine(LoadAsyncScene());
    }

    IEnumerator LoadAsyncScene()
    {
        yield return null;

        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(loadSceneName);
        asyncScene.allowSceneActivation = false;
        float timeC = 0;

        while(!asyncScene.isDone)
        {
            yield return null;
            
            timeC += Time.deltaTime;

            if(asyncScene.progress >= 0.9f)
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1, timeC / delay);

                if(loadingBar.fillAmount >= 0.99f)
                {
                    asyncScene.allowSceneActivation = true;
                }
            }

            else
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, asyncScene.progress, timeC / delay);

                if (loadingBar.fillAmount >= asyncScene.progress)
                    timeC = 0f;
            }

        }
    }

    
}
