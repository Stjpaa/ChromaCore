//	Radar shader by Brian Smith (steampunkdemon.itch.io)
//	MIT Licence

shader_type canvas_item;

uniform vec4 line_color : source_color = vec4(0.0, 0.4, 0.0, 1.0);
uniform vec4 beam_color : source_color = vec4(0.5, 1.0, 0.5, 1.0);
uniform vec4 trail_color : source_color = vec4(0.0, 0.5, 0.0, 1.0);
uniform vec4 blip_color : source_color = vec4(0.5, 1.0, 0.5, 1.0);
uniform bool show_edges = false;
uniform float range_lines : hint_range(0.0, 10.0, 1.0) = 4.0;
uniform float sector_lines : hint_range(0.0, 10.0, 1.0) = 4.0;
uniform float line_width : hint_range(0.01, 0.1) = 0.01;
uniform float beam_angle : hint_range(0.0, 1.0) = 0.2;
uniform float beam_width : hint_range(0.0, 0.05) = 0.003;
uniform float trail_width : hint_range(0.0, 1.0) = 0.5;
uniform float blip_size : hint_range(0.1, 0.5) = 0.1;
uniform float blip_softness : hint_range(0.0, 1.0) = 0.4;
uniform float blip_presistence : hint_range(1.0, 5.0) = 2.0;
uniform vec2 blip_position = vec2(0.5, 0.5);

float greater_than(float x, float y) {
  return max(sign(x - y), 0.0);
}

void fragment() {
	vec2 uv = UV * 2.0 - 1.0;
	float a = (atan(uv.y, uv.x) + 3.141592656) / 6.283185312;
	float l = length(uv);

//	Uncomment the following line if you are applying the shader to a TextureRect.
//	COLOR = texture(TEXTURE,UV);

//	When using this shader in a game I suggest removing the following line of code and
//	replacing all occurrences of 'current_beam_angle' with 'beam_angle'.
//	You should then manually set the shader's beam_angle parameter so you know where the beam is
//	and therefore when the blip is not visible and its position can be updated.
	float current_beam_angle = mod(beam_angle * TIME, 1.0);

	a = a - 1.0 * greater_than(a, current_beam_angle);

//	If you do not want to render the trail remove the following line of code.
	COLOR.rgb = mix(COLOR.rgb, trail_color.rgb, trail_color.a * max(0.0, (1.0 - (current_beam_angle - a) / trail_width)));

//	If you do not want to render the beam remove the following line of code.	
	//COLOR.rgb = mix(COLOR.rgb, beam_color.rgb, beam_color.a * greater_than(a, current_beam_angle - beam_width));
}