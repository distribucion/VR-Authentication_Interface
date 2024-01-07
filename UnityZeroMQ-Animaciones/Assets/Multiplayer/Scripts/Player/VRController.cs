using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public class VRController : PersonControllerBase
{
    [SerializeField] private VrControllerHandler handler;

    private Transform localTransform;

    // Start is called before the first frame update
    void Awake()
    {
        localTransform = GetComponent<Transform>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target">Avatar</param>
    /// <param name="runtimeAnimatorController"></param>
    /// <param name="session"></param>
    public override void Setup(GameObject target, 
        RuntimeAnimatorController runtimeAnimatorController, 
        SessionManager session)
    {
        var animator = target.GetComponent<Animator>();
        animator.avatar = null;
        handler.Setup(target);
    }

    
}
