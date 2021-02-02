using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // Start is called before the first frame update
    public string sceneName;
    public float sceneChangeDelayTime = 0f;
    public Animator fadeOut_Panel_Animator;
    public string backGroundMusicName;


    public void m_ChangeScene() 
    {
        SoundManager._instance.PlaySound("Title_click", 2);

        if(fadeOut_Panel_Animator)
            fadeOut_Panel_Animator.SetBool("FadeOut", true);

        StartCoroutine(DelayToSceneChange(sceneName, sceneChangeDelayTime));
    }

    public void m_ChangeScene(string sceneName, float delay = 0, Animator fadeOutPanelAnimator = null, string bgmName = null)
    {
        if (fadeOutPanelAnimator)
            fadeOutPanelAnimator.SetBool("FadeOut", true);

        else if(fadeOut_Panel_Animator)
            fadeOut_Panel_Animator.SetBool("FadeOut", true);

        StartCoroutine(DelayToSceneChange(sceneName, delay, bgmName));
    }

    public IEnumerator DelayToSceneChange(string sceneName, float delay = 0f, string bgmName = null)
    {
        yield return new WaitForSeconds(delay);

        Camera.main.enabled = false;

        SceneManager.LoadScene(sceneName);

        if(bgmName!=null)
            SoundManager._instance.ChangeBGM(bgmName, 0.3f);
        else
            SoundManager._instance.ChangeBGM(backGroundMusicName,0.3f);
    }
}
