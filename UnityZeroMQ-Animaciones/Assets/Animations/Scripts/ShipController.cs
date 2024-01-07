using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public GameObject player;
    public AudioSource islandSound;
    public AudioSource shipTravelling;
    public Material newSkybox;
    private Vector3 previousPosition;
    private bool shouldMove;


    // Start is called before the first frame update
    void Start()
    {
        previousPosition = transform.position;
        shouldMove = false;
        shipTravelling.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldMove)
        {
            transform.Translate(Vector3.left * Time.deltaTime * 20);

            // Check if the ship is moving
            if (transform.position != previousPosition)
            {
                // Check if the audio is playing
                if (islandSound.isPlaying)
                {
                    // If so, stop it
                    islandSound.Stop();
                    // And play the travelling sound
                    shipTravelling.Play();

                    // And change the skybox
                    RenderSettings.skybox = newSkybox;
                }
            }
            previousPosition = transform.position;
        }
    }

    public void StartMoving()
    {
        shouldMove = true;
        player.transform.SetParent(transform);
    }
}


