-- Version
#version 140

-- Vertex
#include BasicShader.Version
in vec3 InPosition;
uniform mat4 ModelViewProjectionMatrix;

void main()
{
	gl_Position = ModelViewProjectionMatrix * vec4(InPosition,1);
}

-- Fragment
#include BasicShader.Version
out vec4 FragColor;

void main()
{
	FragColor = vec4(1);
}