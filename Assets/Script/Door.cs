using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int DoorLv;
    public static bool TokeiKey = false;
    public static bool HeyaKey = false;
    public static bool KinkoKey = false;

    public static bool OpenNow = false;//StatusでSEを管理する用
    public static bool JumpNow = false;//同様

    public static bool DInArea = false;//Statusで切り替え管理用
    bool InArea = false;

    void Start()
    {
        OpenNow = false;
        TokeiKey = false;
        HeyaKey = false;
        KinkoKey = false;
        InArea = false;
        DInArea = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && InArea)
        {
            if (DoorLv <= Status.OpenKeyLV)
            {
                OpenNow = true;
                this.gameObject.SetActive(false);
                falsee();
            }
            if (DoorLv == 4 && TokeiKey)//時計塔
            {
                OpenNow = true;
                this.gameObject.SetActive(false);
                falsee();
            }
            if (DoorLv == 5 && HeyaKey)//当主の部屋
            {
                OpenNow = true;
                this.gameObject.SetActive(false);
                falsee();
            }
            if (DoorLv == 21 && KinkoKey)//金庫
            {
                OpenNow = true;
                this.gameObject.SetActive(false);
                falsee();
            }

            if (DoorLv == 6)
            {
                JumpNow = true;
                Status.WarpArea = true;
                falsee();
            }
            if (DoorLv == 7)
            {
                Status.WarpArea2 = true;
                falsee();
            }
            if (DoorLv == 8)
            {
                Status.WarpArea3 = true;
                falsee();
            }
        }

    }

    void falsee()//コード短縮用
    {
        Status.TwTextStr = "";
        InArea = false;
        DInArea = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InArea = true;
            if(DoorLv < 9 || DoorLv >20) DInArea = true;
            

            if (DoorLv <= Status.OpenKeyLV) Status.TwTextStr = "Qキーを押して開く";
            else Status.TwTextStr = "今のレベルでは開けられない";

            if (DoorLv == 4 && TokeiKey) Status.TwTextStr = "Qキーを押して開く";
            else if (DoorLv == 4 && !TokeiKey) Status.TwTextStr = "時計塔の鍵が必要";
            if (DoorLv == 5 && HeyaKey) Status.TwTextStr = "Qキーを押して開く";
            else if (DoorLv == 5 && !HeyaKey) Status.TwTextStr = "当主の部屋の鍵が必要";
            if (DoorLv == 6) Status.TwTextStr = "Qキーを押して\n外に飛び降りる";
            if (DoorLv == 7) Status.TwTextStr = "配膳用エレベーター\nQキーを押して2階に行く";
            if (DoorLv == 8) Status.TwTextStr = "配膳用エレベーター\nQキーを押して1階に行く";
            if (DoorLv == 21 && KinkoKey) Status.TwTextStr = "Qキーを押して開く";
            else if (DoorLv == 21 && !KinkoKey) Status.TwTextStr = "金庫の鍵が必要";

            //Hint
            if (DoorLv == 9) Status.TwTextStr = "この館の地下には箱が大量に置かれていた。\nこの建物の構造と箱の配置から違和感を抱いたのは私だけだろうか？隠し部屋がある可能性はないか・・・？";
            if (DoorLv == 10) Status.TwTextStr = "以前この館の使用人として潜入した時に、\n時計塔の地下に金庫があるという噂を耳にしたことがある。時計塔の周りに仕掛けがあるのかもしれない・・・。";
            if (DoorLv == 11) Status.TwTextStr = "Wキーで前進、A,S,Dキーで右や左、後ろに移動できます。\nマウスを動かすことで、視点を変えることができます。";
            if (DoorLv == 12) Status.TwTextStr = "Shiftキーを押しながら移動すると、走ることが出来ます";
            if (DoorLv == 13 && Status.OpenKeyLV == 1) Status.TwTextStr = "左上の扉のアイコンは開錠レベルを表しています。現在はLV1で、白のドアを開けることが出来ます。";
            if (DoorLv == 13 && Status.OpenKeyLV == 2) Status.TwTextStr = "開錠レベルは現在はLV2で、白、青のドアを開けることが出来ます。";
            if (DoorLv == 13 && Status.OpenKeyLV == 3) Status.TwTextStr = "開錠レベルは現在はLV3で、白、青、赤のドアを開けることが出来ます。";
            if (DoorLv == 14 && Status.ItemGetLV == 1) Status.TwTextStr = "左上の宝箱のアイコンは泥棒レベルを表しています。現在はLV1で、白のアイテムを盗むことが出来ます。";
            if (DoorLv == 14 && Status.ItemGetLV == 2) Status.TwTextStr = "泥棒レベルは現在はLV2で、白、青のアイテムを盗むことが出来ます。";
            if (DoorLv == 14 && Status.ItemGetLV == 3) Status.TwTextStr = "泥棒レベルは現在はLV3で、白、青、赤のアイテムを盗むことが出来ます。";
            if (DoorLv == 15) Status.TwTextStr = "この形のアイテムは、拾うとスキルのレベルが上がります。これの場合は開錠レベルが2に上がります。";
            if (DoorLv == 16) Status.TwTextStr = "専用の鍵が必要な扉も存在します。";
            if (DoorLv == 17) Status.TwTextStr = "左上の時計のアイコンは時計レベルを表しています。Eキーを押すことで時を止めることができ、止めている間は検知されません。";
            if (DoorLv == 18) Status.TwTextStr = "監視カメラや警備員、センサーに触れるとライフが減ります。0になるとゲームオーバーになるので触れないでください。";
            if (DoorLv == 19) Status.TwTextStr = "この先には入り口は無かったはずだ。";
            if (DoorLv == 20) Status.TwTextStr = "Spaceキーでジャンプ、Ctrlキーでしゃがみます。";
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InArea = false;
            DInArea = false;
            Status.TwTextStr = "";
        }
    }
}
