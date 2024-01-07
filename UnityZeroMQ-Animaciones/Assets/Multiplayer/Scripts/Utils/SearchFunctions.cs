using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchFunctions 
{
    public GameObject GetChildWithName(GameObject parent,string name)
    {
        GameObject retrunGO = null;
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).name == name)
            {
                retrunGO = parent.transform.GetChild(i).gameObject;
            }

            GameObject tmp = GetChildWithName(parent.transform.GetChild(i).gameObject, name);

            retrunGO = (tmp != null ? tmp : retrunGO);
        }
        return retrunGO;
    }
}
