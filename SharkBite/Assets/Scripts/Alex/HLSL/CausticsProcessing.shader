Shader "Unlit/CausticsProcessing"
{
    Properties
    {
        _TimeCustom("Speed", float) = 1.36
        _Scale("Scale", float) = 0.257
        _Power("Power", float) = 0.2
        _Color("Color", Color) = (1,1,1,1)
        _Brightness("Brightness", float) = 2.78
        _MainTex ("Texture", 2D) = "white" {}
        _SecondTex ("Texture 2", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            float _TimeCustom, _Scale, _Power, _Brightness;
            sampler2D _MainTex, _SecondTex;
            float4 _MainTex_ST, _Color, _Angle;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float2 worldPos : TEXCOORD2;
                float3 worldNormal : TEXCOORD3;
            };




            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float customTime = _Time.y * _TimeCustom;

                fixed4 mask1 = tex2D(_MainTex,  i.worldPos*_Scale + float2(0,        customTime));
                fixed4 mask2 = tex2D(_SecondTex, i.worldPos*_Scale + float2(customTime, 0));

                mask1 = saturate(mask1/2 +mask2/2);
                mask1.rgb = pow(mask1.rgb, _Power);

                mask1.rgb = 1 - mask1.rgb;

                _Color.a = mask1.r;

                return _Color * _Brightness;
            }
            ENDCG
        }
    }
}
