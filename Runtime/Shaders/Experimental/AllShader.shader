/*
      struct vertexInput {
         float4 vertex : POSITION; // position (in object coordinates,
            // i.e. local or model coordinates)
         float4 tangent : TANGENT;
            // vector orthogonal to the surface normal
         float3 normal : NORMAL; // surface normal vector (in object
            // coordinates; usually normalized to unit length)
         float4 texcoord : TEXCOORD0;  // 0th set of texture
            // coordinates (a.k.a. “UV”; between 0 and 1)
         float4 texcoord1 : TEXCOORD1; // 1st set of tex. coors.
         float4 texcoord2 : TEXCOORD2; // 2nd set of tex. coors.
         float4 texcoord3 : TEXCOORD3; // 3rd set of tex. coors.
         fixed4 color : COLOR; // color (usually constant)
      };
*/

Shader "Excluded/AllShaders"{
   SubShader {
   		Lighting Off
		Cull Off
		//ZWrite Off
		//ZTest Always

      Pass {
         CGPROGRAM

         #pragma vertex vert
         #pragma fragment frag

         struct vertexInput {
            float4 vertex : POSITION;
            float4 tangent : TANGENT;
            float3 normal : NORMAL;
            float4 texcoord : TEXCOORD0;
            float4 texcoord1 : TEXCOORD1;
            float4 texcoord2 : TEXCOORD2;
            float4 texcoord3 : TEXCOORD3;
            fixed4 color : COLOR;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 col : TEXCOORD0;
         };

         vertexOutput vert(vertexInput input){
            vertexOutput output;

            output.pos =  UnityObjectToClipPos(input.vertex);
            //output.col = input.texcoord; // set the output color

            // other possibilities to play with:

            // output.col = input.vertex;
            output.col = input.tangent;
            // output.col = float4(input.normal, 1.0);
            // output.col = input.texcoord;
            // output.col = input.texcoord1;
            // output.col = input.texcoord2;
            // output.col = input.texcoord3;
            // output.col = input.color;

            return output;
         }

         float4 frag(vertexOutput input) : COLOR         {
            return input.col;
         }

         ENDCG
      }
   }
}
