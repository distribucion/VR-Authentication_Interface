using UnityEngine;
using NetMQ;
using NetMQ.Sockets;
using System.Text;
using System.Threading.Tasks;

public class Receiver : MonoBehaviour
{
    private const string ipAddress = "192.168.41.1"; // La direcci�n IP a la que te quieres enlazar.
    private const int port = 8000; // El puerto en el que quieres escuchar.

    private bool isListening = false;

    private void Start()
    {
        // Iniciamos una nueva tarea para escuchar en segundo plano.
        // Esto asegurar� que la escucha no bloquee el hilo principal de Unity.
        // Tambi�n puede usar un hilo separado para la escucha de conexiones en una aplicaci�n Unity m�s compleja.
        StartListening();
    }

    private void StartListening()
    {
        isListening = true;
        Task.Run(ListenForConnections);
    }

    private void ListenForConnections()
    {
        using (var responder = new ResponseSocket())
        {
            Debug.Log("Antes bind");
            responder.Bind($"tcp://" + ipAddress + ":" + port);
            Debug.Log("Despues bind. Escuchando...");
            while (isListening)
            {
                // Esperar hasta que haya una conexi�n entrante.
                string message = responder.ReceiveFrameString(Encoding.UTF8);

                // Manejar el mensaje recibido.
                // En este ejemplo, simplemente lo imprimimos en la consola.
                Debug.Log($"Mensaje recibido: " + message);
            }
        }
    }

    private void OnApplicationQuit()
    {
        // Al salir de la aplicaci�n, detenemos la escucha de conexiones.
        isListening = false;
    }
}
