Shader "Neodroid/Plotting/ValueRangePlotter"
{
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment

            #include "UnityCG.cginc"

            float4 _Range;

            float4 Vertex(uint vertix_id : SV_VertexID) : SV_Position
            {
                float p = 1.0 / 512 * vertix_id;
                float sx = (p * 2 - 1) * _ScreenParams.x / _ScreenParams.y;
                float x = lerp(_Range.x, _Range.y, p);
                float y = sin(x);
                float sy = lerp(_Range.z, _Range.w, y);
                //return UnityObjectToClipPos(float4(x, y, 0, 1));
                return UnityWorldToClipPos(float4(sx, sy, 0, 1));
            }

            half4 Fragment(float4 vertex : SV_Position) : SV_Target
            {
                return 1;
            }

            ENDCG
        }
    }
}
