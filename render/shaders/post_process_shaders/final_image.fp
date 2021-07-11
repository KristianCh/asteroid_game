varying mediump vec4 position;
varying mediump vec2 var_texcoord0;

uniform lowp sampler2D mesh_original;
uniform lowp sampler2D sprite_original;
uniform lowp sampler2D gui_original;

void main()
{
	float[25] kernel = float[] (
		-0.125, -0.125, -0.125, -0.125, -0.125,
		-0.125, -1.0, -1.0, -1.0, -0.125, 
		-0.125, -1.0, 10.0, -1.0, -0.125,
		-0.125, -1.0, -1.0, -1.0, -0.125,
		-0.125, -0.125, -0.125, -0.125, -0.125
	);

	vec4 gui_color = texture(gui_original, var_texcoord0.xy);
	vec4 sprite_color = texture(sprite_original, var_texcoord0.xy);
	vec4 mesh_color = vec4(0);
	int index = 0;
	for (int i = -2; i <= 2; i++) {
		for (int j = -2; j <= 2; j++) {
			vec2 shift = vec2(i,j) / textureSize(mesh_original,0) * 2;
			mesh_color += kernel[index++] * texture(mesh_original, var_texcoord0.xy + shift);
		}
	}
	vec4 final_color = vec4(mesh_color.xyz, 1);
	final_color = final_color * (1 - sprite_color.w) + sprite_color;
	final_color = final_color * (1 - gui_color.w) + gui_color;
	
	gl_FragColor = final_color;
}