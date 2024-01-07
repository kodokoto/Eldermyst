using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelationRenderPass : ScriptableRenderPass
{
    private PixelationRenderFeature.PixelationPassSettings settings;
    private RTHandle colourBuffer;
    private RenderTargetIdentifier pixelatedBuffer;
    private int pixelatedBufferID = Shader.PropertyToID("_PixelatedBuffer");

    private Material pixelationMaterial;
    private int pixelScreenHeight, pixelScreenWidth;
    public PixelationRenderPass(PixelationRenderFeature.PixelationPassSettings settings)
    {
        this.settings = settings;
        this.renderPassEvent = settings.renderPassEvent;
        this.pixelationMaterial = settings.pixelationMaterial;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        colourBuffer = renderingData.cameraData.renderer.cameraColorTargetHandle;
        RenderTextureDescriptor pixelatedBufferDescriptor = renderingData.cameraData.cameraTargetDescriptor;
        pixelatedBufferDescriptor.depthBufferBits = 0;

        pixelScreenHeight = settings.screenHeight;
        pixelScreenWidth = (int) (pixelScreenHeight * renderingData.cameraData.camera.aspect + 0.5f);

        pixelationMaterial.SetVector("_BlockCount", new Vector2(pixelScreenWidth, pixelScreenHeight));
        pixelationMaterial.SetVector("_BlockSize", new Vector2(1f / pixelScreenWidth, 1f / pixelScreenHeight));
        pixelationMaterial.SetVector("_HalfBlockSize", new Vector2(0.5f / pixelScreenWidth, 0.5f / pixelScreenHeight));

        pixelatedBufferDescriptor.width = pixelScreenWidth;
        pixelatedBufferDescriptor.height = pixelScreenHeight;

        cmd.GetTemporaryRT(pixelatedBufferID, pixelatedBufferDescriptor, FilterMode.Point);
        
        pixelatedBuffer = new RenderTargetIdentifier(pixelatedBufferID);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, new ProfilingSampler("Pixelation Pass")))
        {
            // Blitter.BlitCameraTexture(cmd, colourBuffer, pixelatedBuffer, pixelationMaterial, 0);
            // Blitter.BlitCameraTexture(cmd, pixelatedBuffer, colourBuffer);
            cmd.Blit(colourBuffer, pixelatedBuffer, pixelationMaterial, 0);
            cmd.Blit(pixelatedBuffer, colourBuffer);
        }
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(pixelatedBufferID);
    }

}