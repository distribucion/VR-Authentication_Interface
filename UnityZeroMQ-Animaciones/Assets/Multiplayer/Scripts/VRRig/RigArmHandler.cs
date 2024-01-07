using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(TwoBoneIKConstraint))]
public class RigArmHandler : MonoBehaviour
{
    TwoBoneIKConstraint constraints;

    // Start is called before the first frame update
    void Awake()
    {
        constraints = GetComponent<TwoBoneIKConstraint>();
    }

    // configurar referencias
    public void SetConstraints(Transform root, Transform mid, Transform tip)
    {
        constraints.data.root = root;
        constraints.data.mid = mid; 
        constraints.data.tip = tip;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
