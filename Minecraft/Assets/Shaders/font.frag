#version 330 core

in vec2 tex;

uniform vec3 color;
uniform sampler2D texture0;

out vec4 FragColor;

void main() 
{
	vec4 texColor = texture(texture0,tex);
    if(texColor.a < 0.1) discard;
    FragColor = vec4(texColor.rgb * color,1.0);
}