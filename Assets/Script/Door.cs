using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int DoorLv;
    public static bool TokeiKey = false;
    public static bool HeyaKey = false;
    public static bool KinkoKey = false;

    public static bool OpenNow = false;//Status��SE���Ǘ�����p
    public static bool JumpNow = false;//���l

    public static bool DInArea = false;//Status�Ő؂�ւ��Ǘ��p
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
            if (DoorLv == 4 && TokeiKey)//���v��
            {
                OpenNow = true;
                this.gameObject.SetActive(false);
                falsee();
            }
            if (DoorLv == 5 && HeyaKey)//����̕���
            {
                OpenNow = true;
                this.gameObject.SetActive(false);
                falsee();
            }
            if (DoorLv == 21 && KinkoKey)//����
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

    void falsee()//�R�[�h�Z�k�p
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
            

            if (DoorLv <= Status.OpenKeyLV) Status.TwTextStr = "Q�L�[�������ĊJ��";
            else Status.TwTextStr = "���̃��x���ł͊J�����Ȃ�";

            if (DoorLv == 4 && TokeiKey) Status.TwTextStr = "Q�L�[�������ĊJ��";
            else if (DoorLv == 4 && !TokeiKey) Status.TwTextStr = "���v���̌����K�v";
            if (DoorLv == 5 && HeyaKey) Status.TwTextStr = "Q�L�[�������ĊJ��";
            else if (DoorLv == 5 && !HeyaKey) Status.TwTextStr = "����̕����̌����K�v";
            if (DoorLv == 6) Status.TwTextStr = "Q�L�[��������\n�O�ɔ�э~���";
            if (DoorLv == 7) Status.TwTextStr = "�z�V�p�G���x�[�^�[\nQ�L�[��������2�K�ɍs��";
            if (DoorLv == 8) Status.TwTextStr = "�z�V�p�G���x�[�^�[\nQ�L�[��������1�K�ɍs��";
            if (DoorLv == 21 && KinkoKey) Status.TwTextStr = "Q�L�[�������ĊJ��";
            else if (DoorLv == 21 && !KinkoKey) Status.TwTextStr = "���ɂ̌����K�v";

            //Hint
            if (DoorLv == 9) Status.TwTextStr = "���̊ق̒n���ɂ͔�����ʂɒu����Ă����B\n���̌����̍\���Ɣ��̔z�u�����a����������͎̂��������낤���H�B������������\���͂Ȃ����E�E�E�H";
            if (DoorLv == 10) Status.TwTextStr = "�ȑO���̊ق̎g�p�l�Ƃ��Đ����������ɁA\n���v���̒n���ɋ��ɂ�����Ƃ����\�����ɂ������Ƃ�����B���v���̎���Ɏd�|��������̂�������Ȃ��E�E�E�B";
            if (DoorLv == 11) Status.TwTextStr = "W�L�[�őO�i�AA,S,D�L�[�ŉE�⍶�A���Ɉړ��ł��܂��B\n�}�E�X�𓮂������ƂŁA���_��ς��邱�Ƃ��ł��܂��B";
            if (DoorLv == 12) Status.TwTextStr = "Shift�L�[�������Ȃ���ړ�����ƁA���邱�Ƃ��o���܂�";
            if (DoorLv == 13 && Status.OpenKeyLV == 1) Status.TwTextStr = "����̔��̃A�C�R���͊J�����x����\���Ă��܂��B���݂�LV1�ŁA���̃h�A���J���邱�Ƃ��o���܂��B";
            if (DoorLv == 13 && Status.OpenKeyLV == 2) Status.TwTextStr = "�J�����x���͌��݂�LV2�ŁA���A�̃h�A���J���邱�Ƃ��o���܂��B";
            if (DoorLv == 13 && Status.OpenKeyLV == 3) Status.TwTextStr = "�J�����x���͌��݂�LV3�ŁA���A�A�Ԃ̃h�A���J���邱�Ƃ��o���܂��B";
            if (DoorLv == 14 && Status.ItemGetLV == 1) Status.TwTextStr = "����̕󔠂̃A�C�R���͓D�_���x����\���Ă��܂��B���݂�LV1�ŁA���̃A�C�e���𓐂ނ��Ƃ��o���܂��B";
            if (DoorLv == 14 && Status.ItemGetLV == 2) Status.TwTextStr = "�D�_���x���͌��݂�LV2�ŁA���A�̃A�C�e���𓐂ނ��Ƃ��o���܂��B";
            if (DoorLv == 14 && Status.ItemGetLV == 3) Status.TwTextStr = "�D�_���x���͌��݂�LV3�ŁA���A�A�Ԃ̃A�C�e���𓐂ނ��Ƃ��o���܂��B";
            if (DoorLv == 15) Status.TwTextStr = "���̌`�̃A�C�e���́A�E���ƃX�L���̃��x�����オ��܂��B����̏ꍇ�͊J�����x����2�ɏオ��܂��B";
            if (DoorLv == 16) Status.TwTextStr = "��p�̌����K�v�Ȕ������݂��܂��B";
            if (DoorLv == 17) Status.TwTextStr = "����̎��v�̃A�C�R���͎��v���x����\���Ă��܂��BE�L�[���������ƂŎ����~�߂邱�Ƃ��ł��A�~�߂Ă���Ԃ͌��m����܂���B";
            if (DoorLv == 18) Status.TwTextStr = "�Ď��J������x�����A�Z���T�[�ɐG���ƃ��C�t������܂��B0�ɂȂ�ƃQ�[���I�[�o�[�ɂȂ�̂ŐG��Ȃ��ł��������B";
            if (DoorLv == 19) Status.TwTextStr = "���̐�ɂ͓�����͖��������͂����B";
            if (DoorLv == 20) Status.TwTextStr = "Space�L�[�ŃW�����v�ACtrl�L�[�ł��Ⴊ�݂܂��B";
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
