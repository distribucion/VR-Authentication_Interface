using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class VerificacionUsuarioInterfaz : MonoBehaviour
{
    public TMP_InputField contraseñaNameInput;
    public TMP_InputField usuarioNameInput;

    public Button accessButton;
    public FirebaseApp _app;
    public bool autenticado = false;

    // Start is called before the first frame update
    void Start()
    {


    }



    [System.Serializable]
    public class FirestoreConfig
    {
        public string collectionName;
        public string documentName;

        public string contraseñaName;
        public string usuarioName;
    }
    public void PrubaBasicaAut()
    {
        string contraseñaName = contraseñaNameInput.text;
        string usuarioName = usuarioNameInput.text;


        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("users").Document(usuarioName);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {

            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                //Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));
                Dictionary<string, object> docRef = snapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in docRef)
                {
                    if (pair.Key == "contraseña" && pair.Value.ToString() == contraseñaName)

                    {
                        Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));
                        autenticado = true;


                        FirestoreConfig config = new FirestoreConfig
                        {
                            contraseñaName = contraseñaNameInput.text,
                            usuarioName = usuarioNameInput.text
                        };
                        string configJson = JsonUtility.ToJson(config);
                        PlayerPrefs.SetString("FirestoreConfig", configJson);
                        Debug.Log("Datos guardados Localmente");


                        VerificarDatosGuardadosLocalmente();

                    }


                }
            }
            else
            {
                //Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
            }

            if (!autenticado)
            {
                Debug.Log("Controaseña incorrecta");
                return;
            }

        });

    }

    public void VerificarDatosGuardadosLocalmente()
    {
        // Cargar la configuración guardada localmente
        string savedConfigJson = PlayerPrefs.GetString("FirestoreConfig");
        FirestoreConfig savedConfig = JsonUtility.FromJson<FirestoreConfig>(savedConfigJson);

        // Verificar si los datos están guardados localmente
        if (!string.IsNullOrEmpty(savedConfig.contraseñaName) && !string.IsNullOrEmpty(savedConfig.usuarioName))
        {
            Debug.Log("Los datos se han cargado correctamente.");
            string contraseñaName = savedConfig.contraseñaName;
            string usuarioName = savedConfig.usuarioName;
            Debug.Log("Puedes usar estos valores en tu aplicación.");

            // Cargar otra escena
            SceneManager.LoadScene("main");
        }
        else
        {
            Debug.Log("La configuracion aun no se a guardado");
            // La configuración aún no se ha guardado o ha sido eliminada.
        }
    }

}
