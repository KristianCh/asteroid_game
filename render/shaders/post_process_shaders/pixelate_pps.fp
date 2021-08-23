varying highp vec4 position;
varying highp vec2 var_texcoord0;

uniform highp sampler2D original;

void main()
{
	vec2 dxy = vec2(2, 2) / textureSize(original,0);
	vec2 coord = vec2(dxy.x * floor(var_texcoord0.x / dxy.x) + dxy.x*0.5, dxy.y * floor(var_texcoord0.y / dxy.y) + dxy.y*0.5);
	vec4 color = texture2D(original, coord.xy);
	gl_FragColor = color;
}
