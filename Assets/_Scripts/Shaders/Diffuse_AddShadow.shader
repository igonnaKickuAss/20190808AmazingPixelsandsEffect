Shader "OLiOYouxiShaders/VertFragShaders/Diffuse_AddShadow"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		//_BumpMap ("Normalmap", 2D) = "bump" {}		//不要法线纹理
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_Cutoff ("Alpha Cutoff", Range (0,1)) = 0.5

	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="TransparentCutOut" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
			
		}
			LOD 300


		Cull Off	//不能剔除背面，否则反转就看不见徐泰勒了
		Lighting On
		ZWrite Off
		Fog { Mode Off }
		

		CGPROGRAM
		#pragma surface surf Lambert alpha vertex:vert addshadow alphatest:_Cutoff 
		#pragma multi_compile DUMMY PIXELSNAP_ON 

		sampler2D _MainTex;
		sampler2D _BumpMap;
		fixed4 _Color;

		struct Input
		{
			float2 uv_MainTex;
			//float2 uv_BumpMap;	//不要法线纹理
			fixed4 color;
		};
		
		void vert (inout appdata_full v, out Input o)
		{
			#if defined(PIXELSNAP_ON) && !defined(SHADER_API_FLASH)
			v.vertex = UnityPixelSnap (v.vertex);
			#endif
			//v.normal = float3(0,0,-1);			//不要法线纹理
			//v.tangent =  float4(1, 0, 0, 1);			//不要法线纹理
			
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.color = _Color;
		}

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			//o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));		//不要法线纹理
		}
		ENDCG
	}

Fallback "Transparent/Cutout/Diffuse"
}
