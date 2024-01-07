using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapTransforms
{
    [field: SerializeField] public Transform VrTarget { get; set; }
    [field: SerializeField] public Transform IkTarget { get; set; }

    [field: SerializeField] public Vector3 TrackingPositionOffset { get; set; }
    [field: SerializeField] public Vector3 TrackingRotationOffset { get; set; }

    public void ExecuteMapping()
    {
        IkTarget.position = VrTarget.TransformPoint(TrackingPositionOffset);
        IkTarget.rotation = VrTarget.rotation * Quaternion.Euler(TrackingRotationOffset);
    }
}

public class VrControllerHandler : MonoBehaviour
{
    [SerializeField] private MapTransforms head;
    [SerializeField] private MapTransforms leftHand;
    [SerializeField] private MapTransforms rightHand;

    [SerializeField] private float turnSmoothness;
    [SerializeField] private Transform ikHead;
    [SerializeField] private Vector3 headBodyOffset;

    private Transform localTransform;
    void Awake()
    {
        localTransform = GetComponent<Transform>();
    }

    public void Setup(GameObject target)
    {
        var helper = new SearchFunctions();
        //var hips = target.GetNamedChild("Armature")?.GetNamedChild("Hips");

        // Definir restricciones
        var ikRightArmAvatar = helper.GetChildWithName(target, "IKRightArm");
        var ikLeftArmAvatar = helper.GetChildWithName(target, "IKLeftArm");
        var ikHeadAvatar = helper.GetChildWithName(target, "IKHead");

        var leftTarget = helper.GetChildWithName(ikLeftArmAvatar, "Target");
        var rightTarget = helper.GetChildWithName(ikRightArmAvatar, "Target");

        if (ikHeadAvatar != null && leftTarget != null)
        {
            head.IkTarget = ikHeadAvatar.transform;
            ikHead = ikHeadAvatar.transform;

            leftHand.IkTarget = leftTarget.transform;
            rightHand.IkTarget = rightTarget.transform;
            
        }
        else
        {
        }
            
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (localTransform != null && head != null && ikHead != null)
        {
            localTransform.position = ikHead.position + headBodyOffset;
            localTransform.forward = Vector3.Lerp(localTransform.forward,
                Vector3.ProjectOnPlane(ikHead.forward, Vector3.up).normalized,
                Time.deltaTime * turnSmoothness);

            head.ExecuteMapping();
            leftHand.ExecuteMapping();
            rightHand.ExecuteMapping();
            //Debug.Log($"eee {head.IkTarget != null} {ikHead != null}");

        }
    }
}
