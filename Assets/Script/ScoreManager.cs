using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // �ǉ����܂��傤

public class ScoreManager : MonoBehaviour
{
    public Text[] score_text = new Text[5]; // Text�I�u�W�F�N�g
    public static int[] Ranking = new int[5];//�擾�p
    public int[] SaveRanking = new int[5];//�ۑ��p
    public static string[] Name = new string[5];//�擾�p
    public string[] SaveName = new string[5];//�ۑ��p

    public static bool FirstStart=false;//�J�n����Update���Ȃ�

    // ���������̏���
    void Start()
    {
        // �X�R�A�̃��[�h
        for (int i = 0; i < 5; i++)
        {
            SaveRanking[i] = PlayerPrefs.GetInt("SCORE" + i, 0);
            SaveName[i] = PlayerPrefs.GetString("NAME" + i, "");
        }
        if (!FirstStart)
            for (int i = 0; i < 5; i++)
            {
                Ranking[i] = SaveRanking[i];
                Name[i] = SaveName[i];
            }

    }
    // �폜���̏���
    void OnDestroy()
    {
        // �X�R�A��ۑ�
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt("SCORE" + i, SaveRanking[i]);
            PlayerPrefs.SetString("NAME" + i, SaveName[i]);
        }
        PlayerPrefs.Save();
    }

    // �X�V
    void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            SaveRanking[i] = Ranking[i];
            SaveName[i] = Name[i];
        }

        score_text[0].text = "1st : " + SaveRanking[0] +"\n  "+ SaveName[0];
        score_text[1].text = "2nd : " + SaveRanking[1] + "\n  " + SaveName[1];
        score_text[2].text = "3rd : " + SaveRanking[2] + "\n  " + SaveName[2];
        score_text[3].text = "4th : " + SaveRanking[3] + "\n  " + SaveName[3];
        score_text[4].text = "5th : " + SaveRanking[4] + "\n  " + SaveName[4];

    }
}