Shader "Environment/TrailPingPong"
{
	Properties
	{
		[MainTexture] _MainTex("Texture", 2D) = "white" {}
		_DeltaTime("DeltaTime", Float) = 0
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

				fixed4 _Color;
				float _DeltaTime;
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
					fixed4 fragColor = lerp(tex2D(_MainTex, IN.uv), float4(0, 0, 0, 0), 0.0025 + _DeltaTime * 50);
					if (fragColor.x + fragColor.y + fragColor.z < 0.1) {
						fragColor = float4(0, 0, 0, 0);
					}

					return fragColor;
				}

				ENDHLSL
			}
		}
}