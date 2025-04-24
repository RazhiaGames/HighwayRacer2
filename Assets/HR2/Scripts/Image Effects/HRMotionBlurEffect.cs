//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HRMotionBlurEffect : ScriptableRendererFeature {

    [System.Serializable]
    public class MotionBlurSettings {

        public Material motionBlurMaterial = null;
        [Range(0, 1)] public float blurAmount = 0.5f;
        public Texture maskTexture = null;
    }

    public MotionBlurSettings settings = new MotionBlurSettings();

    class MotionBlurPass : ScriptableRenderPass {

        public Material material;
        public float blurAmount;
        public Texture maskTexture;
        private RenderTargetIdentifier currentTarget;

        public MotionBlurPass(Material material, float blurAmount, Texture maskTexture) {

            this.material = material;
            this.blurAmount = blurAmount;
            this.maskTexture = maskTexture;

        }

        public void Setup(RenderTargetIdentifier currentTarget) {

            this.currentTarget = currentTarget;

        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {

            if (material == null) {

                Debug.LogError("Material not set for MotionBlurPass");
                return;

            }

            CommandBuffer cmd = CommandBufferPool.Get("Motion Blur Effect");

            RenderTargetIdentifier source = currentTarget;

            // Set blur amount and mask texture
            material.SetFloat("_BlurAmount", blurAmount);
            material.SetTexture("_MaskTex", maskTexture);

            // Temporary RenderTexture to avoid modifying the source directly
            int tempID = Shader.PropertyToID("_TempTexture");
            cmd.GetTemporaryRT(tempID, renderingData.cameraData.cameraTargetDescriptor);
            RenderTargetIdentifier tempRT = new RenderTargetIdentifier(tempID);

            cmd.Blit(source, tempRT, material);
            cmd.Blit(tempRT, source);

            cmd.ReleaseTemporaryRT(tempID);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);

        }

    }

    MotionBlurPass motionBlurPass;

    public override void Create() {

        motionBlurPass = new MotionBlurPass(settings.motionBlurMaterial, settings.blurAmount, settings.maskTexture) {
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents
        };

    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {

        if (settings.motionBlurMaterial == null) {

            Debug.LogWarning("Missing Motion Blur Material. MotionBlurPass will not execute.");
            return;
        }

        renderer.EnqueuePass(motionBlurPass); // letting the renderer know which passes will be used before allocation

    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData) {

        motionBlurPass.Setup(renderer.cameraColorTargetHandle);  // use of target after allocation

    }

}
