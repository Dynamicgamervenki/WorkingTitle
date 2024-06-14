using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClimb : MonoBehaviour
{
    AnimatorManager animatorManager;
    PlayerLocomotion playerLocomotion;
    InputManager inputManager;

    private void Awake()
    {
        animatorManager = FindObjectOfType<AnimatorManager>();
        playerLocomotion = FindObjectOfType<PlayerLocomotion>();
        inputManager = FindObjectOfType<InputManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerLocomotion.climbingCliff = true;
            Quaternion targetRotation = Quaternion.Euler(23.445f, -93,-0.761f);
            other.transform.rotation = Quaternion.Slerp(other.transform.rotation, targetRotation,1.0f);
            //playerLocomotion.rb.AddForce(transform.up * 10.0f);
            //animatorManager.anim.SetBool("isRopeClimbing", false);
            //animatorManager.PlayTargetAnimations("Climbing", false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerLocomotion.climbingCliff = false;
            animatorManager.anim.SetBool("isRopeClimbing", false);
            Debug.Log("Exit");
            //playerLocomotion.climbingCliff = false;
           // playerLocomotion.ropeDetectionRaidus = 0.2f;
        }
    }
}
