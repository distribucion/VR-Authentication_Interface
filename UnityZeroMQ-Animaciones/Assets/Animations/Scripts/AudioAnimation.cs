using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAnimation : StateMachineBehaviour
{
    public AudioClip audioClip;

    private AudioSource audioSource;
    private bool audioReproducido = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (audioClip != null && !audioReproducido)
        {
            if (audioSource == null)
            {
                audioSource = animator.gameObject.GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    audioSource = animator.gameObject.AddComponent<AudioSource>();
                }
            }
            audioSource.PlayOneShot(audioClip);
            audioReproducido = true;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        audioReproducido = false;
    }
}