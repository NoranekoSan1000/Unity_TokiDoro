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
                if (ItemNUM >= 1) GetItem[ItemNUM] = true; //ItemNUM�Ԗڂ̃A�C�e���Q�b�g
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
            else Status.TwTextStr = "���̃��x���ł͓��߂Ȃ�";
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
            if (ItemNUM == -10) Status.TwTextStr = "���ɂ̌�\nQ�L�[�������ďE��";
            if (ItemNUM == -9) Status.TwTextStr = "Q�L�[��������\n�`���[�g���A�����I��";
            if (ItemNUM == -8)
            {
                if (Status.WatchLV == 2) Status.TwTextStr = "���vLV 2 �� 3 \nQ�L�[��LV�A�b�v";
                else Status.TwTextStr = "���vLV��2�ɂ��Ȃ���Ή���o���Ȃ�";
            }
            if (ItemNUM == -7) Status.TwTextStr = "�D�_LV 2 �� 3 \nQ�L�[��LV�A�b�v";
            if (ItemNUM == -6) Status.TwTextStr = "�J��LV 2 �� 3 \nQ�L�[��LV�A�b�v";
            if (ItemNUM == -5) Status.TwTextStr = "���vLV 1 �� 2 \nQ�L�[��LV�A�b�v";
            if (ItemNUM == -4) Status.TwTextStr = "�D�_LV 1 �� 2 \nQ�L�[��LV�A�b�v";
            if (ItemNUM == -3) Status.TwTextStr = "�J��LV 1 �� 2 \nQ�L�[��LV�A�b�v";
            if (ItemNUM == -2) Status.TwTextStr = "����̕����̌�\nQ�L�[�������ďE��";
            if (ItemNUM == -1) Status.TwTextStr = "���v���̌�\nQ�L�[�������ďE��";

            if (ItemNUM == 1) Status.TwTextStr = "���ʂ̚� 400$\nQ�L�[�������ē���";//*4
            if (ItemNUM == 2) Status.TwTextStr = "�g�p�[�Y�̎w�� 10000$\nQ�L�[�������ē���";
            if (ItemNUM == 3) Status.TwTextStr = "���ʂ̃e�B�[�Z�b�g 200$\nQ�L�[�������ē���";//*2
            if (ItemNUM == 4) Status.TwTextStr = "���r�[�̎w�� 8000$\nQ�L�[�������ē���";
            if (ItemNUM == 5) Status.TwTextStr = "���̃C���S�b�g�̎R 120000$\nQ�L�[�������ē���";//*2
            if (ItemNUM == 6) Status.TwTextStr = "���̃C���S�b�g 40000$\nQ�L�[�������ē���";//*2
            if (ItemNUM == 7) Status.TwTextStr = "�g���C���C�g�T�t�@�C�A 1000000$\nQ�L�[�������ē���";
            if (ItemNUM == 8) Status.TwTextStr = "�U���̃g���C���C�g�T�t�@�C�A 500$\nQ�L�[�������ē���";
            if (ItemNUM == 9) Status.TwTextStr = "�㎿�ȃ��C�� 20000$\nQ�L�[�������ē���";
            if (ItemNUM == 10) Status.TwTextStr = "�����ȊG�� 30000$\nQ�L�[�������ē���";
            if (ItemNUM == 11) Status.TwTextStr = "�悭����G�� 800$\nQ�L�[�������ē���";//*2
            if (ItemNUM == 12) Status.TwTextStr = "���E�ɂЂƂ����̉� 10000$\nQ�L�[�������ē���";
            if (ItemNUM == 13) Status.TwTextStr = "���� 1$\nQ�L�[�������ē���";
            if (ItemNUM == 14) Status.TwTextStr = "�M�����ς��̃��C�� 400$\nQ�L�[�������ē���";

            //max 1,434,101$   SindoBonus 30000  BEST 2,898,202 -> 2,625,781 -> 2,337,361 -> 2,048,941 -> 1,760,521
        }
    }
}
