using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScreenSizer : MonoBehaviour
{
    CanvasScaler canvasScaler;
    Vector2 screen;

    private void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
    }

    private void FixedUpdate()
    {
        screen.x = Screen.width;
        screen.y = Screen.height;
        canvasScaler.referenceResolution = screen;
    }
}
