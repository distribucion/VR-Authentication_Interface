using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionManager : MonoBehaviour
{
    public InputActionAsset inputActionAsset;
    private InputAction switcher;
    
    public ActionBasedContinuousMoveProvider continousMoveProvider;
    public TeleportationProvider teleportationProvider;

    private void Awake()
    {
        teleportationProvider.enabled = false;
        continousMoveProvider.enabled = true;

        switcher = inputActionAsset.FindActionMap("LocomotionSwitcher").FindAction("Switcher");
    }

    void Start()
    {
        if (switcher != null) {
            switcher.performed += SwitchLocomotion;
        }
    }

    private void SwitchLocomotion(InputAction.CallbackContext obj)
    {
        continousMoveProvider.enabled = !continousMoveProvider.enabled;
        teleportationProvider.enabled = !teleportationProvider.enabled;
    }
}
