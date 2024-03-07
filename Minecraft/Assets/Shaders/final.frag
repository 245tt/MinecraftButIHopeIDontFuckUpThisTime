#version 330 core

in vec2 TexCoords;
in float brightness;

out vec4 FragColor;

uniform sampler2D texture0;

void main()
{
	
	FragColor = texture(texture0,TexCoords) * brightness;
}