shader_type canvas_item;

uniform sampler2D gradient;
uniform float brightness : hint_range(0.0, 3.0) = 1.0;

void fragment() {
	vec4 main_texture = texture(TEXTURE, UV);
	vec3 out_color = texture(gradient, vec2(main_texture.r, 0)).rgb;
	COLOR = vec4(
		out_color * main_texture.g * brightness,
		main_texture.a
	);
}