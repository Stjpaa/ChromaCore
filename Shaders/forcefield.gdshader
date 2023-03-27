shader_type canvas_item;

uniform float width : hint_range(10, 1000) = 30;
uniform float height : hint_range(10, 1000) = 30;
uniform vec3 particle_color : source_color;
uniform float particle_size : hint_range(0.0, 1.0) = 0.2;
uniform float particle_rotation_speed : hint_range(0.0, 10.0) = 5;

uniform vec2 direction = vec2(0, 1);
uniform float strength : hint_range(0.0, 10.0) = 0.5;
uniform int quality = 2;

void vertex(){
	vec2 blurSize = abs(direction) * 2.0;
	VERTEX *= blurSize + 1.0;
	UV = (UV - 0.5) * (blurSize + 1.0) + 0.5;
}

float insideUnitSquare(vec2 v) {
    vec2 s = step(vec2(0.0), v) - step(vec2(1.0), v);
    return s.x * s.y;   
}

vec2 random(vec2 uv) {
	return vec2(fract(sin(dot(uv.xy,
		vec2(12.9898,78.233))) * 43758.5453123));
}

float worley(vec2 uv, float columns, float rows, float time) {
	
	vec2 index_uv = floor(vec2(uv.x * columns, uv.y * rows));
	vec2 fract_uv = fract(vec2(uv.x * columns, uv.y * rows));
	
	float minimum_dist = 1.0;  
	
	for (int y= -1; y <= 1; y++) {
		for (int x= -1; x <= 1; x++) {
			vec2 neighbor = vec2(float(x),float(y));
			vec2 point = random(index_uv + neighbor);

			float random = fract(sin(point.x + 78.233) * 43758.5453123) - 0.5;
			float rot_direction = random / abs(random);
			// sin(x * 𝜋 / 3) + sin(x * 𝜋)
			point = vec2(
				cos(rot_direction * time * point.x * particle_rotation_speed / 3.0) + cos(rot_direction * time * point.x * particle_rotation_speed),
				sin(rot_direction * time * point.y * particle_rotation_speed / 3.0) + sin(rot_direction * time * point.y * particle_rotation_speed)
			) * 0.5 + 0.5;
			
			vec2 diff = neighbor + point - fract_uv;
			float dist = length(diff);
			minimum_dist = min(minimum_dist, dist);
		}
	}
	
	return minimum_dist;
}

vec4 run_worley(vec2 uv) {
	float worley = worley(uv + direction * TIME * strength, width, height, TIME);
	return vec4(particle_color, step(1.0 - particle_size, 1.0-worley) * (1.0-worley));
}

void fragment() {
	float inSquare = insideUnitSquare(UV);
	float numSamples = inSquare;
	COLOR = run_worley(UV) * inSquare;
	vec2 stepSize = direction/(float(quality)) / 50.0 * strength;
	vec2 uv;
	for(int i = 1; i <= quality; i++){
		uv = UV + stepSize * float(i);
		inSquare = insideUnitSquare(uv);
		numSamples += inSquare;
		COLOR += run_worley(uv) * inSquare;
		
		uv = UV - stepSize * float(i);
		inSquare = insideUnitSquare(uv);
		numSamples += inSquare;
		COLOR += run_worley(uv) * inSquare;
	}
	COLOR.rgb /= numSamples;
	COLOR.a /= float(quality)*2.0 + 1.0;
}
