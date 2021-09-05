varying mediump vec4 position;
varying mediump vec2 var_texcoord0;

uniform lowp sampler2D mesh_original;
uniform lowp sampler2D sprite_original;
uniform lowp sampler2D gui_original;
uniform lowp sampler2D fade_original;
uniform lowp vec4 time;
uniform lowp vec4 transition;

vec2 fade(vec2 t) {return t*t*t*(t*(t*6.0-15.0)+10.0);}
vec4 permute(vec4 x){return mod(((x*34.0)+1.0)*x, 289.0);}

float cnoise(vec2 P){
	vec4 Pi = floor(P.xyxy) + vec4(0.0, 0.0, 1.0, 1.0);
	vec4 Pf = fract(P.xyxy) - vec4(0.0, 0.0, 1.0, 1.0);
	Pi = mod(Pi, 289.0); // To avoid truncation effects in permutation
	vec4 ix = Pi.xzxz;
	vec4 iy = Pi.yyww;
	vec4 fx = Pf.xzxz;
	vec4 fy = Pf.yyww;
	vec4 i = permute(permute(ix) + iy);
	vec4 gx = 2.0 * fract(i * 0.0243902439) - 1.0; // 1/41 = 0.024...
	vec4 gy = abs(gx) - 0.5;
	vec4 tx = floor(gx + 0.5);
	gx = gx - tx;
	vec2 g00 = vec2(gx.x,gy.x);
	vec2 g10 = vec2(gx.y,gy.y);
	vec2 g01 = vec2(gx.z,gy.z);
	vec2 g11 = vec2(gx.w,gy.w);
	vec4 norm = 1.79284291400159 - 0.85373472095314 * 
	vec4(dot(g00, g00), dot(g01, g01), dot(g10, g10), dot(g11, g11));
	g00 *= norm.x;
	g01 *= norm.y;
	g10 *= norm.z;
	g11 *= norm.w;
	float n00 = dot(g00, vec2(fx.x, fy.x));
	float n10 = dot(g10, vec2(fx.y, fy.y));
	float n01 = dot(g01, vec2(fx.z, fy.z));
	float n11 = dot(g11, vec2(fx.w, fy.w));
	vec2 fade_xy = fade(Pf.xy);
	vec2 n_x = mix(vec2(n00, n01), vec2(n10, n11), fade_xy.x);
	float n_xy = mix(n_x.x, n_x.y, fade_xy.y);
	return 2.3 * n_xy;
}

float func(float x, float c) {
	return 4*x+c + sin(var_texcoord0.y*10 + time.x*3)/10;
}

float dist(vec2 p0, vec2 p1, vec2 p2) {
	return abs((p2.x-p1.x)*(p1.y-p0.y)-(p1.x-p0.x)*(p2.y-p1.y)) / sqrt(pow(p2.x-p1.x, 2) + pow(p2.y-p1.y, 2));
}

void main()
{
	vec4 final_color = vec4(1);

	float[25] kernel = float[] (
		-0.125, -0.125, -0.125, -0.125, -0.125,
		-0.125, -1.0, -1.0, -1.0, -0.125, 
		-0.125, -1.0, 10.0, -1.0, -0.125,
		-0.125, -1.0, -1.0, -1.0, -0.125,
		-0.125, -0.125, -0.125, -0.125, -0.125
	);

	float t = mod(time.x/2, 6)-1.5;
	float d = dist(var_texcoord0, vec2(0, func(0, -t)), vec2(1, func(1, -t)));
	float mult = 1;
	if (d < 0.2) {
		mult = 1.5-d*2.5;
	}
	vec2 texture_dims = textureSize(mesh_original,0);
	vec4 gui_color = texture(gui_original, var_texcoord0.xy);
	vec4 sprite_color = texture(sprite_original, var_texcoord0.xy);
	vec4 fade_color = texture(fade_original, var_texcoord0.xy);
	if (fade_color.r < 0.05 && fade_color.g < 0.05 && fade_color.b < 0.05) fade_color = vec4(0, 0, 0, 1);
	vec4 mesh_color = vec4(0);
	vec4 mesh_color2 = vec4(0);
	int index = 0;
	int mag = 2;
	for (int i = -2; i <= 2; i++) {
		for (int j = -2; j <= 2; j++) {
			vec2 shift = vec2(i,j) / texture_dims * mag;
			mesh_color += kernel[index++] * texture(mesh_original, var_texcoord0.xy + shift);
		}
	}
	//mesh_color = texture(mesh_original, var_texcoord0.xy);
	float alpha = 0;
	if (mesh_color.x > 0 || mesh_color.y > 0 || mesh_color.z > 0 || mesh_color.w > 0) {
		alpha = (mesh_color.x + mesh_color.y + mesh_color.z) / 4;
	}
	mesh_color = vec4(mesh_color.xyz, alpha); /*(mesh_color.x + mesh_color.y + mesh_color.z) / 4);*/
	
	final_color = sprite_color * (1 + (mult-1)/2) * vec4(5*fade_color.x + 1, 5*fade_color.y + 1, 5*fade_color.z + 1, 1);
	final_color = final_color * (1 - fade_color.w * 0.25) + vec4(fade_color.xyz, fade_color.w * 0.25) * 0.5;
	final_color = final_color * (1 - mesh_color.w) + mesh_color;
	final_color = (final_color * (1 - gui_color.w) + gui_color * mult) * (1 + (mult-1)/2);

	if (transition.x > 0) {
		final_color = mix(final_color * (1-transition.x), vec4(
			cnoise(var_texcoord0.xy * 200 + vec2(sin(time.x), cos(time.x))), 
			cnoise(var_texcoord0.xx * 200 + vec2(sin(time.x), cos(time.x))), 
			cnoise(var_texcoord0.yy * 200 + vec2(sin(time.x), cos(time.x))), 
			2.5) / 2.5, transition.x);
		}
	
	gl_FragColor = final_color;
}