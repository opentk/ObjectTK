#version 150

// this shader processes points and outputs two triangles (a quad)
layout (points) in;
layout (triangle_strip, max_vertices = 4) out;

flat in vec2 Position[1];
flat in int Texture[1];

smooth out vec2 TextureCoordinates;
flat out int TextureLayer;

uniform mat4 ModelViewProjectionMatrix;

void main()
{
	TextureLayer = Texture[0];
	vec4 p = vec4(Position[0], 0, 1);
	float d = 0.5;
	gl_Position = ModelViewProjectionMatrix * (p + vec4(-d,-d, 0, 0));
	TextureCoordinates = vec2(0,0);
    EmitVertex();
    gl_Position = ModelViewProjectionMatrix * (p + vec4( d,-d, 0, 0));
	TextureCoordinates = vec2(1,0);
    EmitVertex();
    gl_Position = ModelViewProjectionMatrix * (p + vec4(-d, d, 0, 0));
	TextureCoordinates = vec2(0,1);
    EmitVertex();
	gl_Position = ModelViewProjectionMatrix * (p + vec4( d, d, 0, 0));
	TextureCoordinates = vec2(1,1);
    EmitVertex();
    EndPrimitive();
}