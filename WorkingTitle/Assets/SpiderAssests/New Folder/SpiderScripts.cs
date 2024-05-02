using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderScripts : MonoBehaviour
{
    public Transform PlayerRef;
    [Range(0f, 50f)]
    public float distance;
    
    public int spiderHealthValue=100;
    public int Health
    {
        get { if (spiderHealthValue < 0) { transform.GetComponent<Animator>().SetTrigger("Dead"); } return spiderHealthValue=Mathf.Clamp(spiderHealthValue,0,100); }
        set { spiderHealthValue = value; }
    }
}
