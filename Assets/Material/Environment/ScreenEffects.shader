Shader "Environment/ScreenEffects"
{
	Properties
	{
		[MainTexture] _MainTex("Texture", 2D) = "white" {}
		_PixelSize("Pixel Size", Float) = 2
	}

	SubShader
	{
		Pass
		{
			HLSLPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#define PI 3.1415926535897932384626433832795

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
			float _PixelSize;
			sampler2D _MainTex;

			float2 curveRemapUV(float2 curvature, float2 uv)
			{
				uv = uv * 2.0 - 1.0;
				float2 offset = abs(uv.yx) / float2(curvature.x, curvature.y);
				uv = uv + uv * offset * offset;
				uv = uv * 0.5 + 0.5;
				return uv;
			}

			float4 scanLineIntensity(float uv, float resolution, float opacity)
			{
				float x_offset = 1 / 1920;
				float y_offset = 1 / 1080;
				float intensity = sin(uv * resolution * PI * 2.0);
				intensity = ((0.5 * intensity) + 0.5) * 0.9 + 0.1;
				float i = pow(intensity, opacity);
				return float4(i, i, i, 1.0);
			}

			float4 vignetteIntensity(float2 uv, float2 resolution, float opacity, float roundness)
			{
				float intensity = uv.x * uv.y * (1.0 - uv.x) * (1.0 - uv.y);
				float i = clamp(pow((resolution.x / roundness) * intensity, opacity), 0.0, 1.0);
				return float4(i, i, i, 1.0);
			}

			float4 getCrtColor(float2 coords, float4 color) {
				float2 scanLineOpacity = float2(0.1, 0.1);
				float2 screenResolution = float2(320, 240);
				color *= vignetteIntensity(coords, screenResolution, 0.5, length(screenResolution) / 200);
				color *= scanLineIntensity(coords.x, screenResolution.y, scanLineOpacity.x);
				color *= scanLineIntensity(coords.y, screenResolution.x, scanLineOpacity.y);
				color *= float4(1.8, 1.8, 1.8, 1.0);
				if (coords.x < 0.0 || coords.y < 0.0 || coords.x > 1.0 || coords.y > 1.0) {
					color = float4(0.0, 0.0, 0.0, 1.0);
				}
				return color;
			}

			float2 pixelRemapUV(float2 uv) {
				float2 dxy = float2(_PixelSize, _PixelSize) / float2(1920, 1080);
				float2 coord = float2(dxy.x * floor(uv.x / dxy.x) + dxy.x * 0.5, dxy.y * floor(uv.y / dxy.y) + dxy.y * 0.5);
				return coord;
			}

			v2f vert(appdata IN)
			{
				v2f OUT;

				OUT.position = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;

				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				float2 curvature = float2(4, 4);
				float2 coords = curveRemapUV(curvature, IN.uv);
				float4 color = tex2D(_MainTex, coords);

				color = getCrtColor(coords, color);
				fixed4 fragColor = color;

				return fragColor;
			}

			ENDHLSL
		}
	}
}