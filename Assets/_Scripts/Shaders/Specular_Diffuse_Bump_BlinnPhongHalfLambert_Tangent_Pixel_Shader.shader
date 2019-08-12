//在切线空间下计算光照模型

Shader "OLiOYouxiShaders/VertFragShaders/Specular_Diffuse_Bump_BlinnPhongHalfLambert_Tangent_Pixel_Shader"
{
	Properties
	{
		_Color ("反射率", color) = (1, 1, 1, 1)
		_MainTex ("纹理", 2D) = "white" {}
		_BumpMap ("法线映射", 2D) = "bump" {}
		_BumpScale ("凹凸大小", float) = 1.0
		_Specular ("高光颜色", color) = (1, 1, 1, 1)
		_Gross ("光泽度", Range(8, 256)) = 20
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
				float3 tangentLightDir : TEXCOORD1;
				float3 tangentViewDir : TEXCOORD2;

			};
			
			v2f vert (appdata v)
			{
				//这里计算光照模型
				v2f o;
				
				o.vertex = UnityObjectToClipPos(v.vertex);

				o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;

				o.texcoord.zw = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;

				//计算binormal（次法线）
				float3 binormal = cross(normalize(v.normal), normalize(v.tangent.xyz)) * v.tangent.w;

				//构造 一个从模型空间到切线空间的 向量转换矩阵
				float3x3 rotation = float3x3(v.tangent.xyz, binormal, v.normal);

				//同 内置的方法
				//TANGENT_SPACE_ROTATION;

				o.tangentLightDir = mul(rotation, ObjSpaceLightDir(v.vertex)).xyz;

				o.tangentViewDir = mul(rotation, ObjSpaceViewDir(v.vertex)).xyz;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed3 tangentLightDir = normalize(i.tangentLightDir);

				fixed3 tangentViewDir = normalize(i.tangentViewDir);

				//从法线纹理中（normal map）拿到 “纹素” （Texel）
				fixed4 packedNormal = tex2D(_BumpMap, i.texcoord.zw);	//zw存储了 凹凸纹理的 uv

				fixed3 tangentNormal;

				//如果纹理不是被标记为 “法线纹理”（normal map），那就需要我们通过公式 pixel = (normal + 1) / 2	即：normal = pixel * 2 - 1	进行反映射
				//tangentNormal.xy = (packedNormal.xy * 2 - 1) * _BumpScale;	//公式
				//tangentNormal.z = sqrt(1.0 - saturate(dot(tangentNormal.xy, tangentNormal.xy)));

				//如果已经标记了，使用 内置函数 进行采样 得到切线空间下的法线方向
				tangentNormal = UnpackNormal(packedNormal);
				tangentNormal.xy *= _BumpScale;		//对xy进行变化， z轴保持不变， 也就是说这坐标系中，这个向量更接近xy的平面
				tangentNormal.z = sqrt(1.0 - saturate(dot(tangentNormal.xy, tangentNormal.xy)));

				fixed3 albedo = tex2D(_MainTex, i.texcoord).rgb * _Color.rgb;	//使用纹理去采样漫反射颜色

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb * albedo;		//拿到环境光，与漫反射颜色混合

				fixed3 halfLambert = dot(tangentNormal, tangentLightDir) * 0.5 + 0.5;			//应用公式 算出漫反射

				fixed3 diffuse = _LightColor0.rgb * _Color.rgb * halfLambert;		//应用公式 算出漫反射

				fixed3 halfDir = normalize(tangentLightDir + tangentViewDir);		//应用公式 算出高光

				fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(saturate(dot(tangentNormal, halfDir)), _Gross);	//应用公式 算出高光

				return fixed4(ambient + diffuse + specular, 1.0);

			}

			ENDCG
		}
	}

	FallBack "Specular"
}
