using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    [SerializeField] int damage;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.tag=="Spider")
        {
            if(other.gameObject.transform.TryGetComponent(out SpiderScripts spiderRef))
            {
                spiderRef.Health-=damage;
                Debug.Log(spiderRef.spiderHealthValue);
            }
        }
    }
}
