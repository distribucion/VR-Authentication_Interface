using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using TMPro;

public class ShowKey : MonoBehaviour
{
    public TMP_InputField contrase�aNameInput;
    public TMP_InputField usuarioNameInput;

    // Start is called before the first frame update
    void Start()
    {
        contrase�aNameInput = GetComponent<TMP_InputField>();
        contrase�aNameInput.onSelect.AddListener(x => OpenKeyBoard());

        usuarioNameInput = GetComponent<TMP_InputField>();
        usuarioNameInput.onSelect.AddListener(x => Open());

    }
    public void OpenKeyBoard()
    {
        NonNativeKeyboard.Instance.InputField = contrase�aNameInput;
       


        NonNativeKeyboard.Instance.PresentKeyboard(contrase�aNameInput.text);

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
