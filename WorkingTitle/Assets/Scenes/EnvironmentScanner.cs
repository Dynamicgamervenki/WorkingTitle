using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentScanner : MonoBehaviour
{
    [Header("Forword Raycast")]
    public Vector3 forwordRayOffset = new Vector3(0, 0.25f, 0);
    public float forwordRayLength = 0.8f;
    public LayerMask obstacleLayer;

    [Header("Height Raycast")]
    public float heightRayLength = 5.0f;

    public ObstacleHitData ObstacleCheck()
    {
        ObstacleHitData obstacleHitData = new ObstacleHitData();
        var forwardOrigin = transform.position + forwordRayOffset;
        obstacleHitData.forwordHitFound = Physics.Raycast(forwardOrigin, transform.forward, out obstacleHitData.forwordHit, forwordRayLength, obstacleLayer);
        Debug.DrawRay(forwardOrigin, transform.forward * forwordRayLength, (obstacleHitData.forwordHitFound) ? Color.red : Color.black);

        var heightOrigin = obstacleHitData.forwordHit.point + Vector3.up * heightRayLength;
        if (obstacleHitData.forwordHitFound)
        {
            obstacleHitData.heightHitFound = Physics.Raycast(heightOrigin, Vector3.down, out obstacleHitData.heightHit, heightRayLength, obstacleLayer);
            Debug.DrawRay(heightOrigin, Vector3.down * heightRayLength, (obstacleHitData.heightHitFound) ? Color.red : Color.white);
        }

        return obstacleHitData;
    }

}

public struct ObstacleHitData
{
    public bool forwordHitFound;
    public RaycastHit forwordHit;

    public bool heightHitFound;
    public RaycastHit heightHit;
}
