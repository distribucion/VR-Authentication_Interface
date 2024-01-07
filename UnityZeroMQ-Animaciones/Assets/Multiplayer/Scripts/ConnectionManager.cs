using Fusion;
using Fusion.Sockets;
//using Fusion.XR.Shared.Rig;
using ReadyPlayerMe.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static System.Collections.Specialized.BitVector32;
using Random = UnityEngine.Random;

/// <summary>
/// Administra las conexiones
/// Crea el jugador una vez que se conecta
/// </summary>
public class ConnectionManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkRunner runner;


    [Header("Eventos")]
    // Una vez que se conecte al servidor
    public UnityEvent OnConnectToServer;            
    
    [Header("Configuraciones Photon Fusion")]    
    public INetworkSceneManager sceneManager;                       //Runner, se obtiene del Runner de este mismo gamobject,
                                                                    //sino se crea uno automáticamente si no se setea"

    [Header("Configuraciones de la Sala")]
    [SerializeField] private GameMode mode = GameMode.Shared;
    [SerializeField] private string roomName = "Cardboard Room";
    [SerializeField] private bool connectOnStart = false;

    [Header("Jugador")]
    [SerializeField] private NetworkObject playerPrefab;
    [SerializeField] private NetworkObject playerPrefabVr;
    [SerializeField] private Transform playerAnchor;                // Donde se ancla la
                                                                    // posición del jugador (a la cámara)
    [field: SerializeField] private NetworkObject sessionData;
    void Awake()
    {
        // Check if a runner exist on the same game object
        if (runner == null) runner = GetComponent<NetworkRunner>();

        // Create the Fusion runner and let it know that we will be providing user input
        if (runner == null) runner = gameObject.AddComponent<NetworkRunner>();
        runner.ProvideInput = true;

        
    }

    private async void Start()
    {
        // Si se conecta al servidor al iniciar
        if (connectOnStart) await Connect();

        // Permisos sobre el micrfono (App Android)
#if PLATFORM_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
            }
#endif
    }

    /// <summary>
    /// Conexión al servidor
    /// </summary>
    /// <returns></returns>
    public async Task Connect()
    {
        // Create the scene manager if it does not exist
        if (sceneManager == null) sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();

        if (OnConnectToServer != null) OnConnectToServer.Invoke();
        
        // Start or join (depends on gamemode) a session with a specific name
        var args = new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = sceneManager
        };
        await runner.StartGame(args);
    }


    /// <summary>
    /// Cuando un jugador se une a la sala
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="player"></param>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        //Debug.Log("id:" + player.PlayerId);
        //Debug.Log("runner:" + runner.LocalPlayer.PlayerId);
        if (player == runner.LocalPlayer)
        {
            MultiplayerGameManager.Instance.CurrentPlayer = player;
            //MultiplayerGameManager.Instance.Init(runner, player);

            //NetworkObject networkPlayerDataObject = runner.Spawn(sessionData, position: transform.position,//transform.position, 
            //    rotation: transform.rotation, player, (runner, obj) =>
            //    {
            //    });

            //var sessionManager = networkPlayerDataObject.gameObject.GetComponent<SessionManager>();
            //sessionManager.AvatarUrl = MultiplayerSession.CurrentAvatar;
            //sessionManager.UserId = player.PlayerId;

            var distanceToPlayer = Random.Range(0, 3);
            var randomVector = Random.onUnitSphere;
            randomVector.y = playerAnchor.transform.position.y;

            var spawnPosition = new Vector3(0, 0, 0);// playerAnchor.transform.position + randomVector.normalized * distanceToPlayer;

            // Spawn the user prefab for the local user
            NetworkObject networkPlayerObject = runner.Spawn
                (
                    MultiplayerGameManager.Instance.RigType == ClientType.VR
                        ? playerPrefabVr
                        : playerPrefab, 
                    position: spawnPosition,//playerAnchor.transform.position,//transform.position, 
                    rotation: transform.rotation, 
                    player, 
                    (runner, obj) =>
                    {
                    }
                );

            //networkPlayerDataObject.transform.parent = networkPlayerObject.transform;

            // asociado a cada instancia y replicado a todos los jugadores para
            // cargar el modelo correspondiente
            var sessionManager = networkPlayerObject.gameObject.GetComponentInChildren<SessionManager>();
            if (sessionManager != null)
            {
                sessionManager.AvatarUrl = MultiplayerSession.CurrentAvatar;
                sessionManager.UserId = player.PlayerId; 
            }
            var avatarManager = networkPlayerObject.GetComponent<PlayerAvatarManager>();
            if (avatarManager != null)
            {
                
                PersonControllerBase controller = MultiplayerGameManager.Instance.RigType switch
                {
                    ClientType.Desktop => MultiplayerGameManager.Instance.DesktopController,
                    ClientType.Android_Carboard => MultiplayerGameManager.Instance.CardboardController,
                    _ => MultiplayerGameManager.Instance.VRController
                };
                avatarManager.PersonController = controller;
                networkPlayerObject.transform.parent = controller.transform;

                // posicionar al avatar en modo FPS, en caso de VR y cardboard
                if (MultiplayerGameManager.Instance.RigType == ClientType.Android_Carboard
                    || MultiplayerGameManager.Instance.RigType == ClientType.VR)
                {
                    var refer = networkPlayerObject.GetComponent<Transform>();
                    refer.localPosition = new Vector3(0, refer.localPosition.y, 0);
                }
            }
        }
    }

    #region Unused INetworkRunnerCallbacks 
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

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
