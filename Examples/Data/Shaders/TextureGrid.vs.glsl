#version 140
in vec2 InPosition;
in int InTexture;

flat out vec2 Position;
flat out int Texture;

void main()
{	
	Position = InPosition;
	Texture = InTexture;
}