using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using TMPro;

public class ShowKey : MonoBehaviour
{
    public TMP_InputField contraseñaNameInput;
    public TMP_InputField usuarioNameInput;

    // Start is called before the first frame update
    void Start()
    {
        contraseñaNameInput = GetComponent<TMP_InputField>();
        contraseñaNameInput.onSelect.AddListener(x => OpenKeyBoard());

        usuarioNameInput = GetComponent<TMP_InputField>();
        usuarioNameInput.onSelect.AddListener(x => Open());

    }
    public void OpenKeyBoard()
    {
        NonNativeKeyboard.Instance.InputField = contraseñaNameInput;
       


        NonNativeKeyboard.Instance.PresentKeyboard(contraseñaNameInput.text);

    }
    public void Open() 
    {
        NonNativeKeyboard.Instance.InputField = usuarioNameInput;
        NonNativeKeyboard.Instance.PresentKeyboard(usuarioNameInput.text);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
