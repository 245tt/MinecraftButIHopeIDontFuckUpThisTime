#version 330 core

layout (location = 0) in vec3 Apos;
layout (location = 1) in vec2 ATex;
layout (location = 2) in float ABri;

out vec2 TexCoords;
out float brightness;

uniform mat4 model;
uniform mat4 projection;
uniform mat4 view;

void main()
{
    TexCoords = ATex;
    brightness = ABri;
    gl_Position = vec4(Apos,1.0) * model * view * projection;
}