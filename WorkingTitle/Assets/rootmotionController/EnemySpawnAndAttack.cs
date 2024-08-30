using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnAndAttack : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;
    Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Mechanics>().gameObject;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(patrol == true)
        {
            float distance = DistanceBtwPlayerAndEnemie();
            EnemyLookAtPlayer();
            if (distance > 1)
            {
                Chase();
            }
            if (distance <= 1)
            {
                Attack();
            }
        }

    }
    

    private void Chase()
    {
        animator.SetBool("Walk", true);
        transform.position = Vector3.MoveTowards(transform.position,player.transform.position,2 * Time.deltaTime);
    }

    private void Attack()
    {
        animator.SetBool("Walk", false);
        animator.SetFloat("Attack", 1.0f);
    }

    private float DistanceBtwPlayerAndEnemie()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }

    public void EnemyLookAtPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0; // Keep the rotation only in the horizontal pl
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); 
    }

    bool patrol = false;

}
