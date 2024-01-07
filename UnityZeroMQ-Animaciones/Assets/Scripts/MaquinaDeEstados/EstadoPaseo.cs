using UnityEngine;
using System.Collections;

public class EstadoPaseo : MonoBehaviour {

    public Transform[] WayPoints;
    //public Color ColorEstado = Color.green;

    private MaquinaDeEstados maquinaDeEstados;
    private ControladorNavMesh controladorNavMesh;
    //private ControladorVision controladorVision;
    private int siguienteWayPoint;

    public Animator animator;
    private bool check = true;

    void Awake()
    {
        maquinaDeEstados = GetComponent<MaquinaDeEstados>();
        controladorNavMesh = GetComponent<ControladorNavMesh>();
        //controladorVision = GetComponent<ControladorVision>();
    }
	
	// Update is called once per frame
	void Update () {
        // Ve al jugador?
        //RaycastHit hit;
        //if(controladorVision.PuedeVerAlJugador(out hit))
        //{
        //    controladorNavMesh.perseguirObjectivo = hit.transform;
        //    maquinaDeEstados.ActivarEstado(maquinaDeEstados.EstadoPersecucion);
        //    return;
        //}

        if (controladorNavMesh.HemosLlegado() && check == true)
        {
            StartCoroutine(WalkPause());
            check = false;
            //Debug.Log("dentro");
          
        }
	}

    void OnEnable()
    {
        //maquinaDeEstados.MeshRendererIndicador.material.color = ColorEstado;
        ActualizarWayPointDestino();
    }

    void ActualizarWayPointDestino()
    {

        animator.SetBool("isWalking", true);
        controladorNavMesh.ActualizarPuntoDestinoNavMeshAgent(WayPoints[siguienteWayPoint].position);

    }

    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player") && enabled)
    //    {
    //        maquinaDeEstados.ActivarEstado(maquinaDeEstados.EstadoAlerta);
    //    }
    //}

    private IEnumerator WalkPause()
    {
       
        animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(3.0f);

        if (maquinaDeEstados.EstadoPaseo.enabled == true)
        {
            siguienteWayPoint = (siguienteWayPoint + 1) % WayPoints.Length;
            ActualizarWayPointDestino();
            check = true;
        }
        else
        {
            siguienteWayPoint = (siguienteWayPoint + 1) % WayPoints.Length;
            check = true;
            animator.SetBool("isWalking", false);
            yield break;
        }
        


    }
}
