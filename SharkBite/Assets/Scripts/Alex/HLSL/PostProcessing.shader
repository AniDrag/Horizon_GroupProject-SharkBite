Shader "Unlit/PostProcessing"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Brightness ("Brightness, default = 0", Range(-1,1)) = 0
        _Contrast ("Contrast, default = 1", Range(0,2)) = 1
        _Saturation ("Saturation, default = 1", Range(0,2)) = 1
        _Gamma ("Gamma, default = 1", Range(0.1, 3)) = 1
        _Exposure ("Exposure, default = 0", Range(-2,2)) = 0
        _VignetteStrength ("Vignette effect, default = 0", Range(0,3)) = 0


        _MainTex("InputTex", 2D) = "white" {}
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"



            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            float _Brightness;
            float _Contrast;
            float _Saturation;
            float _Gamma;
            float _Exposure;
            float _VignetteStrength; 

            float useEdgeDetection; 
            float invertColors;
            float sepiaColors;
            float grayscaleColors;

            float4 _Color;

            sampler2D _MainTex;
            float2 _TexelSize;
            float4 _MainTex_ST;




            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float3 AdjustBrightness(float3 color, float brightness)
            {
                return saturate(color + brightness);
            }

            float3 AdjustContrast(float3 color, float contrast)
            {
                return 0.5 + (color - 0.5) * contrast;
            }

            float3 AdjustSaturation(float3 color, float saturation)
            {
                float3 grayscale = dot(color, float3(0.299, 0.587, 0.114));
                return lerp(grayscale, color, saturation);
            }

            float3 AdjustGamma(float3 color, float gamma)
            {
                return pow(color, gamma);
            }

            float3 AdjustExposure(float3 color, float exposure)
            {
                return color * pow(2.0, exposure);
            }

            float3 InvertColor(float3 color)
            {
                return 1.0 - color;
            }

            float3 ApplyVignette(float3 color, float2 uv, float strength)
            {
                float2 center = float2(0.5, 0.5);
                float dist = distance(uv, center);
                float vignette = smoothstep(0.8, 0.5, dist * strength);
                return color * vignette;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                
                col.rgb = AdjustBrightness(col.rgb, _Brightness);
                col.rgb = AdjustContrast(col.rgb, _Contrast);
                col.rgb = AdjustSaturation(col.rgb, _Saturation);
                col.rgb = AdjustGamma(col.rgb, _Gamma);
                col.rgb = AdjustExposure(col.rgb, _Exposure);
                col.rgb = ApplyVignette(col.rgb, i.uv, _VignetteStrength);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col * _Color;
            }
            ENDCG
        }
    }
}
