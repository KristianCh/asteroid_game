Shader "Environment/Quad"
{
	Properties
	{
		_BGTex("Background Texture", 2D) = "white" {}
		_TrailTex("Trail Texture", 2D) = "white" {}
		_ObjectTex("Object Texture", 2D) = "white" {}
		_BloomTex("Bloom Texture", 2D) = "white" {}
	}

	SubShader
	{
		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			static float2 sobelSamplePoints[9] = {
				float2(-1, 1), float2(0, 1), float2(1, 1),
				float2(-1, 0), float2(0, 0), float2(1, 0),
				float2(-1, -1), float2(0, -1), float2(1, -1),
			};

			static float sobelXMatrix[9] = {
				1, 0, -1,
				2, 0, -2,
				1, 0, -1
			};

			static float sobelYMatrix[9] = {
				1, 2, 1,
				0, 0, 0,
				-1, -2, -1
			};

			static float2 textureDims = float2(1920, 1080);

			sampler2D _BGTex;
			sampler2D _TrailTex;
			sampler2D _ObjectTex;
			sampler2D _BloomTex;

			v2f vert(appdata IN)
			{
				v2f OUT;

				OUT.position = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;

				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{	
				float4 bgColor = tex2D(_BGTex, IN.uv);
				float4 trailColor = tex2D(_TrailTex, IN.uv);
				float4 objectColor = tex2D(_ObjectTex, IN.uv);
				float4 bloomColor = tex2D(_BloomTex, IN.uv);
				float4 sobel = 0;
				[unroll] for (int i = 0; i < 9; i++) {
					float4 c = tex2D(_ObjectTex, IN.uv + (sobelSamplePoints[i] * 1) / textureDims);
					sobel += c * sobelXMatrix[i] + c * sobelYMatrix[i];
				}
				fixed4 fragColor = ((bgColor* (1 - trailColor.a * 0.8) + trailColor * trailColor.a * 0.8)* (1 - objectColor.a) + objectColor * objectColor.a)* (1 + sobel * 10 + bloomColor);

				return fragColor;
			}

			ENDHLSL
		}
	}
}