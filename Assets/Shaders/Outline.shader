Shader "Hidden/Outline"
{
	HLSLINCLUDE

		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

		TEXTURE2D_X(_MaskTex);
		SAMPLER(sampler_MaskTex);

		TEXTURE2D_X(_MainTex);
		SAMPLER(sampler_MainTex);
		float2 _MainTex_TexelSize;

		float4 _Color;
		int _Width;
		float _GaussSamples[32];

		struct Varyings
		{
			float4 positionCS : SV_POSITION;
			float2 uv : TEXCOORD0;
		};

		struct Attributes
		{
			uint vertexID : SV_VertexID;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		Varyings VertexSimple(Attributes input)
		{
			Varyings output;

			UNITY_SETUP_INSTANCE_ID(input);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

			output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
			output.uv = GetFullScreenTriangleTexCoord(input.vertexID);

			return output;
		}

		float CalcIntensity(float2 uv, float2 offset)
		{
			float intensity = 0;
			for (int k = -_Width; k <= _Width; ++k)
			{
				intensity += SAMPLE_TEXTURE2D_X(_MainTex, sampler_MainTex, uv + k * offset).r * _GaussSamples[abs(k)];
			}

			return intensity;
		}

		float4 FragmentH(Varyings i) : SV_Target
		{
			float intensity = CalcIntensity(i.uv, float2(_MainTex_TexelSize.x, 0));
			return float4(intensity, intensity, intensity, 1);
		}

		float4 FragmentV(Varyings i) : SV_Target
		{
			if (SAMPLE_TEXTURE2D_X(_MaskTex, sampler_MaskTex, i.uv).r > 0)
			{
				discard;
			}

			const float intensity = CalcIntensity(i.uv, float2(0, _MainTex_TexelSize.y));
			return float4(_Color.rgb, saturate(_Color.a * intensity * 20));
		}

	ENDHLSL

	SubShader
	{
		Tags{ "RenderPipeline" = "UniversalPipeline" }

		Cull Off
		ZWrite Off
		ZTest Always
		Lighting Off

		Pass
		{
			Name "HPass"
			HLSLPROGRAM

			#pragma target 3.5
			#pragma multi_compile_instancing
			#pragma vertex VertexSimple
			#pragma fragment FragmentH

			ENDHLSL
		}

		Pass
		{
			Name "VPassBlend"
			Blend SrcAlpha OneMinusSrcAlpha
			
			HLSLPROGRAM

			#pragma target 3.5
			#pragma multi_compile_instancing
			#pragma vertex VertexSimple
			#pragma fragment FragmentV

			ENDHLSL
		}
	}
}
