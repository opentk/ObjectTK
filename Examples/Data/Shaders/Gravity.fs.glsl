#version 140
out vec4 FragColor;

void main()
{
	// calculate normal
	vec3 normal;
	normal.xy = gl_PointCoord * 2 - vec2(1);
	float r2 = dot(normal.xy, normal.xy);
	// skip pixels outside the sphere
	if (r2 > 1) discard;
	// draw white fragment
	FragColor = vec4(1);
}