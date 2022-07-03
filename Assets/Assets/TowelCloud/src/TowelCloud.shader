/*
	=================================================
	TowelCloud

	Copyright (c) 2020-2022 towel_funnel

	This software is released under the Zlib License.
	https://opensource.org/licenses/zlib-license

	このコードはZlibライセンスです
	https://ja.wikipedia.org/wiki/Zlib_License
	=================================================

	ライセンスに関する補足（以下はライセンス文ではありません）
	このコードはとくに改変をしない場合、商用非商用問わず自由に使うことができます
	改変する場合の注意事項は上記Wikipediaを参考にしてください
	利用した空間内で著作者表示をする義務はありません
	もし表示する場合はshader名「TowelCloud」 作者名「towel_funnel」としてください
*/

Shader "Towel/TowelCloud"
{
	Properties
	{
			_noiseMap ("noiseMap ノイズ画像", 2D) = "white" {}
		// dev0
		// dev0
			_scale ("scale 雲の大きさ", Float) = 80
			_cloudy ("cloudy 曇り度合い", Range (0, 1)) = 0.4
			_soft ("soft 雲の柔らかさ", Range (0.0001, 0.9999)) = 0.2
		[Header(sky)]
			_skyMap ("skyMap 空の色", 2D) = "white" {}
			[Toggle]_skyTransparent ("skyTransparent 空を透過する", Float) = 0
			_rotateY ("rotateY 空の前方の角度", Range (-180, 180)) = 0
			_rotateZ ("rotateZ 光の上下角度", Range (-180, 180)) = 60
		[Header(cloud)]
			_cloudMap ("cloudMap 雲の色", 2D) = "white" {}
			_farMixRate ("farMixRate 水平線近くで空の色を雲に反映する割合", Range (0, 1)) = 0.6
			_farMixLength ("farMixLength 水平線近くで空の色を雲に反映する距離", Range (0, 1)) = 0.3
			_cloudFogColor("cloudFogColor 水平線近くで雲に反映する単色", Color) = (1, 1, 1, 0.1)
			_cloudFogLength("cloudFogLength 単色のグラデーション幅", Range (0, 1)) = 0.3
		[Header(horizon)]
			[Toggle]_yMirror ("yMirror 下方向をミラーする", Float) = 0
			[Toggle]_underFade ("underFade 下の方の雲を消す", Float) = 1
			_underFadeStart ("underFadeStart 消し始める位置", Range (-1, 1)) = -0.5
			_underFadeWidth ("underFadeWidth 消すときのグラデーション幅", Range (0.0001, 0.9999)) = 0.2
			[Toggle]_groundFill ("groundFill 下方向を塗りつぶす", Float) = 0
			_groundFillColor ("groundFillColor 下方向を塗りつぶす色", Color) = (0.13, 0.11, 0.1, 1)
		[Header(move)]
			_moveRotation ("moveRotation 雲の移動方向", Range (0, 360)) = 0
			_speed_parameter ("speed 雲の速度", Float) = 1
			_shapeSpeed_parameter ("shapeSpeed 雲の変形量", Float) = 1
			_speedOffset ("speedOffset 雲の細かい部分の速度差", Float) = 0.2
			_speedSlide ("speedSlide 雲の細かい部分の横方向への速度", Float) = 0.1
		[Header(rim)]
			_rimMap ("rimMap 雲のふちの色", 2D) = "white" {}
			_rimForce ("rimForce ふちの光の強さ", Float) = 0.5
			_rimNarrow ("rimNarrow ふちの光の細さ", Float) = 2
		[Header(scattering)]
			[Toggle] _scattering ("scattering 拡散光を使う", Float) = 0
			_scatteringColor ("scatteringColor 拡散光の色", Color) = (1, 1, 1, 1)
			_scatteringForce ("scatteringForce 拡散光の強さ", Range (0, 3)) = 0.8
			_scatteringRange ("scatteringRange 拡散光の影響を受ける範囲", Range (0, 1)) = 0.3
			_scatteringNarrow ("scatteringNarrow 拡散光のふちの細さ", Float) = 1
		[Header(faceWind)]
			_faceWindScale_parameter ("faceWindScale 表面風の大きさ", Float) = 1
			_faceWindForce_parameter ("faceWindForce 表面風の強さ", Float) = 1
			_faceWindMove ("faceWindMove 表面風の移動速度", Float) = 1.3
			_faceWindMoveSlide ("faceWindMoveSlide 表面風の細かい部分の移動速度", Float) = 1.8
		[Header(farWind)]
			_farWindDivision ("farWindDivision 遠方風の分割数", Int) = 35
			_farWindForce_parameter ("farWindForce 遠方風の強さ", Float) = 1
			_farWindMove ("farWindMove 遠方風の移動速度", Float) = 2
			_farWindTopEnd ("farWindTopEnd 遠方風の上の消える位置", Float) = 0.5
			_farWindTopStart ("farWindTopStart 遠方風の上の弱まり始める位置", Float) = 0.3
			_farWindBottomStart ("farWindBottomStart 遠方風の下の弱まり始める位置", Float) = 0.1
			_farWindBottomEnd ("farWindBottomEnd 遠方風の下の消える位置", Float) = -0.1
		[Header(stream)]
			[Toggle] _stream ("stream 気流", Float) = 1
			_streamForce ("streamForce 気流の強さ", Float) = 5
			_streamScale ("streamScale 気流の大きさ", Float) = 5
			_streamMove ("streamMove 気流の移動速度", Float) = 1.5
		[Header(etc)]
			_fbmScaleUnder ("fbmScaleUnder 雲の細かい部分の変形値", Float) = 0.43
			_boost ("boost 雲の光を強める値", Float) = 1.1
			_chine ("chine 雲の尾根のやわらかさ", Float) = 0.5
			_alphaRate ("alphaRate 全体の透明度", Range (0, 1)) = 1
	}
	SubShader
	{
	// ==== head ====
		Tags
		{
			"RenderType"="Background"
			"Queue"="Transparent-100"
			"PreviewType"="SkyBox"
		}
		ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha
	// ==== pass ====
		Pass
		{
		// ==== head ====
			CGPROGRAM
			#pragma shader_feature_local _STREAM_ON
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "SimplexNoise3D.hlsl"
			#include "Quaternion.hlsl"
			#include "Easing.hlsl"
			// 定数
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
		// dev1
			uniform sampler2D _noiseMap;
			uniform float _scale;
			uniform float _cloudy;
			uniform float _soft;
			// sky
			uniform sampler2D _skyMap;
			uniform float _skyTransparent;
			uniform float _rotateY;
			static const float _rotateYOffset = 0;
			uniform float _rotateZ;
			static const float _rotateZOffset = 0;
			// cloud
			uniform sampler2D _cloudMap;
			uniform float _farMixRate;
			uniform float _farMixLength;
			uniform float4 _cloudFogColor;
			uniform float _cloudFogLength;
			// horizon
			uniform float _yMirror;
			uniform float _underFade;
			uniform float _underFadeStart;
			uniform float _underFadeWidth;
			uniform float _groundFill;
			uniform float4 _groundFillColor;
			// move
			uniform float _moveRotation;
			uniform float _speed_parameter;
			uniform float _shapeSpeed_parameter;
			uniform float _speedOffset;
			uniform float _speedSlide;
			// rim
			uniform sampler2D _rimMap;
			uniform float _rimForce;
			uniform float _rimNarrow;
			// scattering
			uniform float _scattering;
			uniform float4 _scatteringColor;
			uniform float _scatteringForce;
			uniform float _scatteringPassRate;
			uniform float _scatteringRange;
			uniform float _scatteringNarrow;
			// faceWind
			uniform float _faceWindScale_parameter;
			uniform float _faceWindForce_parameter;
			uniform float _faceWindMove;
			uniform float _faceWindMoveSlide;
			// farWind
			uniform int _farWindDivision;
			uniform float _farWindForce_parameter;
			uniform float _farWindMove;
			uniform float _farWindTopEnd;
			uniform float _farWindTopStart;
			uniform float _farWindBottomStart;
			uniform float _farWindBottomEnd;
			// stream
			uniform float _streamScale;
			uniform float _streamForce;
			uniform float _streamMove;
			// etc
			uniform float _fbmScaleUnder;
			uniform float _boost;
			uniform float _chine;
			uniform float _alphaRate;
		// ==== 調節定数====
			static const float _speed_base = 0.1;
			static const float _shapeSpeed_base = 0.1;
			static const float _faceWindScale_base = 0.015;
			static const float _faceWindForce_base = 0.15;
			static const float _farWindForce_base = 0.0012;
		// ==== var ====
			static const int noiseLoop = 5; // ノイズのループ数、高いと品質が上がるが処理が重くなる
			static const float km = 1000; // 値が大きすぎるのでひとまずkm表示
			static const float planetR_km = 6000;// 惑星半径（km表示）
			static const float cloudHeight_km = 10;// 雲の生成される地上からの高さ
			static const float adjustRate_km = 15;// 雲の調節処理が行われる基準値（低いほど細かく行われるが、非連続になる）
			static const float adjustOffset = 1; // 雲の調節処理を行わない回数（高いほど近くの雲で行わなくなる）
			static const float adjustMax = 4; // 雲の調節処理の最大数（高いと水平線以下に縞模様が出現、低いと水平線近くが細かくなってしまう）
			static const float scaleBase = 10; // 大きさの解像度ベースの変更
		// ==== 数学関数 ====
			inline float remap(float value, float minOld, float maxOld, float minNew, float maxNew)
			{
				float rangeOld = (maxOld - minOld);
				if (rangeOld == 0)	// ゼロ除算回避
				{
					return minNew;
				}
				return minNew + (value - minOld) * (maxNew - minNew) / rangeOld;
			}
			#define SRT_EPSILON 1.0e-4
			inline float4 alphaBlend(float4 back, float4 front)
			{
				if(back.a < SRT_EPSILON)
				{
					// 0除算しないように、背面の色が透明の場合は前面の色を使用する
					return front;
				}
				else
				{
					float4 result = 0;
					result.a = front.a + back.a * (1 - front.a);
					result.rgb = (front.rgb * front.a + back.rgb * back.a * (1 - front.a)) / result.a;
					return saturate(result);
				}
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
			
			// ノイズを組み合わせる
			float4 createFbm (float3 coord, float totalScale, float3 totalOffset, float3 fbmOffset, float3 speed, float fbmAdjust)
			{
				// 変数宣言
				float3 offset;
				// fbm変動量
				float swingRate = 2;
				// fbm変動値
				float swing = 1;
				float nowSwing;
				float smallness = 1; // reScale
				// 振幅合計
				float totalSwing = 0;
				// 引数整理
				float smallnessRate = _fbmScaleUnder;
				float totalSmallness = 1 / totalScale * pow(smallnessRate, -noiseLoop);
				if (totalScale == 0)	// ゼロ除算回避
				{
					totalSmallness = 0;
				}
				// ノイズ本体
				float4 noise = float4(0, 0, 0, 0);
				float4 nowNoise;
				// 前処理
				float baseOffset = totalScale * scaleBase;
				coord.xz += totalOffset;
				fbmOffset /= noiseLoop;
				// adjast
				float adjustBase = floor(fbmAdjust);
				float adjustSmallRate = frac(fbmAdjust);
				float adjustLargeRate = 1 - adjustSmallRate;
				smallness = pow(smallnessRate, adjustBase);
				// 最初のループ値の打ち消し
				swing /= swingRate;
				smallness /= smallnessRate;
				// fbmを生成
				for (int loopIndex = 0; loopIndex < noiseLoop; loopIndex++)
				{
					float adjustIndex = adjustBase + loopIndex;
					// ループ値
					swing *= swingRate;
					nowSwing = swing;
					if (loopIndex == 0)
					{
						nowSwing *= adjustLargeRate;
					}
					if (loopIndex == noiseLoop - 1)
					{
						nowSwing *= adjustSmallRate;
					}
					smallness *= smallnessRate;
					// ノイズの生成
					offset = -fbmOffset * length(speed.xz) * (adjustIndex - noiseLoop) + speed + baseOffset;
					// 2d
					nowNoise = snoise3d_grad((coord + offset) * smallness * totalSmallness, _chine);
					// noiseの最大値に近い場合頂点の近くにあると推測する。値に応じてViewDirの値を補完で強めて合成する
					noise += nowNoise * nowSwing;
					// 振幅合計を記録
					totalSwing += nowSwing;
				}
				// 合計振幅で減衰させる
				noise /= totalSwing;
				// 
				return noise;
			}
			
			inline float revertOrZero (float value)
			{
				float revert = 1 / value;
				if (value == 0) return 0;	// ゼロ除算回避
				return revert;
			}
			
			// １段階のみのノイズ
			inline float4 createSingle (float3 coord, float totalScale, float3 totalOffset, float3 speed)
			{
				float totalSmallness = revertOrZero(totalScale);
				// ノイズ本体
				return snoise3d_grad((coord + speed + totalScale * scaleBase) * totalSmallness, 0.5);	// この0.5は勘
			}
			
			// 2Dテクスチャを使ったノイズ
			inline float4 createTextureNoise (float2 coord, float totalScale, float2 speed)
			{
				float totalSmallness = revertOrZero(totalScale);
				// ノイズ本体
				return tex2D(_noiseMap, (coord + speed + totalScale * scaleBase) * totalSmallness);
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
			// == パラメータ調節
				float _speed = _speed_base * _speed_parameter;
				float _shapeSpeed = _shapeSpeed_base * _shapeSpeed_parameter;
				float _faceWindScale = _faceWindScale_base * _faceWindScale_parameter;
				float _faceWindForce = _faceWindForce_base * _faceWindForce_parameter;
				float _farWindForce = _farWindForce_base * _farWindForce_parameter;
			// == 前処理
				float planetR = planetR_km * km;
				float cloudHeight = cloudHeight_km * km;
				float adjustRate = adjustRate_km * km;
				float3 viewDir = normalize(createReViewDir(worldPos));
				float3 viewDirOrigin = viewDir;
				if (_yMirror == 1 && 0 < viewDir.y)
				{
					viewDir.y *= -1;
				}
			// == 横方向グリッドを計算
				float farWindVerticalRate = 1.0 / 6.0;
				// 横分割
				float sideAngle = atan2(viewDir.x, viewDir.z) / (PI * 2);
				float farGridX = frac(sideAngle * _farWindDivision);
				// 縦分割
				float farGridY = frac(viewDir.y * _farWindDivision * farWindVerticalRate);
				// 反映比率
				float2 farGrid = float2(farGridX, farGridY);
				float farRateBase = -viewDir.y;
				float farTopStep = step(_farWindTopStart, farRateBase);
				float farGridRate = saturate(farTopStep * (farRateBase - _farWindTopEnd) / (_farWindTopStart - _farWindTopEnd) +
					(1 - farTopStep) * (farRateBase - _farWindBottomEnd) / (_farWindBottomStart - _farWindBottomEnd));
				
			// == 遠い空を角度スペースでズラす
				float farWindMove = 0.03 * _farWindMove;
				float4 farWindNoise = createTextureNoise(farGrid * 2, 1, float2(_Time.y * farWindMove, _Time.y * farWindMove)) * 2 - 1;
				float3 farSlide = normalize(farWindNoise.xyz) * farGridRate * _farWindForce;
				viewDir += farSlide;
			// == 前処理2
				float3 reViewDir = -viewDir;
				float vy = reViewDir.y;
				float totalR = cloudHeight + planetR;
				float topRate = asin(clamp(reViewDir.y, -1, 1)) * 2 / PI;
			// == 半円天球を計算
				// 視点を半球まで伸ばすまでの比率の計算
				float viewDistance = sqrt(totalR * totalR - (1 - vy * vy) * (planetR * planetR)) - vy * planetR;
				// 座標の計算
				float3 ovalCoord = reViewDir * viewDistance;
				// 距離の算出
				float ovalCoodLength = length(ovalCoord);
				float adjustBase = pow(ovalCoodLength / adjustRate, 0.55); // sqrtはちょうどよいカーブを作るためであり、ルートの意味は無い
				// オクターブ調節値
				// adjustBase = min(adjustBase, 10);
				adjustBase = clamp(adjustBase - adjustOffset, 0, adjustMax);
			// == ノイズの生成
				// オフセット値を計算
				float4 moveQuaternion =  rotate_angle_axis(_moveRotation * PI / 180, float3(0, 1, 0));
				float3 fbmOffset = float3(_speedOffset, 0, _speedSlide);
				float3 speed = _Time.y * float3(_speed, _shapeSpeed, 0) * km;
				speed = rotate_vector(speed, moveQuaternion);
				fbmOffset = rotate_vector(fbmOffset, moveQuaternion);
			// == 表面風を作成
				float2 faceWindSpeed = speed.xz * _faceWindMove;
				float2 faceWindSpeedSlide = speed.xz * _faceWindMoveSlide;
				float4 faceWindNoise = createTextureNoise(ovalCoord.xz, _faceWindScale * 2 * _scale * km, faceWindSpeedSlide);
				float4 faceWindNoise2 = createTextureNoise(ovalCoord.xz, _faceWindScale * _scale * km, faceWindSpeed);
				// 乱流値から座標をずらす
				float faceWindOctaveRate = 1.9;
				float3 slide = normalize(faceWindNoise.xyz + faceWindNoise2.xyz * faceWindOctaveRate);
				ovalCoord += faceWindNoise.xyz * _faceWindForce * faceWindOctaveRate * km;
				ovalCoord += slide * _faceWindForce * km;
			// == 乱流を作成
				#ifdef _STREAM_ON
					float3 streamSpeed = speed * _streamMove;
					float4 streamNoise = createSingle(ovalCoord, _streamScale * _scale * km, 1 * km, streamSpeed);
					// 乱流値から座標をずらす
					ovalCoord += streamNoise.xyz * _streamForce * km;
				#endif
			// == 実ノイズを生成;
				float4 noise = createFbm(ovalCoord, _scale * km, 1 * km, fbmOffset, speed, adjustBase);
				// ノイズの最後が強度の値になる
				float cloudNoisePower = clamp(noise.w,-1, 1) * 0.5 + 0.5;
				// 水平線処理
				if (_underFade == 1)
				{
					float fadeMax = (1 - _underFadeStart) * _underFadeWidth + _underFadeStart;
					float fadeRate = remap(topRate, _underFadeStart, fadeMax, 0, 1);
					cloudNoisePower *= saturate(fadeRate);
				}
			// == ソフト値と曇度から濃さを算出
				float soft2 = _soft * _soft;	// 感度の調節
				float cloudSoftUnder = 1 - _cloudy - soft2 * 1;
				float cloudSoftTop = cloudSoftUnder + soft2 * 2;
				float cloudPower = saturate(remap(cloudNoisePower, cloudSoftUnder, cloudSoftTop, 0, 1));
				cloudPower = cubicInOut(saturate(cloudPower));
				float cloudAreaRate = saturate(remap(cloudNoisePower, cloudSoftUnder, 1, 0, 1));
				// ノーマル方向をフォース空間から求める
				// フォース空間の求める先が雲の中央と仮定し、その逆方向のノーマライズを雲表面とすることができる
				// ただし、求まる値は大気面の切り取られた値なので、雲の濃い部分はより正面向きであるという補正が必要になる
				float3 cloudWorldNormal = -noise.xyz;
			// == ノーマルの方向を調節
				// 円の中心（星の中心）からのベクトル
				float3 earthCenterDir = ovalCoord;
				earthCenterDir.y += cloudHeight * 3; // この値は若干不完全
				earthCenterDir = normalize(earthCenterDir);
				// 必要なクオタニオンの用意
				float3 underVector = float3(0, -1, 0);
				float4 viewQuaternion = from_to_rotation(underVector, earthCenterDir);
				float4 viewQuaternionR = q_inverse(viewQuaternion);
				// 雲の存在する範囲の端を丸める処理
				float quadAreaRate = cloudAreaRate;
				quadAreaRate = quadOut(saturate(quadAreaRate));
				cloudWorldNormal.xz *= -1;
				cloudWorldNormal = rotate_vector(cloudWorldNormal, viewQuaternion);
				cloudWorldNormal.y = cloudAreaRate;
				cloudWorldNormal = rotate_vector(cloudWorldNormal, viewQuaternionR);
				cloudWorldNormal.xz *= -1;
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
				float normalDot = dot(cloudWorldNormal, lightVector);
				float normalDotForUv = normalDot * 0.5 + 0.5;
				float viewDotToLightForUv = dot(viewDir, lightVector) * 0.5 + 0.5;
				float viewDotToSkyForUv = dot(viewDir, skyVector) * 0.5 + 0.5;
			// == 雲の色を確定
				float2 cloudUv = float2(viewDotToLightForUv, normalDotForUv);
				float4 cloudColor = tex2D(_cloudMap, cloudUv);
			// == 空の色を用意
				float2 skyUv = float2(viewDotToSkyForUv, saturate(topRate));
				float4 skyColor = tex2D(_skyMap, skyUv);
				if (_skyTransparent == 1)
				{
					skyColor.a = 0;
				}
			// == 境界が光る度合いを用意
				float rimPowerR = cloudAreaRate * _rimNarrow;
				rimPowerR = quadOut(saturate(rimPowerR));
				float rimPower = (1 - rimPowerR) * _rimForce;
				rimPower = saturate(rimPower);
				// 境界の光の色を設定
				float2 rimUv = float2(viewDotToLightForUv, normalDotForUv);
				float3 rimColor =  tex2D(_rimMap, rimUv);
				cloudColor.rgb =  rimColor.rgb * rimPower + cloudColor.rgb;
			// == 透過拡散光
				float scatteringPower = 0;
				if (_scattering)
				{
					float scatteringPowerR = cloudAreaRate * _scatteringNarrow;
					scatteringPowerR = quadOut(saturate(scatteringPowerR));
					float scatteringPower = (1 - scatteringPowerR) * _scatteringForce;
					// 範囲を限定する
					float scatteringPowerRateRaw = saturate((_scatteringRange - viewDotToLightForUv) / _scatteringRange);
					if (_scatteringRange == 0)	// ゼロ除算回避
					{
						scatteringPowerRateRaw = 0;
					}
					// 境界を丸く
					scatteringPower *= quadIn(scatteringPowerRateRaw);
					scatteringPower = saturate(scatteringPower);
					// 合成
					cloudColor.rgb += scatteringPower * _scatteringColor * _scatteringForce;
				}
			// == 水平線近くの空の色が混ざる処理
				float skyMixRate = saturate(remap(topRate, 0, _farMixLength, _farMixRate, 0));
				if (_farMixLength == 0) skyMixRate = 0;
				cloudColor.rgb = cloudColor.rgb * (1 - skyMixRate) + skyColor.rgb * skyMixRate;
			// == 水平線近くの単色が混ざる処理
				float fogRate = saturate(remap(topRate, 0, _cloudFogLength, _cloudFogColor.a, 0));
				if (_cloudFogLength == 0) fogRate = 0;
				cloudColor.rgb = cloudColor.rgb * (1 - fogRate) + _cloudFogColor.rgb * fogRate;
				
			// == 最終合成
				// まずはそれぞれのアルファをまとめる、
				float cloudAlpha = saturate(cloudColor.a * cloudPower);
				float skyAlpha = skyColor.a;
				// 全体のカラーは、Back * (1 - skyAlpha) * (1 - cloudAlpha) + Sky * skyAlpha * (1 - cloudAlpha) + Cloud * cloudAlpha となる
				// このうちBackの部分は自動計算されるので、求めるのはshaderで扱う範囲の最終色とアルファ
				float cloudColorRate = cloudAlpha;
				float skyColorRate = skyAlpha * (1 - cloudAlpha);
				float totalAlpha = cloudColorRate + skyColorRate;
				float inShaderCloudColorRate = cloudColorRate / totalAlpha;
				if (totalAlpha == 0)	// ゼロ除算回避
				{
					inShaderCloudColorRate = 0;
				}
				// 色の足し合わせ
			 	float3 totalRgb = cloudColor.rgb * inShaderCloudColorRate + skyColor.rgb * (1 - inShaderCloudColorRate);
			// == 反映
				float4 endColor = float4(0, 0, 0, 1);
				endColor.rgb = totalRgb;
				endColor.rgb *= _boost;
				endColor.a = totalAlpha * _alphaRate;
			// == 地面の塗りつぶし
				float whitespace = 0.1; // 適当
				float groundFillRate = saturate(remap(viewDirOrigin.y, whitespace, 1, 0, 1));
				groundFillRate = cubicOut(cubicOut(groundFillRate));
				float4 groundFillColor = _groundFillColor;
				groundFillColor.a *= groundFillRate * _groundFill;	// 0ならキャンセル
				endColor = alphaBlend(endColor, groundFillColor);
			// dev2
			// dev2
				
			// == end
				return endColor;
			}
			ENDCG
		}
	}
}