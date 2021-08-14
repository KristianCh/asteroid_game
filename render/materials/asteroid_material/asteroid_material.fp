/*varying highp vec3 var_position;
uniform lowp vec4 color;

void main()
{
    gl_FragColor = vec4(color.xyz, 1);
}*/

varying highp vec4 var_position;
varying mediump vec3 var_normal;
varying mediump vec2 var_texcoord0;
varying mediump vec4 var_light;

uniform lowp vec4 color;

void main()
{
    // Diffuse light calculations
    vec3 ambient_light = vec3(0.2);
    vec3 diff_light = vec3(normalize(var_light.xyz - var_position.xyz));
    diff_light = max(dot(var_normal,diff_light), 0.0) + ambient_light;
    diff_light = clamp(diff_light, 0.0, 1.0);

    gl_FragColor = vec4(color.rgb * (0.5 + 0.5 * diff_light), 1.0);
}

