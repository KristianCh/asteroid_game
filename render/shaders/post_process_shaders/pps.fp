#define PI 3.1415926535897932384626433832795

varying highp vec4 position;
varying highp vec2 var_texcoord0;

uniform lowp vec4 time;
uniform lowp vec4 data;
uniform lowp vec4 options;
uniform highp sampler2D final;

vec4 getPixelatedColor(vec2 coords) {
	vec2 dxy = vec2(2, 2) / textureSize(final,0);
	vec2 coord = vec2(dxy.x * floor(coords.x / dxy.x) + dxy.x*0.5, dxy.y * floor(coords.y / dxy.y) + dxy.y*0.5);
	vec4 color = texture2D(final, coord.xy);
	return color;
}

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
	float x_offset = 1/textureSize(final,0).x;
	float y_offset = 1/textureSize(final,0).y;
	float intensity = sin(uv * resolution * PI * 2.0);
	intensity = ((0.5 * intensity) + 0.5) * 0.9 + 0.1;
	return vec4(vec3(pow(intensity, opacity)), 1.0);
}

vec4 vignetteIntensity(vec2 uv, vec2 resolution, float opacity, float roundness)
{
	float intensity = uv.x * uv.y * (1.0 - uv.x) * (1.0 - uv.y);
	return vec4(vec3(clamp(pow((resolution.x / roundness) * intensity, opacity), 0.0, 1.0)), 1.0);
}

vec4 getCrtColor(vec2 coords, vec4 color) {
	vec2 scanLineOpacity = vec2(0.25);
	vec2 screenResolution = vec2(data.z, data.w) * (3 + sin(time.x/25)*0.025);
	color *= vignetteIntensity(coords, screenResolution, 0.5, length(screenResolution)/200);
	color *= scanLineIntensity(coords.x, screenResolution.y, scanLineOpacity.x);
	color *= scanLineIntensity(coords.y, screenResolution.x, scanLineOpacity.y);
	color *= vec4(vec3(1.8), 1.0);
	if (coords.x < 0.0 || coords.y < 0.0 || coords.x > 1.0 || coords.y > 1.0){
		color = vec4(0.0, 0.0, 0.0, 1.0);
	} 
	return color;
}

void main()
{
	vec2 curvature = vec2(4, 4);
	vec2 coords = vec2(var_texcoord0.x, var_texcoord0.y);
	if (options.x == 1) {
		coords = curveRemapUV(curvature, vec2(var_texcoord0.x, var_texcoord0.y));
	}
	vec4 color = texture2D(final, coords);
	if (options.y == 1) {
		color = getPixelatedColor(coords);
	}
	if (options.x == 1) {
		color = getCrtColor(coords, color);
	}
	gl_FragColor = color;
}
