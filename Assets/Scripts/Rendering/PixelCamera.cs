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

        // if display is deactivated, activate it
        if (!display.gameObject.activeSelf) display.gameObject.SetActive(true);



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
        Debug.Log("Current screen size " + Screen.width + " " + Screen.height);
        Debug.Log("Resized to " + pixelWidth + " " + pixelHeight);
    }

}
