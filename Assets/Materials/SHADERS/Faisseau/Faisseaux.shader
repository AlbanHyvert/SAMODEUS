// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Faisseaux"
{
	Properties
	{
		_linear_gradient("linear_gradient", 2D) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float3 worldPos;
		};

		uniform sampler2D _linear_gradient;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color98 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			o.Albedo = color98.rgb;
			float4 color55 = IsGammaSpace() ? float4(0,1.223529,2.980392,0.007843138) : float4(0,1.55866,11.051,0.007843138);
			float3 ase_worldPos = i.worldPos;
			float2 appendResult80 = (float2(( ase_worldPos.x * 1 ) , ( ase_worldPos.z * 0.01 )));
			float2 panner37 = ( 1.0 * _Time.y * float2( 0,-0.5 ) + appendResult80);
			float4 lerpResult23 = lerp( float4( 0,0,0,0 ) , tex2D( _linear_gradient, panner37 ) , ( step( 0.0 , ase_worldPos.x ) * ( 1.0 - step( 0.0 , ( ase_worldPos.x - 0.2 ) ) ) ));
			o.Emission = ( color55 * lerpResult23 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17500
0;0;1920;1018;5139.504;672.9395;1;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;15;-4711.863,-414.9494;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;88;-3804.193,133.4741;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ScaleNode;78;-4190.319,-509.7688;Inherit;False;1;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;92;-3577.996,250.9489;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleNode;79;-4181.219,-392.7691;Inherit;False;0.01;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;80;-3919.917,-425.269;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StepOpNode;97;-3376.777,327.6794;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;90;-3251.01,356.4734;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;37;-3746.266,-359.2753;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StepOpNode;95;-3432.981,36.5683;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;59;-3567.993,-387.6833;Inherit;True;Property;_linear_gradient;linear_gradient;2;0;Create;True;0;0;False;0;-1;580f4df698c4ecb46b965f736c77bcb4;580f4df698c4ecb46b965f736c77bcb4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-3086.59,149.162;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;23;-2990.4,-187.9699;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;55;-2761.57,-350.9442;Inherit;False;Constant;_Color0;Color 0;3;1;[HDR];Create;True;0;0;False;0;0,1.223529,2.980392,0.007843138;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;44;-6208.546,-611.7552;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;89;-3457.285,572.3702;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;25;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;58;-3895.72,33.00777;Inherit;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;75;-4391.592,-164.9615;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;46;-5869.173,-623.6459;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-5883.477,-362.9063;Inherit;True;Property;_Float7;Float 7;1;0;Create;True;0;0;False;0;0.4297664;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;98;-1589.707,-194.0417;Inherit;False;Constant;_Color1;Color 1;3;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;76;-4515.729,-629.8376;Inherit;False;Constant;_Float8;Float 8;3;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;74;-4728.294,-738.2191;Inherit;False;Constant;_Vector1;Vector 1;3;0;Create;True;0;0;False;0;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexCoordVertexDataNode;81;-4064.969,213.8019;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-2485.967,-172.9442;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-6218.667,-296.2315;Inherit;False;Property;_Float6;Float 6;0;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;45;-5594.121,-590.1799;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-1331.11,-62.53342;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Faisseaux;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;78;0;15;1
WireConnection;92;0;88;1
WireConnection;79;0;15;3
WireConnection;80;0;78;0
WireConnection;80;1;79;0
WireConnection;97;1;92;0
WireConnection;90;0;97;0
WireConnection;37;0;80;0
WireConnection;95;1;88;1
WireConnection;59;1;37;0
WireConnection;93;0;95;0
WireConnection;93;1;90;0
WireConnection;23;1;59;0
WireConnection;23;2;93;0
WireConnection;89;0;88;3
WireConnection;58;0;15;0
WireConnection;46;0;42;0
WireConnection;46;1;44;2
WireConnection;54;0;55;0
WireConnection;54;1;23;0
WireConnection;45;0;46;0
WireConnection;45;1;43;0
WireConnection;0;0;98;0
WireConnection;0;2;54;0
ASEEND*/
//CHKSM=3B26F4F91F2649444380AFFA1207DBFA71F899FF