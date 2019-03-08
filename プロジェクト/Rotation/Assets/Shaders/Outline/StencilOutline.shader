// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Outline/StencilOutline"
{
	Properties
	{
		_OutlineColor("Outline Color",Color) = (0,0,0,1)
		_OutlineWidth("Outline Width",Range(0.0,5.0)) = 0.01
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		LOD 100

		Pass
		{
			Stencil
			{
				Ref 1	//深度の書き込み値
				Comp always
				Pass replace
			}

			Cull Front
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				half4 vertex : POSITION;
				half3 normal : NORMAL;
			};

			struct v2f
			{
				half4 pos : SV_POSITION;
				UNITY_FOG_COORDS(0)
				float4 color : COLOR;
			};

			half _OutlineWidth;
			float4 _OutlineColor;

			v2f vert (appdata v)
			{
				//v2f o = (v2f)0;

				//o.pos = UnityObjectToClipPos(v.vertex + v.normal * _OutlineWidth);
				//return o;

				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				float2 offset = TransformViewToProjection(norm.xy);

				o.pos.xy += offset * o.pos.z * _OutlineWidth;
				o.color = _OutlineColor;
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				return _OutlineColor;
			}
			ENDCG
		}
	}
}
