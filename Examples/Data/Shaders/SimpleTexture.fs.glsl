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