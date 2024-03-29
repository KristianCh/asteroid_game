Shader "Environment/Background"
{
	Properties
	{
		[MainTexture] _MainTex("Texture", 2D) = "white" {}
		[MainColor] _Color("Colour", Color) = (1, 1, 1, 1)
		_Time("Time", Float) = 0
		_Saturation("Saturation", Float) = 1
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

			sampler2D _MainTex;
			float _Saturation;
			fixed4 _Color;

			v2f vert(appdata IN) 
			{
				v2f OUT;

				OUT.position = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;

				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				float time_div = 0.05;
				float var = 30;
				float2 offset = float2(cos(_Time.x / time_div + IN.uv.x * var), sin(_Time.x / time_div + IN.uv.y * var)) / float2(1920, 1080) * 25;
				float4 color = pow(tex2D(_MainTex, IN.uv + offset) * 1.2, 4) * 0.5;
				
				float4 grayscale_color = dot(color, float4(0.299f, 0.587f, 0.114f, 0.0f));
				fixed4 fragColor = lerp(grayscale_color, color, _Saturation);

				return fragColor;
			}

			ENDHLSL
		}
	}
}