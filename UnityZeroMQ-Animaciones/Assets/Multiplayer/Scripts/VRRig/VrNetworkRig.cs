using Fusion;
//using Fusion.XR.Shared.Rig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkTransform))]
public class VrNetworkRig : NetworkBehaviour
{
    public VrLocalRig localRig;

    //public NetworkHand leftHand;
    //public NetworkHand rightHand;
    //public NetworkHeadset headset;

    [HideInInspector]
    public NetworkTransform networkTransform;

    public virtual bool IsLocalNetworkRig => Object.HasStateAuthority;

    private void Awake()
    {
        if (networkTransform == null) networkTransform = GetComponent<NetworkTransform>();
    }

    public override void Spawned()
    {
        base.Spawned();
        if (IsLocalNetworkRig)
        {
            localRig = FindObjectOfType<VrLocalRig>();
            if (localRig == null) Debug.LogError("Missing HardwareRig in the scene");
        }
    }
}
