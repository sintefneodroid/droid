Shader "Neodroid/Spaces/WorldSpace" {

	Properties {
        _from_center_span ("_from_center_span", Vector) = (10,10,10)
	}

    SubShader {
    Blend One Zero

      Pass {
         CGPROGRAM

         #pragma vertex vert
         #pragma fragment frag
         #include "UnityCG.cginc"

         float3 _from_center_span;

         struct vertexInput {
            float4 vertex : POSITION;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 position_in_world_space : TEXCOORD0;
         };

         vertexOutput vert(vertexInput input) {
            vertexOutput output;

            output.pos = UnityObjectToClipPos(input.vertex);
            output.position_in_world_space = mul(unity_ObjectToWorld, input.vertex); // transformation of input.vertex from object coordinates to world coordinates;
            return output;
         }

         float4 frag(vertexOutput input) : COLOR          {
                return float4(input.position_in_world_space.xyz, 1.0);
         }

         ENDCG
      }
    }


	Fallback "Unlit/Color"
}
