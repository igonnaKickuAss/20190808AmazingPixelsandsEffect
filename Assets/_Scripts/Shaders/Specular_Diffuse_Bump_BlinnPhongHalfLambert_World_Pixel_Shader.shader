// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

//在世界空间下计算光照模型

Shader "OLiOYouxiShaders/VertFragShaders/Specular_Diffuse_Bump_BlinnPhongHalfLambert_World_Pixel_Shader"
{
	Properties
	{
		_Color ("反射率", color) = (1, 1, 1, 1)
		_MainTex ("纹理", 2D) = "white" {}
		_BumpMap ("法线映射", 2D) = "bump" {}
		_BumpScale ("凹凸大小", float) = 1.0
		_Specular ("高光颜色", color) = (1, 1, 1, 1)
		_Gross ("光泽度", Range(8, 256)) = 20
		_CutOff ("CutOff(A)", Range(0, 1)) = 0.5
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "LightMode"="ForwardBase" }
		Cull back

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BumpMap;
			float4 _BumpMap_ST;
			float _BumpScale;
			fixed4 _Specular;
			float _Gross;
			fixed _CutOff;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;	//TANGENT 语义为 让unity把顶点的切线方向填充到tangent变量中，	顶点法线 作z轴， 顶点切线方向 作x轴， 顶点法线与切线的叉积， 作y轴
				float4 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 texcoord : TEXCOORD0;
				//因为寄存器最多存储float4型变量，想要将4x3矩阵传递到片段着色器，那就用以下方法
				//实际上，我们只需要3x3矩阵，但是为了充分利用存储空间，我们把世界空间下的顶点位置存储在这些变量的w分量中
				float4 t2w0 : TEXCOORD1;
				float4 t2w1 : TEXCOORD2;
				float4 t2w2 : TEXCOORD3;
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				
				o.vertex = UnityObjectToClipPos(v.vertex.xyz);	//裁剪空间

				o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;

				o.texcoord.zw = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;

				float3 worldVertex = mul((float4x4)unity_ObjectToWorld, v.vertex).xyz;

				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);

				fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);

				fixed3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;

				//w分量放入了世界空间 顶点位置，以提高性能
				o.t2w0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldVertex.x);
				o.t2w1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldVertex.y);
				o.t2w2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldVertex.z);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//这里计算光照模型
				float3 worldVertex = float3(i.t2w0.w, i.t2w1.w, i.t2w2.w);

				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(worldVertex));

				fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldVertex));

				fixed3 bump = UnpackNormal(tex2D(_BumpMap, i.texcoord.zw));

				bump.xy *= _BumpScale;

				bump.z = sqrt(1.0 - saturate(dot(bump.xy, bump.xy)));

				bump = normalize(half3(dot(i.t2w0.xyz, bump), dot(i.t2w1.xyz, bump), dot(i.t2w2.xyz, bump)));

				fixed3 albedo = tex2D(_MainTex, i.texcoord).rgb * _Color.rgb;	//使用纹理去采样漫反射颜色

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb * albedo;		//拿到环境光，与漫反射颜色混合

				fixed3 halfLambert = dot(bump, worldLightDir) * 0.5 + 0.5;			//应用公式 算出漫反射

				fixed3 diffuse = _LightColor0.rgb * _Color.rgb * halfLambert;		//应用公式 算出漫反射

				fixed3 halfDir = normalize(worldLightDir + worldViewDir);		//应用公式 算出高光

				fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(saturate(dot(bump, halfDir)), _Gross);	//应用公式 算出高光

				fixed4 col = fixed4(ambient + diffuse + specular, 1.0);

				////alpha Test
				//clip(col.a - _CutOff);				

				return col;

			}

			ENDCG
		}
	}

	FallBack "Specular"
}
