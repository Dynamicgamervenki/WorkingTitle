using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTrigger : MonoBehaviour
{
    private int _hitCounter;

    public void SetHitCounter(int hitValue)
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.TryGetComponent(out Animator animator)) 
        {
            animator.SetTrigger("hit");
        }
    }
}
