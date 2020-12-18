using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemScript : MonoBehaviour
{
    //1 for heal, 0 for trap
    public bool type;

    void OnCollisionEnter(Collision collision)
    {
        if(type)
        { }
    }
}
