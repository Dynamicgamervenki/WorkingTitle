using UnityEngine;

public class EnemyHealthScript : HealthScript
{
    Animator _animator;
   override public void setHealth(float value)
    {
        if(!IsDead())
        {
            _health -= value;
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
        if (base.IsDead())
        {
            transform.GetComponent<Animator>().SetBool("Dead",base.IsDead());
            //_animator.CrossFade("EnemyDeath", 0.5f);
            //_animator.Play("EnemyDeath");
        }
        return base.IsDead();
    }
}
