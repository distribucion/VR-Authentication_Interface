using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class voice_control : MonoBehaviour
{
    private MaquinaDeEstados maquinaDeEstados;
    private ControladorNavMesh controladorNavMesh;

    [SerializeField]
    private Rigidbody companionPrefab;

    private Dictionary<string, Action> keywordActions = new Dictionary<string, Action>();
    private KeywordRecognizer keywordRecognizer;

    public Transform target;
    public float speed;
    private bool check = false;
    public Animator animator;
    private Quaternion originRot;

    public Transform cameraTarget;


    void Awake()
    {
        maquinaDeEstados = GetComponent<MaquinaDeEstados>();
        controladorNavMesh = GetComponent<ControladorNavMesh>();
    }

    void Start()
    {
        keywordActions.Add("adelante", Adelante);
        keywordActions.Add("gira a la derecha", GiraDerecha);
        keywordActions.Add("gira a la izquierda", GiraIzquierda);
        keywordActions.Add("anda", Anda);
        keywordActions.Add("baila", Baila);
        keywordActions.Add("hola", Hola);


        keywordRecognizer = new KeywordRecognizer(keywordActions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();
    }
    
    private void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Keyword: " + args.text);
        keywordActions[args.text].Invoke();

        if(args.text == "anda")
        {
            check = true;
        }
    }

    private void Hola()
    {
        transform.LookAt(cameraTarget);
        Debug.Log("Hola");
        maquinaDeEstados.ActivarEstado(maquinaDeEstados.EstadoPresentacion);
        animator.SetBool("isWalking", false);
    }

    private void Adelante()
    {
        transform.position = new Vector3(2.26f, 0.2f, 3.19f);
        Debug.Log("Adelante");
    }

    private void GiraDerecha()
    {
        transform.Rotate(0f, -40f, 0f);
        Debug.Log("GiraDerecha");
    }

    private void GiraIzquierda()
    {
        transform.Rotate(0f, 40f, 0f);
        Debug.Log("GiraIzquierda");
    }

    private void Anda()
    {
        originRot = transform.rotation;
        Debug.Log("Anda");
    }

    private void Baila()
    {
        animator.SetBool("isDancing", true);
        Debug.Log("Baila");
        StartCoroutine(DanceSequence());
    }

    private IEnumerator DanceSequence()
    {
        yield return new WaitForSeconds(16.8f);
        animator.SetBool("isDancing", false);
    }

    private void Update()
    {
        if (check)
        {
            animator.SetBool("isWalking", true);

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, target.position - transform.position, speed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            
            if(transform.position == target.position)
            {
                check = false;
                animator.SetBool("isWalking", false);
                transform.rotation = originRot;
            }
        }
    }


}
