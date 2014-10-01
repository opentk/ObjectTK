#version 140
in vec3 InPosition;
in vec2 InTexCoord;

smooth out vec2 TexCoord;

uniform mat4 ModelViewProjectionMatrix;

void main()
{
	// transform vertex position
	gl_Position = ModelViewProjectionMatrix * vec4(InPosition,1);
	// pass through texture coordinate
	TexCoord = InTexCoord;
}