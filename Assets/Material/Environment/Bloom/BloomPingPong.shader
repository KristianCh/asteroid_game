Shader "Environment/BloomPingPong"
{
	Properties
	{
		[MainTexture] _MainTex("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Pass
		{
			HLSLPROGRAM
			// compilation directives for this snippet, e.g.:
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			// the shader program itself

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			static float gaussKernel[9] = {
					1, 2, 1,
					2, 4, 2,
					1, 2, 1
			};

			static float2 gaussSamplePoints[9] = {
					float2(-1, 1), float2(0, 1), float2(1, 1),
					float2(-1, 0), float2(0, 0), float2(1, 0),
					float2(-1, -1), float2(0, -1), float2(1, -1),
			};

			static float2 textureDims = float2(1920, 1080);
			sampler2D _MainTex;

			v2f vert(appdata IN)
			{
				v2f OUT;

				OUT.position = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;

				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				float4 color = 0;
				[unroll] for (int i = 0; i < 9; i++) {
					color += tex2D(_MainTex, IN.uv + (gaussSamplePoints[i] * 2) / textureDims) * gaussKernel[i];
				}
				color /= 16;

				fixed4 fragColor = color;

				return fragColor;
			}

			ENDHLSL
		}
	}
}