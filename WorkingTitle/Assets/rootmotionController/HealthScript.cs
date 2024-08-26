using UnityEngine;

abstract public class HealthScript : MonoBehaviour
{
    public float _health=100f;
    virtual public void setHealth(float value) { }
    virtual public float getHealth() {  return _health; }
    virtual public bool IsDead()
    {
        return (_health<=0)?true:false;
    }
}
