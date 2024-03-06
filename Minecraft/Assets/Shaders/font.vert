#version 330 core

layout (location = 0) in vec2 Apos;
layout (location = 1) in vec2 ATex;

out vec2 tex;

void main()
{
	tex = ATex;
	gl_Position = vec4(Apos.x,Apos.y,0.0,1.0);
}