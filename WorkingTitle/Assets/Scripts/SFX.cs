using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    [SerializeField] AudioClip _electricWooshSound;
    [SerializeField] AudioClip _spiderWalkSound;
   public void ElectricWoosh(AnimationEvent e)
    {
        AudioSource.PlayClipAtPoint(_electricWooshSound, PlayerManager.Instance._ThirdPersonControllerInstance._swordCollider.transform.position);
    }
}
