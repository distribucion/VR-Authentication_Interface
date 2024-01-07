using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public Transform cameraTarget;

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cameraTarget.transform.rotation * Vector3.forward, cameraTarget.transform.rotation * Vector3.up);

    }
}
