varying highp vec4 var_position;
varying mediump vec3 var_normal;
varying mediump vec2 var_texcoord0;
varying mediump vec4 var_light;

uniform lowp sampler2D tex0;
uniform lowp vec4 tint;

void main()
{
    // Pre-multiply alpha since all runtime textures already are
    vec4 tint_pm = vec4(tint.xyz * tint.w, tint.w);
    vec4 color = texture2D(tex0, var_texcoord0.xy) * tint_pm;

    // Diffuse light calculations
    vec3 ambient_light = vec3(0.2);
    vec3 diff_light = vec3(normalize(var_light.xyz - var_position.xyz));
    diff_light = max(dot(var_normal,diff_light), 0.0) + ambient_light;
    diff_light = clamp(diff_light, 0.0, 1.0);
    if (dot(var_normal,vec3(0, 0, 1)) > 0) {
        color.rgb *= 0.1 - dot(var_normal,vec3(0, 0, -1)) * 0.1;
    }

    gl_FragColor = vec4(color.rgb*(0.75 + diff_light * 0.25), 1.0);
}

