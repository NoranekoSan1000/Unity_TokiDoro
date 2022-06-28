using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    public RectTransform[] MoveUI = new RectTransform[3];
    float [] Move = new float[3];//移動用
    float MoveTimer = 0;//
    int MoveCount = 0;//動かすオブジェクトナンバー

    public GameObject Button;
    public Image Rank;
    public Sprite[] RankSprite= new Sprite[7];

    public Text Text_1;
    public Text Text_2;
    public Text Text_3;
    public Text Text_4;
    public Text Text_5;
    public GameObject ScrollView;

    int LifeB, SindoB;
    public static int Total;

    public GameObject Back;

    public GameObject EndText_1;
    public GameObject EndText_2;
    public GameObject EndText_3;
    public GameObject EndText_4;
    public GameObject EndText_5;
    public GameObject SkipText;

    public Text[] ItemText = new Text[15];
    public Image[] ItemImage = new Image[15];
    public Sprite[] Itemsprite = new Sprite[15];
    public Sprite questionImage;

    int AllAmount = 0;

    public Text InputText;

    void Start()
    {
        MoveTimer = 0;
        MoveCount = 0;

        Application.targetFrameRate = 60;
        Back.SetActive(false);
        Text_1.text = "";
        Text_2.text = "";
        Text_3.text = "";
        Text_4.text = "";
        Text_5.text = "";
        ScrollView.SetActive(false);

        LifeB = (int)(Status.Score * (Status.Hp * 0.2f));
        SindoB = (int)((Status.OpenKeyLV + Status.ItemGetLV + Status.WatchLV - 3) * 5000);
        Total = Status.Score + LifeB + SindoB;

        Rank.enabled = false;
        Rank.sprite = RankSprite[0];

        AllAmount = 0;
        for (int i = 1; i <= 14; i++) if (Item.GetItem[i]) AllAmount += 1;

        ScoreManager.FirstStart = true;
        //ランキング
        for (int i = 0; i < 5; i++)
            {
                if (ScoreManager.Ranking[i] <= Total)
                {
                    for (int j = 3; j >= i; j--)
                    {
                        ScoreManager.Ranking[j+1] = ScoreManager.Ranking[j];
                    }
                    ScoreManager.Ranking[i] = Total;
                    break;
                }
            }
        InputText.text = "NoName";



    }

    int Score;
    void Update()
    {     
        for (int i = 0; i <= 13; i++)
        {
            if (Item.GetItem[i + 1]) ItemImage[i].sprite = Itemsprite[i];
            else ItemImage[i].sprite = questionImage;
        }

        if (Item.GetItem[1]) ItemText[0].text = "普通の壺";
        if (Item.GetItem[2]) ItemText[1].text = "トパーズの指輪";
        if (Item.GetItem[3]) ItemText[2].text = "普通のティーセット";
        if (Item.GetItem[4]) ItemText[3].text = "ルビーの指輪";
        if (Item.GetItem[5]) ItemText[4].text = "金のインゴットの山";
        if (Item.GetItem[6]) ItemText[5].text = "金のインゴット";
        if (Item.GetItem[7]) ItemText[6].text = "トワイライト\nサファイア";
        if (Item.GetItem[8]) ItemText[7].text = "偽物のトワイライト\nサファイア";
        if (Item.GetItem[9]) ItemText[8].text = "上質なワイン";
        if (Item.GetItem[10]) ItemText[9].text = "高価な絵画";
        if (Item.GetItem[11]) ItemText[10].text = "よくある絵画";
        if (Item.GetItem[12]) ItemText[11].text = "世界に\nひとつだけの花";
        if (Item.GetItem[13]) ItemText[12].text = "ごみ";
        if (Item.GetItem[14]) ItemText[13].text = "樽いっぱいの\nワイン";

        MoveTimer += 1.0f;
        if (MoveTimer > 60 && MoveCount < 9)
        {
            MoveCount += 1;
            MoveTimer = 0;
        }

        for (int i = 0; i < 3; i++)
        {
            if (MoveCount == i) Move[i] = 40.0f;
            else Move[i] = 0f;
        }
        MoveUI[0].position -= new Vector3(Move[0], 0, 0);
        MoveUI[1].position += new Vector3(Move[1], 0, 0);
        MoveUI[2].position -= new Vector3(Move[2], 0, 0);
        if (MoveCount == 3) Text_1.text = "盗品の総額　" + Status.Score + " $";
        if (MoveCount == 4) Text_2.text = "ライフボーナス　" + LifeB;
        if (MoveCount == 5) Text_3.text = "進度ボーナス　" + SindoB;
        if (MoveCount == 6) 
        {
            Text_5.text = "入手アイテム　" + AllAmount +" / " +14 ;
            ScrollView.SetActive(true);
        }
        if (MoveCount == 7) Text_4.text = "Total Score　" + Total;
        if (MoveCount == 8) FadeController.isFadeOut = true;
        if (MoveCount == 9) Button.SetActive(true);

        if (0 <= Total && Total < 7000) Rank.sprite = RankSprite[0];
        if (7000 <= Total && Total < 30000) Rank.sprite = RankSprite[1];
        if (30000 <= Total && Total < 300000) Rank.sprite = RankSprite[2];
        if (300000 <= Total && Total < 1200000) Rank.sprite = RankSprite[3];
        if (1200000 <= Total && Total < 2300000) Rank.sprite = RankSprite[4];
        if (2300000 <= Total && Total < 2898202) Rank.sprite = RankSprite[5];
        if (2898202 <= Total) Rank.sprite = RankSprite[6];

        if (MoveTimer > 250 && MoveCount >= 10 && MoveCount < 15)
        {
            MoveCount += 1;
            MoveTimer = 0;
        }
        if (MoveCount == 10) EndText_1.SetActive(true);
        if (MoveCount == 11)
        {
            EndText_1.SetActive(false);
            EndText_2.SetActive(true);
        }
        if (MoveCount == 12)
        {
            EndText_2.SetActive(false);
            EndText_3.SetActive(true);
        }
        if (MoveCount == 13)
        {
            EndText_3.SetActive(false);
            EndText_4.SetActive(true);
        }
        if (MoveCount == 14)
        {
            EndText_4.SetActive(false);
            EndText_5.SetActive(true);
        }
        if (MoveCount == 15)
        {
            SceneManager.LoadScene(0);
            MoveCount = 16;
        }
    }

    public void TitleButton()
    {
        //ランキング名前セット
        for (int i = 0; i < 5; i++)
        {
            if (ScoreManager.Ranking[i] <= Total)
            {
                for (int j = 3; j >= i; j--)
                {
                    ScoreManager.Name[j + 1] = ScoreManager.Name[j];
                }
                ScoreManager.Name[i] = InputText.text;
                break;
            }
        }
        
        Back.SetActive(true);
        SkipText.SetActive(true);
        ScrollView.SetActive(false);
        MoveCount = 10;
        MoveTimer = 0;
    }

    public void SkipButton()
    {
        SceneManager.LoadScene(0);
    }
}
