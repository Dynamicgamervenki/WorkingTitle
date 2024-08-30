using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public ThirdPersonController _ThirdPersonControllerInstance;
    public PlayerComboSystem _PlayerComboSystemInstance;
    public StarterAssetsInputs _StarterAssetsInputsInstance;
    public PlayerComboSystem _playerComboSystemInstance;
    public PlayerRopeClimb _PlayerRopeClimbInstance;
    public SwordEquip _SwordEquipInstance;
    public InputManager inputManager;
    public AnimatorManager animatorManager;
    public CustomBoxDetection AttackDetect;
    public RootMotionController RootMotionControllerInstance;
    public NewRootMotionController NewRootMotionControllerInstance;
    public PlayerHealth PlayerHealth;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
        Transform Temp = GameObject.FindGameObjectWithTag("Player").transform;
        inputManager=Temp.GetComponent<InputManager>();
        AttackDetect=Temp.GetComponentInChildren<CustomBoxDetection>();
        _ThirdPersonControllerInstance = Temp.GetComponent<ThirdPersonController>();
        _PlayerComboSystemInstance = Temp.GetComponent<PlayerComboSystem>();
        _StarterAssetsInputsInstance = Temp.GetComponent<StarterAssetsInputs>();
        _playerComboSystemInstance = Temp.GetComponent<PlayerComboSystem>();
        _PlayerRopeClimbInstance = Temp.GetComponentInChildren<PlayerRopeClimb>();
        _SwordEquipInstance = Temp.GetComponentInChildren<SwordEquip>();
        RootMotionControllerInstance=Temp.GetComponent<RootMotionController>();

    }
}
