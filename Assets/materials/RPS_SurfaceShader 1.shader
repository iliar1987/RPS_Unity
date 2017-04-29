Shader "Custom/RPS_SurfaceShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_SpecularStrength ("Specular Strength", Range(0,1)) = 1.0
		_Emission ("Emission",Range(0.0,1.0)) = 0.5
		_Occlusion ("Occlusion",Range(0.0,1.0)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200
		cull off 
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf StandardSpecular fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _SpecularStrength;
		fixed4 _Color;
		float _Emission;
		float _Occlusion;

		void surf (Input IN, inout SurfaceOutputStandardSpecular o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			//o.Metallic = _Metallic;

			o.Smoothness = _Glossiness;
			o.Alpha = c.a;

			float3 emm = clamp(length(c.rgb - 0.5f) * c.rgb,0,1);
			o.Emission = emm * _Emission;
			o.Occlusion = _Occlusion;
			//o.Specular = clamp(emm * _SpecularStrength - 0.5,0,1);
			o.Specular = length(emm) * _SpecularStrength;
			//o.Emission = clamp(max(max(c.r,c.g),c.b) * c.rgb * _Emission,0,1);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
