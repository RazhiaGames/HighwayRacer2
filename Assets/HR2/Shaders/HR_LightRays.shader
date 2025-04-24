Shader "Custom/HR_LightRays"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _LightPos ("Light Position", Vector) = (0, 0, 0, 0)
        _Intensity ("Intensity", Range(0, 10)) = 1.0
        _AnimationSpeed ("Animation Speed", Range(0, 10)) = 1.0
        _RayColor ("Ray Color", Color) = (1, 1, 1, 1)
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
                float3 worldPos : TEXCOORD1;
                float4 shadowCoord : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _LightPos;
            float _Intensity;
            float _AnimationSpeed;
            float4 _RayColor;

            sampler2D _ShadowMap;
            float4x4 _WorldToShadow;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // Calculate shadow coordinates
                o.shadowCoord = mul(_WorldToShadow, float4(o.worldPos, 1.0));
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);
                float3 lightDir = normalize(_LightPos.xyz - i.worldPos);
                float intensity = max(0, dot(lightDir, float3(0, 0, 1))) * _Intensity;

                // Sample shadows
                float shadow = tex2Dproj(_ShadowMap, i.shadowCoord).r;

                col.rgb *= intensity * _RayColor.rgb * shadow;
                col.a = 1.0; // Ensure alpha is set
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
