using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelOutlinePass : ScriptableRenderPass
{
    private PixelOutlineRenderFeature.PixelOutlineSettings settings;
    private RTHandle colourBuffer;
    private RenderTargetIdentifier pixelatedBuffer;
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
        colourBuffer = renderingData.cameraData.renderer.cameraColorTargetHandle;
        RenderTextureDescriptor pixelatedBufferDescriptor = renderingData.cameraData.cameraTargetDescriptor;
        pixelatedBufferDescriptor.depthBufferBits = 24;

        pixelationMaterial.SetFloat("_NormalThreshold", settings.normalThreshold);
        pixelationMaterial.SetFloat("_DepthThreshold", settings.depthThreshold);
        
        pixelatedBuffer = new RenderTargetIdentifier(pixelatedBufferID);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, new ProfilingSampler("Pixelation Pass")))
        {
            Blitter.BlitCameraTexture(cmd, colourBuffer, colourBuffer, pixelationMaterial, 0);
            // cmd.Blit(colourBuffer, colourBuffer, pixelationMaterial, 0);
            // cmd.Blit(pixelatedBuffer, colourBuffer);
        }
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(pixelatedBufferID);
    }

}