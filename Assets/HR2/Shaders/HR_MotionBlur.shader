Shader "Custom/HR_MotionBlur"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _VelocityTex ("Velocity Texture", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "white" {}
        _BlurAmount ("Blur Amount", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _VelocityTex;
            sampler2D _MaskTex;
            float _BlurAmount;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float2 velocity = tex2D(_VelocityTex, i.uv).xy * _BlurAmount;
                half4 color = half4(0, 0, 0, 0);
                float mask = tex2D(_MaskTex, i.uv).r;

                for (int j = -4; j <= 4; j++)
                {
                    color += tex2D(_MainTex, i.uv + velocity * j / 8.0);
                }

                color /= 9.0;
                return lerp(tex2D(_MainTex, i.uv), color, mask);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
