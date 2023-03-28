shader_type canvas_item;

uniform vec2 direction;
uniform float strength : hint_range(0.0, 10.0) = 1.0;
uniform float time : hint_range(0.0, 1) = 0;

void vertex() {
	// sin(x / 𝜋 *60) * (1 - x)
	//float time = mod(TIME * 0.5, 1);
	VERTEX += normalize(direction) * strength * (sin(time / PI * 60.0) * (1.0 - time));
}

void fragment() {
	texture(TEXTURE, UV);
}

