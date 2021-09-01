varying mediump vec2 var_texcoord0;

uniform lowp sampler2D texture_sampler;

void main()
{
    vec4 color = texture2D(texture_sampler, var_texcoord0.xy);
    if (color.x < 0.1 && color.y < 0.1 && color.z < 0.1) {
        color = vec4(0);
    }
    gl_FragColor = color;
}



