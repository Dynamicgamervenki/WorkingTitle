
public class PlayerHealth : HealthScript
{
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
        if(base.IsDead())
        {
            PlayerManager.Instance.NewRootMotionControllerInstance.playerAnimator().SetBool("PlayerDead", base.IsDead());
        }
        return base.IsDead();
    }
}
