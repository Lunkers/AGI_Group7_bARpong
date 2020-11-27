Shader "Custom/LiquidInContainer"
{
    // Shader for liquid in a container
    // Based on https://twitter.com/minionsart/status/986374665399685121
    // and https://github.com/CaptainProton42/LiquidContainerDemo/blob/master/assets/shaders/liquid.shader
    Properties
    {
       	_Tint ("Tint", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
        _FillAmount ("Fill Amount", Range(-10,10)) = 0.0
		[HideInInspector] _WobbleX ("WobbleX", Range(-1,1)) = 0.0
		[HideInInspector] _WobbleZ ("WobbleZ", Range(-1,1)) = 0.0
        _TopColor ("Top Color", Color) = (1,1,1,1)
		_FoamColor ("Foam Line Color", Color) = (1,1,1,1)
        _Rim ("Foam Line Width", Range(0,0.1)) = 0.0    
		_RimColor ("Rim Color", Color) = (1,1,1,1)
	    _RimPower ("Rim Power", Range(0,10)) = 0.0
        _Height("Height",Float) = 0.32
        _Width("Width", Float) = 0.1
    }
    //TODO: Rewrite to surface shader????
    SubShader
    {
        Tags {"Queue" = "Geometry" "DisableBatching" = "True" }
        Pass {
            ZWrite On
            Cull Off //front and back faces are needed
            AlphaToMask On //Transparency
        

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
                float3 normal: NORMAL;
            };

            struct v2f {
                float2 uv: TEXCOORD0;
                float4 vertex: SV_POSITION;
                float3 viewDir: COLOR;
                float3 normal: COLOR2;
                float fillEdge: TEXCOORD2;
                float3 worldPos: TEXCOORD3;
                half3 worldNormal: TEXCOORD4; 
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _FillAmount, _WobbleX, _WobbleZ;
            float4 _TopColor, _RimColor, _FoamColor, _Tint;
            float _Rim, _RimPower, _Width, _Height;

            //rotate a vertex theta degrees around the y-axis
            float4 RotateAroundYInDegrees (float4 vertex, float degrees) {
                float alpha = degrees * UNITY_PI / 180; //convert to radians
                float sina, cosa;
                sincos(alpha, sina, cosa);
                float2x2 m = float2x2(cosa, sina, -sina, cosa); //rotation matrix
                return float4(vertex.yz, mul(m, vertex.xz)).xzyw; //return vertex rotated around y-axis
            }

            v2f vert (appdata v) {
                v2f o; //output
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                //get world position of vertex
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex.xyz);
                //get world space direction of up vector
                float d = dot(float3(unity_ObjectToWorld[0][1], unity_ObjectToWorld[1][1], unity_ObjectToWorld[2][1]), float3(0,1,0));
                float m = lerp(_Width, _Height, abs(d));

                //combine rotations with worldPos, using wobble
                //float3 worldPosAdjusted = worldPos + (worldPosX * _WobbleX) + (worldPosZ * _WobbleZ);

                //how high up should the liquid be
                // TODO: 
                o.fillEdge = _FillAmount * m;
                
                o.worldPos = worldPos;
                o.worldNormal = UnityObjectToWorldNormal(v.vertex);
                o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
                o.normal =  v.normal; 
                return o;
            }
            fixed4 frag (v2f i, fixed facing: VFACE) :SV_Target {
                if(i.worldPos.y > i.fillEdge){
                    discard;
                }
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * _Tint;
                //rim light
                float dotProduct = 1 - pow(dot(i.normal, i.viewDir), _RimPower);

                float4 RimResult = smoothstep(0.5, 1.0, dotProduct);
                RimResult *= _RimColor;


                //foam edge
                float4 foam = (step(i.fillEdge, 0.5) - step(i.fillEdge, (0.5 - _Rim)));

                float4 foamColored = foam * (_FoamColor * 0.9);

                //rest of the liquid
                float4 result = step(i.fillEdge, 0.5) - foam;
                float4 resultColored = result * col;


                //add together
                //float4 finalResult = resultColored + foamColored;
                //finalResult.rgb += RimResult;
                float4 finalResult = col;

                //color for backfaces and top
                float4 topColor = _TopColor * (foam + result);

                //VFACE returns positive if front facing, negative for backfacing
                
                return facing > 0 ? finalResult : topColor;
            }
            //TODO custom lighting
            ENDCG

        }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
        
    }
}
