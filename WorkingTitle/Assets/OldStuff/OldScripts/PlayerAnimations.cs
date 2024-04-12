using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    Animator animator;
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayerMovementBlend(float Xinput,float Yinput)
    {
        animator.SetFloat("XInput",Xinput,0.05f,Time.deltaTime);
        animator.SetFloat("YInput",Yinput,0.05f,Time.deltaTime);
    }
}
