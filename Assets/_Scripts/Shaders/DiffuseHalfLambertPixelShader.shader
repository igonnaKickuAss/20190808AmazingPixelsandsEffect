//逐顶点漫反射光照
//公式 c_diffuse = (c_light * m_diffuse) * (a(n_normal 点乘 I_lightdir) + b)	（大多数情况下，a和b的取值均为.5f）
//																				=>  c_diffuse = (c_light * m_diffuse) * (.5f(n_normal 点乘 I_lightdir) + .5f)
//即： 与兰伯特模型相比，半兰伯特模型没有通过max操作限定法线与光照方向点积的范围(>= 0)，而是对其点积进行a倍的缩放再加上一个b大小的偏移。

Shader "OLiOYouxiShaders/VertFragShaders/DiffuseHalfLambertPixelShader"
{
	Properties
	{
		_Diffuse ("Diffuse", color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags { "LightMode"="ForwardBase" }
		Cull back

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "Lighting.cginc"
			fixed4 _Diffuse;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;		//改变了的存在是为了能够访问顶点法线。。。通过NORMAL语义告诉unity要把模型顶点法线信息存储到normal变量中，为了传递给片段着色器
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed3 worldNormal : TEXCOORD0;		//并不是必须使用TEXCOORD0语义，也可以用COLOR
			};

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);		//顶点的模型空间转换到裁剪空间

				o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);	//顶点法线的模型空间转换到世界空间
				
				return o;
			}
			
			fixed4 frag (v2f i) : SV_TARGET0
			{
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;		//获得当前环境光
				
				fixed3 worldNormal = normalize(i.worldNormal);		//世界空间法线归一

				fixed3 worldLight = normalize(_WorldSpaceLightPos0.xyz);		//世界空间光线方向归一
				
				fixed3 halfLambert = dot(worldNormal, worldLight) * 0.5 + 0.5;			//应用公式 算出漫反射

				fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * halfLambert;		//应用公式 算出漫反射
				
				return fixed4(ambient + diffuse, 1.0);		//输出颜色	//环境光 与 漫反射 相加
			}

			ENDCG
		}
	}
}
