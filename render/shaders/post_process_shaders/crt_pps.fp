#define PI 3.1415926535897932384626433832795

varying highp vec4 position;
varying highp vec2 var_texcoord0;

uniform lowp vec4 time;
uniform lowp vec4 viewport;
uniform highp sampler2D original;

vec2 curveRemapUV(vec2 curvature, vec2 uv)
{
	uv = uv * 2.0 - 1.0;
	vec2 offset = abs(uv.yx) / vec2(curvature.x, curvature.y);
	uv = uv + uv * offset * offset;
	uv = uv * 0.5 + 0.5;
	return uv;
}

vec4 scanLineIntensity(float uv, float resolution, float opacity)
{
	float intensity = sin(uv * resolution * PI * 2.0);
	intensity = ((0.5 * intensity) + 0.5) * 0.9 + 0.1;
	return vec4(vec3(pow(intensity, opacity)), 1.0);
}

vec4 vignetteIntensity(vec2 uv, vec2 resolution, float opacity, float roundness)
{
	float intensity = uv.x * uv.y * (1.0 - uv.x) * (1.0 - uv.y);
	return vec4(vec3(clamp(pow((resolution.x / roundness) * intensity, opacity), 0.0, 1.0)), 1.0);
}

void main()
{
	vec2 curvature = vec2(4, 4);
	vec2 scanLineOpacity = vec2(1, 1) * 0.25;
	vec2 screenResolution = /*textureSize(original,0)*/ vec2(viewport.z, viewport.w) * (3 + sin(time.x/25)*0.025);
	vec2 remappedUV = curveRemapUV(curvature, vec2(var_texcoord0.x, var_texcoord0.y));
	vec4 baseColor = texture2D(original, remappedUV);
	baseColor *= vignetteIntensity(remappedUV, screenResolution, 1, length(screenResolution)/200);
	baseColor *= scanLineIntensity(remappedUV.x, screenResolution.y, scanLineOpacity.x);
	baseColor *= scanLineIntensity(remappedUV.y, screenResolution.x, scanLineOpacity.y);
	baseColor *= vec4(vec3(1.8), 1.0);
	if (remappedUV.x < 0.0 || remappedUV.y < 0.0 || remappedUV.x > 1.0 || remappedUV.y > 1.0){
		gl_FragColor = vec4(0.0, 0.0, 0.0, 1.0);
	} else {
		gl_FragColor = baseColor;
	}
}
