using UnityEngine;
using System.Collections;

public class MaquinaDeEstados : MonoBehaviour {

    public MonoBehaviour EstadoPaseo;
    public MonoBehaviour EstadoPresentacion;
    public MonoBehaviour EstadoPregunta;
    public MonoBehaviour EstadoInicial;

    public MeshRenderer MeshRendererIndicador;

    private MonoBehaviour estadoActual;

    void Start () {
        ActivarEstado(EstadoInicial);
	}
	
    public void ActivarEstado(MonoBehaviour nuevoEstado)
    {
        if(estadoActual!=null) estadoActual.enabled = false;
        estadoActual = nuevoEstado;
        estadoActual.enabled = true;
    }

}
