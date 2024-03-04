#version 330 core

layout (location = 0) in vec3 Apos;
layout (location = 1) in vec2 ATex;


uniform mat4 model;
uniform mat4 projection;
uniform mat4 view;

void main()
{
    gl_Position = vec4(Apos,1.0) * model * view * projection;
}