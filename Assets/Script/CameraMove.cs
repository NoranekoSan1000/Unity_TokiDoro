using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public bool hunter = false;

    float ThisTimer = 0;
    float Rotate = 0.5f;

    private Animator animator;

    public int HunterNUM = 0;

    void Start()
    {
        //�ϐ�anim�ɁAAnimator�R���|�[�l���g��ݒ肷��
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Status.MutekiTimer <= 0)//muteki���łȂ�
        {
            if (!hunter && ThisTimer < 3) ThisTimer+=Time.deltaTime;
            else
            {
                ThisTimer = 0;
                Rotate *= -1;
            }
            this.gameObject.transform.Rotate(0, Rotate, 0);

            if (hunter)
            {
                animator.SetFloat("MovingSpeed", 1.0f); // �ĊJ
            }
        }
        else
        {
            if(hunter)
            {
                animator.SetFloat("MovingSpeed", 0.0f); // �ꎞ��~
            }
        }

    }
}
