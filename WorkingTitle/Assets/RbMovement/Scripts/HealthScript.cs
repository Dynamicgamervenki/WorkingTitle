using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float health;
    bool dead;
    public float Health {  get {dead = (health <= 0) ? true : false;return health=Mathf.Clamp(health,0f,100f); } set {  health = value; } }
    private void Update()
    {
        --Health;
    }
}
