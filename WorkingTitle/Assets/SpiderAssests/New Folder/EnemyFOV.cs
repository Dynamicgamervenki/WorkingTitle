using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;


[ExecuteInEditMode]
public class EnemyFOV : MonoBehaviour
{
    [Header("Enemy Detection Variables")]
    public float distance = 10;
    public float angle = 30;
    public float height = 1.0f;
    public Color meshColor = Color.cyan;
    public int scanFrequency = 30;
    public LayerMask layers;
    public List<GameObject> Objects = new List<GameObject>();
    public Transform Player;

    public Animator animator;
    Collider[] colliders = new Collider[50];
    Mesh mesh;
    int count;
    float scanInterval;
    float scanTimer;
    public bool isDetected;
    public bool isChasing;
    public static EnemyFOV instance;
    

    Vector3 Temp = new Vector3();
    public float DistBtw = 0;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        scanInterval = 1.0f / scanFrequency;

        instance= this;
    }

    // Update is called once per frame
    void Update()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }


    }



    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layers);
        Objects.Clear();
        for(int i = 0; i < count; ++i)
        {
            GameObject obj = colliders[i].gameObject;
            if (IsInSight(obj))
            {
                Objects.Add(obj);
                if(obj.gameObject.tag == "Player")
                {
                    Player = obj.gameObject.transform;
                    isDetected = true;
                    isChasing = true;
                  

                    Temp = obj.transform.position;
                    Temp.x -= DistBtw;
                    Temp.z -= DistBtw;
                    //GetComponent<NavMeshAgent>().SetDestination(obj.transform.position);
                    GetComponent<NavMeshAgent>().SetDestination(Temp);
                }
                else
                {
                   
                    isDetected = false;
                    isChasing = false;
                }
            }
        }
    }

    

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position; 
        Vector3 dest   = transform.position;
        Vector3 direction=dest- origin;

        if (direction.y < 0 || direction.y > height) 
        {
            return false;
        }
        direction.y = 0;
        float deltaAngle=Vector3.Angle(direction, transform.forward);
        if (deltaAngle >  angle) 
        { 
           return false ;

        }


        return true;
    }




    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;


        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCentre = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCentre = bottomCentre + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

        int vert = 0;

        //left side
        vertices[vert++] = bottomCentre;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCentre;
        vertices[vert++] = bottomCentre;

        //right side
        vertices[vert++] = bottomCentre;
        vertices[vert++] = topCentre;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCentre;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; ++i)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;

            //far side---
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;
            vertices[vert++] = bottomLeft;

            //top----
            vertices[vert++] = topCentre;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;


            //bottom----
            vertices[vert++] = bottomCentre;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;

        }



        for (int i = 0; i < numVertices; ++i)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();



        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
        scanInterval = 1.0f / scanFrequency;
    }
    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }
        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i < count; ++i) {
            Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
        }

        Gizmos.color *= Color.white;
        foreach(var obj in Objects) 
        {
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
        }
    }

    //Attack
    public void IsAttacking()
    {
        animator.SetTrigger("IsAttacking");
        if(isDetected||isChasing)
        {
            animator.SetTrigger("IsAttacking");
        }
        else
        {
            animator.SetBool("IsPatrolling", true);
        }
    }
    private void OnEnable()
    {
        Objects.Clear();
    }
    private void OnDisable()
    {
        Objects.Clear();
    }
}

