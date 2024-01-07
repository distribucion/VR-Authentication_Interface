using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimonController : MonoBehaviour
{
    public ShipController shipController;

    public void OnColiisionEnter()
    {
        shipController.StartMoving();
    }
}
