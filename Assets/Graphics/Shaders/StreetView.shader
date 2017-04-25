Shader "StreetViewConverter/StreetView" {
	Properties {
      _Cube ("Cubemap", CUBE) = "" {}
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      Cull Front
      CGPROGRAM
      #pragma surface surf Lambert
      struct Input {
          float3 worldRefl;
      };
      samplerCUBE _Cube;
      void surf (Input IN, inout SurfaceOutput o) {
          o.Emission = texCUBE (_Cube, IN.worldRefl).rgb;
      }
      ENDCG
    } 
    Fallback "Diffuse"
}
