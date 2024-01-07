using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookAround : MonoBehaviour
{
    float rotationX = 0f;
    float rotationY = 0f;

    public float sensitivity = 15f;

    float inputX, inputZ;

    void Update()
    {
        rotationY += Input.GetAxis("Mouse X") * sensitivity;
        rotationX += Input.GetAxis("Mouse Y") * -1 * sensitivity;
        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);

        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");
        if (inputX != 0)
            rotate();
        if (inputZ != 0)
            move();
    }

    private void move()
    {
        transform.position += transform.forward * inputZ * Time.deltaTime * 4;
    }

    private void rotate()
    {
        transform.position += transform.right * inputX * Time.deltaTime * 4;
    } 
}
