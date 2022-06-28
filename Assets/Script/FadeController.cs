using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{

	int Wid=0, Hei=0;
	float fadeSpeed = 0.02f;        //�����x���ς��X�s�[�h���Ǘ�
	float red, green, blue, alfa;   //�p�l���̐F�A�s�����x���Ǘ�

	public static bool isFadeOut = false;  //�t�F�[�h�A�E�g�����̊J�n�A�������Ǘ�����t���O
	public static bool isFadeIn = false;   //�t�F�[�h�C�������̊J�n�A�������Ǘ�����t���O

	Image fadeImage;                //�����x��ύX����p�l���̃C���[�W

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

		if (RankImg)//�T�C�Y�ύX���s��
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
		alfa -= fadeSpeed;                //a)�s�����x�����X�ɉ�����
		SetAlpha();                      //b)�ύX�����s�����x�p�l���ɔ��f����
		if (alfa <= 0)
		{                    //c)���S�ɓ����ɂȂ����珈���𔲂���
			isFadeIn = false;
			fadeImage.enabled = false;    //d)�p�l���̕\�����I�t�ɂ���
		}
	}

	void StartFadeOut()
	{
		fadeImage.enabled = true;  // a)�p�l���̕\�����I���ɂ���
		alfa += fadeSpeed;         // b)�s�����x�����X�ɂ�����
		SetAlpha();               // c)�ύX���������x���p�l���ɔ��f����
		if (alfa >= 1)
		{             // d)���S�ɕs�����ɂȂ����珈���𔲂���
			isFadeOut = false;
		}
	}

	void SetAlpha()
	{
		fadeImage.color = new Color(red, green, blue, alfa);
	}
}
