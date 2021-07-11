varying mediump vec4 position;
varying mediump vec2 var_texcoord0;

uniform lowp sampler2D mesh_original;
uniform lowp sampler2D fade_original;

void main()
{
	vec4 fade_color = texture2D(fade_original, var_texcoord0.xy) * 0.8;
	gl_FragColor = mix(fade_color, vec4(0, 0, 0, 0), 1 - 0.075);// *  texture2D(mesh_original, var_texcoord0.xy);
}
