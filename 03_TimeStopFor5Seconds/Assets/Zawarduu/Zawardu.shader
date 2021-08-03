Shader "Hidden/Zawardu" {
    Properties {
        [HideInInspector]
        _MainTex ("Base (RGB)", 2D) = "white" {}

        _CenterX("CenterX", Range(0,1)) = 0.5
        _CenterY("CenterY", Range(0,1)) = 0.5
        _Radius("Radius", Range(0.001,1.4)) = 1.0
        _HueShift ("HueShift", 2D) = "white" {}
    }
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            uniform sampler2D _MainTex;
            uniform sampler2D _HueShift;  

            float _Radius;
            float _CenterX;
            float _CenterY;
            const float PI = 3.1415926535;

            fixed4 frag (v2f_img i) : SV_Target
            {
                //Some values to get perfect circle regardless the screen size
                float scX = _ScreenParams.x/_ScreenParams.y;
                float scY = _ScreenParams.x/_ScreenParams.x; //Could write 1 but wrote it like this show you where it came from
                
                // Circle from https://halisavakis.com/my-take-on-shaders-custom-masks-part-i/
                fixed4 orCol = tex2D(_MainTex, i.uv);
                float dist = length(float2(i.uv.x - _CenterX, i.uv.y - _CenterY) * float2(scX,scY));
                float circle = saturate(dist/_Radius);
                float circleAlpha = circle;
                float a = 1-circleAlpha;
                half4 circleMask = (orCol.rgb, a * orCol.a);

                //Distortion
                half2 n = i.uv;
                half2 d = n * 2.0 - 1.0;
                d *= 0.3f * _Radius;
                i.uv -= d * circleMask;
                i.uv = saturate(i.uv);
                float4 distorion = tex2D(_MainTex, i.uv);

                //Hue
                float4 hueShift = tex2D(_HueShift,i.uv * 0.1 + _Radius);

                //Final mix
                half4 circleStepped = 1-step(circleMask, 0.1f);
                return (1-distorion) * circleStepped + distorion * (1-circleStepped) + hueShift * circleMask * 0.4f;
            }
            ENDCG
        }
    }
}