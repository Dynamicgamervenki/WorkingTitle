using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBounce : MonoBehaviour
{
    [SerializeField] private UnityEvent playerBounceEvent;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            playerBounceEvent?.Invoke();
        }
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerBounceEvent.Invoke();
        }
    }
}
