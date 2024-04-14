using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRopeClimb : MonoBehaviour
{
    //public PlayerController controller;
    public bool _ropeClimbDetect;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if(other.gameObject.CompareTag("Rope"))
        {
            _ropeClimbDetect = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Rope"))
        {
            _ropeClimbDetect = false;
        }
    }
   
}
