#version 330 core

layout (location = 0) in vec3 Apos;
layout (location = 1) in vec2 ATex;

out vec2 TexCoords;

uniform mat4 model;
uniform mat4 projection;
uniform mat4 view;

void main()
{
    TexCoords = ATex;
    gl_Position = vec4(Apos,1.0) * model * view * projection;
}