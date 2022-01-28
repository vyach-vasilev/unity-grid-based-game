Shader "VFX/ScalePingPongXZ"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [Hdr]_Color("Color", Color) = (1,1,1,1)
        _Amplitude("Amplitude", Range(0.1, 10)) = 0.5
        _ScaleSize("Scale Size", Range(0.5, 10)) = 1.4
        _Speed("Speed", Range(0.25, 5)) = 0.75
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            half4 _Color;
            half _Amplitude;
            half _ScaleSize;
            half _Speed;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 UpdateTransform(float4 position)
            {
                position.xz = position.xz * _Amplitude * (abs(frac(_Time.y * _Speed) - 0.5) + _ScaleSize);
                return position;
            }
            
            v2f vert (appdata v)
            {
                v2f o;
                v.vertex = UpdateTransform(v.vertex);
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);
                return col * _Color;
            }
            ENDHLSL
        }
    }
}
