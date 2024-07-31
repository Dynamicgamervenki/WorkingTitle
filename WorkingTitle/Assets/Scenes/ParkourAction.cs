using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour System/New parkour action")]
public class ParkourAction : ScriptableObject
{
    public string animName;
    public string obstacleTag;
    public float minHeight;
    public float maxHeight;
    public float postActionDelay = 0;
    public bool rotateToObstacle;

    [Header("Target Matching")]
    public bool enableTargetMatching;
    public AvatarTarget avatarTarget;
    public float matchStartTym;
    public float matchTargetTym;
    public Vector3 matchposWeight = new Vector3(0, 1, 0);


    public Vector3 MatchPos { get; set; }
    public Quaternion TargetRotation { get; set; }

    public bool Mirror { get; set; }


    public virtual bool CheckIfPossible(ObstacleHitData obstacleHitData, Transform player)
    {
        if (!string.IsNullOrEmpty(obstacleTag) && obstacleHitData.forwordHit.transform.tag != obstacleTag)
            return false;


        float height = obstacleHitData.heightHit.point.y - player.position.y;
        if (height < minHeight || height > maxHeight)
            return false;

        if (rotateToObstacle)
            TargetRotation = Quaternion.LookRotation(-obstacleHitData.forwordHit.normal);


        if (enableTargetMatching)
            MatchPos = obstacleHitData.heightHit.point;

        return true;

    }
}
