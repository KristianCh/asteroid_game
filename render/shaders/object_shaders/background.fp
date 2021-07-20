varying mediump vec2 var_texcoord0;

uniform lowp sampler2D texture_sampler;
uniform lowp vec4 time;
uniform lowp vec4 tint;

void main()
{
    // Pre-multiply alpha since all runtime textures already are
    float time_div = 1;
    float var = 75;
    vec2 offset = vec2(cos(time.x/time_div + var_texcoord0.x * var), sin(time.x/time_div + var_texcoord0.y * var)) / 
        textureSize(texture_sampler,0) * 25;
    lowp vec4 tint_pm = vec4(tint.xyz * tint.w, tint.w);
    gl_FragColor = texture2D(texture_sampler, var_texcoord0.xy + offset) * tint_pm;
}
