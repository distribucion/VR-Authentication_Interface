using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationActivation : MonoBehaviour
{
    public GameObject rayInteractor;
    public InputActionProperty teleportationInput;
    public TeleportationProvider teleportationProvider;

    private void Awake()
    {
        rayInteractor.SetActive(false);
    }
    private void Update()
    {
       // if (!teleportationProvider.isActiveEnabled) { return;  }
        //Similar al input.GetAxis
        if (teleportationInput.action.ReadValue<Vector2>().x > 0 || teleportationInput.action.ReadValue<Vector2>().y > 0) 
        {
            rayInteractor.SetActive(true);
        } else
        {
            rayInteractor.SetActive(false);
        }
    }

}
