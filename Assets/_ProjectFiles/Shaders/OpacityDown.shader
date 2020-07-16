Shader "Unlit/OpacityDown"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _Opacity("Opacity",Range(0,1)) = 0
        _BackColor("_BackColor", Color) = (0,0,0,1)
        
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

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            fixed _Opacity;
            
            fixed4 _BackColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                
                fixed isBackground = step(_Opacity, i.uv.y);
                fixed4 newBackColor = (_BackColor*isBackground);
                fixed4 newForeColor = (col * (1-isBackground));
                
                fixed4 computedColor = newBackColor + newForeColor;
                
                computedColor.a = max(newBackColor,newForeColor);
                return computedColor;
            }
            ENDCG
        }
    }
}
