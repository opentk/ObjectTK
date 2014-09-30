#version 140
in vec3 InVertex;
uniform mat4 ModelViewProjectionMatrix;

void main()
{
	gl_Position = ModelViewProjectionMatrix * vec4(InVertex,1);
}