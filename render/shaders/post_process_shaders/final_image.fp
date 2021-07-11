varying mediump vec4 position;
varying mediump vec2 var_texcoord0;

uniform lowp sampler2D mesh_original;

void main()
{
	float[25] kernel = float[] (
		-0.125, -0.125, -0.125, -0.125, -0.125,
		-0.125, -1.0, -1.0, -1.0, -0.125, 
		-0.125, -1.0, 10.0, -1.0, -0.125,
		-0.125, -1.0, -1.0, -1.0, -0.125,
		-0.125, -0.125, -0.125, -0.125, -0.125
	);
	
	vec4 mesh_color = vec4(0);
	int index = 0;
	for (int i = -2; i <= 2; i++) {
		for (int j = -2; j <= 2; j++) {
			vec2 shift = vec2(i,j) / textureSize(mesh_original,0) * 2;
			mesh_color += kernel[index++] * texture(mesh_original, vec2(var_texcoord0.x, var_texcoord0.y) + shift);
		}
	}
	gl_FragColor = vec4(mesh_color.xyz, 1);
}

/*varying mediump vec4 position;
varying mediump vec2 var_texcoord0;

uniform lowp sampler2D mesh_original;

void main()
{
	vec4 color = texture2D(mesh_original, var_texcoord0.xy);
	// Desaturate the color sampled from the original texture
	float grey = color.r * 0.3 + color.g * 0.59 + color.b * 0.11;
	gl_FragColor = vec4(grey, grey, grey, 1.0);
}*/