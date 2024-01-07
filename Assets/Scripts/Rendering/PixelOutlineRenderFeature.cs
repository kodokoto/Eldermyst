using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PixelOutlineRenderFeature : ScriptableRendererFeature
{

    [System.Serializable]
    public class PixelOutlineSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        public float normalThreshold = 0.4f;
        public float depthThreshold = 0.1f;
        public Shader pixelationShader;
    }

    [SerializeField] private PixelOutlineSettings settings;

    private PixelOutlinePass pixelOutlinePass;

    public override void Create()
    {
        pixelOutlinePass = new PixelOutlinePass(settings);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        // configure input 
        pixelOutlinePass.ConfigureInput(ScriptableRenderPassInput.Color |ScriptableRenderPassInput.Depth | ScriptableRenderPassInput.Normal);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pixelOutlinePass);
    }

}
