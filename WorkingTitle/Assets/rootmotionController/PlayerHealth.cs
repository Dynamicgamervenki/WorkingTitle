using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthScript
{
    override public void setHealth(float value)
    {
        if(!IsDead())
        {
            _health = value;
        }
    }
    override public float getHealth() 
    { 
        if(!IsDead())
        {
            return _health;
        }
        return 0f;
    }
    public override bool IsDead()
    {
        if(IsDead())
        {
            transform.GetComponent<Animator>().SetBool("PlayerDeath", base.IsDead());
            transform.GetComponent<RootMotionController>().enabled = false;
        }
        return base.IsDead();
    }
}
