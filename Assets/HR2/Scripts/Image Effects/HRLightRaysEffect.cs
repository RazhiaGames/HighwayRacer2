//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HRLightRaysEffect : ScriptableRendererFeature {

    [System.Serializable]
    public class LightRaysSettings {

        public Material lightRaysMaterial = null;
        public bool useMainDirectionalLight = true;
        public float intensity = 1.0f;
        public float animationSpeed = 1.0f;
        public Color rayColor = Color.white;

    }

    public LightRaysSettings settings = new LightRaysSettings();

    class LightRaysPass : ScriptableRenderPass {

        public Material material;
        private RenderTargetIdentifier currentTarget;
        private bool useMainDirectionalLight;
        private float intensity;
        private float animationSpeed;
        private Color rayColor;

        public LightRaysPass(Material material, bool useMainDirectionalLight, float intensity, float animationSpeed, Color rayColor) {

            this.material = material;
            this.useMainDirectionalLight = useMainDirectionalLight;
            this.intensity = intensity;
            this.animationSpeed = animationSpeed;
            this.rayColor = rayColor;

        }

        public void Setup(RenderTargetIdentifier currentTarget) {

            this.currentTarget = currentTarget;

        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {

            if (material == null) {

                Debug.LogError("Material not set for LightRaysPass");
                return;

            }

            CommandBuffer cmd = CommandBufferPool.Get("Light Rays Effect");

            RenderTargetIdentifier source = currentTarget;

            // Set light position to the material
            if (useMainDirectionalLight) {

                Light mainLight = RenderSettings.sun;

                if (mainLight != null && mainLight.type == LightType.Directional) {

                    Vector3 lightDir = -mainLight.transform.forward;
                    material.SetVector("_LightPos", new Vector4(lightDir.x, lightDir.y, lightDir.z, 0));

                }

            }

            // Set intensity, animation speed, and ray color
            material.SetFloat("_Intensity", intensity);
            material.SetFloat("_AnimationSpeed", animationSpeed);
            material.SetColor("_RayColor", rayColor);

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

    LightRaysPass lightRaysPass;

    public override void Create() {

        lightRaysPass = new LightRaysPass(settings.lightRaysMaterial, settings.useMainDirectionalLight, settings.intensity, settings.animationSpeed, settings.rayColor) {
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents
        };

    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {

        if (settings.lightRaysMaterial == null) {
            Debug.LogWarning("Missing Light Rays Material. LightRaysPass will not execute.");
            return;
        }

        renderer.EnqueuePass(lightRaysPass); // letting the renderer know which passes will be used before allocation

    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData) {

        lightRaysPass.Setup(renderer.cameraColorTargetHandle);  // use of target after allocation

    }

}
