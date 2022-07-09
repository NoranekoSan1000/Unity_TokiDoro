using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMove : MonoBehaviour
{
    bool IsMove = false;
    float Rotate=2f;
    float RoTimer = 0f;
    int Counter = 0;

    public bool reverse;

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
            if (reverse) this.gameObject.transform.Rotate(0, Rotate, 0);
            else this.gameObject.transform.Rotate(0, -Rotate, 0);
            Counter++;
            RoTimer = 0;
            
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(Input.GetKeyDown(KeyCode.Q)) IsMove = true;
    }

}
