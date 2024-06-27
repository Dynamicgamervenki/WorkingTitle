using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBoxDetection : MonoBehaviour
{
    public Vector3 boxCenter = Vector3.zero;

    // Size of the box
    public Vector3 boxSize = new Vector3(1, 1, 1);

    // Rotation of the box
    public Quaternion boxOrientation = Quaternion.identity;

    // LayerMask to filter colliders
    public LayerMask layerMask;
    public Collider ReturnCollider()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, boxSize / 2, transform.rotation, layerMask);
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
    // Optional: Visualize the box in the Scene view
    void DrawBox(Vector3 center, Vector3 size, Quaternion orientation)
    {
        Gizmos.color = Color.red;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(center, orientation, size);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }

    void OnDrawGizmos()
    {
        DrawBox(transform.position, boxSize, transform.rotation);
    }

}

