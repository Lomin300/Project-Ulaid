using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CinematicSystem : MonoBehaviour
{
    public static CinematicSystem _instance;
    public GameObject cinematicLine;
    public Text screenName;
    public Text screenText;
    public Animator playerAni;
    public Animator bossAni;
    public LoadScene loadScene;

    [HideInInspector]
    public int currentCut = 1;
    [HideInInspector]
    public float delay;
    [HideInInspector]
    public bool moveCameraCoroutineState = false;
    [HideInInspector]
    public bool moveCameraSizerCoroutineState = false;
    [HideInInspector]
    public bool typingCoroutineState = false;
    [HideInInspector]
    public bool moveObjectCoroutineState = false;
    [HideInInspector]
    public bool typingEndSwitch = false;
    [HideInInspector]
    public Dictionary<string, IEnumerator> coroutineDic = new Dictionary<string, IEnumerator>();

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        
    }


    public void SetMainCameraSizer(float size) => Camera.main.orthographicSize = size;

    public void SetMainCameraPos(Vector3 targetPos) => Camera.main.transform.position = targetPos;

    public void SetDelay(float num) => delay = num;

    public void MoveMainCameraPos(Vector3 targetPos, float speed)
    {
        coroutineDic.Add("MoveCameraCoroutine", MoveCameraCoroutine(targetPos, speed));

        StartCoroutine(coroutineDic["MoveCameraCoroutine"]);
    }

    public void MoveObjectPos(GameObject currentObject,Vector3 targetPos, float speed)
    {
        coroutineDic.Add("MoveObjectCoroutine", MoveObjectCoroutine(currentObject, targetPos, speed));

        StartCoroutine(coroutineDic["MoveObjectCoroutine"]);
    }

    public void MoveMainCameraSizer(float size, float speed)
    {
        coroutineDic.Add("MoveCameraSizerCoroutine", MoveCameraSizerCoroutine(size, speed));

        StartCoroutine(coroutineDic["MoveCameraSizerCoroutine"]);
    }

    public void BossAllAnimationClear()
    {
        bossAni.SetBool("BossCut1", false);
        bossAni.SetBool("BossCut2", false);
        bossAni.SetBool("BossCut3", false);
        bossAni.SetBool("BossCut4", false);
        bossAni.SetBool("BossCut5", false);
    }

    public void PlayerAllAnimationClear()
    {
        playerAni.SetInteger("AnimState", 0);
        playerAni.SetBool("Embroid", false);
        playerAni.SetBool("PlayerCut1", false);
        playerAni.SetBool("PlayerCut2", false);
    }

    public void ChangeScreenName(string text)
    {
        screenName.text = "[" + text + "]";
    }

    public void ChangeScreenText(string text)
    {
        if(coroutineDic.ContainsKey("TypingScreenTextCoroutine"))
            StopCoroutine(coroutineDic["TypingScreenTextCoroutine"]);

        coroutineDic.Remove("TypingScreenTextCoroutine");

        screenText.text = text;
    }

    public void TypingScreenText(string text, float typingSpeed = 0.1f)
    {
        screenText.text = null;

        if (coroutineDic.ContainsKey("TypingScreenTextCoroutine"))
            StopCoroutine(coroutineDic["TypingScreenTextCoroutine"]);

        
        coroutineDic.Add("TypingScreenTextCoroutine", TypingScreenTextCoroutine(text, typingSpeed));

        StartCoroutine(coroutineDic["TypingScreenTextCoroutine"]);
    }

    public void TypingParam(string allText)
    {
        Debug.Log("currentCut = " + currentCut);
        if (Input.GetKeyDown("c") || Input.GetKeyDown("x"))
        {
            if(typingEndSwitch)
            {
                Debug.Log("겟키다운 들어옴, 타입스위치 true");
                typingEndSwitch = false;
                delay = 0;
            }

            else
            {
                Debug.Log("겟키다운 들어옴, 타입스위치 false");
                ChangeScreenText(allText);
                typingEndSwitch = true;
            }
        }

        if (typingCoroutineState)
        {
            typingCoroutineState = false;
            delay = 2;
        }
    }

    public void NextCut()
    {
        currentCut = Mathf.Abs(currentCut);
        currentCut++;
        
    }

    #region 코루틴 모음
    public IEnumerator MoveCameraCoroutine(Vector3 targetPos, float speed)
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / 60f);

            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, targetPos, speed);
            if (Vector3.Distance(Camera.main.transform.position, targetPos) <= 0.1f)
            {
                Camera.main.transform.position = targetPos;
                moveCameraCoroutineState = true;
                coroutineDic.Remove("MoveCameraCoroutine");
                break;
            }

        }
    }

    public IEnumerator MoveObjectCoroutine(GameObject currentObject, Vector3 targetPos, float speed)
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / 60f);
            currentObject.transform.position = Vector3.MoveTowards(currentObject.transform.position, targetPos, speed);
            if (Vector3.Distance(currentObject.transform.position, targetPos) <= 0.1f)
            {
                currentObject.transform.position = targetPos;
                moveObjectCoroutineState = true;
                coroutineDic.Remove("MoveObjectCoroutine");
                break;
            }

        }
    }

    public IEnumerator MoveCameraSizerCoroutine(float targetSize, float speed)
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / 60f);

            Camera.main.orthographicSize += speed;
            if (targetSize - Camera.main.orthographicSize <= 1)
            {
                Camera.main.orthographicSize = targetSize;
                moveCameraSizerCoroutineState = true;
                coroutineDic.Remove("MoveCameraSizerCoroutine");
                break;
            }

        }
    }

    public IEnumerator TypingScreenTextCoroutine(string text, float typingSpeed = 0.1f)
    {
        int index = 0;

        while (true)
        {
            yield return new WaitForSeconds(typingSpeed);
            screenText.text += text[index];
            index++;

            if(index >= text.Length)
            {
                typingCoroutineState = true;
                coroutineDic.Remove("TypingScreenTextCoroutine");
                break;
            }
        }
    }
    #endregion
}
