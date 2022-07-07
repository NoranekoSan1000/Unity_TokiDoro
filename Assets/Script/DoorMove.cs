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
        // 左クリックが押されたら
        if (IsMove)
        {
            // 向こう側へ -130 まで回転できるようにする（初めは 0 になっています）
            limits.min = 90;
        }
        else
        {
            // デフォルトの位置で、左クリックが押されていないときは回転しない
            limits.min = 0;
        }
        // デフォルトの位置ではドアが反発しない。
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
