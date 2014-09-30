#version 140
in vec3 InPosition;
uniform mat4 ModelViewProjectionMatrix;

void main()
{
	gl_Position = ModelViewProjectionMatrix * vec4(InPosition,1);
}