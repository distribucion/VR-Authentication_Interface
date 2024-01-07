using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInformation : MonoBehaviour
{
    [field:SerializeField] public int PlayerId { get; set; }
    [field: SerializeField] public string AvatarURL { get; set; }
}
