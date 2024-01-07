using ReadyPlayerMe.Core;
using ReadyPlayerMe.Samples;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AvatarLoader : MonoBehaviour
{
    private readonly Vector3 avatarPositionOffset = new Vector3(0, -0.08f, 0);

    [SerializeField]
    [Tooltip("RPM avatar URL or shortcode to load")]
    public string avatarUrl;
    private GameObject avatar;
    private AvatarObjectLoader avatarObjectLoader;
    [SerializeField]
    [Tooltip("Animator to use on loaded avatar")]
    private RuntimeAnimatorController animatorController;
    [SerializeField]
    [Tooltip("If true it will try to load avatar from avatarUrl on start")]
    private bool loadOnStart = true;
    [SerializeField]
    [Tooltip("Preview avatar to display until avatar loads. Will be destroyed after new avatar is loaded")]
    private GameObject previewAvatar;

    //[Networked]
    [field: SerializeField] public TextMeshPro text { get; set; }

    public event Action OnLoadComplete;

    private void Start()
    {
        // asignación del avatar definido en el momento de conectarse e instanciar
        // el jugador local
        if (transform.parent.GetComponentInChildren<SessionManager>() != null)
        {
            var sesion = transform.parent.GetComponentInChildren<SessionManager>();
            avatarUrl = sesion.AvatarUrl;
            //Debug.Log(avatarUrl);
            //text.text = "Player " + sesion.UserId.ToString();
        }
        else
        {
            //Debug.Log("null");
        }
        avatarObjectLoader = new AvatarObjectLoader();
        avatarObjectLoader.OnCompleted += OnLoadCompleted;
        avatarObjectLoader.OnFailed += OnLoadFailed;

        if (previewAvatar != null)
        {
            SetupAvatar(previewAvatar);
        }
        if (loadOnStart)
        {
            LoadAvatar(avatarUrl);
        }
    }

    private void OnLoadFailed(object sender, FailureEventArgs args)
    {
        OnLoadComplete?.Invoke();
    }

    private void OnLoadCompleted(object sender, CompletionEventArgs args)
    {
        if (previewAvatar != null)
        {
            Destroy(previewAvatar);
            previewAvatar = null;
        }
        SetupAvatar(args.Avatar);
        OnLoadComplete?.Invoke();
    }

    private void SetupAvatar(GameObject targetAvatar)
    {
        if (avatar != null)
        {
            Destroy(avatar);
        }

        avatar = targetAvatar;
        // Re-parent and reset transforms
        avatar.transform.parent = transform;
        avatar.transform.localPosition = avatarPositionOffset;
        avatar.transform.localRotation = Quaternion.Euler(0, 0, 0);


        
        

        //var controller =GetComponent<ThirdPersonController>();
        var avatarmanager = GetComponentInParent<PlayerAvatarManager>();
        if (avatarmanager != null)
        {
            var session = transform.parent.GetComponentInChildren<SessionManager>();
            var controller = avatarmanager.PersonController;
            // LOCAL
            if (controller != null)
            {
                
                controller.Setup(avatar, animatorController, session);
            } 
            else // REMOTO
            {
                // asignar el animador en caso de que no sea local
                if (GetComponent<ThirdPersonAnimationSync>() != null)
                {
                    var anim = GetComponent<ThirdPersonAnimationSync>();
                    anim.sessionInfo = session;
                    //anim.animator = avatar.GetComponent<Animator>();
                    //anim.Init(animatorController);
                    //Debug.Log(avatar.GetComponent<Animator>()!=null);
                    anim.animator = avatar.GetComponent<Animator>();
                    anim.animator.runtimeAnimatorController = animatorController;
                    anim.animator.applyRootMotion = false;


                    var sesion = transform.parent.GetComponentInChildren<SessionManager>();

                }
            }
        }
    }

    public void LoadAvatar(string url)
    {
        //remove any leading or trailing spaces
        avatarUrl = url.Trim(' ');
        avatarObjectLoader.LoadAvatar(avatarUrl);
    }
}
