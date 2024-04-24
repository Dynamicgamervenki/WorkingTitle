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
        _ThirdPersonControllerInstance= Temp.GetComponent<ThirdPersonController>();
        _PlayerComboSystemInstance=Temp.GetComponent<PlayerComboSystem>();
        _StarterAssetsInputsInstance=Temp.GetComponent<StarterAssetsInputs>();
    }
}
