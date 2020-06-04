Shader "Unlit/LiquidHolder"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _LiquidMap("LiquidMap", 2D) = "black"{}
        _LiquidColor("LiquidColor",Color) = (1,0.5,0.1,1)
        _Opacity("Opacity",Range(0,1)) = 0
    }
    SubShader
    {
        // Default sprite shader settings
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
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            // Properties
            sampler2D _MainTex;
            sampler2D _LiquidMap;
            
            half4 _LiquidColor;
            
            half _Opacity;
            
            float4 _MainTex_ST;

            // Functions 
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                half liquidMapAlpha = tex2D(_LiquidMap, i.uv).a;
                half isLiquid = step(0.1,liquidMapAlpha*step(i.uv.y,_Opacity));
                //return col = step(0.001,tex2D(_LiquidMap, i.uv).a);
                // Apply color if liquid
                half4 computedColor = (_LiquidColor*isLiquid);
                col.rgb *= (1-isLiquid)+computedColor;
                col.a = saturate(computedColor.a * liquidMapAlpha+col.a);
                
                return col;
            }
            ENDCG
        }
    }
}
