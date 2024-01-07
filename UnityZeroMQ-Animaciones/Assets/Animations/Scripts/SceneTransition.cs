using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public GameObject globo;
    public string targetScene;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == globo)
        {
            // Agregamos un retraso de 2 segundos
            StartCoroutine(CambiarDeEscena());
        }
    }

    IEnumerator CambiarDeEscena()
    {
        // Esperamos 2 segundos
        yield return new WaitForSeconds(10);

        // Cambiamos de escena
        SceneManager.LoadScene(targetScene);
    }
}


//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class SceneTransition : MonoBehaviour
//{
//    public GameObject globo;
//    public string targetScene;

//    public void OnTriggerEnter(Collider other)
//    {
//        if (other.gameObject == globo)
//        {
//            SceneManager.LoadScene(targetScene);
//        }
//    }
//}
