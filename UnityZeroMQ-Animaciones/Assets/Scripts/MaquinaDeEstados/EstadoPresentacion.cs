using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class EstadoPresentacion : MonoBehaviour {

    public float velocidadGiroBusqueda = 120f;
    public float duracionBusqueda = 10f;
    //public Color ColorEstado = Color.yellow;

    private MaquinaDeEstados maquinaDeEstados;
    private ControladorNavMesh controladorNavMesh;
    //private ControladorVision controladorVision;
    private float tiempoBuscando;

    public Canvas dialogueBox;

	void Awake () {
        maquinaDeEstados = GetComponent<MaquinaDeEstados>();
        controladorNavMesh = GetComponent<ControladorNavMesh>();
        //controladorVision = GetComponent<ControladorVision>();
	}

    void OnEnable()
    {
        //maquinaDeEstados.MeshRendererIndicador.material.color = ColorEstado;
        controladorNavMesh.DetenerNavMeshAgent();
        tiempoBuscando = 0f;
    }
	
	void Update () {
        //RaycastHit hit;
        //if (controladorVision.PuedeVerAlJugador(out hit))
        //{
        //    controladorNavMesh.perseguirObjectivo = hit.transform;
        //    maquinaDeEstados.ActivarEstado(maquinaDeEstados.EstadoPregunta);
        //    return;
        //}
        if (dialogueBox.enabled != true)
        {
            dialogueBox.enabled = true;
        }


        //transform.Rotate(0f, velocidadGiroBusqueda * Time.deltaTime, 0f);
        tiempoBuscando += Time.deltaTime;
        if(tiempoBuscando >= duracionBusqueda)
        {
            dialogueBox.enabled = false;
            maquinaDeEstados.ActivarEstado(maquinaDeEstados.EstadoPaseo);
            return;
        }
	}
}
