/*
	TowelSun
	
	TowelCloudと同時に使うことを想定している、簡易的な太陽と空の描画シェーダー

	Copyright (c) 2020 towel_funnel

	This software is released under the Zlib License.
	https://opensource.org/licenses/zlib-license

	このコードはZlibライセンスです。
	https://ja.wikipedia.org/wiki/Zlib_License

	このコードはとくに改変をしない場合自由に使うことができます
	改変する場合の注意事項は上記Wikipediaを参考にしてください

	VRCワールドなど、利用した空間内で著作者表示をする義務はありません
	ただし、可能な範囲で書いておいてもらえると、使おうと思った他の人の検索の助けになるので嬉しいです！
*/

Shader "Towel/TowelSun"
{
	Properties
	{
		// dev0
		[Toggle] _ForObject("オブジェクトとして配置する（skyboxにしない）", Float) = 0
		[Header(sky)]
		_skyMap ("空の色（上→空の上部, 左→空の前方）", 2D) = "white" {}
		_rotateY ("空の前方の角度（デフォルトはX方向）", Range (-180, 180)) = 0
		_rotateZ ("光の上下角度（デフォルトは水平）", Range (-180, 180)) = 60
		[Header(sun)]
		_sunColor ("太陽の色", Color) = (1, 1, 1, 1)
		_sunPower ("太陽の最大の力", Float) = 1.02
		_sunRange ("太陽の範囲", Float) = 0.02
		_sunAroundPower ("太陽周囲の光の強さ", Float) = 2
		_sunAroundRange ("太陽周囲の光の範囲", Float) = 0.08
		[Header(etc)]
		_boost ("雲の光を強める値（高いとブルームの影響を受ける）", Float) = 1
		_alphaRate ("全体の透明度", Range (0, 1)) = 1
	}
	SubShader
	{
	// ==== head ====
		Tags
		{
			"RenderType"="Background"
			"Queue"="Transparent-200"
			"PreviewType"="SkyBox"
		}
		
		ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha
	// 
		Pass
		{
		// ==== head ====
			Cull [_ForObject]
			CGPROGRAM
			#pragma shader_feature _FOROBJECT_ON
			#pragma shader_feature _TURBULENCE_ON
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "SimplexNoise3D.hlsl"
			#include "Quaternion.hlsl"
			#include "Easing.hlsl"
			
			#ifndef PI
			#define PI 3.14159265359f
			#endif
			
		// ==== セマンティクス ====
			struct appdata
			{
				float4 vertex : POSITION;
			};
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORD0;
			};
			
		// ==== パラメータ ====
			// dev1
			
			uniform sampler2D _skyMap;
			uniform float _rotateY;
			static const float _rotateYOffset = 0;
			uniform float _rotateZ;
			static const float _rotateZOffset = 0;
			
			uniform float4 _sunColor;
			uniform float _sunPower;
			uniform float _sunRange;
			uniform float _sunAroundPower;
			uniform float _sunAroundRange;
		
			uniform float _boost;
			uniform float _alphaRate;
			
		// ==== var ====
			static const int noiseLoop = 6; // ノイズのループ数、高いと品質が上がるが処理が重くなる
			static const float km = 1000; // 値が大きすぎるのでひとまずkm表示
			static const float planetR_km = 6000;// 惑星半径（km表示）
			static const float cloudHeight_km = 20;// 雲の生成される地上からの高さ
			static const float adjustRate_km = 15;// 雲の調節処理が行われる基準値（低いほど細かく行われるが、非連続になる）
			static const float adjustOffset = 1; // 雲の調節処理を行わない回数（高いほど近くの雲で行わなくなる）
			static const float adjustMax = 5; // 雲の調節処理の最大数（高いと水平線以下に縞模様が出現、低いと水平線近くが細かくなってしまう）
		// ==== 数学関数 ====
			inline float remap(float value, float minOld, float maxOld, float minNew, float maxNew)
			{
				return minNew + (value - minOld) * (maxNew - minNew) / (maxOld - minOld);
			}
		// ==== vertex ====
			// vert
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.worldPos.xyz = worldPos;
				o.worldPos.w = 0;

				return o;
			}
		// ==== fragment関数 ====
			// slerp
			float3 slerpFloat3(float3 start, float3 end, float rate)
			{
				float _dot = dot(start, end);
				clamp(_dot, -1, 1);
				float theta = acos(_dot) * rate;
				float3 relative = normalize(end - start * _dot);
				return (start * cos(theta)) + (relative * sin(theta));
			}
			
			// 視点から見る先へのベクトルの生成
			inline float3 createReViewDir (float3 worldPos)
			{
				float3 worldViewDir = UnityWorldSpaceViewDir(worldPos.xyz);
				worldViewDir = normalize(worldViewDir);
				return worldViewDir;
			}
		// ==== fragment ====
			// frag
			fixed4 frag (v2f i) : SV_Target
			{
			// == head
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				// 必要な書き下し
				float3 worldPos = i.worldPos.xyz;
				
			// == 前処理
				float3 viewDir = normalize(createReViewDir(worldPos));
				float3 reViewDir = -viewDir;
			// == 光の方向を回転する
				// 光の方向を用意、基準はX+方向
				float3 lightVector = float3(1, 0, 0);
				float3 skyVector = float3(1, 0, 0);
				float4 quaternionY =  rotate_angle_axis((_rotateY - _rotateYOffset) * PI / 180, float3(0, 1, 0));
				float4 quaternionZ =  rotate_angle_axis((_rotateZ - _rotateZOffset) * PI / 180, float3(0, 0, 1));
				lightVector = rotate_vector(lightVector, quaternionZ);
				lightVector = rotate_vector(lightVector, quaternionY);
				skyVector = rotate_vector(skyVector, quaternionY);
				// それぞれのdotから光影響を計算
				float viewDotToLightForUv = dot(viewDir, lightVector) * 0.5 + 0.5;
				float viewDotToSkyForUv = dot(viewDir, skyVector) * 0.5 + 0.5;
			// == 空の色を用意
				float2 skyUv = float2(viewDotToSkyForUv, clamp(reViewDir.y, 0, 1));
				float4 skyColor = tex2D(_skyMap, skyUv);
			// == 太陽
				// 基本値
				float sunPower = _sunPower;
				// 範囲を限定する
				sunPower *= quadIn(clamp((_sunRange - viewDotToLightForUv) / _sunRange, 0, 1));
				// 基本値
				float sunAroundPower = _sunAroundPower;
				// 範囲を限定する
				sunAroundPower *= quadIn(clamp((_sunAroundRange - viewDotToLightForUv) / _sunAroundRange, 0, 1));
			// == 最終合成
				float endAlpha = 1;
				float4 endColor = skyColor * _boost;
				endColor *= 1 + sunAroundPower;
				endColor += sunPower;
			// == アルファを使うかどうか分岐
				#ifdef _FOROBJECT_ON
					endColor.a = endAlpha * _alphaRate;
				#else
					endColor.a = endAlpha;
				#endif
				
				// dev2
			// == end
				return endColor;
			}
			ENDCG
		}
	}
}