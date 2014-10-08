#version 140
smooth in vec2 TextureCoordinates;
flat in int TextureLayer;

out vec4 FragColor;

uniform sampler2DArray TextureData;

void main()
{
	FragColor = texture(TextureData, vec3(TextureCoordinates, TextureLayer));
    //FragColor = vec4(TextureCoordinates.rg, TextureLayer/220.0, 1);
}