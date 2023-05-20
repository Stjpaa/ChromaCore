shader_type canvas_item;

uniform vec4 bg_color: source_color;
uniform vec2 position;

float rand(vec2 st) {
    return fract(sin(dot(st.xy, vec2(12.9898,78.233))) * 43758.5453123);
}

void fragment() {
	vec2 big_star_offset = position * 0.13;
	vec2 small_star_offset = position * 0.1;
	float size = 100.0;
	float small_size = 5.0;
	float prob = 0.9;
	vec2 pos = floor(1.0 / size * (FRAGCOORD.xy + big_star_offset));
	float color = 0.0;
	float starValue = rand(pos);
	
	COLOR = vec4(0.0, 0.0, 0.0, 1.0);

	if (starValue > prob)
	{
		vec2 center = size * pos + vec2(size, size) * 0.5;
		float t = 0.9 + 0.2 * sin(TIME * 8.0 + (starValue - prob) / (1.0 - prob) * 45.0);
		color = 1.0 - distance(FRAGCOORD.xy + big_star_offset, center) / (0.5 * size);
		color = color * t / (abs(FRAGCOORD.y + floor(big_star_offset.y) - center.y)) * t / (abs(FRAGCOORD.x + floor(big_star_offset.x) - center.x));
	}

	if(rand(
		floor(FRAGCOORD.xy + small_star_offset) / 20.0
	) > 0.996) {
		COLOR = vec4(vec3(1.0),1.0);
	}

	if (rand(floor(FRAGCOORD.xy + small_star_offset) / 20.0) > 0.996)
	{
		float r = rand(floor(FRAGCOORD.xy + small_star_offset));
		color = r * (0.85 * sin(TIME * (r * 5.0) + 720.0 * r) + 0.95);
	}
	
	COLOR = vec4(vec3(color),1.0) + bg_color;
}