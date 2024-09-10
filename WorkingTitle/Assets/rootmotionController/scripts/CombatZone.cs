using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if( PlayerManager.Instance.mechanics.weaponEquipped)
        {
            PlayerManager.Instance.NewRootMotionControllerInstance.playerAnimator().SetLayerWeight(3, 1);
            PlayerManager.Instance.NewRootMotionControllerInstance.sword.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerManager.Instance.NewRootMotionControllerInstance.playerAnimator().SetLayerWeight(3, 0);
        PlayerManager.Instance.NewRootMotionControllerInstance.sword.gameObject.SetActive(false);
    }
}
