#version 140
smooth in vec2 TexCoord;

out vec4 FragColor;

uniform sampler2D Texture;

void main()
{
	FragColor = texture(Texture, TexCoord);
	//FragColor = vec4(TexCoord, 0, 1);
}