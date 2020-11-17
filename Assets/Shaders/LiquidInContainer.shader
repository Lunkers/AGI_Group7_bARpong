Shader "Custom/LiquidInContainer"
{
    // Shader for making somewhat realistic liquid in a container
    // Based on https://www.youtube.com/watch?v=dFv8lM-kS4E
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _BumpMap ("Normal map of top of liquid", 2D) = "bump" {}
        _BumpStrength ("Normal map strength", float) = 1
        _BumpMapInner ("Inner normal map", 2D) = "bump"{}
        _BumpInnerStrength("Inner normal strength", float) = 1
        _TopColor("Top section color", Color) = (1,1,1,1)

        _Ruffle ("Ruffle strength", float) = 10
        _PlanePosition("Cutoff plane position", Vector) = (0,0,0,1)
        _PlaneNormal("Cutoff plane normal", Vector) = (0,1,0,0) 
    }
    SubShader
    {
        Tags { "Queue"="Geometry" }


        CGINCLUDE

        float3 _PlaneNormal;
        float3 _PlanePosition;
        
        // determine if geometry should be visible or not
        bool checkVisibility(float3 worldPosition) {
            //dot product between world position of vertex and plane normal
            float dotWorldPlane = dot(worldPosition - _PlanePosition, _PlaneNormal);
            return dotWorldPlane > 0; //return wether or not the world position is culled by the plane
        }

        ENDCG

        GrabPass {
            "_GrabTexture"
        }

       Cull Front //render backside geometry
       CGPROGRAM
       #pragma surface surf Standard addshadow
       #pragma vertex vert
       #pragma target 3.0

       struct Input {
           half2 uv_Tex;
           float3 worldPos;
           float4 screenPos;
           float3 normal;
           float3 viewDir;
           float3 worldNormal;
           float4 grabUV;
           INTERNAL_DATA
       };

       sampler2D _MainTex;
       sampler2D _BumpMap;
       sampler2D _GrabTexture;
       fixed4 _Color;
       fixed4 _TopColor;
       float _BumpStrength;
       half _Glossiness;
       half _Metallic;
       float _RotationSpeed;

       void vert (inout appdata_full v, out Input o) {
           UNITY_INITIALIZE_OUTPUT(Input, o);
           float4 pos = UnityObjectToClipPos(v.vertex);
           o.grabUV = ComputeGrabScreenPos(pos); 
       }

       void surf (Input IN, inout SurfaceOutputStandard o ) {
           if (checkVisibility(IN.worldPos)) discard; //remove parts that shouldn't be visible

           float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
           screenUV *= float2(8,6);

           float4 background = tex2Dproj( _GrabTexture, IN.grabUV) * _TopColor;
           o.Albedo = lerp(background.rgb, _TopColor.rgb, _TopColor.a);
           
            half3 worldT = WorldNormalVector(IN, half3(1,0,0));
            half3 worldB = WorldNormalVector(IN, half3(0,1,0));
            half3 worldN = WorldNormalVector(IN, half3(0,0,1));
            half3x3 world2Tangent = half3x3(worldT, worldB, worldN);
		
            o.Normal = mul(world2Tangent, _PlaneNormal);
           o.Metallic = _Metallic;
           o.Smoothness = _Glossiness;
       }

       ENDCG

       Cull Back
       
       CGPROGRAM
       #pragma surface surf Standard addshadow
       #pragma vertex vert
       #pragma target 3.0

       sampler2D _MainTex;
       sampler2D _BumpMapInner;
       sampler2D _GrabTexture;
       float _BumpStrengthInner;

       struct Input {
           float2 uv_MainTex;
           float2 uv_BumpMapInner;
           float3 worldPos;
           float4 screenPos;
           float4 grabUV;
       };

       half _Glossiness;
       half _Metallic;
       fixed4 _Color;
       fixed4 _TopColor;

       void vert (inout appdata_full v, out Input o) {
           UNITY_INITIALIZE_OUTPUT(Input, o);
           float pos = UnityObjectToClipPos(v.vertex);
           o.grabUV = ComputeGrabScreenPos(pos);
       }

       void surf(Input IN, inout SurfaceOutputStandard o) {
           float4 background = tex2Dproj(_GrabTexture, IN.grabUV) * _Color;
           o.Albedo = lerp(background.rgb, _Color.rgb, _Color.a);

           o.Metallic = _Metallic;
           o.Smoothness = _Glossiness;
           o.Normal= UnpackScaleNormal(tex2D (_BumpMapInner, IN.uv_BumpMapInner), _BumpStrengthInner);
       }
        ENDCG
    }
}
