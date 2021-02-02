using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene_After : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CinematicSystem._instance.loadScene.enabled = true;

        CutSceneParam();

        if (CinematicSystem._instance.delay <= 0)
            CutSceneManager();

        CinematicSystem._instance.delay -= Time.deltaTime;
    }

    public void CutSceneInit()
    {
        switch (CinematicSystem._instance.currentCut)
        {
            case 1:
                break;

            case 2:
                break;

            case 3:

                break;

            case 4:
                break;

            case 5:

                break;

            case 6:

                break;

            case 7:

                break;

            case 8:

                break;

            case 9:

                break;

            default:

                break;

        }
    }

    public void CutSceneManager()
    {
        switch (CinematicSystem._instance.currentCut)
        {
            case 1:
                CinematicSystem._instance.PlayerAllAnimationClear();
                CinematicSystem._instance.cinematicLine.SetActive(false);
                CinematicSystem._instance.SetMainCameraSizer(80);
                CinematicSystem._instance.SetMainCameraPos(new Vector3(-16, -14.5f, -10));
                CinematicSystem._instance.BossAllAnimationClear();
                CinematicSystem._instance.bossAni.SetBool("BossCut4",true);
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(2);
                break;

            case 2:
                CinematicSystem._instance.screenName.color = new Color(1, 0.9f, 0, 1);
                CinematicSystem._instance.ChangeScreenName("밀레드");
                CinematicSystem._instance.TypingScreenText("말도 안돼....그 기술은.....");
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(999);
                break;

            case 3:
                CinematicSystem._instance.ChangeScreenName("밀레드");
                CinematicSystem._instance.TypingScreenText("그렇군........마지막까지 유산을 남겨두고 간건가.....");
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(999);
                break;

            case 4:
                CinematicSystem._instance.ChangeScreenName("밀레드");
                CinematicSystem._instance.TypingScreenText("그녀의 유산을 물려받았다. 그래도 가겠다는 건가?");
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(999);
                
                break;

            case 5:
                CinematicSystem._instance.playerAni.transform.localScale = Vector3.one;
                CinematicSystem._instance.MoveObjectPos(CinematicSystem._instance.playerAni.gameObject, new Vector3(-77.4f, -45.6f, 0), 0.5f);
                CinematicSystem._instance.playerAni.SetInteger("AnimState", 2);
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                //CinematicSystem._instance.SetDelay(2f);
                break;

            case 6:
                CinematicSystem._instance.ChangeScreenName("밀레드");
                CinematicSystem._instance.TypingScreenText("아이단!!!");
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(999);
                break;

            case 7:
                CinematicSystem._instance.SetDelay(0.6f);
                CinematicSystem._instance.MoveObjectPos(CinematicSystem._instance.playerAni.gameObject, new Vector3(-200f, -45.6f, 0), 0.8f);
                CinematicSystem._instance.playerAni.SetInteger("AnimState", 2);
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(2f);
                break;

            case 8:
                CinematicSystem._instance.MoveMainCameraPos(new Vector3(45, -14.5f, -10), 1f);
                CinematicSystem._instance.BossAllAnimationClear();
                CinematicSystem._instance.bossAni.SetBool("BossCut5", true);
                CinematicSystem._instance.ChangeScreenName("밀레드");
                CinematicSystem._instance.TypingScreenText("다나한.... 이게 그대가 말했던 최악의 경우인가?");
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(999);
                break;

            case 9:
                CinematicSystem._instance.ChangeScreenName("밀레드");
                CinematicSystem._instance.TypingScreenText("그럼에도 나는...");
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(999);
                break;

            case 10:
                CinematicSystem._instance.ChangeScreenName("밀레드");
                CinematicSystem._instance.TypingScreenText("..........아이단...");
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(999);
                break;

            case 11:
                CinematicSystem._instance.MoveMainCameraPos(new Vector3(-150, -14.5f, -10), 1f);
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(1);
                break;

            case 12:
                CinematicSystem._instance.loadScene.enabled = true;
                break;

            case 13:
                break;

            case 14:
                break;

            case 15:
                
                break;

            case 16:

                break;

            case 17:

                break;

            default:

                break;

        }

    }

    public void CutSceneParam()
    {
        switch (CinematicSystem._instance.currentCut)
        {
            case 1:
                break;

            case 2:
                break;

            case 3:
                CinematicSystem._instance.TypingParam("말도 안돼....그 기술은.....");
                /*if (CinematicSystem._instance.moveObjectCoroutineState)
                {
                    CinematicSystem._instance.moveObjectCoroutineState = false;
                    CinematicSystem._instance.bossAni.SetBool("BossCut1", false);
                }

                if (CinematicSystem._instance.moveCameraCoroutineState)
                {
                    CinematicSystem._instance.moveCameraCoroutineState = false;
                    CinematicSystem._instance.delay = 0;
                }*/

                break;

            case 4:
                CinematicSystem._instance.TypingParam("그렇군........마지막까지 유산을 남겨두고 간건가.....");
                break;

            case 5: //아이단 : "......." 타이핑 중
                CinematicSystem._instance.TypingParam("그녀의 유산을 물려받았다. 그래도 가겠다는 건가?");
                
                break;

            case 6:
                if (CinematicSystem._instance.moveObjectCoroutineState)
                {
                    CinematicSystem._instance.moveObjectCoroutineState = false;
                    CinematicSystem._instance.PlayerAllAnimationClear();
                }
                break;

            case 7:
                if (CinematicSystem._instance.moveObjectCoroutineState)
                {
                    CinematicSystem._instance.moveObjectCoroutineState = false;
                    CinematicSystem._instance.PlayerAllAnimationClear();
                }
                CinematicSystem._instance.TypingParam("아이단!!!");
                break;

            case 8:
                if (CinematicSystem._instance.moveObjectCoroutineState)
                {
                    CinematicSystem._instance.moveObjectCoroutineState = false;
                    CinematicSystem._instance.PlayerAllAnimationClear();
                }
                break;

            case 9:
                CinematicSystem._instance.TypingParam("다나한.... 이게 그대가 말했던 최악의 경우인가?");
                break;

            case 10:
                CinematicSystem._instance.TypingParam("그럼에도 나는...");
                break;

            case 11:
                CinematicSystem._instance.TypingParam("..........아이단...");
                break;

            case 12:

                break;

            case 13:

                break;

            case 14:

                break;

            case 15:

                break;

            case 16:

                break;

            case 17:

                break;

            default:

                break;

        }
    }
}
