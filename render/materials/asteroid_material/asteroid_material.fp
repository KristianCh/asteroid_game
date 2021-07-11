varying highp vec3 var_position;
uniform lowp vec4 color;

void main()
{
    gl_FragColor = vec4(color.xyz, 1);
}

