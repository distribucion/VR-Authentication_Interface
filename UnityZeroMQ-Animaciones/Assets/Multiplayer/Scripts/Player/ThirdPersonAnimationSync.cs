using Fusion;
//using Fusion.XR.Shared.Rig;
using ReadyPlayerMe.Samples;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class ThirdPersonAnimationSync : NetworkBehaviour
{
    public Animator animator;
    public SessionManager sessionInfo { get; set; }
    // Start is called before the first frame update
    //public void Init(RuntimeAnimatorController runtimeAnimatorController)
    //{
    //    animator = GetComponentInChildren<Animator>();
    //    animator.runtimeAnimatorController = runtimeAnimatorController;
    //    animator.applyRootMotion = false;

    //    Debug.Log($"initialized: {animator != null}");
    //}

    public override void FixedUpdateNetwork()
    {
        //if (GetInput<AnimationUpdateInput>(out var input) == false
        //    //|| animator == null
        //    ) return;

        if (sessionInfo != null)
        {
            Debug.Log($"Update...{sessionInfo.AnimationId} - {sessionInfo.AnimationValue}");
            if(animator!= null)
            {

            animator.SetFloat(sessionInfo.AnimationId, sessionInfo.AnimationValue);
            }
        }
    }

    public void UpdateAnimations()
    {
        var inputR = GetInput<AnimationUpdateInput>();
        Debug.Log($"Update...{inputR.HasValue} - {(animator != null)}");
        // update the rig at each network tick
        if (GetInput<AnimationUpdateInput>(out var input))
        {
            Debug.Log("get .." + input.Value.ToString() + (animator != null));
            //Debug.Log("2...");
            if (animator != null && input.Value>0f)
            {
                Debug.Log(".."+input.Value.ToString()); 
                //animator.SetFloat(input.Id, input.Value);
            }
            //animator.SetBool(IsGroundedHash, true);
            //ApplyInputToRigParts(input);

            //ApplyInputToHandPoses(input);

        }
    }
}
