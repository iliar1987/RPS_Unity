const float2 k = vec2(23.1406926327792690,2.6651441426902251);

float rnd( int seed, vec2 uv ) { return fract( cos( mod( asfloat(seed), 1024. * dot(uv,k) ) ) ); }
