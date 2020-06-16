/**
 * Unity sprite ghost shader.
 * Draws the sprite using only one color if _SingleColor is enabled.
 * The core CGPROGRAM was based on the Sprites/Default shader.
*/
Shader "Sprites/Ghost"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[MaterialToggle] _SingleColor ("Single color", Float) = 1
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			sampler2D _MainTex;
			sampler2D _AlphaTex;
			fixed4 _Color;
			float _SingleColor;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}			

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = lerp(
					tex2D(_MainTex, IN.texcoord),
					tex2D(_MainTex, IN.texcoord).a, 
					_SingleColor) * IN.color;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}
