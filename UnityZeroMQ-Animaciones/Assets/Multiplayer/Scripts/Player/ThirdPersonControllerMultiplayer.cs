using Fusion;
using Fusion.Sockets;
//using Fusion.XR.Shared.Rig;
using ReadyPlayerMe.Samples;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AnimationUpdateInput : INetworkInput
{
    public int Id;
    public float Value;
}


[RequireComponent(typeof(ThirdPersonMovementMultiplayer), typeof(PlayerInput))]
public class ThirdPersonControllerMultiplayer : PersonControllerBase// : MonoBehaviour//, INetworkRunnerCallbacks
{
    private const float FALL_TIMEOUT = 0.15f;

    private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    private static readonly int JumpHash = Animator.StringToHash("JumpTrigger");
    private static readonly int FreeFallHash = Animator.StringToHash("FreeFall");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");

    private Transform playerCamera;
    private Animator animator;
    private Vector2 inputVector;
    private Vector3 moveVector;
    private GameObject avatar;
    private ThirdPersonMovementMultiplayer thirdPersonMovement;
    private PlayerInput playerInput;

    private float fallTimeoutDelta;

    [SerializeField]
    [Tooltip("Useful to toggle input detection in editor")]
    private bool inputEnabled = true;
    private bool isInitialized;


    //[SerializeField] private NetworkRunner runner;
    [SerializeField] private SessionManager sessionInfo;

    private void Init()
    {
        thirdPersonMovement = GetComponent<ThirdPersonMovementMultiplayer>();
        playerInput = GetComponent<PlayerInput>();
        playerInput.OnJumpPress += OnJump;
        isInitialized = true;

        //if (runner == null)
        //{
        //    Debug.LogWarning("Runner has to be set in the inspector to forward the input");
        //}
        //if (runner) runner.AddCallbacks(this);
    }

    public override void Setup(GameObject target, 
        RuntimeAnimatorController runtimeAnimatorController,
        SessionManager session)
    {
        if (!isInitialized)
        {
            Init();
        }

        avatar = target;
        thirdPersonMovement.Setup(avatar);
        animator = avatar.GetComponent<Animator>();
        animator.runtimeAnimatorController = runtimeAnimatorController;
        animator.applyRootMotion = false;

        sessionInfo = session;

    }

    private void Update()
    {
        if (avatar == null)
        {
            return;
        }
        if (inputEnabled)
        {
            playerInput.CheckInput();
            var xAxisInput = playerInput.AxisHorizontal;
            var yAxisInput = playerInput.AxisVertical;
            thirdPersonMovement.Move(xAxisInput, yAxisInput);
            thirdPersonMovement.SetIsRunning(playerInput.IsHoldingLeftShift);
        }
        UpdateAnimator();
        UpdateSessionInfo();
    }

    void UpdateSessionInfo()
    {
        sessionInfo.AnimationId = MoveSpeedHash;
        sessionInfo.AnimationValue = thirdPersonMovement.CurrentMoveSpeed;
    }

    private void UpdateAnimator()
    {
        var isGrounded = thirdPersonMovement.IsGrounded();
        animator.SetFloat(MoveSpeedHash, thirdPersonMovement.CurrentMoveSpeed);
        animator.SetBool(IsGroundedHash, isGrounded);
        if (isGrounded)
        {
            fallTimeoutDelta = FALL_TIMEOUT;
            animator.SetBool(FreeFallHash, false);
        }
        else
        {
            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                animator.SetBool(FreeFallHash, true);
            }
        }
    }

    private void OnJump()
    {
        if (thirdPersonMovement.TryJump())
        {
            animator.SetTrigger(JumpHash);
        }
    }

    #region INetworkRunnerCallbacks (Sync)
    //public void OnInput(NetworkRunner runner, NetworkInput input)
    //{
    //    AnimationUpdateInput rigInput = new AnimationUpdateInput
    //    {
    //      Id = MoveSpeedHash,
    //      Value =  thirdPersonMovement.CurrentMoveSpeed
    //    };
    //    input.Set(rigInput);
    //    //Debug.Log($"send + {thirdPersonMovement.CurrentMoveSpeed}");
    //} 
    #endregion

    
}
