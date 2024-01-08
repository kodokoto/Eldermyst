using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelOutlinePass : ScriptableRenderPass
{
    private PixelOutlineRenderFeature.PixelOutlineSettings settings;
    private RTHandle colourBuffer;
    private RTHandle pixelatedBuffer;
    private int pixelatedBufferID = Shader.PropertyToID("_PixelBuffer");

    private Material pixelationMaterial;
    public PixelOutlinePass(PixelOutlineRenderFeature.PixelOutlineSettings settings)
    {
        this.settings = settings;
        this.renderPassEvent = settings.renderPassEvent;
        // create material from shader
        this.pixelationMaterial = CoreUtils.CreateEngineMaterial(settings.pixelationShader);
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        var desc = renderingData.cameraData.cameraTargetDescriptor;
        desc.depthBufferBits = 0; // Color and depth cannot be combined in RTHandles

        RenderingUtils.ReAllocateIfNeeded(ref pixelatedBuffer, desc, name: "_TemporaryColorTexture");

        var renderer = renderingData.cameraData.renderer;
        colourBuffer = renderer.cameraColorTargetHandle;
        
        pixelationMaterial.SetFloat("_NormalThreshold", settings.normalThreshold);
        pixelationMaterial.SetFloat("_DepthThreshold", settings.depthThreshold);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, new ProfilingSampler("Pixelation Pass")))
        {
            Blitter.BlitCameraTexture(cmd, colourBuffer, pixelatedBuffer, pixelationMaterial, 0);
            Blitter.BlitCameraTexture(cmd, pixelatedBuffer, colourBuffer);
        }
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public void ReleaseTargets() {
        pixelatedBuffer?.Release();
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(pixelatedBufferID);
    }

}