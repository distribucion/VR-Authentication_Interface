using ReadyPlayerMe.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CardboardCameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject CameraToFollow;
    private bool isFollowing;
    private Transform localTransform;

    private void Awake()
    {
        localTransform = GetComponent<Transform>(); 
    }
    // Start is called before the first frame update
    void Start()
    {
        if (CameraToFollow == null)
        {
            SDKLogger.LogWarning("Cardboard", "No seteado la cámara");
            enabled = false;
            return;
        }
        isFollowing= true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isFollowing)
        {
            localTransform.forward = new Vector3(CameraToFollow.transform.forward.x,
                localTransform.forward.y,
                CameraToFollow.transform.forward.z);
        }
    }
}
