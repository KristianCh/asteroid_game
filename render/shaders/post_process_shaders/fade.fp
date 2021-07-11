varying mediump vec4 position;
varying mediump vec2 var_texcoord0;

uniform lowp sampler2D mesh;

void main()
{
	gl_FragColor = texture2D(mesh, var_texcoord0.xy) * (1-0.1666);
}
