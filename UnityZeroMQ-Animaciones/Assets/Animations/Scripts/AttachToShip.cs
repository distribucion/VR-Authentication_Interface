using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToShip : MonoBehaviour
{
    public GameObject ship;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == ship)
        {
            transform.SetParent(collision.transform);
        }
    }
}
