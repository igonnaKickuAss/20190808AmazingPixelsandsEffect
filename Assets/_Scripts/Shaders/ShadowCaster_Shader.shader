//shadowCaster Pass 仅仅只是把深度写入纹理中

Shader "OLiOYouxiShaders/VertFragShaders/ShadowCaster_Shader"
{

	SubShader
	{
		Name "OLiOShadowCaster"
		Tags { "LightMode"="ShadowCaster" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#pragma multi_compile_shadowcaster

			#include "UnityCG.cginc"

			struct v2f
			{
				V2F_SHADOW_CASTER;
			};

			v2f vert (appdata_base v)
			{
				v2f o;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				SHADOW_CASTER_FRAGMENT(i);
			}
			ENDCG
		}
	}
}
