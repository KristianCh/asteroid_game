varying mediump vec4 position;
varying mediump vec2 var_texcoord0;

uniform lowp sampler2D mesh_original;
uniform lowp sampler2D sprite_original;
uniform lowp sampler2D gui_original;
uniform lowp sampler2D fade_original;
uniform lowp vec4 time;

float func(float x, float c) {
	return 4*x+c;
}

float dist(vec2 p0, vec2 p1, vec2 p2) {
	return abs((p2.x-p1.x)*(p1.y-p0.y)-(p1.x-p0.x)*(p2.y-p1.y)) / sqrt(pow(p2.x-p1.x, 2) + pow(p2.y-p1.y, 2));
}

void main()
{
	float[25] kernel = float[] (
		-0.125, -0.125, -0.125, -0.125, -0.125,
		-0.125, -1.0, -1.0, -1.0, -0.125, 
		-0.125, -1.0, 10.0, -1.0, -0.125,
		-0.125, -1.0, -1.0, -1.0, -0.125,
		-0.125, -0.125, -0.125, -0.125, -0.125
	);

	float t = mod(time.x, 6)-1.5;
	float d = dist(var_texcoord0, vec2(0, func(0, -t)), vec2(1, func(1, -t)));
	float mult = 1;
	if (d < 0.1) {
		mult = 1.5-d*5;
	}
	vec2 texture_dims = textureSize(mesh_original,0);
	vec4 gui_color = texture(gui_original, var_texcoord0.xy);
	vec4 sprite_color = texture(sprite_original, var_texcoord0.xy);
	vec4 fade_color = texture(fade_original, var_texcoord0.xy);
	if (fade_color.r < 0.05 && fade_color.g < 0.05 && fade_color.b < 0.05) fade_color = vec4(0, 0, 0, 1);
	vec4 mesh_color = vec4(0);
	int index = 0;
	for (int i = -2; i <= 2; i++) {
		for (int j = -2; j <= 2; j++) {
			vec2 shift = vec2(i,j) / texture_dims * 3;
			mesh_color += kernel[index++] * texture(mesh_original, var_texcoord0.xy + shift);
		}
	}
	
	mesh_color = vec4(mesh_color.xyz, (mesh_color.x + mesh_color.y + mesh_color.z) / 3);
	
	vec4 final_color = fade_color * 0.25 * (1 - mesh_color.w) + mesh_color;
	final_color = final_color * (1 - sprite_color.w) + sprite_color;
	final_color = final_color * (1 - gui_color.w) + gui_color;
	final_color = final_color * mult;
	
	gl_FragColor = final_color;
}