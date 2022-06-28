using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{

	int Wid=0, Hei=0;
	float fadeSpeed = 0.02f;        //透明度が変わるスピードを管理
	float red, green, blue, alfa;   //パネルの色、不透明度を管理

	public static bool isFadeOut = false;  //フェードアウト処理の開始、完了を管理するフラグ
	public static bool isFadeIn = false;   //フェードイン処理の開始、完了を管理するフラグ

	Image fadeImage;                //透明度を変更するパネルのイメージ

	public bool RankImg = false;
	public static bool FadeoutStart = false;

	void Start()
	{	
		fadeImage = GetComponent<Image>();
		red = fadeImage.color.r;
		green = fadeImage.color.g;
		blue = fadeImage.color.b;
		alfa = fadeImage.color.a;
		if (!RankImg) isFadeIn = true;
	}

	
	void Update()
	{
		if (!RankImg&& FadeoutStart)
        {
			isFadeOut = true;
        }

		if (RankImg)//サイズ変更を行う
		{
			var targetSize = new Vector2(650 - Wid, 650 - Hei);
			GetComponent<RectTransform>().sizeDelta = targetSize;
		}

		if (isFadeIn)
		{
			StartFadeIn();
			if (RankImg && Wid > 0) Wid -= 4;
			if (RankImg && Hei > 0) Hei -= 4;
		}

		if (isFadeOut)
		{
			StartFadeOut();
			if (RankImg && Wid < 200) Wid+=4;
			if (RankImg && Hei < 200) Hei+=4;
		}
	}

	void StartFadeIn()
	{
		alfa -= fadeSpeed;                //a)不透明度を徐々に下げる
		SetAlpha();                      //b)変更した不透明度パネルに反映する
		if (alfa <= 0)
		{                    //c)完全に透明になったら処理を抜ける
			isFadeIn = false;
			fadeImage.enabled = false;    //d)パネルの表示をオフにする
		}
	}

	void StartFadeOut()
	{
		fadeImage.enabled = true;  // a)パネルの表示をオンにする
		alfa += fadeSpeed;         // b)不透明度を徐々にあげる
		SetAlpha();               // c)変更した透明度をパネルに反映する
		if (alfa >= 1)
		{             // d)完全に不透明になったら処理を抜ける
			isFadeOut = false;
		}
	}

	void SetAlpha()
	{
		fadeImage.color = new Color(red, green, blue, alfa);
	}
}
