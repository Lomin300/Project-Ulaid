using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene_Before : MonoBehaviour
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
                CinematicSystem._instance.bossAni.SetBool("BossCut1", true);
                Debug.Log("컷씬 INIT");
                break;

            case 4:
                break;

            case 5: //아이단 : "......." 타이핑 중

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
            case 1: //석상 머리 바라봄
                CinematicSystem._instance.playerAni.SetBool("Embroid", true);
                CinematicSystem._instance.SetMainCameraSizer(80);
                CinematicSystem._instance.SetMainCameraPos(new Vector3(-16, 107, -10));
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(2);
                break;

            case 2: //아래로 내려오는 카메라 워킹
                CinematicSystem._instance.MoveMainCameraPos(new Vector3(-16, 3, -10), 0.6f);
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(999);
                break;

            case 3: //시네마틱 라인을 지우고 시야 범위 확장
                CinematicSystem._instance.cinematicLine.SetActive(false);
                CinematicSystem._instance.MoveMainCameraSizer(120, 3);
                CinematicSystem._instance.MoveObjectPos(CinematicSystem._instance.bossAni.gameObject, new Vector3(45, -45.4f, 0), 0.65f);
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(999);
                break;

            case 4: //플레이어 대사 출력
                CinematicSystem._instance.screenName.color = new Color(0.6f, 0, 0.85f, 1);
                CinematicSystem._instance.ChangeScreenName("???");
                CinematicSystem._instance.TypingScreenText(".......", 0.5f);
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(999);
                break;

            case 5:
                CinematicSystem._instance.BossAllAnimationClear();
                CinematicSystem._instance.bossAni.SetBool("BossCut2", true);
                CinematicSystem._instance.screenName.color = new Color(1, 0.9f, 0, 1);
                CinematicSystem._instance.ChangeScreenName("밀레드");
                CinematicSystem._instance.TypingScreenText("이곳은 우리의 우상이 묻혀있는 곳.", 0.1f);
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(999);
                break;

            case 6:
                CinematicSystem._instance.PlayerAllAnimationClear();
                CinematicSystem._instance.playerAni.SetBool("PlayerCut1", true);
                CinematicSystem._instance.TypingScreenText("그리고 끝까지 너를 지켜주던 은인의 무덤이지.", 0.1f);
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(999);
                break;

            case 7:
                CinematicSystem._instance.TypingScreenText("그녀는 너의 행동을 원하지 않을거다..울라드에서 조차도...", 0.1f);
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(999);
                break;

            case 8:
                CinematicSystem._instance.TypingScreenText("......... 그래도 갈텐가?", 0.1f);
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(999);
                break;

            case 9:
                CinematicSystem._instance.PlayerAllAnimationClear();
                CinematicSystem._instance.playerAni.SetBool("PlayerCut2", true);
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(2.5f);
                break;

            case 10:
                CinematicSystem._instance.TypingScreenText("그런가...", 0.1f);
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(999);
                break;

            case 11:
                CinematicSystem._instance.BossAllAnimationClear();
                CinematicSystem._instance.bossAni.SetBool("BossCut3", true);
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(2.5f);
                break;

            case 12:
                CinematicSystem._instance.TypingScreenText("그녀가 구한 너를 개죽음으로 몰고갈 순 없다.", 0.1f);
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(999);
                break;

            case 13:
                CinematicSystem._instance.TypingScreenText("또한 그녀가 원하지 않은 길을 걷게할 수도 없고!", 0.1f);
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(999);
                break;

            case 14:
                CinematicSystem._instance.TypingScreenText("그러니 원망마라..설령 남은 빛조차 잊어버릴 지라도!", 0.1f);
                CinematicSystem._instance.NextCut();
                CutSceneInit();
                CinematicSystem._instance.SetDelay(999);
                break;

            case 15:
                CinematicSystem._instance.loadScene.enabled = true;
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

                if (CinematicSystem._instance.moveObjectCoroutineState)
                {
                    CinematicSystem._instance.moveObjectCoroutineState = false;
                    CinematicSystem._instance.bossAni.SetBool("BossCut1", false);
                }

                if (CinematicSystem._instance.moveCameraCoroutineState)
                {
                    CinematicSystem._instance.moveCameraCoroutineState = false;
                    CinematicSystem._instance.delay = 0;
                }

                break;

            case 4:
                if (CinematicSystem._instance.moveObjectCoroutineState)
                {
                    CinematicSystem._instance.moveObjectCoroutineState = false;
                    CinematicSystem._instance.bossAni.SetBool("BossCut1", false);
                }

                if (CinematicSystem._instance.moveCameraSizerCoroutineState)
                {
                    CinematicSystem._instance.moveCameraSizerCoroutineState = false;
                    CinematicSystem._instance.delay = 0;
                }
                break;

            case 5: //아이단 : "......." 타이핑 중
                if (CinematicSystem._instance.moveObjectCoroutineState)
                {
                    CinematicSystem._instance.moveObjectCoroutineState = false;
                    CinematicSystem._instance.bossAni.SetBool("BossCut1", false);
                }

                CinematicSystem._instance.TypingParam(".......");
                break;

            case 6:
                CinematicSystem._instance.TypingParam("이곳은 우리의 우상이 묻혀있는 곳");
                break;

            case 7:
                CinematicSystem._instance.TypingParam("그리고 끝까지 너를 지켜주던 은인의 무덤이지.");
                break;

            case 8:
                CinematicSystem._instance.TypingParam("그녀는 너의 행동을 원하지 않을거다..울라드에서 조차도...");
                break;

            case 9:
                CinematicSystem._instance.TypingParam("......... 그래도 갈텐가?");
                break;

            case 10:

                break;

            case 11:
                CinematicSystem._instance.TypingParam("그런가...");
                break;

            case 12:

                break;

            case 13:
                CinematicSystem._instance.TypingParam("그녀가 구한 너를 개죽음으로 몰고갈 순 없다.");
                break;

            case 14:
                CinematicSystem._instance.TypingParam("또한 그녀가 원하지 않은 길을 걷게할 수도 없고!");
                break;

            case 15:
                CinematicSystem._instance.TypingParam("그러니 원망마라..설령 남은 빛조차 잊어버릴 지라도!");
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
