using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator anim;
    private int Horizontal;
    private int Vertical;
                                                       public bool Balancing = false;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        Horizontal = Animator.StringToHash("Horizontal");   
        Vertical = Animator.StringToHash("Vertical");
    }

    public void PlayTargetAnimations(string targetAnimation , bool isInteracting)
    {
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnimation, 0.2f);
    }

    public void UpdateAnimatorValues(float horizontalMovement,float verticalMovement,bool is_Sprinting,bool climbingCliff)
    {
        //Animation Snapping
        float snappedHorizontal;
        float snappedVertical;


        #region Snapped Horizontal
        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            snappedHorizontal = 0.5f;
        }
        else if (horizontalMovement > 0.55f)
        {
            snappedHorizontal = 1;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            snappedHorizontal = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }
        #endregion

        #region Snapped Vertical
        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            snappedVertical = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            snappedVertical = 1;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            snappedVertical = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }
        #endregion

        if (is_Sprinting)
        {
            snappedHorizontal = horizontalMovement;
            snappedVertical = 2.0f;
        }

        if(climbingCliff)
        {
            snappedVertical = 2.0f;
        }

        //if(is_RopeSwinging)
        //{
        //    snappedHorizontal = horizontalMovement;
        //    snappedVertical = 2.0f;
        //}


        anim.SetFloat(Horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        anim.SetFloat(Vertical, snappedVertical, 0.1f, Time.deltaTime);

        //anim.SetFloat(Horizontal, Horizontal, 0.1f, Time.deltaTime);
        //anim.SetFloat(Vertical, Vertical, 0.1f, Time.deltaTime);
    }

}
