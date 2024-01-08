using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PixelCamera : MonoBehaviour
{
    // [SerializeField] private int pixelScaleFactor = 1;
    [SerializeField] private int targetScreenHeight = 360;

    private Camera cam;
    private RenderTexture output;

    public RawImage display;
    private int screenWidth, screenHeight;
    // Start is called before the first frame update

    void Awake()
    {
        if (!cam) cam = GetComponent<Camera>();
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        // change display to stretch to screen
        display.rectTransform.anchorMin = new Vector2(0, 0);
        display.rectTransform.anchorMax = new Vector2(1, 1);
        display.rectTransform.offsetMin = new Vector2(0, 0);
        display.rectTransform.offsetMax = new Vector2(0, 0);

        Resize(targetScreenHeight * screenWidth / screenHeight, targetScreenHeight);
    }

    // Update is called once per frame
    void Update()
    {
        if (screenWidth != Screen.width || screenHeight != Screen.height) {
            screenWidth = Screen.width;
            screenHeight = Screen.height;
            Resize(targetScreenHeight * screenWidth / screenHeight, targetScreenHeight);
        }
    }

    private void Resize(int pixelWidth, int pixelHeight)
    {
        output = new RenderTexture(pixelWidth, pixelHeight, 24){
            filterMode = FilterMode.Point,
            antiAliasing = 1
        };

        cam.targetTexture = output;
        display.texture = output;

        Debug.Log("Set display texture to " + output.width + " " + output.height);

        Debug.Log("Current screen size " + Screen.width + " " + Screen.height);
        Debug.Log("Resized to " + pixelWidth + " " + pixelHeight);
    }

}
