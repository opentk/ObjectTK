-- Vertex
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

-- Fragment
#version 140
out vec4 FragColor;

in vec3 OutVelocity;

//note: could use uniforms to set the colors from the application
const vec3 ColorFast = vec3(1.0, 0.0, 0.0);
const vec3 ColorSlow = vec3(0.0, 0.0, 1.0);

void main()
{
	// calculate position within the point sprite
	vec2 spritePosition = gl_PointCoord * 2 - vec2(1);
	float r2 = dot(spritePosition, spritePosition);
	
	// skip pixels outside the circle
	if (r2 > 1) discard;
	
	// draw fragment using a color gradient controlled by the total speed of the particle
	// and darken the color the closer the current fragment is to the edge of the particles circle
	FragColor = vec4(mix(ColorSlow, ColorFast, length(OutVelocity)) * (1.0 - r2), 1.0);
}