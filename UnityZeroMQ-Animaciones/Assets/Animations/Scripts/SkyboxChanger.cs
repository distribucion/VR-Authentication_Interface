using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    public Material initialSkybox;
    public Material[] skyboxes;
    private int currentSkyboxIndex = 0;
    
    private void Start()
    {
        RenderSettings.skybox = initialSkybox;
        DynamicGI.UpdateEnvironment();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            ChangeSkybox();
        }
    }

    private void ChangeSkybox()
    {
        currentSkyboxIndex = (currentSkyboxIndex + 1) % (skyboxes.Length + 1);

        if (currentSkyboxIndex == skyboxes.Length)
        {
            RenderSettings.skybox = initialSkybox;
        }
        else
        {
            RenderSettings.skybox = skyboxes[currentSkyboxIndex];
        }

        DynamicGI.UpdateEnvironment();
    }
}