varying mediump vec4 position;
varying mediump vec2 var_texcoord0;

uniform lowp sampler2D fade_original;

void main()
{
	float[25] kernel = float[] (
		1.0,  4.0,  6.0,  4.0, 1.0,
		4.0, 16.0, 24.0, 16.0, 4.0,
		6.0, 24.0, 36.0, 24.0, 6.0,
		4.0, 16.0, 24.0, 16.0, 4.0,
		1.0,  4.0,  6.0,  4.0, 1.0);

	vec4 fade_color = vec4(0);
	int index = 0;
	for (int i = -2; i <= 2; i++) {
		for (int j = -2; j <= 2; j++) {
			vec2 shift = vec2(i,j) / textureSize(fade_original,0);
			fade_color += kernel[index++] * texture2D(fade_original, var_texcoord0.xy + shift) / 256;
		}
	}
	gl_FragColor = mix(fade_color, vec4(0, 0, 0, 0), 1 - 0.1);
}
