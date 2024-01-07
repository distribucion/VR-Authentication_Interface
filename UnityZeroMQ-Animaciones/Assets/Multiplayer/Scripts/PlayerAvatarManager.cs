using ReadyPlayerMe.Samples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarManager : MonoBehaviour
{
    //[SerializeField] private ThirdPersonLoader thirdPersonLoader;
    [field:SerializeField] public PersonControllerBase PersonController { get; set; }

    //public void SetAvatar(string avatarUrl)
    //{
    //    if(thirdPersonLoader != null)
    //        thirdPersonLoader.avatarUrl = avatarUrl;
    //}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
