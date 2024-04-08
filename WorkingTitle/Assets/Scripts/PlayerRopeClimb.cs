using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRopeClimb : MonoBehaviour
{
    public PlayerController controller;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Rope"))
        {
            controller.rope_Climb = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Rope"))
        {
            controller.rope_Climb = false;
        }
    }
   
}
