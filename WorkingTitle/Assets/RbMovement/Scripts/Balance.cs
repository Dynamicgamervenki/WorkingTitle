using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Balance : MonoBehaviour
{
    //AnimatorManager animatorManager;

    public UnityEvent PlayerBalance, Unbalance;

    private void Awake()
    {
        //animatorManager = FindObjectOfType<AnimatorManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //animatorManager.Balancing = true;
            //animatorManager.anim.SetLayerWeight(2, 1.0f);
            Debug.Log("Balancign shit init");
             PlayerBalance.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //animatorManager.Balancing = false;
            //animatorManager.anim.SetLayerWeight(2,0f);
            Unbalance.Invoke();
            Debug.Log("My Nigger");
        }
    }
}
