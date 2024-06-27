using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSpherDetection : MonoBehaviour
{
    [SerializeField]
    private float radius = 5f;
    [SerializeField]
    private LayerMask layerMask;
    Collider[] hitColliders;
    public Collider ReturnCollider()
    {
        hitColliders = Physics.OverlapSphere(transform.position, radius, layerMask);
        foreach (Collider collider in hitColliders)
        {
            if (collider)
            {
                Debug.Log(collider.gameObject);
                return collider;
            }
        }
        return null;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
