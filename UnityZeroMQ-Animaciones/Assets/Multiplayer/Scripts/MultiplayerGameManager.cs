using Fusion;
using Photon.Realtime;
using ReadyPlayerMe.Samples;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MultiplayerGameManager : Singleton<MultiplayerGameManager>
{
    [field: SerializeField] public ClientType RigType { get; set; }
    [field: SerializeField] public NetworkRunner Runner { get; set; }
    [field: SerializeField] private NetworkObject session;

    [Header("Desktop Rig")]
    [SerializeField] private GameObject desktopRig;
    [field: SerializeField] public ThirdPersonControllerMultiplayer DesktopController { get; set; }

    [Header("Cardboard Rig")]
    [SerializeField] private GameObject cardboardRig;
    [field: SerializeField] public CardboardController CardboardController { get; set; }
    
    [Header("VR Rig")]
    [SerializeField] private GameObject vrRig;
    [field: SerializeField] public VRController VRController { get; set; }

    private SessionManager sessionManager;

    public PlayerRef CurrentPlayer { get; set; }
    public string GetCurrentAvatar() 
    {
        int id = CurrentPlayer.PlayerId;
        var playersInfo = FindObjectsOfType<SessionManager>();
        if (playersInfo?.Length > 0)
        {
            Debug.Log(playersInfo.Where(f => f.UserId == id).Count());
            var user = playersInfo.Where(f => f.UserId == id).FirstOrDefault();
            if (user != null)
            {
                id = user.UserId;
            }
        }
        return MultiplayerSession.GetAvatar(id);        
    }

    // Start is called before the first frame update
    void Start()
    {
        // Definir el Rig Activo
        desktopRig.SetActive(RigType == ClientType.Desktop);
        cardboardRig.SetActive(RigType == ClientType.Android_Carboard);
        vrRig.SetActive(RigType == ClientType.VR);
    }

    public void Init(NetworkRunner runner, PlayerRef player)
    {
    }

    //private void Awake()
    //{
    //    if (Instance != null && Instance != this)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }
    //    Instance = this;
    //    DontDestroyOnLoad(gameObject);
    //}

    // Update is called once per frame
    void Update()
    { 
    }
}
