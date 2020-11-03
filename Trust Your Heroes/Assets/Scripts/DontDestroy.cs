using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script used so the object with the YourHeroTeam script doesn't get destroyed

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Respawn");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
