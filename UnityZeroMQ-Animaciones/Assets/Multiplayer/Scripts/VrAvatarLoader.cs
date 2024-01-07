using ReadyPlayerMe.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static UnityEngine.GraphicsBuffer;

public class VrAvatarLoader : MonoBehaviour
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

    //[Tooltip("Rig para animaciones")]
    //[SerializeField] private GameObject avatarRig;

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
                if (previewAvatar == null)
                {
                    SetupAvatarRig();
                    CreateRigStructure();
                    controller.Setup(avatar, animatorController, session);
                }
            }
            else // REMOTO
            {
                if (previewAvatar == null)
                {
                    var animator = avatar.GetComponent<Animator>();
                    animator.avatar = null;
                    SetupAvatarRig();
                    CreateRigStructure();
                }
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

    private void SetupAvatarRig()
    {
        var rig = gameObject.GetNamedChild("VRAvatarRig");// MultiplayerGameManager.Instance.Runner.Spawn(avatarRig);
        rig.transform.parent = avatar.transform;
    }
    // Crear la estructura para las animaciones VR
    private void CreateRigStructure()
    {

        //#if !UNITY_STANDALONE_WIN

        #region MyRegion
        var helper = new SearchFunctions();
        var hips = avatar.GetNamedChild("Armature")?.GetNamedChild("Hips");

        var rig = avatar.GetComponentInChildren<Rig>().gameObject;

        var rigBuilder = avatar.AddComponent<RigBuilder>();
        

        var layer = new RigLayer(rig.GetComponent<Rig>());

        rigBuilder.layers ??= new List<RigLayer>();
        rigBuilder.layers.Add(layer);

        // Definir restricciones
        var rightArm = helper.GetChildWithName(rig.gameObject, "IKRightArm");
        var leftArm = helper.GetChildWithName(rig.gameObject, "IKLeftArm");
        var head = helper.GetChildWithName(rig.gameObject, "IKHead");

        if (leftArm != null && rightArm != null && head != null)
        {
            leftArm.GetComponent<RigArmHandler>().SetConstraints(helper.GetChildWithName(hips, "LeftArm").transform, helper.GetChildWithName(hips, "LeftForeArm").transform, helper.GetChildWithName(hips, "LeftHand").transform);

            rightArm.GetComponent<RigArmHandler>().SetConstraints(helper.GetChildWithName(hips, "RightArm").transform, helper.GetChildWithName(hips, "RightForeArm").transform, helper.GetChildWithName(hips, "RightHand").transform);

            // alinear derecha
            helper.GetChildWithName(rightArm, "Target").transform.position = helper.GetChildWithName(hips, "RightHand").transform.position;
            helper.GetChildWithName(rightArm, "Hint").transform.position = helper.GetChildWithName(hips, "RightForeArm").transform.position;

            // alinear izquierda
            helper.GetChildWithName(leftArm, "Target").transform.position = helper.GetChildWithName(hips, "LeftHand").transform.position;
            helper.GetChildWithName(leftArm, "Hint").transform.position = helper.GetChildWithName(hips, "LeftForeArm").transform.position;

            // alinear cabeza
            var headConstraints = head.GetComponent<MultiParentConstraint>();
            if (headConstraints != null)
            {
                headConstraints.data.constrainedObject = helper.GetChildWithName(hips, "Head").transform;
                headConstraints.data.sourceObjects = new WeightedTransformArray
            {
                new WeightedTransform { transform = headConstraints.gameObject.GetComponent<Transform>(), weight = 1 }
            };
            }
            rigBuilder.Build();
        } 
        #endregion

        #region Para añadir el dibujado de huesos:
        var boneRenderer = avatar.AddComponent<BoneRenderer>();

            if (hips != null)
            {
                //List<GameObject> lista= new List<GameObject>();
                //avatar.GetNamedChild("Armature").GetChildGameObjects(lista);
                //var l = hips.GetComponentInChildren<Transform>();
                //foreach (Transform t in l)
                //    Debug.Log(t.name);




                //boneRenderer.transforms = new Transform[]
                //{
                //helper.GetChildWithName(hips, "Head").transform,
                //helper.GetChildWithName(hips, "Neck").transform,
                //helper.GetChildWithName(hips, "LeftArm").transform,
                //helper.GetChildWithName(hips, "LeftForeArm").transform,
                //helper.GetChildWithName(hips, "LeftHand").transform,
                //};
            }
#endregion

//#endif    

    }
}
