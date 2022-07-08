using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMove : MonoBehaviour
{
    bool IsMove = false;
    float Rotate = 2f;
    float RoTimer = 0f;
    int Counter = 0;

    /*HingeJoint hj;
    JointLimits limits;

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
        
    }*/

    void Start()
    {
        IsMove = false;
    }
    void Update()
    {
        if(IsMove) RoTimer += Time.deltaTime;

        if (RoTimer >= 0.01 && Counter <=45)
        {
            Debug.Log(Counter);
            this.gameObject.transform.Rotate(0, Rotate, 0);
            Counter++;
            RoTimer = 0;
            
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(Input.GetKeyDown(KeyCode.Q)) IsMove = true;
    }

}
