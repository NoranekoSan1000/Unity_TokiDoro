using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Status : MonoBehaviour
{
    public static int Hp;
     
    public static int OpenKeyLV;
    public static int ItemGetLV;
    public static int WatchLV;

    public static int Score = 400;
    
    public Text ScoreText;
    public Text TimerText;

    public GameObject[] HpImg = new GameObject[5];

    public Image[] LvImg = new Image[3];
    public Sprite[] OpenKey = new Sprite[3];
    public Sprite[] ItemGet = new Sprite[3];
    public Sprite[] Watch = new Sprite[3];

    float CoolTime = 0;
    int Min, Sec;
    public static float Timer;//300

    public GameObject TextWindow;
    public Text TextWindowText;
    public static string TwTextStr = "";

    public GameObject MutekiClr;
    public GameObject MutekiCTImg;
    public Text MutekiText;
    public Text MutekiCTText;
    public static float MutekiTimer = -1;
    float MutekiCoolTime =-1;

    public GameObject TokeitouText;
    public GameObject TikasituText;
    public GameObject DaikinkoText;

    public static bool WarpArea = false;
    public static bool WarpArea2 = false;
    public static bool WarpArea3 = false;

    public bool Tutorial = false;

    AudioSource audioSource;
    public AudioClip SE_booon;
    public AudioClip SE_door;
    public AudioClip SE_Item;
    public AudioClip SE_jumptyaku;

    public GameObject SE_Watch;

    public GameObject UseItemImg1;
    public GameObject UseItemImg2;

    bool GoResultBool = false;

    // Start is called before the first frame update
    void Start()
    {
        UseItemImg1.SetActive(true);
        UseItemImg2.SetActive(true);
        audioSource = GetComponent<AudioSource>();
        SE_Watch.SetActive(false);

        Hp = 5;
        OpenKeyLV =1;
        ItemGetLV = 1;
        WatchLV = 1;
        Score = 0;
        CoolTime = 0;
        Timer = 300;
        TwTextStr = "";
        MutekiTimer = -1;
        MutekiCoolTime = -1;
        WarpArea = false;
        WarpArea2 = false;
        WarpArea3 = false;
        FadeController.FadeoutStart = false;
        FadeController.isFadeOut = true;
        GoResultBool = false;

        Application.targetFrameRate = 60;
        SpriteSet();
    }

    // Update is called once per frame
    void Update()
    {
        HPImgSet();
        ScoreSet();
        LeftTime();
        SpriteSet();
        TextWindowSet();
        MutekiSet();
        Warp();
        Play_SE();
        CanUseItem();

        if (Door.TokeiKey) TokeitouText.SetActive(true);
        else TokeitouText.SetActive(false);
        if (Door.HeyaKey) TikasituText.SetActive(true);
        else TikasituText.SetActive(false);
        if (Door.KinkoKey) DaikinkoText.SetActive(true);
        else DaikinkoText.SetActive(false);

        if (CoolTime > -1 )CoolTime -= Time.deltaTime;

    }

    void HPImgSet()
    {
        for (int i = 0; i < 5; i++)
        {
            if (Hp > i) HpImg[i].SetActive(true);
            else HpImg[i].SetActive(false);
        }

    }

    void ScoreSet()
    {
        ScoreText.text = Score + " $";
    }


    void LeftTime()
    {

        if (!Tutorial && MutekiTimer <= 0) Timer -= Time.deltaTime;

        Min = ((int)Timer / 60);
        Sec = (int)Timer - ((int)Timer / 60) * 60;
        if (Sec >= 10) TimerText.text = Min + ":" + Sec;
        else TimerText.text = Min + ":0" + Sec;

        if (Hp <= 0 && Timer > 0) Timer = 0;

        if (Tutorial && Hp <= 0)
        {
            SceneManager.LoadSceneAsync("Title");
            Hp = 1;
        }

        if ((Timer <= 0) && !FadeController.FadeoutStart)
        {
            FadeController.FadeoutStart = true;
        }
        if(Timer <= -3)
        {
            if (!GoResultBool)
            {
                SceneManager.LoadSceneAsync("Result");
                GoResultBool = true;
            }
                Timer = -1;
        }
    }

    void SpriteSet()
    {
        for (int i = 0; i < 3; i++) if (OpenKeyLV == i + 1) LvImg[0].sprite = OpenKey[i];
        for (int i = 0; i < 3; i++) if (ItemGetLV == i + 1) LvImg[1].sprite = ItemGet[i];
        for (int i = 0; i < 3; i++) if (WatchLV == i + 1) LvImg[2].sprite = Watch[i];
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Seeker"))
        {
            if (Hp > 0 && 0 >= CoolTime && MutekiTimer < 0)
            {
                Hp -= 1;
                CoolTime = 4;
            }
        }
    }

    public GameObject MenuPanel;
    public static bool MenuOpen = false;

    public void MENUButton()
    {

        if (MenuOpen)
        {
            MenuOpen = false;
            MenuPanel.SetActive(false);      
        }
        else
        {
            MenuOpen = true;
            MenuPanel.SetActive(true);
        } 
    }

    public void GoTitle()
    {
        SceneManager.LoadSceneAsync("Title");
    }

    public void GoResult()
    {
        if (!GoResultBool)
        {
            GoResultBool = true;
            SceneManager.LoadSceneAsync("Result");
        }
    }

    void TextWindowSet()
    {
        if(TwTextStr == "")TextWindow.SetActive(false);
        else TextWindow.SetActive(true);

        TextWindowText.text = TwTextStr;
    }

    void MutekiSet()
    {
        if (MutekiCoolTime >= 0)
        {
            MutekiCoolTime -= Time.deltaTime;
            MutekiCTImg.SetActive(true);
            if(MutekiTimer <= 0)MutekiCTText.text = (int)MutekiCoolTime+ "";
        }
        else
        {
            MutekiCTImg.SetActive(false);
            MutekiCTText.text =  "";
        }

        if (MutekiTimer >=0)
        {
            MutekiTimer-=Time.deltaTime;
            MutekiClr.SetActive(true);
            MutekiText.text = (int)MutekiTimer+"";
        }
        else
        {
            SE_Watch.SetActive(false);

            MutekiClr.SetActive(false);
            MutekiText.text = "";
        }

        if (WatchLV == 1 && MutekiCoolTime <= 0 && Input.GetKey(KeyCode.E))
        {
            MutekiTimer = 2;
            audioSource.PlayOneShot(SE_booon);
            SE_Watch.SetActive(true);

            if (!Tutorial)MutekiCoolTime = 17;
            else MutekiCoolTime = 4;
        }
        if (WatchLV == 2 && MutekiCoolTime <= 0 && Input.GetKeyDown(KeyCode.E))
        {
            MutekiTimer = 4;
            audioSource.PlayOneShot(SE_booon);
            SE_Watch.SetActive(true);

            if (!Tutorial) MutekiCoolTime = 19;
            else MutekiCoolTime = 6;
        }
        if (WatchLV == 3 && MutekiCoolTime <= 0 && Input.GetKeyDown(KeyCode.E))
        {
            MutekiTimer = 6;
            audioSource.PlayOneShot(SE_booon);
            SE_Watch.SetActive(true);

            if (!Tutorial) MutekiCoolTime = 21;
            else MutekiCoolTime = 8;
        }
    }

    void Warp()
    {
        if (WarpArea)
        {
            this.gameObject.transform.position = new Vector3(149.67f, 101.18f, 227.08f);
            WarpArea = false;
        }
        if (WarpArea2)
        {
            this.gameObject.transform.position = new Vector3(104.795f, 113.3f, 197.0331f);
            WarpArea2 = false;
        }
        if (WarpArea3)
        {
            this.gameObject.transform.position = new Vector3(111.786f, 100.65f, 186.1046f);
            WarpArea3 = false;
        }
    }

    void Play_SE()
    {
        if (Door.OpenNow)
        {
            audioSource.PlayOneShot(SE_door);
            Door.OpenNow = false;
        }
        if (Door.JumpNow)
        {
            audioSource.PlayOneShot(SE_jumptyaku);
            Door.JumpNow = false;
        }
        if (Item.GetNow)
        {
            audioSource.PlayOneShot(SE_Item);
            Item.GetNow = false;
        }
    }

    void CanUseItem()
    {
        if (Door.DInArea) UseItemImg1.SetActive(false);
        else  UseItemImg1.SetActive(true);


        if (Item.IInArea) UseItemImg2.SetActive(false);
        else UseItemImg2.SetActive(true);
    }

}
