using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMove : MonoBehaviour
{
    HingeJoint hj;
    JointLimits limits;

    bool IsMove = false;

    // Start is called before the first frame update
    void Start()
    {
        hj = GetComponent<HingeJoint>();
        limits = hj.limits;
        IsMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ���N���b�N�������ꂽ��
        if (IsMove)
        {
            // ���������� -130 �܂ŉ�]�ł���悤�ɂ���i���߂� 0 �ɂȂ��Ă��܂��j
            limits.min = 90;
        }
        else
        {
            // �f�t�H���g�̈ʒu�ŁA���N���b�N��������Ă��Ȃ��Ƃ��͉�]���Ȃ�
            limits.min = 0;
        }
        // �f�t�H���g�̈ʒu�ł̓h�A���������Ȃ��B
        limits.bounciness = 0f;
        hj.limits = limits;
        
    }

    void OnTriggerStay(Collider other)
    {
        if(Input.GetKeyDown(KeyCode.Q)) IsMove = true;
        Debug.Log(IsMove);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }
}
