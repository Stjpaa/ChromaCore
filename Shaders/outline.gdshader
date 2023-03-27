shader_type canvas_item;

uniform vec4 line_color : source_color = vec4(1.0);
uniform float width : hint_range(0, 10) = 0.5;

uniform float trail_speed : hint_range(0.0, 10) = 1;

void vertex() {
	// resize vertices to get space to draw the outline
	VERTEX += (UV * 2.0 - 1.0) * width;
}

float greater_than(float x, float y) {
  return max(sign(x - y), 0.0);
}

void fragment() {
	vec2 uv = UV;
	vec4 out_color;
	
	vec2 texture_pixel_size = vec2(1.0) / (vec2(1.0) / TEXTURE_PIXEL_SIZE + vec2(width * 2.0));
		
	uv = (uv - texture_pixel_size * width) * TEXTURE_PIXEL_SIZE / texture_pixel_size;
		
	if (uv != clamp(uv, vec2(0.0), vec2(1.0))) {
		out_color.a = 0.0;
	} else {
		out_color = texture(TEXTURE, uv);
	}

	vec2 trail_uv = UV * 2.0 - 1.0;
	float a = (atan(trail_uv.y, trail_uv.x) + 3.141592656) / 6.283185312;
	float b = (atan(-trail_uv.y, -trail_uv.x) + 3.141592656) / 6.283185312;

	float beam_angle = 0.2;
	float trail_width = 0.5;
	float current_beam_angle = mod(beam_angle * TIME * trail_speed, 1.0);
	a = a - 1.0 * greater_than(a, current_beam_angle);
	b = b - 1.0 * greater_than(b, current_beam_angle);
	float trail_strength = max(0.0, (1.0 - (current_beam_angle - a) / trail_width));
	trail_strength += max(0.0, (1.0 - (current_beam_angle - b) / trail_width));
	
	// float time_width = width * 0.7 + sin(TIME * width_speed) * width * 0.3;


	vec2 size = vec2(width * 0.01);
	

	float outline = 0.0;
	// left
	outline += texture(TEXTURE, UV + vec2(-size.x, 0)).a;
	// right
	outline += texture(TEXTURE, UV + vec2(size.x, 0)).a;
	// up
	outline += texture(TEXTURE, UV + vec2(0, size.y)).a;
	// down
	outline += texture(TEXTURE, UV + vec2(0, -size.y)).a;
	
	
	outline += texture(TEXTURE, UV + vec2(-size.x, size.y)).a;
	outline += texture(TEXTURE, UV + vec2(-size.x, size.y * 0.5)).a;
	
	outline += texture(TEXTURE, UV + vec2(size.x, size.y)).a;
	outline += texture(TEXTURE, UV + vec2(size.x, size.y * 0.5)).a;
	
	outline += texture(TEXTURE, UV + vec2(-size.x, -size.y)).a;
	outline += texture(TEXTURE, UV + vec2(-size.x, -size.y * 0.5)).a;
	
	outline += texture(TEXTURE, UV + vec2(size.x, -size.y)).a;
	outline += texture(TEXTURE, UV + vec2(size.x, -size.y * 0.5)).a;
	
	
	outline = min(outline, 1.0);
	
	COLOR = mix(out_color, vec4(line_color.rgb, line_color.a * trail_strength), outline - out_color.a);

}