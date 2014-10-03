#version 150
// Copyright (c) 2008 the OpenTK Team. See license.txt for legal bla

// custom vertex attribute
in vec3 InPosition;
in vec3 InNormal;
in vec3 InTangent;
in vec2 InTexCoord;

// transformation matrix
uniform mat4 ModelViewMatrix;
uniform mat4 ModelViewProjectionMatrix;
uniform mat3 NormalMatrix;

// world uniforms
uniform vec3 Light_Position;
uniform vec3 Camera_Position;

// MUST be written to for FS
out vec2 TexCoord;
out vec3 VaryingLightVector;
out vec3 VaryingEyeVector;

void main()
{
	gl_Position = ModelViewProjectionMatrix * vec4(InPosition,1);
	TexCoord = InTexCoord;

	vec3 nor = normalize(NormalMatrix * InNormal);
	vec3 tan = normalize(NormalMatrix * InTangent);
	vec3 bi = cross(nor, tan);
  
	// need positions in tangent space
	vec3 vertex = vec3(ModelViewMatrix * vec4(InPosition,1));

	vec3 temp = Light_Position - vertex;
	VaryingLightVector.x = dot(temp, tan); // optimization, calculate dot products rather than building TBN matrix
	VaryingLightVector.y = dot(temp, bi);
	VaryingLightVector.z = dot(temp, nor);

	temp = Camera_Position - vertex;
	VaryingEyeVector.x = dot(temp, tan);
	VaryingEyeVector.y = dot(temp, bi);
	VaryingEyeVector.z = dot(temp, nor);
}