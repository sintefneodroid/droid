Shader "Neodroid/Spaces/ViewSpace" {

	Properties {

	}

    SubShader {
    Blend One Zero

      Pass {
         CGPROGRAM

         #pragma vertex vert
         #pragma fragment frag

         struct vertexInput {
            float4 vertex : POSITION;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 position_in_view_space : TEXCOORD0;
         };

         vertexOutput vert(vertexInput input) {
            vertexOutput output;

            output.pos = UnityObjectToClipPos(input.vertex); // Transformation all the way to clip space
            output.position_in_view_space = mul(UNITY_MATRIX_V,mul(unity_ObjectToWorld, input.vertex)); // transformation of input.vertex from object to world to view coordinates;
            return output;
         }

         float4 frag(vertexOutput input) : COLOR          {
                return float4((input.position_in_view_space.xyz* .5 + 1.0) , 1.0);
         }

         ENDCG
      }
    }


	Fallback "Unlit/Color"
}
