shader_type canvas_item;

uniform vec4 color : source_color = vec4(1.0);
uniform float width : hint_range(0, 10) = 1.0;
uniform float width_speed : hint_range(0.1, 10) = 1;
uniform int pattern : hint_range(0, 2) = 0; // diamond, circle, square

uniform float beam_angle : hint_range(0.0, 1.0) = 0.2;
uniform float trail_speed : hint_range(0.1, 10) = 1;
uniform float trail_width : hint_range(0.0, 1.0) = 0.5;

void vertex() {
	// resize vertices to get space to draw the outline
	VERTEX += (UV * 2.0 - 1.0) * width;
}

float greater_than(float x, float y) {
  return max(sign(x - y), 0.0);
}

float hasContraryNeighbour(vec2 uv, vec2 texture_pixel_size, sampler2D texture, float time_width) {
	float alpha = 0.0;
	for (float i = -ceil(time_width); i <= ceil(time_width); i++) {
		float x = abs(i) > time_width ? time_width * sign(i) : i;
		float offset;
		
		if (pattern == 0) {
			offset = time_width - abs(x);
		} else if (pattern == 1) {
			offset = floor(sqrt(pow(time_width + 0.5, 2) - x * x));
		} else if (pattern == 2) {
			offset = time_width;
		}
		
		for (float j = -ceil(offset); j <= ceil(offset); j++) {
			float y = abs(j) > offset ? offset * sign(j) : j;
			vec2 xy = uv + texture_pixel_size * vec2(x, y);
			
			if (!(xy != clamp(xy, vec2(0.0), vec2(1.0)) || texture(texture, xy).a == 0.0)) {
				alpha += color.a / (pow(x,2)+pow(y,2)) * (1.0-pow(2.0, -time_width));
			}
		}
	}
	
	return clamp(alpha, 0.0, 1.0);
}

void fragment() {
	vec2 uv = UV;
	
	vec2 texture_pixel_size = vec2(1.0) / (vec2(1.0) / TEXTURE_PIXEL_SIZE + vec2(width * 2.0));
		
	uv = (uv - texture_pixel_size * width) * TEXTURE_PIXEL_SIZE / texture_pixel_size;
		
	if (uv != clamp(uv, vec2(0.0), vec2(1.0))) {
		COLOR.a = 0.0;
	} else {
		COLOR = texture(TEXTURE, uv);
	}

	vec2 trail_uv = UV * 2.0 - 1.0;
	float a = (atan(trail_uv.y, trail_uv.x) + 3.141592656) / 6.283185312;
	float l = length(trail_uv);

	float current_beam_angle = mod(beam_angle * TIME * trail_speed, 1.0);
	a = a - 1.0 * greater_than(a, current_beam_angle);
	float trail_strength = max(0.0, (1.0 - (current_beam_angle - a) / trail_width));
	
	float time_width = width * 0.7 + sin(TIME * width_speed) * width * 0.3;

	float alpha = hasContraryNeighbour(uv, TEXTURE_PIXEL_SIZE, TEXTURE, time_width);
	//lit = pow(lit, 12.);
	if ((COLOR.a == 0.0) && alpha != 0.0) {


		COLOR.rgb = color.rgb;
		COLOR.a = alpha * trail_strength;
	}
}