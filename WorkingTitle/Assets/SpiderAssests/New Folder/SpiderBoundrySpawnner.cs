using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBoundrySpawnner : MonoBehaviour
{
    [SerializeField]private GameObject spiders;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag=="Player")
        {
            spiders.SetActive(true);
        }
    }


}
