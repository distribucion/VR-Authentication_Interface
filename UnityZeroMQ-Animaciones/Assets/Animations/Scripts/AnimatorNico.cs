using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimatorNico : MonoBehaviour
{
    public Animator animationNico;
    public Transform playerPoint;
    public Transform playPoint;
    public Transform shipPoint;
    public Transform paintPoint;

    private bool playTriggerActivated = false;
    private bool shipTriggerActivated = false;
    private bool paintTriggerActivated = false;

    public float followDistance = 4.0f;
    public float lookSpeed = 2.0f;
    public float lookAtDistance = 10.0f;

    private float animationLength = 8.0f;
    public float paintAnimationDuration = 9.0f;
    public float playAnimationDuration = 16.0f;
    public float shipAnimationDuration = 5.0f;

    private State currentState = State.FollowingPlayer;

    private enum State
    {
        FollowingPlayer,
        GoingToPlayPoint,
        GoingToPaintPoint,
        GoingToShipPoint,
    }

    private bool isWalking = false;
    private bool nicoLocomotionActive = true;
    private bool isAnimationPlaying = false;
    private bool isPlaying = false;
    private bool isShipping = false;
    private bool isPainting = false;
    private bool isAudioPlaying = false;
    public AudioClip audioNicoMessage;

    private Dictionary<string, float> triggerCooldowns = new Dictionary<string, float>();

    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "00_Home")
        {
            animationNico.SetTrigger("Home");
        }
        else if (currentScene == "01_School")
        {
            StartCoroutine(StartAnimation());
            //animationNico.SetTrigger("Hello");
        }
        else if (currentScene == "02_Island")
        {
            animationNico.SetTrigger("Island");
        }
        else if (currentScene == "03_Park")
        {
            animationNico.SetTrigger("Park");
        }
    }
    private IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(5f);
        animationNico.SetTrigger("Hello");

        yield return new WaitForSeconds(2.0f);
        currentState = State.GoingToPaintPoint;

        yield return new WaitForSeconds(6.5f);
        currentState = State.GoingToPlayPoint;

        yield return new WaitForSeconds(17.0f);
        currentState = State.GoingToShipPoint;
    }

    void NicoLocomotion()
    {
        Transform target;
        float distanceThreshold;
        float newYPosition = transform.position.y;

        switch (currentState)
        {
            case State.GoingToPlayPoint:
                target = playPoint;
                distanceThreshold = followDistance;
                break;
            case State.GoingToShipPoint:
                target = shipPoint;
                distanceThreshold = followDistance;
                break;
            case State.GoingToPaintPoint:
                target = paintPoint;
                distanceThreshold = followDistance;
                break;
            default:
                target = playerPoint;
                distanceThreshold = 12f;
                break;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);

        if (currentState == State.FollowingPlayer)
        {
            distanceThreshold = 12f;
        }

        if (distanceToTarget > distanceThreshold)
        {
            animationNico.SetBool("Walk", true);
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            Quaternion angle = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, angle, 0.5f);

            Vector3 newPosition = transform.position + transform.forward * 1 * Time.deltaTime;
            newPosition.y = newYPosition;
            transform.position = newPosition;
        }
        else
        {
            animationNico.SetBool("Walk", false);
        }

        if (distanceToTarget <= lookAtDistance)
        {
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookSpeed);
        }

        if (currentState == State.GoingToPlayPoint)
        {
            if (Vector3.Distance(transform.position, playPoint.position) <= distanceThreshold)
            {
                currentState = State.FollowingPlayer;
            }
        }

        if (currentState == State.GoingToShipPoint)
        {
            if (Vector3.Distance(transform.position, shipPoint.position) <= distanceThreshold)
            {
                currentState = State.FollowingPlayer;
            }
        }

        if (currentState == State.GoingToPaintPoint)
        {
            if (Vector3.Distance(transform.position, paintPoint.position) <= distanceThreshold)
            {
                currentState = State.FollowingPlayer;
            }
        }
    }

    void Update()
    {
        if (nicoLocomotionActive && !isAudioPlaying && !isAnimationPlaying)
        {
            NicoLocomotion();
        }
    }

    public void OnTriggerEnter(Collider other)

    {
        if (!isAudioPlaying && !isAnimationPlaying)
        {
            string triggerTag = other.tag;

            if (triggerCooldowns.ContainsKey(triggerTag) && Time.time < triggerCooldowns[triggerTag])
            {
                return;
            }

            switch (triggerTag)
            {
                case "PlayTrigger":
                    ActivateTrigger("Play");
                    break;
                case "ShipTrigger":
                    ActivateTrigger("Ship");
                    break;
                case "PaintTrigger":
                    ActivateTrigger("Paint");
                    break;
                default:
                    currentState = State.FollowingPlayer;
                    break;
            }
        }
    }

    private void ActivateTrigger(string triggerTag)
    {
        isAnimationPlaying = true;
        isWalking = animationNico.GetBool("Walk");
        animationNico.SetBool("Walk", false);
        nicoLocomotionActive = false;

        float animationDuration = animationLength;

        switch (triggerTag)
        {
            case "Play":
                isPlaying = true;
                animationNico.SetTrigger("Play");
                animationDuration = playAnimationDuration;
                StartCoroutine(WaitForAnimationEnd(() => isPlaying = false));
                break;
            case "Ship":
                isShipping = true;
                animationNico.SetTrigger("Ship");
                animationDuration = shipAnimationDuration;
                StartCoroutine(WaitForAnimationEnd(() => isShipping = false));
                break;
            case "Paint":
                isPainting = true;
                animationNico.SetTrigger("Paint");
                animationDuration = paintAnimationDuration;
                StartCoroutine(WaitForAnimationEnd(() => isPainting = false));
                break;
            default:
                currentState = State.FollowingPlayer;
                break;
        }

        animationLength = animationDuration;
    }

    private IEnumerator WaitForAnimationEnd(Action onAnimationEnd)
    {
        yield return new WaitForSeconds(animationLength);

        animationNico.SetBool("Walk", isWalking);
        nicoLocomotionActive = true;

        isAnimationPlaying = false;
        onAnimationEnd.Invoke();
    }
}


//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class AnimatorNico : MonoBehaviour
//{
//    public Animator animationNico;
//    public Transform playerPoint;
//    public Transform paintPoint;
//    public Transform shipPoint;
//    public Transform playPoint;

//    public float followDistance = 4.0f;
//    public float lookSpeed = 2.0f;
//    public float lookAtDistance = 10.0f;

//    private enum State { FollowingPlayer, GoingToPaintPoint, GoingToShipPoint, GoingToPlayPoint }
//    private State currentState = State.FollowingPlayer;

//    private bool isWalking = false;
//    private bool nicoLocomotionActive = true;
//    private bool isPainting = false;
//    private bool isShipping = false;
//    private bool isPlaying = false;

//    public AudioClip audioClipPaint;
//    public AudioClip audioClipShip;
//    public AudioClip audioClipPlay;

//    private bool isAnimationPlaying = false;
//    private bool isRepeatWakeUpNicoRunning = false;
//    public AudioClip audioNicoMessage;
//    private bool isAudioPlaying = false;

//    private bool isAnimationInProgress = false;


//    void Start()
//    {
//        string currentScene = SceneManager.GetActiveScene().name;

//        if (currentScene == "01_Home")
//        {
//            animationNico.SetTrigger("Home");
//        }
//        else if (currentScene == "02_School")
//        {
//            //animationNico.SetTrigger("Hello");
//        }
//        else if (currentScene == "03_Island")
//        {
//            //animationNico.SetTrigger("Island");
//        }
//        StartCoroutine(PlayNicoMessage());
//    }

//    private IEnumerator PlayNicoMessage()
//    {
//        AudioSource audioSource = GetComponent<AudioSource>();
//        if (audioSource == null)
//        {
//            audioSource = gameObject.AddComponent<AudioSource>();
//        }

//        // Espera 45 segundos antes de la primera ejecución
//        yield return new WaitForSeconds(45f);

//        while (true)
//        {
//            if (!isAnimationPlaying)
//            {
//                yield return new WaitForSeconds(0.05f);

//                isWalking = animationNico.GetBool("Walk");
//                animationNico.SetBool("Walk", false);
//                nicoLocomotionActive = false;
//                isAnimationPlaying = true;

//                if (audioNicoMessage != null && !audioSource.isPlaying)
//                {
//                    audioSource.clip = audioNicoMessage;
//                    audioSource.Play();

//                    isAudioPlaying = true;
//                    yield return new WaitForSeconds(audioNicoMessage.length);
//                    isAudioPlaying = false;
//                }

//                isAnimationPlaying = false;

//                animationNico.SetBool("Walk", isWalking);
//                nicoLocomotionActive = true;

//                // Espera 45 segundos antes de la siguiente ejecución
//                yield return new WaitForSeconds(45f);
//            }
//            else
//            {
//                yield return null; // Espera un frame si hay animación en curso
//            }
//        }
//    }

//    void NicoLocomotion()
//    {
//        Transform target;
//        float distanceThreshold;
//        float newYPosition = transform.position.y;

//        switch (currentState)
//        {
//            case State.GoingToPaintPoint:
//                target = paintPoint;
//                distanceThreshold = followDistance;
//                break;
//            case State.GoingToShipPoint:
//                target = shipPoint;
//                distanceThreshold = followDistance;
//                break;
//            case State.GoingToPlayPoint:
//                target = playPoint;
//                distanceThreshold = followDistance;
//                break;
//            default:
//                target = playerPoint;
//                distanceThreshold = 12f;
//                break;
//        }

//        float distanceToTarget = Vector3.Distance(transform.position, target.position);

//        transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);

//        if (currentState == State.FollowingPlayer)
//        {
//            distanceThreshold = 12f;
//        }

//        if (distanceToTarget > distanceThreshold)
//        {
//            animationNico.SetBool("Walk", true);
//            Vector3 directionToTarget = (target.position - transform.position).normalized;
//            Quaternion angle = Quaternion.LookRotation(directionToTarget);
//            transform.rotation = Quaternion.RotateTowards(transform.rotation, angle, 0.5f);

//            Vector3 newPosition = transform.position + transform.forward * 1 * Time.deltaTime;
//            newPosition.y = newYPosition;
//            transform.position = newPosition;
//        }
//        else
//        {
//            animationNico.SetBool("Walk", false);
//        }

//        if (distanceToTarget <= lookAtDistance)
//        {
//            Vector3 directionToTarget = (target.position - transform.position).normalized;
//            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
//            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookSpeed);
//        }

//        if (currentState == State.GoingToPaintPoint)
//        {
//            if (Vector3.Distance(transform.position, paintPoint.position) <= distanceThreshold)
//            {
//                currentState = State.FollowingPlayer;
//            }
//        }

//        if (currentState == State.GoingToShipPoint)
//        {
//            if (Vector3.Distance(transform.position, shipPoint.position) <= distanceThreshold)
//            {
//                currentState = State.FollowingPlayer;
//            }
//        }

//        if (currentState == State.GoingToPlayPoint)
//        {
//            if (Vector3.Distance(transform.position, playPoint.position) <= distanceThreshold)
//            {
//                currentState = State.FollowingPlayer;
//            }
//        }
//    }

//    void Update()
//    {
//        if (nicoLocomotionActive && !isAudioPlaying && !isAnimationPlaying)
//        {
//            NicoLocomotion();
//        }

//        if (!isAudioPlaying && !isAnimationPlaying) // Verifica si no se está reproduciendo audioNicoMessage ni otra animación
//        {
//            if (Input.GetKeyDown(KeyCode.O))
//            {
//                currentState = State.GoingToPaintPoint;
//                if (audioClipPaint != null)
//                {
//                    AudioSource.PlayClipAtPoint(audioClipPaint, transform.position);
//                }
//            }

//            if (Input.GetKeyDown(KeyCode.I))
//            {
//                currentState = State.GoingToShipPoint;
//                if (audioClipShip != null)
//                {
//                    AudioSource.PlayClipAtPoint(audioClipShip, transform.position);
//                }
//            }

//            if (Input.GetKeyDown(KeyCode.U))
//            {
//                currentState = State.GoingToPlayPoint;
//                if (audioClipPlay != null)
//                {
//                    AudioSource.PlayClipAtPoint(audioClipPlay, transform.position);
//                }
//            }
//        }
//    }

//    public void OnTriggerEnter(Collider other)
//    {
//        if (!isAudioPlaying && !isAnimationPlaying)
//        {
//            if (other.CompareTag("PaintTrigger"))
//            {
//                isAnimationPlaying = true; // Marca que una animación está en curso
//                isWalking = animationNico.GetBool("Walk");
//                animationNico.SetBool("Walk", false);
//                nicoLocomotionActive = false;
//                isPainting = true;
//                animationNico.SetTrigger("Paint");
//                StartCoroutine(WaitForPaintAnimationEnd());
//            }

//            if (other.CompareTag("ShipTrigger"))
//            {
//                isAnimationPlaying = true;
//                isWalking = animationNico.GetBool("Walk");
//                animationNico.SetBool("Walk", false);
//                nicoLocomotionActive = false;
//                isShipping = true;
//                animationNico.SetTrigger("Ship");
//                StartCoroutine(WaitForShipAnimationEnd());
//            }

//            if (other.CompareTag("PlayTrigger"))
//            {
//                isAnimationPlaying = true;
//                isWalking = animationNico.GetBool("Walk");
//                animationNico.SetBool("Walk", false);
//                nicoLocomotionActive = false;
//                isPlaying = true;
//                animationNico.SetTrigger("Play");
//                StartCoroutine(WaitForPlayAnimationEnd());
//            }
//        }

//        IEnumerator WaitForPaintAnimationEnd()
//        {
//            isPainting = false;

//            yield return new WaitForSeconds(8f);

//            animationNico.SetBool("Walk", isWalking);
//            nicoLocomotionActive = true;

//            isAnimationPlaying = false;
//        }

//        IEnumerator WaitForShipAnimationEnd()
//        {
//            isShipping = false;

//            yield return new WaitForSeconds(6f);

//            animationNico.SetBool("Walk", isWalking);
//            nicoLocomotionActive = true;

//            isAnimationPlaying = false;
//        }

//        IEnumerator WaitForPlayAnimationEnd()
//        {
//            isPlaying = false;

//            yield return new WaitForSeconds(10f);

//            animationNico.SetBool("Walk", isWalking);
//            nicoLocomotionActive = true;

//            isAnimationPlaying = false;
//        }
//    }
//}
