varying highp vec2 var_texcoord0;

uniform highp sampler2D original;

void main()
{
	vec4 color = texture2D(original, var_texcoord0.xy);
	gl_FragColor = color;
}
