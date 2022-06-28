using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Item : MonoBehaviour
{
    public int ItemLv;
    public int ItemNUM;

    public static bool IInArea = false;
    bool InArea = false;

    public static bool GetNow = false;

    public static bool[] GetItem = new bool[15];

    void Start()
    {
        for (int i = 0; i < 15; i++) GetItem[i] = false;

        GetNow = false;
        InArea = false;
        IInArea = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (ItemLv <= Status.ItemGetLV)
        {
            if (Input.GetKeyDown(KeyCode.Q) && InArea && ItemNUM != -8)
            {
                GetNow = true;
                IInArea = false;
                if (ItemNUM >= 1) GetItem[ItemNUM] = true; //ItemNUM番目のアイテムゲット
                Ite(0);
                Status.TwTextStr = "";
                this.gameObject.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.Q) && InArea && ItemNUM == -8)
            {
                if (Status.WatchLV == 2)
                {
                    GetNow = true;
                    IInArea = false;
                    Ite(0);
                    Status.TwTextStr = "";
                    this.gameObject.SetActive(false);
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InArea = true;
            IInArea = true;
            if (ItemLv <= Status.ItemGetLV) Ite(1);
            else Status.TwTextStr = "今のレベルでは盗めない";
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InArea = false;
            IInArea = false;
            Status.TwTextStr = "";
        }
    }

    void Ite(int N)
    {
        if (N == 0)
        {
            if (ItemNUM == -10) Door.KinkoKey = true;
            if (ItemNUM == -9) SceneManager.LoadSceneAsync("Title");
            if (ItemNUM == -8) Status.WatchLV = 3;
            if (ItemNUM == -7) Status.ItemGetLV = 3;
            if (ItemNUM == -6) Status.OpenKeyLV = 3;
            if (ItemNUM == -5) Status.WatchLV = 2;
            if (ItemNUM == -4) Status.ItemGetLV = 2;
            if (ItemNUM == -3) Status.OpenKeyLV = 2;
            if (ItemNUM == -2) Door.HeyaKey = true;
            if (ItemNUM == -1) Door.TokeiKey = true;

            if (ItemNUM == 1) Status.Score += 400;
            if (ItemNUM == 2) Status.Score += 10000;
            if (ItemNUM == 3) Status.Score += 200;
            if (ItemNUM == 4) Status.Score += 8000;
            if (ItemNUM == 5) Status.Score += 120000;
            if (ItemNUM == 6) Status.Score += 40000;
            if (ItemNUM == 7) Status.Score += 1000000;
            if (ItemNUM == 8) Status.Score += 500;
            if (ItemNUM == 9) Status.Score += 20000;
            if (ItemNUM == 10) Status.Score += 30000;
            if (ItemNUM == 11) Status.Score += 800;
            if (ItemNUM == 12) Status.Score += 10000;
            if (ItemNUM == 13) Status.Score += 1;
            if (ItemNUM == 14) Status.Score += 400;
        }
        if (N == 1)
        {
            if (ItemNUM == -10) Status.TwTextStr = "金庫の鍵\nQキーを押して拾う";
            if (ItemNUM == -9) Status.TwTextStr = "Qキーを押して\nチュートリアルを終了";
            if (ItemNUM == -8)
            {
                if (Status.WatchLV == 2) Status.TwTextStr = "時計LV 2 → 3 \nQキーでLVアップ";
                else Status.TwTextStr = "時計LVを2にしなければ回収出来ない";
            }
            if (ItemNUM == -7) Status.TwTextStr = "泥棒LV 2 → 3 \nQキーでLVアップ";
            if (ItemNUM == -6) Status.TwTextStr = "開錠LV 2 → 3 \nQキーでLVアップ";
            if (ItemNUM == -5) Status.TwTextStr = "時計LV 1 → 2 \nQキーでLVアップ";
            if (ItemNUM == -4) Status.TwTextStr = "泥棒LV 1 → 2 \nQキーでLVアップ";
            if (ItemNUM == -3) Status.TwTextStr = "開錠LV 1 → 2 \nQキーでLVアップ";
            if (ItemNUM == -2) Status.TwTextStr = "当主の部屋の鍵\nQキーを押して拾う";
            if (ItemNUM == -1) Status.TwTextStr = "時計塔の鍵\nQキーを押して拾う";

            if (ItemNUM == 1) Status.TwTextStr = "普通の壺 400$\nQキーを押して盗む";//*4
            if (ItemNUM == 2) Status.TwTextStr = "トパーズの指輪 10000$\nQキーを押して盗む";
            if (ItemNUM == 3) Status.TwTextStr = "普通のティーセット 200$\nQキーを押して盗む";//*2
            if (ItemNUM == 4) Status.TwTextStr = "ルビーの指輪 8000$\nQキーを押して盗む";
            if (ItemNUM == 5) Status.TwTextStr = "金のインゴットの山 120000$\nQキーを押して盗む";//*2
            if (ItemNUM == 6) Status.TwTextStr = "金のインゴット 40000$\nQキーを押して盗む";//*2
            if (ItemNUM == 7) Status.TwTextStr = "トワイライトサファイア 1000000$\nQキーを押して盗む";
            if (ItemNUM == 8) Status.TwTextStr = "偽物のトワイライトサファイア 500$\nQキーを押して盗む";
            if (ItemNUM == 9) Status.TwTextStr = "上質なワイン 20000$\nQキーを押して盗む";
            if (ItemNUM == 10) Status.TwTextStr = "高価な絵画 30000$\nQキーを押して盗む";
            if (ItemNUM == 11) Status.TwTextStr = "よくある絵画 800$\nQキーを押して盗む";//*2
            if (ItemNUM == 12) Status.TwTextStr = "世界にひとつだけの花 10000$\nQキーを押して盗む";
            if (ItemNUM == 13) Status.TwTextStr = "ごみ 1$\nQキーを押して盗む";
            if (ItemNUM == 14) Status.TwTextStr = "樽いっぱいのワイン 400$\nQキーを押して盗む";

            //max 1,434,101$   SindoBonus 30000  BEST 2,898,202 -> 2,625,781 -> 2,337,361 -> 2,048,941 -> 1,760,521
        }
    }
}
