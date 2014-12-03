-- Vertex
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

-- Fragment
#version 140
smooth in vec2 TexCoord;

out vec4 FragColor;

uniform sampler2D Texture;
uniform bool RenderTexCoords = false;

void main()
{
	if (RenderTexCoords) FragColor = vec4(TexCoord, 0, 1);
	else FragColor = texture(Texture, TexCoord);
}