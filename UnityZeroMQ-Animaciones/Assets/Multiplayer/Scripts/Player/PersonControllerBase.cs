using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonControllerBase : MonoBehaviour
{
    public virtual void Setup(GameObject target,
        RuntimeAnimatorController runtimeAnimatorController,
        SessionManager session)
    {
    }
}
