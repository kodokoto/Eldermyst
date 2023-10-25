using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineVirtualCamera[] virtualCameras;
    private float fallPanAmount = 0.25f;
    [SerializeField] private float fallPanTime = 0.35f;
    [SerializeField] public float fallSpeedYDampingChangeThreshold = -15f;

    public bool IsLerpingYDamping { get; private set;}
    public bool LerpedFromPlayerFalling {get; set;}
    private CinemachineVirtualCamera currentVirtualCamera;
    private CinemachineFramingTransposer framingTransposer;

    private Coroutine lerpYPanCoroutine;

    private float normYPanAmount;
    private void Awake()
    {
        if (instance == null)
            instance = this;

        for (int i = 0; i < virtualCameras.Length; i++)
        {
            if (virtualCameras[i].enabled)
            {
                currentVirtualCamera = virtualCameras[i];
                framingTransposer = currentVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }

        }
        normYPanAmount = framingTransposer.m_YDamping;
    }

    public void LerpYDamping(bool isPlayerFalling)
    {
        lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;

        float startDampAmount = framingTransposer.m_YDamping;
        float endDampAmount = 0f;

        if (isPlayerFalling)
        {
            endDampAmount = fallPanAmount;
        }
        else 
        {
            endDampAmount = normYPanAmount;
        }

        float elapsedTime = 0f;
        while(elapsedTime < fallPanTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, elapsedTime / fallPanTime);
            framingTransposer.m_YDamping = lerpedPanAmount;

            yield return null;
        }
        IsLerpingYDamping = false;
        
    }
}
