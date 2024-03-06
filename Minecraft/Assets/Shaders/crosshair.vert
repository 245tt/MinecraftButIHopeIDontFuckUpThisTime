#version 330 core

layout (location = 0) in vec2 Apos;

uniform mat4 model;

void main()
{
	//gl_Position = vec4(Apos,1.0) * model * view * projection;
	gl_Position = vec4(Apos,0.0,1.0) * model;
}