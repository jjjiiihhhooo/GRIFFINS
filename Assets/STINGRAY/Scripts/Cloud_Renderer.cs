using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Cloud_Renderer : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        public RenderTargetIdentifier source;

        private Material _material;
        private int downSample;

        private RenderTargetHandle tempRenderTarget;
        private RenderTargetHandle tempRenderTarget2;

        public CustomRenderPass(Material mat, int downSampleAmount)
        {
            _material = mat;
            downSample = downSampleAmount;

            tempRenderTarget.Init("_TemporaryColourTexture");
            tempRenderTarget2.Init("_Temporary_ColourTexture");
            tempRenderTarget.id = downSample;
            tempRenderTarget2.id = downSample * 3;
            //tempRenderTarget2.Init("_TemporaryDepthTexture");
        }

        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in an performance manner.
        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {

        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            RenderTextureDescriptor originalDescriptor = renderingData.cameraData.cameraTargetDescriptor;

            RenderTextureDescriptor descriptor = cameraTargetDescriptor;
            descriptor.msaaSamples = 1;
            descriptor.depthBufferBits = 0;

            cameraTargetDescriptor = descriptor;
            //downSample = 4;
            cameraTargetDescriptor.width /= downSample;
            cameraTargetDescriptor.height /= downSample;
            cameraTargetDescriptor.colorFormat = RenderTextureFormat.DefaultHDR;
            //RenderTextureFormat.ARGB32;

            // Get temporary render textures
            cmd.GetTemporaryRT(tempRenderTarget.id, cameraTargetDescriptor, FilterMode.Bilinear);
            cmd.GetTemporaryRT(tempRenderTarget2.id, originalDescriptor, FilterMode.Bilinear);
        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (renderingData.cameraData.cameraType != CameraType.Reflection)
            {
                RenderTextureDescriptor downscaleCam = renderingData.cameraData.cameraTargetDescriptor;
                RenderTextureDescriptor originalCam = renderingData.cameraData.cameraTargetDescriptor;

                CommandBuffer commandBuffer = CommandBufferPool.Get();

                Blit(commandBuffer, source, tempRenderTarget.Identifier(), _material);

                commandBuffer.GetTemporaryRT(tempRenderTarget.id, downscaleCam);
                commandBuffer.SetGlobalTexture("_CloudsTex", tempRenderTarget.Identifier());

                context.ExecuteCommandBuffer(commandBuffer);
                CommandBufferPool.Release(commandBuffer);
            }
        }

        /// Cleanup any allocated resources that were created during the execution of this render pass.
        public override void FrameCleanup(CommandBuffer cmd)
        {
        }
    }

    [System.Serializable]
    public class _Settings
    {
        public Material material = null;
        public RenderPassEvent renderPass = RenderPassEvent.AfterRenderingTransparents;
        public enum ScreenRes { Full, Half, Quarter, Eighth }

        public ScreenRes resolution;
    }

    public _Settings settings = new _Settings();

    CustomRenderPass m_ScriptablePass;

    public override void Create()
    {
        int downSample = 1;

        if (settings.resolution == _Settings.ScreenRes.Half)
        {
            downSample = 2;
        }
        else if (settings.resolution == _Settings.ScreenRes.Quarter)
        {
            downSample = 4;
        }
        else if (settings.resolution == _Settings.ScreenRes.Eighth)
        {
            downSample = 8;
        }

        m_ScriptablePass = new CustomRenderPass(settings.material, downSample);

        // Configures where the render pass should be injected.
        //m_ScriptablePass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        m_ScriptablePass.renderPassEvent = settings.renderPass;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        m_ScriptablePass.source = renderer.cameraColorTarget;
        renderer.EnqueuePass(m_ScriptablePass);
    }
}


