using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // 追加しましょう

public class ScoreManager : MonoBehaviour
{
    public Text[] score_text = new Text[5]; // Textオブジェクト
    public static int[] Ranking = new int[5];//取得用
    public int[] SaveRanking = new int[5];//保存用
    public static string[] Name = new string[5];//取得用
    public string[] SaveName = new string[5];//保存用

    public static bool FirstStart=false;//開始時はUpdateしない

    // 初期化時の処理
    void Start()
    {
        // スコアのロード
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
    // 削除時の処理
    void OnDestroy()
    {
        // スコアを保存
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt("SCORE" + i, SaveRanking[i]);
            PlayerPrefs.SetString("NAME" + i, SaveName[i]);
        }
        PlayerPrefs.Save();
    }

    // 更新
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