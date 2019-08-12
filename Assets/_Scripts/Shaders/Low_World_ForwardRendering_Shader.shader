// Upgrade NOTE: replaced '_LightMatrix0' with 'unity_WorldToLight'

Shader "OLiOYouxiShaders/VertFragShaders/Low_World_ForwardRendering_Shader"
{
	Properties
	{
		_Color ("主色调", Color) = (1, 1, 1, 1)
		_MainTex ("主纹理", 2D) = "white" {}
		_BumpTex ("凹凸纹理", 2D) = "bump" {}
		_BumpScale ("凹凸程度", Float) = 1
		_ParallaxTex ("高度纹理", 2D) = "black" {}
		_ParallaxScale ("高度程度", Range(0, 0.08)) = 0.04
		_Specular ("高光颜色", Color) = (1, 1, 1, 1)
		_Gloss ("光泽度", Range(8, 256)) = 20
		_Diffuse ("漫反射颜色", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		//这是base_pass
		Pass
		{
			Tags { "LightMode"="ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			//保证我们在shader中使用光照衰减等光照变量可以被正确赋值（必须的）
			#pragma multi_compile_fwdbase

			#include "AutoLight.cginc"
			#include "Lighting.cginc"
			#include "UnityCG.cginc"

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _BumpTex;
			float4 _BumpTex_ST;
			float _BumpScale;

			sampler2D _ParallaxTex;
			float _ParallaxScale;

			fixed4 _Specular;
			float _Gloss;

			fixed4 _Diffuse;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 tangentLightDir : TEXCOORD0;
				float3 tangentViewDir : TEXCOORD1;
				float4 uv : TEXCOORD2;
				float3 worldVertex :TEXCOORD3;
				SHADOW_COORDS(4)
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				o.uv.xy = TRANSFORM_TEX(v.texcoord.xy, _MainTex);

				o.uv.zw = TRANSFORM_TEX(v.texcoord.xy, _BumpTex);

				//建立切线空间
				TANGENT_SPACE_ROTATION;

				o.tangentLightDir = mul(rotation, ObjSpaceLightDir(v.vertex));		//模型空间=>切线空间

				o.tangentViewDir = mul(rotation, ObjSpaceViewDir(v.vertex));	//模型空间=>切线空间

				o.worldVertex = UnityObjectToWorldDir(v.vertex);

				//传递阴影坐标系到像素着色器
				TRANSFER_SHADOW(o);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 normalizeLightDir = normalize(i.tangentLightDir);
				float3 normalizeViewDir = normalize(i.tangentViewDir);

				//高度纹理偏移
				fixed h = tex2D(_ParallaxTex, i.uv.zw).w;
				float2 offset = ParallaxOffset(h, _ParallaxScale, normalizeViewDir);
				i.uv.xy += offset;
				i.uv.zw += offset;

				//凹凸纹理偏移
				fixed3 tangentNormal = UnpackNormal(tex2D(_BumpTex, i.uv.zw));
				tangentNormal.xy *= _BumpScale;
				tangentNormal.z = sqrt(1.0 - dot(tangentNormal.xy, tangentNormal.xy));
			
				fixed3 albedo = tex2D(_MainTex, i.uv.xy) * _Color.rgb;

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb * albedo;

				fixed3 halfLambert = dot(tangentNormal, normalizeLightDir) * 0.5 + 0.5;

				fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * halfLambert;

				fixed3 blinnPhong = normalize(normalizeViewDir + normalizeLightDir);

				fixed _specularHalf = pow(saturate(dot(tangentNormal, blinnPhong)), _Gloss);

				fixed3 specular = _LightColor0.rgb * _Specular.rgb * _specularHalf;

				//fixed shadow = SHADOW_ATTENUATION(i);		//使用阴影坐标对阴影纹理进行采样		//只计算阴影
				//fixed atten = 1;		//光照

				UNITY_LIGHT_ATTENUATION(atten, i, i.worldVertex);		//计算了阴影，也计算了光照衰减

				return fixed4(ambient + (diffuse + specular) * atten, 1.0);
			}

			ENDCG
		}

		Pass
		{
			Tags { "LightMode"="ForwardAdd" }
			Blend One One

			CGPROGRAM
			#pragma vertex vertAdd
			#pragma fragment fragAdd

			//显示声明这是AddPass
			#pragma multi_compile_fwdadd

			#include "AutoLight.cginc"
			#include "Lighting.cginc"
			#include "UnityCG.cginc"

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _BumpTex;
			float4 _BumpTex_ST;
			float _BumpScale;

			sampler2D _ParallaxTex;
			float _ParallaxScale;

			fixed4 _Specular;
			float _Gloss;

			fixed4 _Diffuse;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
				//因为寄存器最多存储float4型变量，想要将4x3矩阵传递到片段着色器，那就用以下方法
				//实际上，我们只需要3x3矩阵，但是为了充分利用存储空间，我们把世界空间下的顶点位置存储在这些变量的w分量中
				float4 t2w0 : TEXCOORD1;
				float4 t2w1 : TEXCOORD2;
				float4 t2w2 : TEXCOORD3;
				SHADOW_COORDS(4)
			};
			
			v2f vertAdd (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				o.uv.xy = TRANSFORM_TEX(v.texcoord.xy, _MainTex);

				o.uv.zw = TRANSFORM_TEX(v.texcoord.xy, _BumpTex);

				//建立切线空间
				TANGENT_SPACE_ROTATION;

				float3 worldVertex = mul((float4x4)unity_ObjectToWorld, v.vertex).xyz;

				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);

				fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);

				fixed3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;

				//w分量放入了世界空间 顶点位置，以提高性能
				o.t2w0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldVertex.x);
				o.t2w1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldVertex.y);
				o.t2w2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldVertex.z);

				//传递阴影坐标系到像素着色器
				TRANSFER_SHADOW(o);

				return o;
			}
			
			fixed4 fragAdd (v2f i) : SV_Target
			{
				//这里计算光照模型
				float3 worldVertex = float3(i.t2w0.w, i.t2w1.w, i.t2w2.w);

#ifdef USING_DIRECTIONAL_LIGHT
				float3 normalizeLightDir = normalize(UnityWorldSpaceLightDir(worldVertex));
#else
				float3 normalizeLightDir = normalize(_WorldSpaceLightPos0.xyz - worldVertex);
#endif

				float3 normalizeViewDir = normalize(UnityWorldSpaceViewDir(worldVertex));

				//高度纹理偏移
				fixed h = tex2D(_ParallaxTex, i.uv.zw).w;
				float2 offset = ParallaxOffset(h, _ParallaxScale, normalizeViewDir);
				i.uv.xy += offset;
				i.uv.zw += offset;

				//凹凸纹理偏移
				fixed3 tangentNormal = UnpackNormal(tex2D(_BumpTex, i.uv.zw));
				tangentNormal.xy *= _BumpScale;
				tangentNormal.z = sqrt(1.0 - dot(tangentNormal.xy, tangentNormal.xy));
				
				fixed3 albedo = tex2D(_MainTex, i.uv.xy) * _Color.rgb;

				fixed3 halfLambert = dot(tangentNormal, normalizeLightDir) * 0.5 + 0.5;

				fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * halfLambert;

				fixed3 blinnPhong = normalize(normalizeViewDir + normalizeLightDir);

				fixed _specularHalf = pow(saturate(dot(tangentNormal, blinnPhong)), _Gloss);

				fixed3 specular = _LightColor0.rgb * _Specular.rgb * _specularHalf;

#ifdef USING_DIRECTIONAL_LIGHT
				//fixed atten = 1.0;
				UNITY_LIGHT_ATTENUATION(atten, i, worldVertex);		//计算了阴影，也计算了光照衰减
#else
				//下方宏，会帮我们把lightCoord，atten 定义
				//float3 lightCoord = mul(unity_WorldToLight, float4(worldVertex, 1)).xyz;
				//fixed atten = tex2D(_LightTexture0, dot(lightCoord, lightCoord).rr).UNITY_ATTEN_CHANNEL;		//获得光线衰减值
				//下面这个效果最差(线性衰减)
				//float distance = length(_WorldSpaceLightPos0.xyz - worldVertex);
				//fixed atten = 1 / distance; 
				UNITY_LIGHT_ATTENUATION(atten, i, worldVertex);		//计算了阴影，也计算了光照衰减
#endif

				return fixed4(albedo * (diffuse + specular) * atten, 1.0);
			}

			ENDCG
		}

	}
	FallBack "OLiOYouxiShaders/VertFragShaders/ShadowCaster_Shader"
		//FallBack "Specular"		//这个语义：ShadowCaster 选项 可以让unity 往回找 拥有 LightMode为ShadowCaster的Pass
}
