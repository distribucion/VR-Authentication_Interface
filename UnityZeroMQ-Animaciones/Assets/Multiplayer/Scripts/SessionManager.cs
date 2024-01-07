using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : NetworkBehaviour
{
    [Networked] public int UserId { get; set; } = 0;
    
    [Capacity(255)]
    [Networked] public string AvatarUrl { get; set; }
    [Networked] public int AnimationId{ get; set; }
    
    [Networked(OnChanged = nameof(OnXyzChanged))]
    public float AnimationValue { get; set; }

    // Has to be public static void
    public static void OnXyzChanged(Changed<SessionManager> changed)
    {
        changed.Behaviour.OnXyzChanged();
    }

    private void OnXyzChanged()
    {
        Debug.Log($"cam {AnimationValue}");
        // Some logic reacting to the value change of the "Xyz" property
    }

    private void Awake()
    {
        
    }
    public override void FixedUpdateNetwork()
    {
        
    }
}
