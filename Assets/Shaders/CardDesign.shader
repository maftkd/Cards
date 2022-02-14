Shader "Custom/CardDesign"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_ColorLight ("Color light", Color) = (0,0,0,0)
		_ColorAlt ("Color Alt" , Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float3 viewDir;
			float3 worldNormal;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		fixed4 _ColorLight;
		fixed4 _ColorAlt;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			fixed dt = smoothstep(0.2,1,dot(IN.viewDir,IN.worldNormal));
            // Albedo comes from a texture tinted by color

			//diamond center
			fixed xb = cos(IN.uv_MainTex.x*112.5)+sin(IN.uv_MainTex.y*94);
			fixed4 col = lerp(_Color,_ColorAlt,dt);
			fixed3 ca=lerp(_ColorLight,col,1-xb);

			//diamond cutoff
			fixed2 cd = IN.uv_MainTex-fixed2(0.5,0.5);
			fixed rs = saturate(dot(cd,cd)*12);
			fixed3 cb =lerp(ca,_ColorLight,rs); 
			
			//ring?
			fixed corn = saturate(dot(cd,cd)*4);
			corn = smoothstep(0.28,.38,corn);
			cb = lerp(cb,fixed3(0,0,0),corn);

			//grey zone
			corn = saturate(dot(cd,cd));
			corn = smoothstep(0.1,.11,corn);
			fixed3 cc = lerp(cb,fixed3(.3,.3,.3),corn);

			//outside corner
			corn = saturate(dot(cd,cd));
			corn = step(0.36,corn);
			o.Albedo=lerp(cc,fixed3(0,0,0),corn);
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness*dt;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
