// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "New Amplify Shader"
{
	Properties
	{
		_FlowMap("FlowMap", 2D) = "white" {}
		_Float0("Float 0", Float) = 1
		_Texture0("Texture 0", 2D) = "white" {}
		_Metalic("Metalic", Range( 0 , 1)) = 0
		_Rough("Rough", Range( 0 , 1)) = 0.9
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Texture0;
		uniform sampler2D _FlowMap;
		uniform float4 _FlowMap_ST;
		uniform float _Float0;
		uniform float _Metalic;
		uniform float _Rough;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_FlowMap = i.uv_texcoord * _FlowMap_ST.xy + _FlowMap_ST.zw;
			float2 blendOpSrc17 = i.uv_texcoord;
			float2 blendOpDest17 = (tex2D( _FlowMap, uv_FlowMap )).rg;
			float2 temp_output_17_0 = ( saturate( (( blendOpDest17 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest17 ) * ( 1.0 - blendOpSrc17 ) ) : ( 2.0 * blendOpDest17 * blendOpSrc17 ) ) ));
			float temp_output_9_0 = ( _Time.y * _Float0 );
			float temp_output_1_0_g5 = temp_output_9_0;
			float temp_output_11_0 = (0.0 + (( ( temp_output_1_0_g5 - floor( ( temp_output_1_0_g5 + 0.5 ) ) ) * 2 ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0));
			float TimeA18 = -temp_output_11_0;
			float2 lerpResult13 = lerp( i.uv_texcoord , temp_output_17_0 , TimeA18);
			float2 Tile27 = i.uv_texcoord;
			float2 FlowA20 = ( lerpResult13 + Tile27 );
			float temp_output_1_0_g4 = (temp_output_9_0*1.0 + 0.5);
			float TimeB35 = -(0.0 + (( ( temp_output_1_0_g4 - floor( ( temp_output_1_0_g4 + 0.5 ) ) ) * 2 ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0));
			float2 lerpResult36 = lerp( i.uv_texcoord , temp_output_17_0 , TimeB35);
			float2 FlowB39 = ( lerpResult36 + Tile27 );
			float TimeAlpha48 = saturate( abs( ( 1.0 - ( temp_output_11_0 / 0.5 ) ) ) );
			float4 lerpResult40 = lerp( tex2D( _Texture0, FlowA20 ) , tex2D( _Texture0, FlowB39 ) , TimeAlpha48);
			float4 Diffuse23 = lerpResult40;
			o.Albedo = Diffuse23.rgb;
			o.Metallic = _Metalic;
			o.Smoothness = _Rough;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17500
694;391;992;575;1824.76;1950.278;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;72;-3227.266,-756.9423;Inherit;False;2542.136;890.9028;Comment;17;8;7;9;31;10;32;33;11;34;12;35;18;43;45;46;47;48;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-3172.241,-561.1473;Inherit;False;Property;_Float0;Float 0;1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;7;-3177.266,-695.0416;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-2892.904,-627.8419;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;31;-2641.246,-297.0052;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;32;-2361.903,-322.9824;Inherit;True;Sawtooth Wave;-1;;4;289adb816c3ac6d489f255fc3caf5016;0;1;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;10;-2627.052,-656.7818;Inherit;True;Sawtooth Wave;-1;;5;289adb816c3ac6d489f255fc3caf5016;0;1;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;73;-3949.001,-2009.266;Inherit;False;2076.468;1095.315;Comment;13;14;15;16;17;37;19;29;13;36;38;28;20;39;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TFHCRemapNode;11;-2371.051,-673.3775;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;33;-2094.503,-368.2089;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;75;-3195.442,-2775.495;Inherit;False;819.7458;349.001;Comment;3;25;26;27;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;14;-3904.002,-1654.847;Inherit;True;Property;_FlowMap;FlowMap;0;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NegateNode;34;-1799.172,-370.1626;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;12;-2056.909,-690.6387;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;18;-1809.929,-706.9424;Inherit;True;TimeA;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;35;-1615.191,-376.4666;Inherit;True;TimeB;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-3719.937,-1959.12;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;-2914.725,-2725.494;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;15;-3513.157,-1641.195;Inherit;True;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-3300.798,-1400.818;Inherit;True;18;TimeA;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;27;-2618.696,-2725.495;Inherit;True;Tile;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;37;-3149.535,-1143.951;Inherit;True;35;TimeB;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;43;-1803.729,-126.639;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;17;-3193.96,-1709.702;Inherit;True;Overlay;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;45;-1553.13,-119.0401;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;36;-2827.544,-1231.246;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;29;-2650.742,-1471.109;Inherit;True;27;Tile;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;13;-2839.617,-1697.492;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;38;-2378.537,-1243.951;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-2416.238,-1628.25;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.AbsOpNode;46;-1341.33,-120.639;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;47;-1131.931,-119.2391;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;74;-1487.796,-1898.541;Inherit;False;1389.477;884.2649;Comment;8;42;22;30;2;41;49;40;23;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;20;-2119.418,-1682.773;Inherit;True;FlowA;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;39;-2115.537,-1235.951;Inherit;True;FlowB;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;22;-1419.318,-1617.64;Inherit;True;20;FlowA;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;30;-1437.796,-1848.541;Inherit;True;Property;_Texture0;Texture 0;3;0;Create;True;0;0;False;0;f528524ca8926d04c96114590e38a7b8;9e6628b6c9139bf4da3cf3e401d6558a;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;48;-928.1309,-125.0402;Inherit;True;TimeAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;42;-1416.506,-1369.665;Inherit;True;39;FlowB;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;49;-1008.758,-1244.277;Inherit;True;48;TimeAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;41;-1101.506,-1482.665;Inherit;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-1116.9,-1738.101;Inherit;True;Property;_Diffuse;Diffuse;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;40;-667.5058,-1701.665;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;23;-341.3196,-1686.616;Inherit;True;Diffuse;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;70;-166.3808,1052.255;Inherit;False;Property;_Emissive;Emissive;6;1;[HDR];Create;True;0;0;False;0;0,2.639865,4.170544,0;0,2.639865,4.170544,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;71;-99.96432,815.0978;Inherit;True;Property;_bite_lambert1_Emissive;bite_lambert1_Emissive;7;0;Create;True;0;0;False;0;-1;a933cab14951ebf428ac46d27329940a;a933cab14951ebf428ac46d27329940a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCGrayscale;68;-155.1808,1251.255;Inherit;True;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;662.3615,10.6897;Inherit;False;Property;_Rough;Rough;5;0;Create;True;0;0;False;0;0.9;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;24;668.5613,-396.1375;Inherit;True;23;Diffuse;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;79.81915,1199.255;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-3145.442,-2716.406;Inherit;True;Property;_Tile;Tile;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;660.5142,-67.93222;Inherit;False;Property;_Metalic;Metalic;4;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;992.3,-132.7;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;New Amplify Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;7;0
WireConnection;9;1;8;0
WireConnection;31;0;9;0
WireConnection;32;1;31;0
WireConnection;10;1;9;0
WireConnection;11;0;10;0
WireConnection;33;0;32;0
WireConnection;34;0;33;0
WireConnection;12;0;11;0
WireConnection;18;0;12;0
WireConnection;35;0;34;0
WireConnection;15;0;14;0
WireConnection;27;0;26;0
WireConnection;43;0;11;0
WireConnection;17;0;16;0
WireConnection;17;1;15;0
WireConnection;45;0;43;0
WireConnection;36;0;16;0
WireConnection;36;1;17;0
WireConnection;36;2;37;0
WireConnection;13;0;16;0
WireConnection;13;1;17;0
WireConnection;13;2;19;0
WireConnection;38;0;36;0
WireConnection;38;1;29;0
WireConnection;28;0;13;0
WireConnection;28;1;29;0
WireConnection;46;0;45;0
WireConnection;47;0;46;0
WireConnection;20;0;28;0
WireConnection;39;0;38;0
WireConnection;48;0;47;0
WireConnection;41;0;30;0
WireConnection;41;1;42;0
WireConnection;2;0;30;0
WireConnection;2;1;22;0
WireConnection;40;0;2;0
WireConnection;40;1;41;0
WireConnection;40;2;49;0
WireConnection;23;0;40;0
WireConnection;68;0;71;0
WireConnection;69;0;70;0
WireConnection;69;1;68;0
WireConnection;0;0;24;0
WireConnection;0;3;50;0
WireConnection;0;4;51;0
ASEEND*/
//CHKSM=3DCFBF8D53F7D0F75AD6078CC9F30E147AE86632