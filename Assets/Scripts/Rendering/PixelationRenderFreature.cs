using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PixelationRenderFeature : ScriptableRendererFeature
{

    [System.Serializable]
    public class PixelationPassSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public int screenHeight = 360;
        public Material pixelationMaterial;
    }

    [SerializeField] private PixelationPassSettings settings;

    private PixelationRenderPass pixelationRenderPass;

    public override void Create()
    {
        pixelationRenderPass = new PixelationRenderPass(settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pixelationRenderPass);
    }

}
