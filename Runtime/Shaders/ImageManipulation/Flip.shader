Shader "Neodroid/PostProcessing/Flip"
{
    HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        float _Flip_x;
        float _Flip_y;

        VaryingsDefault Vert(AttributesDefault v){
            VaryingsDefault o;
            o.vertex = float4(v.vertex.xy, 0.0, 1.0) * float4(_Flip_x, _Flip_y, 1.0, 1.0);
            o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);

        #if UNITY_UV_STARTS_AT_TOP
            o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
        #endif

            o.texcoordStereo = TransformStereoScreenSpaceTex(o.texcoord, 1.0);

            return o;
        }

        float4 Frag(VaryingsDefault i) : SV_Target        {
            float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
            return color;
        }

    ENDHLSL

    SubShader
    {
        Cull Off
         ZWrite Off
         ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex Vert //VertDefault
                #pragma fragment Frag

            ENDHLSL
        }
    }
}
