#version 330 core

layout (location = 0) in vec3 Apos;
layout (location = 1) in vec3 ACol;

out vec3 color;

uniform mat4 view;
uniform mat4 projection;
uniform mat4 model;

void main()
{
	color = ACol;
	//gl_Position = vec4(Apos,1.0) * model * view * projection;
	gl_Position = vec4(vec3(0.0,0.0,1.0),1.0);
}