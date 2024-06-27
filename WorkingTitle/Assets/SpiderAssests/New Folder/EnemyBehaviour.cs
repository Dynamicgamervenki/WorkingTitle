using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent agent;
    public OffMeshLink offAgent;

    public Transform playerREF;

    public CustomSpherDetection FangAttackCollision,RightLegAttactDetection,LeftLegAttactDetection;
    public float attackDistance, followdistance,JumpDistanceMin, JumpDistanceMax;

    [SerializeField]public float TimeToAttack,TimeToJumpAttack;
    [HideInInspector] public float TimerCounter,JumpTimerCounter;
    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        offAgent = GetComponent<OffMeshLink>();
        playerREF=GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    public float ReturnDistance()
    {
       return Vector3.Distance(transform.position, playerREF.position);
    }
    public float ReturnDistance(Transform playerREF)
    {
        return Vector3.Distance(transform.position, playerREF.position);
    }
    Vector3 direction;
    public void EnemyLookAtPlayer()
    {
        Vector3 direction = playerREF.position - transform.position;
        direction.y = 0; // Keep the rotation only in the horizontal pl
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smooth the rotation
        //transform.LookAt(-playerREF.position);
    }  
}
