using Fusion;
using Fusion.Sockets;
//using Fusion.XR.Shared.Rig;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrLocalRig : MonoBehaviour, INetworkRunnerCallbacks
{
    //public HardwareHand leftHand;
    //public HardwareHand rightHand;
    //public HardwareHeadset headset;


    public NetworkRunner runner;
    // Start is called before the first frame update
    void Start()
    {
        if (runner == null)
        {
            Debug.LogWarning("Runner has to be set in the inspector to forward the input");
        }
        if (runner) runner.AddCallbacks(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // INetworkRunnerCallbacks
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    #region INetworkRunnerCallbacks (No Implementados)
    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }


    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }
    #endregion
}
