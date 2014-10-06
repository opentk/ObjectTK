#version 330
smooth in vec3 TexCoord;

out vec4 FragColor;

uniform samplerCube Texture;

void main()
{
	// visualize the interpolated position, i.e. texture coordinate
	//FragColor = vec4(TexCoord, 1);
	// sample from the cube map texture
    FragColor = texture(Texture, TexCoord);
}