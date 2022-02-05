Shader "Hidden/SeeThrough"
{
    Properties
    {
        _BaseColor ("Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        HLSLINCLUDE

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
        #pragma multi_compile_instancing
        
        struct Input
        {
            float4 positionOS: POSITION;
            float2 uv: TEXCOORD0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };
        
        struct Output
        {
            float4 positionCS: SV_POSITION;
            float2 uv: TEXCOORD0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        
		TEXTURE2D_X(_MaskTex);
		SAMPLER(sampler_MaskTex);

		TEXTURE2D_X(_MainTex);
		SAMPLER(sampler_MainTex);
		float2 _MainTex_TexelSize;

		float4 _Color;
        
        UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
            UNITY_DEFINE_INSTANCED_PROP(half4, _BaseColor)
        UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

        Output Vertex(Input input)
        {
            Output output;
            UNITY_SETUP_INSTANCE_ID(input);
            UNITY_TRANSFER_INSTANCE_ID(input, output);
            
            output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
            output.uv = input.uv;
            
            return output;      
        }

        half4 Fragment(Output input) : SV_Target
        {
            UNITY_SETUP_INSTANCE_ID(input);
            return UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _BaseColor);
        }
        
        ENDHLSL        
     
        Pass
        {
            Name "CoverColor - 1"
            ZTest LEqual
            ZWrite Off
            Colormask 0
            Stencil 
            {
                Ref 64
                Comp always
                Pass replace
            }
            
            HLSLPROGRAM
           
            #pragma vertex Vertex
            #pragma fragment Fragment        

            ENDHLSL
        }
        
        Pass
        {
            Name "CoverColor - 2"
            Blend SrcAlpha OneMinusSrcAlpha
            ZTest Greater
            ZWrite Off
            Stencil 
            {
                Ref 64
                Comp NotEqual
                Pass replace
            }

            HLSLPROGRAM
           
            #pragma vertex Vertex
            #pragma fragment Fragment        

            ENDHLSL         
        }
    }
}
