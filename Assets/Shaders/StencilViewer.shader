Shader "Unlit/StencilViewer"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
			Stencil {
				Ref 1
					Comp always
					Pass replace
			}
		Colormask 0 ZWrite Off
        Pass
        {
        }
    }
}
