using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBtwPoints : MonoBehaviour
{
    public Transform posA;
    public Transform posB;
    public float speed = 2.0f;

    private Transform target;

    void Start()
    {
        target = posB;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.001f)
        {
            target = (target == posA) ? posB : posA;
        }
    }
}
