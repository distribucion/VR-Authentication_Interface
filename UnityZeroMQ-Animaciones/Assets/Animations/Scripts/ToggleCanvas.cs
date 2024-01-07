using UnityEngine;

public class ToggleCanvas : MonoBehaviour
{
    public GameObject canvas1;
    public GameObject canvas2;
    public KeyCode keyToPress = KeyCode.P;

    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            bool isActive = !canvas1.activeSelf;
            canvas1.SetActive(isActive);
            canvas2.SetActive(isActive);
        }
    }
}

