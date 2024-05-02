using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class SwordEquip : MonoBehaviour
{

    private bool _meleeEquipAndUnequip;
    private int _MeleeID;
    private void OnEnable()
    {
        _MeleeID = Animator.StringToHash("Melee");
    }
    public void MeleeEquipAndUnequip(InputAction.CallbackContext context)
    {
        _meleeEquipAndUnequip=!_meleeEquipAndUnequip;
        PlayerManager.Instance._ThirdPersonControllerInstance._animator.SetBool(_MeleeID,_meleeEquipAndUnequip);
    }
}
