using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balance : MonoBehaviour
{
    AnimatorManager animatorManager;

    private void Awake()
    {
        animatorManager = FindObjectOfType<AnimatorManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            animatorManager.Balancing = true;
            animatorManager.anim.SetLayerWeight(2, 1.0f);
            Debug.Log("Balancign shit init");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            animatorManager.Balancing = false;
            animatorManager.anim.SetLayerWeight(2,0f);
            Debug.Log("My Nigger");
        }
    }
}
