using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerManager.Instance.NewRootMotionControllerInstance.playerAnimator().SetLayerWeight(3, 1);
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerManager.Instance.NewRootMotionControllerInstance.playerAnimator().SetLayerWeight(3, 0);
    }
}
