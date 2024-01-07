using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateShip : MonoBehaviour
{
    // Indica que el objeto está desactivado
    public GameObject ship;
    public bool isDisabled = true;

    // Se ejecuta cuando el objeto se activa
    void OnEnable()
    {
        // Activa el objeto con un delay
        if (isDisabled)
        {
            Invoke("Activate", 1f);
        }
    }

    // Método para activar el objeto
    public void Activate()
    {
        ship.SetActive(true);
    }
}
