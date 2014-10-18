#version 140
in vec3 InPosition;
in vec3 InVelocity;

out vec3 OutPosition;
out vec3 OutVelocity;

uniform float CenterMass;
uniform float TimeStep;
uniform mat4 ModelViewProjectionMatrix;

void main()
{
	// calculate acceleration by gravity G*m/r^2
	// define G to be 1 or include it in CenterMass, doesn't matter
	float acceleration = CenterMass / dot(InPosition, InPosition);
	
	// perform simple forward euler integration and
	// stream position and velocity via transform feedback into the pong buffer
	OutVelocity = InVelocity + acceleration * TimeStep * -normalize(InPosition);
	OutPosition = InPosition + OutVelocity * TimeStep;
	
	// pass through current position
	gl_Position = ModelViewProjectionMatrix * vec4(OutPosition,1);
	gl_PointSize = clamp(length(acceleration)*2, 4, 10);
}